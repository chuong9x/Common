using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeLi.Common.Revit.Hook
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowHook
    {
        /// <summary>
        /// 
        /// </summary>
        private static int _hMouseHook;

        /// <summary>
        /// 
        /// </summary>
        private static int _hKeyboardHook;

        /// <summary>
        /// 
        /// </summary>
        public WindowHook()
        {
            StartMouseHook();
            StartKeyboardHook();
        }

        /// <summary>
        /// 
        /// </summary>
        ~WindowHook()
        {
            StopKeyboardHook();
            StopMouseHook();
        }

        /// <summary>
        /// 
        /// </summary>
        public event MouseEventHandler OnMouseActivity;

        /// <summary>
        /// 
        /// </summary>
        public event KeyEventHandler OnKeyDown;

        /// <summary>
        /// 
        /// </summary>
        public event KeyPressEventHandler OnKeyPress;

        /// <summary>
        /// 
        /// </summary>
        public event KeyEventHandler OnKeyUp;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartKeyboardHook()
        {
            var hInstance = GetModuleHandle(Process.GetCurrentProcess().MainModule?.ModuleName);

            if (_hKeyboardHook != 0)
                StopKeyboardHook();

            _hKeyboardHook = SetWindowsHookEx(GlobalMsg.WH_KEYBOARD_LL, KeyboardHookProc, hInstance, 0);

            if (_hKeyboardHook == 0)
                StopKeyboardHook();

            return _hMouseHook != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StopKeyboardHook()
        {
            var result = UnhookWindowsHookEx(_hKeyboardHook);

            _hKeyboardHook = 0;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartMouseHook()
        {
            var hInstance = GetModuleHandle(Process.GetCurrentProcess().MainModule?.ModuleName);

            if (_hMouseHook != 0)
                StopMouseHook();

            _hMouseHook = SetWindowsHookEx(GlobalMsg.WH_MOUSE_LL, MouseHookProc, hInstance, 0);

            if (_hMouseHook == 0)
                StopMouseHook();

            return _hMouseHook != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StopMouseHook()
        {
            var result = UnhookWindowsHookEx(_hMouseHook);

            _hMouseHook = 0;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0 || OnMouseActivity == null)
                return CallNextHookEx(_hMouseHook, nCode, wParam, lParam);

            var button = MouseButtons.None;

            switch (wParam)
            {
                case GlobalMsg.WM_LBUTTONDOWN:
                    button = MouseButtons.Left;

                    break;

                case GlobalMsg.WM_RBUTTONDOWN:
                    button = MouseButtons.Right;

                    break;
            }

            var clickNumber = 0;

            if (button != MouseButtons.None)
            {
                if (wParam == GlobalMsg.WM_LBUTTONDBLCLK || wParam == GlobalMsg.WM_RBUTTONDBLCLK)
                    clickNumber = 2;

                else
                    clickNumber = 1;
            }

            var mouseHook = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
            var e = new MouseEventArgs(button, clickNumber, mouseHook.X, mouseHook.Y, 0);

            OnMouseActivity(this, e);

            return CallNextHookEx(_hMouseHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0 || OnKeyDown == null && OnKeyUp == null && OnKeyPress == null)
                return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);

            var keyboardHook = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

            if (OnKeyPress != null && wParam == GlobalMsg.WM_KEYDOWN)
            {
                var keyState = new byte[256];

                GetKeyboardState(keyState);

                var inBuffer = new byte[2];

                if (ToAscii(keyboardHook.VkCode, keyboardHook.ScanCode, keyState, inBuffer, keyboardHook.Flags) == 1)
                    OnKeyPress(this, new KeyPressEventArgs((char)inBuffer[0]));
            }

            if (OnKeyDown != null && (wParam == GlobalMsg.WM_KEYDOWN || wParam == GlobalMsg.WM_SYSKEYDOWN))
            {
                var keyData = (Keys)keyboardHook.VkCode;

                OnKeyDown(this, new KeyEventArgs(keyData));
            }

            if (OnKeyUp != null && (wParam == GlobalMsg.WM_KEYUP || wParam == GlobalMsg.WM_SYSKEYUP))
                OnKeyUp(this, new KeyEventArgs((Keys)keyboardHook.VkCode));

            return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, GlobalHookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uVirtKey"></param>
        /// <param name="uScanCode"></param>
        /// <param name="lpbKeyState"></param>
        /// <param name="lpwTransKey"></param>
        /// <param name="fuState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate int GlobalHookProc(int nCode, int wParam, IntPtr lParam);
    }
}