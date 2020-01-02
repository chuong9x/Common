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
     |  |              Creation Time: 10/30/2019 05:38:41 PM |  |  |     |         |      |
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
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.RevitAddIns;
using KeLi.Common.Tool;
using KeLi.Common.Tool.Other;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    /// Revit context.
    /// </summary>
    public class RevitContext : IDisposable
    {
        /// <summary>
        /// Revit product
        /// Cannot set static variable, if not, throw exception.
        /// </summary>
        private Product _product;

        /// <summary>
        /// Client name.
        /// </summary>
        private static string _clientName;

        /// <summary>
        /// Vendor Id.
        /// </summary>
        private static string _vendorId;

        /// <summary>
        /// Revit Version.
        /// </summary>
        private static string _version;

        /// <summary>
        /// Cannot build an instance.
        /// </summary>
        private RevitContext()
        {
            InitConfig();
            SetEnvironmentVariable();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// Sets config object.
        /// </summary>
        private static void InitConfig()
        {
            _clientName = ConfigUtil.GetValue("ClientName");
            _vendorId = ConfigUtil.GetValue("VendorId");
            _version = ConfigUtil.GetValue("RevitVersion");
        }

        /// <summary>
        /// Creates an instances.
        /// </summary>
        /// <returns></returns>
        public static RevitContext CreateInstance()
        {
            return SingletonFactory<RevitContext>.CreateInstance();
        }

        /// <summary>
        /// Gets revit application.
        /// </summary>
        /// <returns></returns>
        public Application GetApplication()
        {
            var clientId = new ClientApplicationId(Guid.NewGuid(), _clientName, _vendorId);

            _product = Product.GetInstalledProduct();

            // The string must be this 'I am authorized by Autodesk to use this UI-less functionality.'.
            _product.Init(clientId, "I am authorized by Autodesk to use this UI-less functionality.");

            return _product.Application;
        }

        /// <summary>
        /// Gets revit install path by version number.
        /// </summary>
        /// <returns></returns>
        private static string GetRevitInstallPath()
        {
            var products = RevitProductUtility.GetAllInstalledRevitProducts();
            var product = products.FirstOrDefault(f => f.Name.Contains(_version));

            return product.InstallLocation;
        }

        /// <summary>
        /// Sets environment veriable path.
        /// </summary>
        private static void SetEnvironmentVariable()
        {
            var revitPath = new[] { GetRevitInstallPath() };
            var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
            var newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(revitPath));

            Environment.SetEnvironmentVariable("PATH", newPath);
        }

        /// <summary>
        /// Loads dependent dlls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            var file = $"{Path.Combine(GetRevitInstallPath(), assemblyName.Name)}.dll";

            return File.Exists(file) ? Assembly.LoadFile(file) : null;
        }

        /// <summary>
        /// Disponses the revit.
        /// </summary>
        public void Dispose()
        {
            _product?.Exit();
        }
    }
}
