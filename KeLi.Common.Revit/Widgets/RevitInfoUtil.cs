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
     |  |              Creation Time: 02/17/2020 11:53:11 PM |  |  |     |         |      |
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
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using KeLi.Common.Converter.Collections;

using static System.Environment;
using static System.Reflection.BindingFlags;
using static System.StringComparison;
using static System.StringSplitOptions;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Revit file info.
    /// </summary>
    public class RevitInfoUtil : IDisposable
    {
        /// <summary>
        ///     Binding flags.
        /// </summary>
        private const BindingFlags _flags = Static | Instance | Public | NonPublic | InvokeMethod;

        /// <summary>
        ///     Revit file info.
        /// </summary>
        /// <param name="stream"></param>
        private RevitInfoUtil(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            try
            {
                BaseRoot = (StorageInfo)InvokeStorageRoot(null, "CreateOnStream", stream);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot get StructuredStorageRoot!", ex);
            }
        }

        /// <summary>
        ///     Revit file info.
        /// </summary>
        /// <param name="fileName"></param>
        private RevitInfoUtil(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            try
            {
                BaseRoot = (StorageInfo)InvokeStorageRoot(null, "Open", fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot get StructuredStorageRoot!", ex);
            }
        }

        /// <summary>
        ///     Base root.
        /// </summary>
        private StorageInfo BaseRoot { get; }

        /// <summary>
        ///     Disposes this object.
        /// </summary>
        public void Dispose()
        {
            CloseStorageRoot();
        }

        /// <summary>
        ///     Gets revit version number.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static int GetRevitVersionNum(FileInfo fileInfo)
        {
            if (fileInfo is null)
                throw new ArgumentNullException(nameof(fileInfo));

            return GetRevitVersionNum(fileInfo.FullName);
        }

        /// <summary>
        ///     Gets revit version number.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetRevitVersionNum(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new ArgumentNullException(filePath);

            var dict = GetRevitInfoDict(filePath);

            if (dict is null)
                throw new ArgumentNullException(nameof(dict));

            var versionInfo = dict.FirstOrDefault(f => f.Key == "Revit Build").Value;

            if (versionInfo is null)
                throw new ArgumentNullException(nameof(versionInfo));

            var startIndex = versionInfo.IndexOf("Revit", Ordinal) + 6;

            var version = versionInfo.Substring(startIndex, 4);

            return Convert.ToInt32(version);
        }

        /// <summary>
        ///     Gets revit version number.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static int GetRevitVersionNum(Stream fileStream)
        {
            if (fileStream is null)
                throw new ArgumentNullException(nameof(fileStream));

            var dict = GetRevitInfoDict(fileStream);

            if (dict is null)
                throw new ArgumentNullException(nameof(dict));

            var versionInfo = dict.FirstOrDefault(f => f.Key == "Revit Build").Value;

            var startIndex = versionInfo.IndexOf("Revit", Ordinal) + 6;

            var version = versionInfo.Substring(startIndex, 4);

            return Convert.ToInt32(version);
        }

        /// <summary>
        ///     Gets revit file info dictionary.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRevitInfoDict(FileInfo fileInfo)
        {
            if (fileInfo is null)
                throw new ArgumentNullException(nameof(fileInfo));

            return GetRevitInfoDict(fileInfo.FullName);
        }

        /// <summary>
        ///     Gets revit file info dictionary.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRevitInfoDict(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var rawData = GetRawBasicFileInfo(filePath, "BasicFileInfo");

            return GetRevitInfoDict(rawData);
        }

        /// <summary>
        ///     Gets revit file info dictionary.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRevitInfoDict(Stream fileStream)
        {
            if (fileStream is null)
                throw new ArgumentNullException(nameof(fileStream));

            var rawData = GetRawBasicFileInfo(fileStream, "BasicFileInfo");

            return GetRevitInfoDict(rawData);
        }

        /// <summary>
        ///     Gets revit fiel info dictionary.
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetRevitInfoDict(byte[] rawData)
        {
            if (rawData == null)
                throw new ArgumentNullException(nameof(rawData));

            if (rawData.Length == 0)
                throw new TargetParameterCountException(nameof(rawData.Length));

            var rawString = Encoding.Unicode.GetString(rawData);

            var fileInfo = rawString.Split(new[]
            {
                "\0",
                NewLine
            }, RemoveEmptyEntries).ToList();

            fileInfo.RemoveAll(r => !r.Contains(":"));

            fileInfo = fileInfo.Skip(2).ToList();

            var worksharing = fileInfo.FirstOrDefault(f => f.Contains("Worksharing"));

            var index = fileInfo.IndexOf(worksharing);

            if (worksharing is null)
                return fileInfo.ToDictionary2();

            var startIndex = worksharing.IndexOf("Worksharing", Ordinal);

            fileInfo[index] = worksharing.Substring(startIndex);

            return fileInfo.ToDictionary2();
        }

        /// <summary>
        ///     Gets raw basic file info.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="streamName"></param>
        /// <returns></returns>
        private static byte[] GetRawBasicFileInfo(string filePath, string streamName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrWhiteSpace(streamName))
                throw new ArgumentNullException(nameof(streamName));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            if (!IsFileStructuredStorage(filePath))
                throw new NotSupportedException("File isn't a structured storage file!");

            using (var storage = new RevitInfoUtil(filePath))
            {
                if (!storage.BaseRoot.StreamExists(streamName))
                    throw new NotSupportedException($"File doesn't contain {streamName} stream!");

                var streamInfo = storage.BaseRoot.GetStreamInfo(streamName);

                using (var stream = streamInfo.GetStream(FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[stream.Length];

                    stream.Read(buffer, 0, buffer.Length);

                    return buffer;
                }
            }
        }

        /// <summary>
        ///     Gets raw basic file info.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="streamName"></param>
        /// <returns></returns>
        private static byte[] GetRawBasicFileInfo(Stream fileStream, string streamName)
        {
            if (fileStream is null)
                throw new ArgumentNullException(nameof(fileStream));

            if (string.IsNullOrWhiteSpace(streamName))
                throw new ArgumentNullException(nameof(streamName));

            using (var storage = new RevitInfoUtil(fileStream))
            {
                if (!storage.BaseRoot.StreamExists(streamName))
                    throw new NotSupportedException($"File doesn't contain {streamName} stream!");

                var streamInfo = storage.BaseRoot.GetStreamInfo(streamName);

                using (var stream = streamInfo.GetStream(FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[stream.Length];

                    stream.Read(buffer, 0, buffer.Length);

                    return buffer;
                }
            }
        }

        /// <summary>
        ///     Invokes StorageRoot.
        /// </summary>
        /// <param name="storageRoot"></param>
        /// <param name="methodName"></param>
        /// <param name="methodArgs"></param>
        /// <returns></returns>
        private static object InvokeStorageRoot(StorageInfo storageRoot, string methodName, params object[] methodArgs)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException(nameof(methodName));

            var type = typeof(StorageInfo).Assembly.GetType("System.IO.Packaging.StorageRoot", true, false);

            return type.InvokeMember(methodName, _flags, null, storageRoot, methodArgs);
        }

        /// <summary>
        ///     Closes current object.
        /// </summary>
        private void CloseStorageRoot()
        {
            InvokeStorageRoot(BaseRoot, "Close");
        }

        /// <summary>
        ///     If true, this file is structured storage.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool IsFileStructuredStorage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var result = StgIsStorageFile(filePath);

            switch (result)
            {
                case 0:
                    return true;

                case 1:
                    return false;

                default:
                    throw new FileNotFoundException(nameof(filePath));
            }
        }

        /// <summary>
        ///     Imports StgIsStorageFile method in ole32.dll.
        /// </summary>
        /// <param name="pwcsName"></param>
        /// <returns></returns>
        [DllImport("ole32.dll")]
        private static extern int StgIsStorageFile([MarshalAs(UnmanagedType.LPWStr)] string pwcsName);
    }
}