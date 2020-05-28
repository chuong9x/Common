using System.Runtime.InteropServices;

namespace KeLi.Common.Revit.Hook
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class KeyboardHookStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int VkCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ScanCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DwExtraInfo { get; set; }
    }
}