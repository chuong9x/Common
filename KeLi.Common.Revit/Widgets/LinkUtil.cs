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
using System.IO;
using System.Linq;
using System.Reflection;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using KeLi.Common.Revit.Filters;

using static Autodesk.Revit.DB.WorksetConfigurationOption;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Link utility.
    /// </summary>
    public static class LinkUtil
    {
        /// <summary>
        ///     Opens the central file.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serverPath"></param>
        /// <param name="localPath"></param>
        public static Document OpenCentralFile(this Application app, string serverPath, string localPath)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            if (serverPath is null)
                throw new ArgumentNullException(nameof(serverPath));

            if (localPath is null)
                throw new ArgumentNullException(nameof(localPath));

            var serverPathl = new FilePath(serverPath);

            var localPathl = new FilePath(localPath);

            WorksharingUtils.CreateNewLocal(serverPathl, localPathl);

            var option = new OpenOptions
            {
                DetachFromCentralOption = DetachFromCentralOption.DoNotDetach,

                Audit = false
            };

            return app.OpenDocumentFile(localPathl, option);
        }

        /// <summary>
        ///     Compacts the central file.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="centralPath"></param>
        public static void CompactCentralFile(this Application app, string centralPath)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            if (centralPath is null)
                throw new ArgumentNullException(nameof(centralPath));

            var doc = app.OpenDocumentFile(centralPath);

            var option = new SaveOptions { Compact = true };

            doc.Save(option);

            doc.Close(false);
        }

        /// <summary>
        ///     Sets the central file.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="centralPath"></param>
        /// <returns></returns>
        public static void SetCentralFile(this Document doc, string centralPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (centralPath is null)
                throw new ArgumentNullException(nameof(centralPath));

            var saveOption = new SaveAsOptions { OverwriteExistingFile = true };

            var sharingOption = new WorksharingSaveAsOptions
            {
                SaveAsCentral = true,

                OpenWorksetsDefault = SimpleWorksetConfiguration.LastViewed
            };

            saveOption.SetWorksharingOptions(sharingOption);

            doc.SaveAs(centralPath, saveOption);
        }

        /// <summary>
        ///     syncs the central file.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="comment"></param>
        public static void SyncCentralFile(this UIApplication uiapp, string comment)
        {
            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (comment is null)
                throw new ArgumentNullException(nameof(comment));

            var doc = uiapp.ActiveUIDocument.Document;

            var syncOption = new SynchronizeWithCentralOptions();

            var relinqOption = new RelinquishOptions(false)
            {
                CheckedOutElements = true,

                StandardWorksets = false
            };

            syncOption.SetRelinquishOptions(relinqOption);

            var transOption = new TransactWithCentralOptions();

            uiapp.Application.WriteJournalComment(comment, true);

            doc.SynchronizeWithCentral(transOption, syncOption);
        }

        /// <summary>
        ///     detachs from the central file.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="centralPath"></param>
        /// <returns></returns>
        public static Document DetachedFromCentralFile(this Application app, string centralPath)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            if (centralPath is null)
                throw new ArgumentNullException(nameof(centralPath));

            var modelPath = new FilePath(centralPath);

            var option = new OpenOptions
            {
                AllowOpeningLocalByWrongUser = true,

                DetachFromCentralOption = DetachFromCentralOption.DetachAndDiscardWorksets
            };

            return app.OpenDocumentFile(modelPath, option);
        }

        /// <summary>
        ///     Creates a workset.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="gridLvl"></param>
        /// <param name="worksetName"></param>
        /// <returns></returns>
        public static Workset AddWorkset(this Document doc, string gridLvl, string worksetName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (gridLvl is null)
                throw new ArgumentNullException(nameof(gridLvl));

            if (worksetName is null)
                throw new ArgumentNullException(nameof(worksetName));

            if (doc.IsWorkshared)
                doc.EnableWorksharing(gridLvl, worksetName);

            return Workset.Create(doc, worksetName);
        }

        /// <summary>
        ///     Creates a cad link.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="linkPath"></param>
        public static void AddCadLink(this Document doc, string linkPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (linkPath is null)
                throw new ArgumentNullException(nameof(linkPath));

            var options = new DWGImportOptions { OrientToView = true };

            doc.Link(linkPath, options, doc.ActiveView, out _);
        }

        /// <summary>
        ///     Creates a cad link list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="linkPaths"></param>
        public static void AddCadLinkList(this Document doc, List<string> linkPaths)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (linkPaths is null)
                throw new ArgumentNullException(nameof(linkPaths));

            linkPaths.ForEach(doc.AddCadLink);
        }

        /// <summary>
        ///     Creates a revit link.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="linkPath"></param>
        /// <returns></returns>
        public static void AddRevitLink(this Document doc, string linkPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (linkPath is null)
                throw new ArgumentNullException(nameof(linkPath));

            var filePathl = new FilePath(linkPath);
            var linkOption = new RevitLinkOptions(false);

            doc.AutoTransaction(() =>
            {
                var result = RevitLinkType.Create(doc, filePathl, linkOption);

                var instance = RevitLinkInstance.Create(doc, result.ElementId);

                if (!(doc.GetElement(instance.GetTypeId()) is RevitLinkType type))
                    return;

                type.AttachmentType = AttachmentType.Attachment;

                type.PathType = PathType.Relative;
            });
        }

        /// <summary>
        ///     Creates a revit link list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="linkPaths"></param>
        public static void AddRevitLinkList(this Document doc, List<string> linkPaths)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (linkPaths is null)
                throw new ArgumentNullException(nameof(linkPaths));

            linkPaths.ForEach(doc.AddRevitLink);
        }

        /// <summary>
        ///     Deletes the revit link.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="linkPath"></param>
        /// <returns></returns>
        public static void RemoveRevitLink(this Document doc, string linkPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (linkPath is null)
                throw new ArgumentNullException(nameof(linkPath));

            var links = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance));

            foreach (var link in links)
            {
                if (link.Name.Split(':').Length != 3)
                    continue;

                if (!(link is RevitLinkInstance instance))
                    continue;

                var type = doc.GetElement(instance.GetTypeId()) as RevitLinkType;

                type?.Unload(null);

                doc.AutoTransaction(() => doc.Delete(instance.Id));
            }
        }

        /// <summary>
        ///     Deletes the revit link list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="linkPaths"></param>
        /// <returns></returns>
        public static void RemoveRevitLinkList(this Document doc, List<string> linkPaths)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (linkPaths is null)
                throw new ArgumentNullException(nameof(linkPaths));

            linkPaths.ForEach(doc.RemoveRevitLink);
        }

        /// <summary>
        ///     Sets the revit link list.
        /// </summary>
        /// <param name="centralPath"></param>
        /// <param name="linkPaths"></param>
        public static void SetRevitLinkList(string centralPath, List<string> linkPaths)
        {
            if (centralPath is null)
                throw new ArgumentNullException(nameof(centralPath));

            if (linkPaths is null)
                throw new ArgumentNullException(nameof(linkPaths));

            var transData = TransmissionData.ReadTransmissionData(new FilePath(centralPath));

            if (transData is null)
                return;

            // If found not the link, don't submit, otherwise throw file writing exception.
            var flag = false;

            foreach (var referId in transData.GetAllExternalFileReferenceIds())
            {
                var extRef = transData.GetLastSavedReferenceData(referId);

                if (extRef.ExternalFileReferenceType != ExternalFileReferenceType.RevitLink)
                    continue;

                var userPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(extRef.GetPath());

                var linkPath = linkPaths.FirstOrDefault(w => w != null && userPath.Contains(Path.GetFileName(w)));

                if (linkPath is null)
                    continue;

                var elmId = extRef.GetReferencingId();

                transData.SetDesiredReferenceData(elmId, new FilePath(linkPath), PathType.Relative, true);

                flag = true;
            }

            if (!flag)
                return;

            transData.IsTransmitted = true;

            TransmissionData.WriteTransmissionData(new FilePath(centralPath), transData);
        }

        /// <summary>
        ///     Sets the revit link list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="filePaths"></param>
        public static void SetRevitLinkList(this Document doc, List<string> filePaths)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (filePaths is null)
                throw new ArgumentNullException(nameof(filePaths));

            if (filePaths.Count == 0)
                throw new TargetParameterCountException(nameof(filePaths.Count));

            filePaths.ForEach(doc.SetRevitLink);
        }

        /// <summary>
        ///     Sets the revit link.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="filePath"></param>
        public static void SetRevitLink(this Document doc, string filePath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            // main model modifies repeatedly, so the performance is poor.
            var linkTypes = doc.GetRevitLinkList().Where(w => filePath.Contains(w.Name));

            var type = linkTypes.FirstOrDefault(f => f.Name.Split('-').Length == 5);

            if (type is null)
                return;

            type.LoadFrom(new FilePath(filePath), new WorksetConfiguration(OpenLastViewed));

            type.PathType = PathType.Relative;
        }

        /// <summary>
        ///     Gets model's link list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<RevitLinkType> GetRevitLinkList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetTypeElementList<RevitLinkType>();
        }

        /// <summary>
        ///     Saves front model by front end.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="blankPath"></param>
        public static void SaveRevit(this UIApplication uiapp, string blankPath)
        {
            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (blankPath is null)
                throw new ArgumentNullException(nameof(blankPath));

            var doc = uiapp.ActiveUIDocument.Document;

            var saveOption = new SaveOptions { Compact = true };

            doc.Save(saveOption);

            uiapp.OpenAndActivateDocument(blankPath);

            doc.Close(true);
        }

        /// <summary>
        ///     Saves model by back end.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="centralPath"></param>
        public static void SaveRevit(this Document doc, string centralPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (centralPath is null)
                throw new ArgumentNullException(nameof(centralPath));

            var saveOption = new SaveAsOptions { OverwriteExistingFile = true };

            var modelPath = new FilePath(centralPath);

            doc.SaveAs(modelPath, saveOption);

            doc.Close(true);
        }
    }
}