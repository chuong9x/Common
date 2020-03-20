﻿/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: KeLi                          |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace KeLi.Common.Revit.Information
{
    /// <summary>
    ///     Parameter utility.
    /// </summary>
    public static class ParameterUtil
    {
        /// <summary>
        ///     Gets the element's specified parameter.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static Parameter GetParameter(this Element elm, string parameterName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (parameterName is null)
                throw new ArgumentNullException(nameof(parameterName));

            return elm.GetParameters(parameterName).FirstOrDefault(f => !f.IsReadOnly);
        }

        /// <summary>
        ///     Gets the value of the element's parameter.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="parmName"></param>
        /// <returns></returns>
        public static string GetValue(this Element elm, string parmName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (parmName is null)
                throw new ArgumentNullException(nameof(parmName));

            var parameter = elm.LookupParameter(parmName);
            var result = string.Empty;

            switch (parameter.StorageType)
            {
                case StorageType.None:
                    break;

                case StorageType.Integer:
                    result = parameter.AsInteger().ToString();

                    break;

                case StorageType.Double:
                    result = parameter.AsDouble().ToString(CultureInfo.InvariantCulture);

                    break;

                case StorageType.String:
                    result = parameter.AsString();

                    break;

                case StorageType.ElementId:
                    result = parameter.AsElementId().IntegerValue.ToString();

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        /// <summary>
        ///     Gets definition by sharing parameter file path.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="paramName"></param>
        /// <param name="canEdit"></param>
        /// <returns></returns>
        public static Definition GetDefinition(this DefinitionGroup group, string paramName, bool canEdit = false)
        {
            if (group is null)
                throw new ArgumentNullException(nameof(group));

            if (paramName is null)
                throw new ArgumentNullException(nameof(paramName));

            var definition = group.Definitions.get_Item(paramName);

            if (definition != null)
                return definition;

            var opt = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text) { UserModifiable = canEdit };

            return group.Definitions.Create(opt);
        }

        /// <summary>
        ///     Gets definition groups by sharing parameter file path.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static DefinitionGroup GetGroup(this DefinitionGroups groups, string groupName)
        {
            if (groups is null)
                throw new ArgumentNullException(nameof(groups));

            if (groupName is null)
                throw new ArgumentNullException(nameof(groupName));

            return groups.get_Item(groupName) ?? groups.Create(groupName);
        }

        /// <summary>
        ///     Gets definition groups by sharing parameter file path.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="parmPath"></param>
        /// <returns></returns>
        public static DefinitionGroups GetGroupList(this UIApplication uiapp, string parmPath)
        {
            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (parmPath is null)
                throw new ArgumentNullException(nameof(parmPath));

            if (!File.Exists(parmPath))
                File.CreateText(parmPath);

            uiapp.Application.SharedParametersFilename = parmPath;

            return uiapp.Application.OpenSharedParameterFile()?.Groups;
        }

        /// <summary>
        ///     Initializes element or type parameter binding.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="elm"></param>
        /// <param name="parmPath"></param>
        public static void InitParamList(this UIApplication uiapp, Element elm, string parmPath)
        {
            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (parmPath is null)
                throw new ArgumentNullException(nameof(parmPath));

            var doc = uiapp.ActiveUIDocument.Document;

            var bindingMap = doc.ParameterBindings;

            var groups = uiapp.GetGroupList(parmPath);

            var elmCtgs = new CategorySet();

            elmCtgs.Insert(elm.Category);

            foreach (var groupPram in GetGroups(parmPath))
            {
                var paramGroup = groups.GetGroup(groupPram.GroupName);

                foreach (var elmPram in groupPram.Params)
                {
                    var definition = paramGroup.GetDefinition(elmPram.ParamName, elmPram.CanEdit);

                    var binding = bindingMap.get_Item(definition);

                    // If the parameter group's name contains type key, it's means type binding.
                    if (!paramGroup.Name.Contains("Group"))
                    {
                        if (binding is InstanceBinding instanceBinding)
                            bindingMap.ReInsert(definition, instanceBinding);

                        else
                        {
                            instanceBinding = uiapp.Application.Create.NewInstanceBinding(elmCtgs);

                            bindingMap.Insert(definition, instanceBinding);
                        }
                    }
                    else
                    {
                        if (binding is TypeBinding typeBinding)
                            bindingMap.ReInsert(definition, typeBinding);

                        else
                        {
                            typeBinding = uiapp.Application.Create.NewTypeBinding(elmCtgs);

                            bindingMap.Insert(definition, typeBinding);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Getting group list by sharing parameter file path.
        /// </summary>
        /// <returns></returns>
        public static List<GroupParameter> GetGroups(string paramPath)
        {
            if (paramPath is null)
                throw new ArgumentNullException(nameof(paramPath));

            var texts = File.ReadLines(paramPath).ToList();

            var results = new List<GroupParameter>();

            var elmParms = new List<ElementParameter>();

            foreach (var text in texts)
            {
                var items = text.Split('\t');

                if (items[0] == "GROUP")
                    results.Add(new GroupParameter(items[1], items[2]));

                if (items[0] != "PARAM")
                    continue;

                var param = new ElementParameter
                {
                    Guid = items[1],

                    ParamName = items[2],

                    DataType = items[3],

                    DataCatetory = items[4],

                    GroupId = items[5],

                    Visible = Convert.ToBoolean(Convert.ToInt32(items[6])),

                    Description = items[7],

                    CanEdit = Convert.ToBoolean(Convert.ToInt32(items[8]))
                };

                elmParms.Add(param);
            }

            results.ForEach(f => f.Params.AddRange(elmParms.Where(w => w.GroupId == f.Id)));

            return results;
        }
    }
}