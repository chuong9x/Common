/*
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
     |  |              Creation Time: 12/17/2019 01:08:41 PM |  |  |     |         |      |
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
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

using static Autodesk.Revit.DB.ExtensibleStorage.AccessLevel;

namespace KeLi.Common.Revit.Information
{
    /// <summary>
    ///     Extend data utility.
    /// </summary>
    public static class ExtendDataUtil
    {
        /// <summary>
        ///     Creates an entity.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static Entity CreateEntity(string schemaName, params KeyValuePair<string, SchemaFieldType>[] fields)
        {
            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var guid = Guid.NewGuid();
            var schemaBuilder = new SchemaBuilder(guid);

            schemaBuilder.SetReadAccessLevel(Public);
            schemaBuilder.SetWriteAccessLevel(Public);
            schemaBuilder.SetSchemaName(schemaName);

            foreach (var field in fields)
            {
                switch (field.Value)
                {
                    case SchemaFieldType.Simple:
                        schemaBuilder.AddSimpleField(field.Key, typeof(string));

                        break;

                    case SchemaFieldType.List:
                        schemaBuilder.AddArrayField(field.Key, typeof(string));

                        break;

                    case SchemaFieldType.Dictionary:
                        schemaBuilder.AddMapField(field.Key, typeof(string), typeof(string));

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new Entity(schemaBuilder.Finish());
        }

        /// <summary>
        ///     Creates an entity.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static Entity CreateEntity(string schemaName, IEnumerable<KeyValuePair<string, SchemaFieldType>> fields)
        {
            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var guid = Guid.NewGuid();
            var schemaBuilder = new SchemaBuilder(guid);

            schemaBuilder.SetReadAccessLevel(Public);
            schemaBuilder.SetWriteAccessLevel(Public);
            schemaBuilder.SetSchemaName(schemaName);

            foreach (var field in fields)
            {
                switch (field.Value)
                {
                    case SchemaFieldType.Simple:

                        schemaBuilder.AddSimpleField(field.Key, typeof(string));
                        break;

                    case SchemaFieldType.List:

                        schemaBuilder.AddArrayField(field.Key, typeof(string));
                        break;

                    case SchemaFieldType.Dictionary:
                        schemaBuilder.AddMapField(field.Key, typeof(string), typeof(string));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new Entity(schemaBuilder.Finish());
        }

        /// <summary>
        ///     Gets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this Element elm, string fieldName, string schemaName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var guids = elm.GetEntitySchemaGuids();
            var schema = guids.Select(Schema.Lookup).FirstOrDefault(f => f.SchemaName == schemaName);
            var entity = elm.GetEntity(schema);
            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<T>(field) : default;
        }

        /// <summary>
        ///     Sets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="schemaName"></param>
        public static void SetFieldValue<T>(this Element elm, string fieldName, T value, string schemaName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var guids = elm.GetEntitySchemaGuids();
            var schema = guids.Select(Schema.Lookup).FirstOrDefault(f => f.SchemaName == schemaName);
            var entity = elm.GetEntity(schema);

            entity.Set(fieldName, value);
            elm.SetEntity(entity);
        }

        /// <summary>
        ///     Filed type.
        /// </summary>
        public enum SchemaFieldType
        {
            /// <summary>
            ///     Simple type.
            /// </summary>
            Simple,

            /// <summary>
            ///     List type.
            /// </summary>
            List,

            /// <summary>
            ///     Dictionary type.
            /// </summary>
            Dictionary
        }
    }
}