using System.Runtime.InteropServices;

namespace KeLi.Common.Revit.Hook
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int HWnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WHitTestCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DwExtraInfo { get; set; }
    }
}