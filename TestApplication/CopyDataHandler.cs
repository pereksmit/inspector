namespace TestApplication
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal sealed class CopyDataHandler
    {
        public delegate IntPtr HookDelegate(int code, IntPtr wParam, IntPtr lParam);

        // ReSharper disable once InconsistentNaming
        private const int WH_CALLWNDPROC = 4;

        // ReSharper disable once InconsistentNaming
        private const int WM_COPYDATA = 0x004A;

        // this field exists because of hungry GC can collect delegate, which leads to a exception
        // Managed Debugging Assistant 'CallbackOnCollectedDelegate'
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private IntPtr hook;

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

            var threadId = GetCurrentThreadId();

            this.hook = SetWindowsHookEx(WH_CALLWNDPROC, this.MouseHookProc, IntPtr.Zero, threadId);
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
                    var controlHandle = sendData.ControlHandle;

                    var ctrl = Control.FromHandle(controlHandle);

                    Send(sender, controlHandle, ctrl?.Name);
                }
            }

            return CallNextHookEx(this.hook, code, wparam, lparam);
        }

        private static IntPtr IntPtrAlloc<T>(T param) where T : struct
        {
            var retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        private static void Send(IntPtr receiver, IntPtr controlHandle, string controlName)
        {
            var data = new SendData {ControlHandle = controlHandle, ControlName = controlName};
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

            public IntPtr ControlHandle;
        }
        // ReSharper restore InconsistentNaming
        // ReSharper restore MemberCanBePrivate.Local
    }
}