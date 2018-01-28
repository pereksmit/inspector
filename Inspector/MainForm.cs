namespace Inspector
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private readonly MouseHook mouseHook;

        private readonly OverlayForm overlay;

        public MainForm()
        {
            this.InitializeComponent();

            this.overlay = CreateOverlayForm();

            this.mouseHook = new MouseHook();
            this.mouseHook.MouseMove += this.MouseHookMouseMove;
        }

        private static OverlayForm CreateOverlayForm()
        {
            var res = new OverlayForm
            {
                Left = 0,
                Top = 0,
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };

            return res;
        }

        private void MouseHookMouseMove(object sender, MouseMoveEventArgs e)
        {
            this.overlay.ControlBounds = e.ControlRectangle;
            this.overlay.Invalidate();

            this.Send(e.Hwnd);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.overlay.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.overlay?.Close();
            this.mouseHook?.Dispose();
        }

        private static IntPtr IntPtrAlloc<T>(T param) where T : struct
        {
            var retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SendData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string ControlName;

            public IntPtr ControlHandle;

            public IntPtr Sender;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Send(IntPtr.Zero);
        }

        private void Send(IntPtr controlHandle)
        {
            var receiver = Process.GetProcessesByName("TestApplication")[0].MainWindowHandle;

            var data = new SendData { Sender = this.Handle, ControlHandle = controlHandle };

            var copyData = new Native.COPYDATASTRUCT();
            copyData.dwData = new IntPtr(2);
            copyData.cbData = Marshal.SizeOf(data);
            copyData.lpData = IntPtrAlloc(data);
            var ptrCopyData = IntPtrAlloc(copyData);

            Native.SendMessage(receiver, Native.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_COPYDATA)
            {
                // Extract the file name
                var copyData = (Native.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Native.COPYDATASTRUCT));
                var dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    var txt = Marshal.PtrToStructure<SendData>(copyData.lpData);
                    this.textBox2.Text = txt.ControlName;
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
    }
}