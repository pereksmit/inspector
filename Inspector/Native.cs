﻿// ReSharper disable InconsistentNaming

namespace Inspector
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public static class Native
    {
        public delegate bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam);

        public delegate IntPtr HookDelegate(int code, int wParam, IntPtr lParam);

        public delegate void WinEventDelegate(
            IntPtr hWinEventHook,
            uint eventType,
            IntPtr hWnd,
            int idObject,
            int idChild,
            uint dwEventThread,
            uint dwmsEventTime);

        public const int WM_LBUTTONDOWN = 0x0201;

        public const int WH_MOUSE_LL = 14;

        public const int WS_EX_TRANSPARENT = 0x20;

        public const uint SWP_NOSIZE = 0x0001;

        public const uint SWP_NOMOVE = 0x0002;

        public const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;

        public const uint WINEVENT_OUTOFCONTEXT = 0;

        public const uint EVENT_OBJECT_CREATE = 0x8000;

        public static int WM_COPYDATA = 0x004A;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr WindowFromPoint(Point p);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookDelegate lpfn, IntPtr hmod, int dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumThreadWindows(int dwThreadId, EnumWindowsCallback lpfn, IntPtr lParam);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(
            uint eventMin,
            uint eventMax,
            IntPtr hmodWinEventProc,
            WinEventDelegate lpfnWinEventProc,
            uint idProcess,
            uint idThread,
            uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;

            public int cbData;

            public IntPtr lpData;
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
        public struct POINT
        {
            public int x;

            public int y;
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