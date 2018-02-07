namespace Inspector
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class MouseHook
    {
        public static readonly MouseHook Instance = new MouseHook();

        private IntPtr hook;

        // this field exists because of hungry GC can collect delegate, which leads to a exception
        // Managed Debugging Assistant 'CallbackOnCollectedDelegate'
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private Native.HookDelegate hookDelegate;

        private int lastX = int.MinValue;

        private int lastY = int.MinValue;

        private MouseHook()
        {
        }

        public void Hook()
        {
            if (this.hook != IntPtr.Zero)
            {
                return;
            }

            this.hookDelegate = this.MouseHookProc;

            var mainModuleHandle = GetNainModuleHandle();

            this.hook = Native.SetWindowsHookEx(Native.WH_MOUSE_LL, this.hookDelegate, mainModuleHandle, 0);
            if (this.hook != IntPtr.Zero)
            {
                Application.ApplicationExit += (sender, args) => this.Unhook();
                return;
            }

            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode);
        }

        private static IntPtr GetNainModuleHandle()
        {
            using (var p = Process.GetCurrentProcess())
            {
                using (var m = p.MainModule)
                {
                    var result = Native.GetModuleHandle(m.ModuleName);
                    return result;
                }
            }
        }

        private void Unhook()
        {
            if (this.hook == IntPtr.Zero)
            {
                return;
            }

            var ok = Native.UnhookWindowsHookEx(this.hook);
            if (!ok)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }

            this.hook = IntPtr.Zero;
        }

        public event EventHandler<MouseMoveEventArgs> MouseMove;

        public event EventHandler<MouseDownEventArgs> MouseDown;

        private IntPtr MouseHookProc(int code, int wparam, IntPtr lparam)
        {
            var mouse = (Native.MouseHookStruct)Marshal.PtrToStructure(lparam, typeof(Native.MouseHookStruct));

            var x = mouse.pt.x;
            var y = mouse.pt.y;

            if (wparam == Native.WM_LBUTTONDOWN)
            { 
                var w = Native.WindowFromPoint(new Point(x, y));
                if (w != IntPtr.Zero)
                {
                    this.OnMouseDown(new MouseDownEventArgs(w));
                }
            }
            else if (x != this.lastX || y != this.lastY)
            {
                this.lastX = x;
                this.lastY = y;

                var w = Native.WindowFromPoint(new Point(x, y));
                if (w != IntPtr.Zero)
                {
                    this.OnMouseMove(new MouseMoveEventArgs(w, x, y));
                }
            }

            return Native.CallNextHookEx(this.hook, code, wparam, lparam);
        }

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            this.MouseMove?.Invoke(this, e);
        }

        private void OnMouseDown(MouseDownEventArgs e)
        {
            this.MouseDown?.Invoke(this, e);
        }
    }
}