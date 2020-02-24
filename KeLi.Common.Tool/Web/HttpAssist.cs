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
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Newtonsoft.Json;

namespace KeLi.Common.Tool.Web
{
    /// <summary>
    ///     Http Assist.
    /// </summary>
    public static class HttpAssist
    {
        /// <summary>
        ///     The cookies.
        /// </summary>
        public static CookieCollection Cookies { get; } = new CookieCollection();

        /// <summary>
        ///     Gets all type request data.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="postParamDict"></param>
        /// <returns></returns>
        public static string GetRequestResult(this ResponseParam parm, IDictionary<string, string> postParamDict = null)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var postData = string.Empty;

            if (postParamDict != null)
                postData = CreateParameter(postParamDict);

            var response = parm.CreateHttpResponse(postData);

            var stream = response.GetResponseStream();

            if (stream is null)
                throw new NullReferenceException(nameof(stream));

            using (var reader = new StreamReader(stream, parm.EncodeType))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Gets all type request model data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <param name="postParamDict"></param>
        /// <returns></returns>
        public static T GetRequestResult<T>(this ResponseParam parm, IDictionary<string, string> postParamDict = null)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var response = GetRequestResult(parm, postParamDict);

            return JsonConvert.DeserializeObject<T>(response);
        }

        /// <summary>
        ///     Downloads the file.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] DownloadFile(this ResponseParam parm, FileSystemInfo filePath = null)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var response = parm.CreateHttpResponse(string.Empty, filePath);

            var st = response.GetResponseStream();

            var results = new byte[response.ContentLength];

            if (st is null)
                return results;

            st.Read(results, 0, results.Length);
            st.Close();

            return results;
        }

        /// <summary>
        ///     Uploads the file.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string UploadFile(this ResponseParam parm, FileSystemInfo filePath = null)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var response = parm.CreateHttpResponse(string.Empty, filePath);

            var stream = response.GetResponseStream();

            if (stream is null)
                throw new NullReferenceException(nameof(stream));

            using (var reader = new StreamReader(stream, parm.EncodeType))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Sets the request's stream.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="request"></param>
        private static void SetFileStream(FileSystemInfo filePath, WebRequest request)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            using (var fs = new FileStream(filePath.FullName, FileMode.Open, FileAccess.Read))
            {
                var boundary = $"----------{DateTime.Now.Ticks:x}";

                var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                var sb = new StringBuilder();

                sb.Append("--");

                sb.Append(boundary);

                sb.AppendLine();

                sb.Append($"Content-Disposition: \"form-data; name=file; filename={fs.Name}\"");

                sb.AppendLine();

                sb.Append("Content-Type: ");

                sb.Append("application/octet-stream");

                sb.AppendLine();

                sb.AppendLine();

                var postHeader = sb.ToString();

                var postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

                var fileContent = new byte[fs.Length];

                request.ContentType = $"multipart/form-data; boundary={boundary}";

                request.ContentLength = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;

                fs.Read(fileContent, 0, fileContent.Length);

                using (var stream = request.GetRequestStream())
                {
                    // Sends the file header.
                    stream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                    // Sends the file content.
                    stream.Write(fileContent, 0, fileContent.Length);

                    // Send the file time tick.
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                }
            }
        }

        /// <summary>
        ///     Creates http request data.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="postData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static HttpWebResponse CreateHttpResponse(this ResponseParam parm, string postData = null, FileSystemInfo filePath = null)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            HttpWebRequest request;

            if (parm.Url.StartsWith(@"https:\\", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckResultValidation;
                request = WebRequest.Create(parm.Url) as HttpWebRequest;

                if (request != null)
                    request.ProtocolVersion = HttpVersion.Version10;
            }

            else
            {
                request = WebRequest.Create(parm.Url) as HttpWebRequest;
            }

            if (request is null)
                throw new NullReferenceException(nameof(request));

            if (parm.Proxy != null)
                request.Proxy = parm.Proxy;

            switch (parm.Type)
            {
                case RequestType.Post:
                    request.Method = "POST";
                    break;

                case RequestType.Delete:
                    request.Method = "DELETE";
                    break;

                case RequestType.Put:
                    request.Method = "PUT";
                    break;

                case RequestType.Patch:
                    request.Method = "PATCH";
                    break;

                case RequestType.Get:
                    request.Method = "GET";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            request.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";

            request.Headers["Accept-Language"] = "en-US,en;q=0.5";

            request.Headers["Pragma"] = "no-cache";

            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            request.Headers.Add("x-authentication-token", parm.Token);

            request.Headers.Add("X-CORAL-TENANT", parm.TenantId);

            request.Headers.Add("X-AUTH-ID", parm.AuthId);

            request.Headers.Add("X-Authorization", parm.Authorization);

            request.Referer = parm.Referer;

            request.UserAgent = parm.UserAgent;

            request.ContentType = parm.ContentType;

            if (parm.Cookies != null)
            {
                request.CookieContainer = new CookieContainer();

                request.CookieContainer.Add(parm.Cookies);
            }
            else
            {
                request.CookieContainer = new CookieContainer();

                request.CookieContainer.Add(Cookies);
            }

            if (parm.Timeout.HasValue)
                request.Timeout = parm.Timeout.Value * 1000;

            request.Expect = string.Empty;

            if (!string.IsNullOrEmpty(filePath?.FullName))
                SetFileStream(filePath, request);

            if (!string.IsNullOrEmpty(postData))
            {
                var data = parm.EncodeType.GetBytes(postData);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            if (!(request.GetResponse() is HttpWebResponse result))
                throw new NullReferenceException(nameof(result));

            Cookies.Add(request.CookieContainer.GetCookies(new Uri(@"http:\\" + new Uri(parm.Url).Host)));

            Cookies.Add(request.CookieContainer.GetCookies(new Uri(@"https:\\" + new Uri(parm.Url).Host)));

            Cookies.Add(result.Cookies);

            return result;
        }

        /// <summary>
        ///     Creates the parameter.
        /// </summary>
        /// <param name="postParameterDict"></param>
        /// <returns></returns>
        private static string CreateParameter(IDictionary<string, string> postParameterDict)
        {
            if (postParameterDict is null)
                throw new ArgumentNullException(nameof(postParameterDict));

            var buffer = new StringBuilder();

            foreach (var key in postParameterDict.Keys)
                buffer.AppendFormat("&{0}={1}", key, postParameterDict[key]);

            return buffer.ToString().TrimStart('&');
        }

        /// <summary>
        ///     Checks the result vaildation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckResultValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            if (certificate is null)
                throw new ArgumentNullException(nameof(certificate));

            if (chain is null)
                throw new ArgumentNullException(nameof(chain));

            return true;
        }
    }
}