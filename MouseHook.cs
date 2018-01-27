namespace Inspector
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public sealed class MouseHook : IDisposable
    {
        // this field exists because of hungry GC can collect delegate, which leads to a exception
        // Managed Debugging Assistant 'CallbackOnCollectedDelegate'
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Native.HookDelegate hookDelegate;

        private IntPtr hook;

        private IntPtr lastHwnd = IntPtr.Zero;

        private int lastX = int.MinValue;

        private int lastY = int.MinValue;

        public MouseHook()
        {
            this.hookDelegate = this.MouseHookProc;
            var mainModuleHandle = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);

            this.hook = Native.SetWindowsHookEx(Native.WH_MOUSE_LL, this.hookDelegate, mainModuleHandle, 0);
            if (this.hook != IntPtr.Zero)
            {
                return;
            }

            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode);
        }

        public void Dispose()
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

        private IntPtr MouseHookProc(int code, IntPtr wparam, IntPtr lparam)
        {
            var mouse = (Native.MouseHookStruct) Marshal.PtrToStructure(lparam, typeof(Native.MouseHookStruct));

            var x = mouse.pt.x;
            var y = mouse.pt.y;

            if (x != this.lastX || y != this.lastY)
            {
                this.lastX = x;
                this.lastY = y;

                var w = Native.WindowFromPoint(new Point(x, y));
                if (w != IntPtr.Zero && w != this.lastHwnd)
                {
                    this.lastHwnd = w;

                    Native.RECT rect;
                    if (Native.GetWindowRect(w, out rect))
                    {
                        this.OnMouseMove(new MouseMoveEventArgs(w, rect));
                    }
                }
            }

            return Native.CallNextHookEx(this.hook, code, wparam, lparam);
        }

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            this.MouseMove?.Invoke(this, e);
        }
    }
}