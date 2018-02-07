namespace TestApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal sealed class CopyDataHandler
    {
        public delegate IntPtr HookDelegate(int code, IntPtr wParam, IntPtr lParam);

        // ReSharper disable once InconsistentNaming
        private const int WH_CALLWNDPROC = 4;

        // ReSharper disable once InconsistentNaming
        private const int WM_COPYDATA = 0x004A;

        private IntPtr hook;

        // this field exists because of hungry GC can collect delegate, which leads to a exception
        // Managed Debugging Assistant 'CallbackOnCollectedDelegate'
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private HookDelegate hookDelegate;

        public static readonly CopyDataHandler Instance = new CopyDataHandler();

        private const int MessageIdentifier = 22817; //random number

        private CopyDataHandler()
        {
        }

        public void Hook()
        {
            if (this.hook != IntPtr.Zero)
            {
                return;
            }

            this.hookDelegate = this.MouseHookProc;

            var threadId = GetCurrentThreadId();
            
            this.hook = SetWindowsHookEx(WH_CALLWNDPROC, this.hookDelegate, IntPtr.Zero, threadId);
            if (this.hook != IntPtr.Zero)
            {
                Application.ApplicationExit += (sender, args) => this.Unhook();
                return;
            }

            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode);
        }

        private void Unhook()
        {
            if (this.hook == IntPtr.Zero)
            {
                return;
            }

            var ok = UnhookWindowsHookEx(this.hook);
            if (!ok)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }

            this.hook = IntPtr.Zero;
        }

        private IntPtr MouseHookProc(int code, IntPtr wparam, IntPtr lparam)
        {
            var m = (CWPSTRUCT) Marshal.PtrToStructure(lparam, typeof(CWPSTRUCT));

            if (m.message == WM_COPYDATA)
            {
                var copyData = (COPYDATASTRUCT) Marshal.PtrToStructure(m.lparam, typeof(COPYDATASTRUCT));
                var dataType = (int) copyData.dwData;
                if (dataType == MessageIdentifier)
                {
                    var sendData = Marshal.PtrToStructure<SendData>(copyData.lpData);
                    var sender = m.wparam;

                    var fcr = this.FindControl(sendData.ControlHandle, sendData.X, sendData.Y);

                    Send(sender, sendData.ControlHandle, fcr);
                }
            }

            return CallNextHookEx(this.hook, code, wparam, lparam);
        }

        private FindControlResult FindControl(IntPtr hwnd, int x, int y)
        {            
            var ctrl = Control.FromHandle(hwnd);
            if (ctrl == null)
            {
                return new FindControlResult(Rectangle.Empty, null);
            }

            var ctrlType = ctrl.GetType();

            if (ctrlType == typeof(ToolStrip))
            {
                var res = this.FindControl((ToolStrip)ctrl, x, y);
                return res;
            }

            if (ctrlType == typeof(ToolStripDropDownMenu))
            {
                var res = this.FindControl((ToolStripDropDownMenu)ctrl, x, y);
                return res;
            }
            else
            {
                var inside = this.HandleControlInsideToolStripItem(ctrl);
                if (inside != null)
                {
                    return inside;
                }

                var bounds = ctrl.Parent?.RectangleToScreen(ctrl.Bounds) ?? ctrl.Bounds;
                var res = new FindControlResult(bounds, ctrl.Name ?? ctrlType.FullName);
                return res;
            }
        }

        // control inside ToolStripItem
        private FindControlResult HandleControlInsideToolStripItem(Control ctrl)
        {
            var ownerProperty = ctrl.GetType().GetProperty("Owner");
            if (ownerProperty == null)
            {
                return null;
            }

            if (!typeof(ToolStripItem).IsAssignableFrom(ownerProperty.PropertyType))
            {
                return null;
            }

            var owner = (ToolStripItem)ownerProperty.GetValue(ctrl);

            var result = new FindControlResult(owner.Owner.RectangleToScreen(owner.Bounds), owner.Name);
            return result;
        }

        private FindControlResult FindControl(ToolStripDropDownMenu ddm, int x, int y)
        {
            var p = ddm.PointToClient(new Point(x, y));
            var item = GetAllChildren(ddm).FirstOrDefault(c => c.Bounds.Contains(p));

            if (item == null)
            {
                var res = new FindControlResult(new Rectangle(), null);
                return res;
            }
            else
            {
                var res = new FindControlResult(ddm.RectangleToScreen(item.Bounds), item.Name);
                return res;
            }
        }

        private FindControlResult FindControl(ToolStrip ts, int x, int y)
        {
            var p = ts.PointToClient(new Point(x, y));
            var item = GetAllChildren(ts).FirstOrDefault(c => c.Bounds.Contains(p));

            if (item == null)
            {
                var res = new FindControlResult(ts.Parent.RectangleToScreen(ts.Bounds), ts.Name);
                return res;
            }
            else
            {
                var res = new FindControlResult(ts.RectangleToScreen(item.Bounds), item.Name);
                return res;
            }
        }

        private static IEnumerable<ToolStripItem> GetAllChildren(ToolStrip item)
        {
            foreach (var itm in item.Items.OfType<ToolStripItem>())
            {
                foreach (var i in GetAllChildren(itm))
                {
                    yield return i;
                }
            }
        }

        private static IEnumerable<ToolStripItem> GetAllChildren(ToolStripItem item)
        {
            var splitItem = item as ToolStripSplitButton;
            if (splitItem != null)
            {
                foreach (ToolStripItem i in splitItem.DropDownItems)
                {
                    yield return i;
                }
            }

            yield return item;
        }

        private static IEnumerable<ToolStripItem> GetAllChildren(ToolStripDropDownMenu ddm)
        {
            foreach (var itm in ddm.Items.OfType<ToolStripItem>())
            {
                foreach (var i in GetAllChildren(itm))
                {
                    yield return i;
                }
            }
        }

        private static IntPtr IntPtrAlloc<T>(T param) where T : struct
        {
            var retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        private static void Send(IntPtr receiver, IntPtr controlHandle, FindControlResult fcr)
        {
            var data = new SendData { ControlHandle = controlHandle, ControlName = fcr.Name, Rectangle = fcr.Rectangle };
            var dataPtr = IntPtrAlloc(data);

            var copyData = new COPYDATASTRUCT
            {
                dwData = new IntPtr(MessageIdentifier),
                cbData = Marshal.SizeOf(data),
                lpData = dataPtr
            };

            var ptrCopyData = IntPtrAlloc(copyData);

            SendMessage(receiver, WM_COPYDATA, IntPtr.Zero, ptrCopyData);

            Marshal.FreeHGlobal(dataPtr);
            Marshal.FreeHGlobal(ptrCopyData);
        }

        private sealed class FindControlResult
        {
            public Rectangle Rectangle { get; }

            public string Name { get; }

            public FindControlResult(Rectangle rectangle, string name)
            {
                this.Rectangle = rectangle;
                this.Name = name;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int GetCurrentThreadId();

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookDelegate lpfn, IntPtr hmod, int dwThreadId);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        // ReSharper disable InconsistentNaming
        // ReSharper disable MemberCanBePrivate.Local
        [StructLayout(LayoutKind.Sequential)]        
        private struct CWPSTRUCT
        {
            public readonly IntPtr lparam;
            public readonly IntPtr wparam;
            public readonly int message;
            public readonly IntPtr hwnd;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;

            public int cbData;

            public IntPtr lpData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SendData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string ControlName;

            [MarshalAs(UnmanagedType.I4)]
            public int X;

            [MarshalAs(UnmanagedType.I4)]
            public int Y;

            public Rectangle Rectangle;

            public IntPtr ControlHandle;
        }
        // ReSharper restore InconsistentNaming
        // ReSharper restore MemberCanBePrivate.Local
    }
}