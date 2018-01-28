using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApplication
{
    using System.Runtime.InteropServices;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.AddMessageFilter(new MessageFilter());
            Application.Run(new TestForm());
        }

        private static int WM_COPYDATA = 0x004A;

        private const int WM_PAINT = 0x0F;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;

            public int cbData;

            public IntPtr lpData;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SendData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string ControlName;

            public long ControlHandle;

            public long Sender;
        }

        private sealed class MessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg != WM_COPYDATA)
                {
                    return false;
                }                

                Console.WriteLine("WM_COPYDATA");

                var copyData = (COPYDATASTRUCT) Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                var dataType = (int) copyData.dwData;
                if (dataType != 2)
                {
                    return false;
                }

                var txt = Marshal.PtrToStructure<SendData>(copyData.lpData);

                var sender = new IntPtr(txt.Sender);
                var controlHandle = new IntPtr(txt.ControlHandle);

                var ctrl = Control.FromHandle(controlHandle);

                Send(sender, controlHandle, ctrl?.Name);

                return true;
            }

            private static IntPtr IntPtrAlloc<T>(T param) where T : struct
            {
                var retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
                Marshal.StructureToPtr(param, retval, false);
                return retval;
            }

            private static void Send(IntPtr receiver, IntPtr controlHandle, string controlName)
            {
                var data = new SendData { ControlHandle = controlHandle.ToInt64(), ControlName = controlName };
                var dataPtr = IntPtrAlloc(data);

                var copyData = new COPYDATASTRUCT
                {
                    dwData = new IntPtr(2),
                    cbData = Marshal.SizeOf(data),
                    lpData = dataPtr
                };

                var ptrCopyData = IntPtrAlloc(copyData);

                SendMessage(receiver, WM_COPYDATA, IntPtr.Zero, ptrCopyData);

                Marshal.FreeHGlobal(dataPtr);
                Marshal.FreeHGlobal(ptrCopyData);
            }
        }
    }
}
