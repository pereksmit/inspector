namespace TestApplication
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public partial class TestForm : Form
    {
        public TestForm()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tbFormHwnd.Text = this.Handle.ToString("X");
            this.tbTextBoxHwnd.Text = this.textBox.Handle.ToString("X");
            this.tbGroupBoxHwnd.Text = this.groupBox.Handle.ToString("X");
        }

        private static int WM_COPYDATA = 0x004A;

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

            public IntPtr ControlHandle;

            public IntPtr Sender;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                // Extract the file name
                var copyData = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                var dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    var txt = Marshal.PtrToStructure<SendData>(copyData.lpData);

                    var ctrl = Control.FromHandle(txt.ControlHandle);

                    this.Send(txt.Sender, txt.ControlHandle, ctrl?.Name);
                
                    this.textBox.Text = ctrl?.Name ?? "<not found>";
                }
                else
                {
                    MessageBox.Show($"Unknown data type {dataType}");
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private static IntPtr IntPtrAlloc<T>(T param) where T : struct
        {
            var retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        private void Send(IntPtr receiver, IntPtr controlHandle, string controlName)
        {
            var data = new SendData {Sender = this.Handle, ControlHandle = controlHandle, ControlName = controlName};

            var copyData = new COPYDATASTRUCT();
            copyData.dwData = new IntPtr(2);
            copyData.cbData = Marshal.SizeOf(data);
            copyData.lpData = IntPtrAlloc(data);
            var ptrCopyData = IntPtrAlloc(copyData);

            SendMessage(receiver, WM_COPYDATA, IntPtr.Zero, ptrCopyData);
        }
    }
}