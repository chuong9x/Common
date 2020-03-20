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
     |  |              Creation Time: 03/15/2020 07:50:20 AM |  |  |     |         |      |
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
using System.Management;

using HtmlAgilityPack;

using Microsoft.Win32;

namespace KeLi.Common.Tool.Hardware
{
    /// <summary>
    ///     Hardware utility.
    /// </summary>
    public class HardwareUtil
    {
        /// <summary>
        ///     Convert byte to gigabyte.
        /// </summary>
        private const double TO_GB = 1.0 / (1024.0 * 1024.0 * 1024.0);

        /// <summary>
        ///     Gets OS base info.
        /// </summary>
        /// <returns></returns>
        public static OperationSystemInfo GetOsBaseInfo()
        {
            var info = new OperationSystemInfo { CurrentUser = Environment.UserName };

            using (var mcs = new ManagementClass("Win32_ComputerSystem").GetInstances())
            {
                foreach (var moItem in mcs)
                {
                    info.PcType = moItem["Model"].ToString();

                    break;
                }
            }

            var key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");

            if (key != null)
                info.OsName = key.GetValue("ProductName").ToString();

            info.OsVersion = Environment.OSVersion.VersionString;

            return info;
        }

        /// <summary>
        ///     Gets Mainboard info list.
        /// </summary>
        /// <returns></returns>
        public static List<MainboardInfo> GetMainboardInfoList()
        {
            var results = new List<MainboardInfo>();

            using (var mcs = new ManagementClass("Win32_BaseBoard").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var mainboardInfo = new MainboardInfo
                    {
                        MainboardType = mc["Product"].ToString(),

                        MainboardManufacturer = mc["Manufacturer"].ToString()
                    };

                    results.Add(mainboardInfo);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets CPU info list.
        /// </summary>
        /// <returns></returns>
        public static List<CpuInfo> GetCpuList()
        {
            var results = new List<CpuInfo>();

            using (var mcs = new ManagementClass("Win32_Processor").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var cpuInfo = new CpuInfo
                    {
                        CpuSerialNum = mc["ProcessorId"].ToString(),

                        CpuType = mc["Name"].ToString(),

                        CpuManufacturer = mc["Manufacturer"].ToString()
                    };

                    results.Add(cpuInfo);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets RAM info list.
        /// </summary>
        /// <returns></returns>
        public static List<RamInfo> GetRamInfoList()
        {
            var results = new List<RamInfo>();

            using (var mcs = new ManagementClass("Win32_ComputerSystem").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var ramInfo = new RamInfo
                    {
                        RamSize = double.Parse((Convert.ToInt64(mc["TotalPhysicalMemory"]) * TO_GB).ToString("F2")),

                        RamManufacturer = mc["Manufacturer"].ToString()
                    };

                    results.Add(ramInfo);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets physical disk info list.
        /// </summary>
        /// <returns></returns>
        public static List<PhysicalDiskInfo> GetPhysicalDiskInfoList()
        {
            var results = new List<PhysicalDiskInfo>();

            using (var mcs = new ManagementClass("Win32_DiskDrive").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var disk = new PhysicalDiskInfo
                    {
                        DiskSerialNum = mc["SerialNumber"].ToString(),

                        DiskType = mc["Model"].ToString(),

                        DiskSize = (int)(Convert.ToInt64(mc["Size"]) * TO_GB)
                    };

                    results.Add(disk);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets logical disk info list.
        /// </summary>
        /// <returns></returns>
        public static List<LogicalDriveInfo> GetLogicalDiskInfoList()
        {
            var results = new List<LogicalDriveInfo>();

            using (var mcs = new ManagementClass("Win32_LogicalDisk").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var drive = new LogicalDriveInfo
                    {
                        DriveName = mc["Name"].ToString(),

                        DriveDetail = mc["Description"].ToString()
                    };

                    if (Convert.ToInt64(mc["Size"]) > 0)
                    {
                        drive.TotalSize = (int)(Convert.ToInt64(mc["Size"]) * TO_GB);

                        drive.FreeSpace = (int)(Convert.ToInt64(mc["FreeSpace"]) * TO_GB);
                    }

                    results.Add(drive);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets display drive info list.
        /// </summary>
        /// <returns></returns>
        public static List<DisplayDriveInfo> GetDisplayDriveInfoList()
        {
            var results = new List<DisplayDriveInfo>();

            using (var mcs = new ManagementClass("Win32_VideoController").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var card = new DisplayDriveInfo
                    {
                        DriveId = mc["PNPDeviceID"].ToString(),

                        DriveType = mc["Name"].ToString()
                    };

                    results.Add(card);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets sound card info list.
        /// </summary>
        /// <returns></returns>
        public static List<SoundCardInfo> GetSoundCardInfoList()
        {
            var results = new List<SoundCardInfo>();

            using (var mcs = new ManagementClass("Win32_SoundDevice").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    var card = new SoundCardInfo
                    {
                        CardId = mc["PNPDeviceID"].ToString(),

                        CardType = mc["Name"].ToString(),

                        CardManufacturer = mc["Manufacturer"].ToString()
                    };

                    results.Add(card);
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets first network card info.
        /// </summary>
        /// <returns></returns>
        public static NetworkCardInfo GetFirstNetworkInfoCard()
        {
            var result = new NetworkCardInfo();

            using (var mcs = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
            {
                foreach (var mc in mcs)
                {
                    if (!(bool)mc["IPEnabled"])
                        continue;

                    result.LocalIp = ((string[])mc["IpAddress"]).FirstOrDefault();

                    result.MacAddress = mc["MacAddress"].ToString();

                    break;
                }
            }

            var doc = new HtmlWeb().Load("http://www.ip-address.org/");

            result.NetworkIp = doc.DocumentNode.SelectSingleNode(@"//*[@id='myip']/p/span[1]").InnerText;

            return result;
        }
    }
}