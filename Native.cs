// ReSharper disable InconsistentNaming

namespace Inspector
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public static class Native
    {
        public delegate IntPtr HookDelegate(int code, IntPtr wParam, IntPtr lParam);

        public const int WH_MOUSE_LL = 14;

        public const int WS_EX_TRANSPARENT = 0x20;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr WindowFromPoint(Point p);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookDelegate lpfn, IntPtr hmod, int dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            public POINT pt;
            public IntPtr hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public static implicit operator Rectangle(RECT r)
            {
                var res = Rectangle.FromLTRB(r.left, r.top, r.right, r.bottom);
                return res;
            }
        }
    }
}