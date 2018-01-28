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
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            public string ControlName;

            public IntPtr ControlHandle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mwh = Process.GetProcessesByName("TestApplication")[0].MainWindowHandle;
            this.Send(mwh);
        }

        private void Send(IntPtr controlHandle)
        {
            int processId;
            Native.GetWindowThreadProcessId(controlHandle, out processId);

            var process = Process.GetProcessById(processId);

            if (process == null)
            {
                return;
            }

            var receiver = process.MainWindowHandle;
            //var receiver = controlHandle;

            var data = new SendData { ControlHandle = controlHandle };
            var dataPtr = IntPtrAlloc(data);

            var copyData = new Native.COPYDATASTRUCT
            {
                dwData = new IntPtr(2),
                cbData = Marshal.SizeOf(data),
                lpData = dataPtr
            };

            var ptrCopyData = IntPtrAlloc(copyData);

            Native.SendMessage(receiver, Native.WM_COPYDATA, this.Handle, ptrCopyData);

            Marshal.FreeHGlobal(dataPtr);
            Marshal.FreeHGlobal(ptrCopyData);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_COPYDATA)
            {
                var copyData = (Native.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Native.COPYDATASTRUCT));
                var dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    var txt = Marshal.PtrToStructure<SendData>(copyData.lpData);
                    this.textBox2.Text = txt.ControlName;
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        public static T FindWindow<T>(Predicate<T> match) where T : Form
        {
            T result = null;

            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
            {
                Native.EnumThreadWindows(
                    thread.Id,
                    (hwnd, lParam) =>
                    {
                        if (result != null)
                        {
                            return true;
                        }

                        var ctrl = Control.FromHandle(hwnd) as T;
                        if (ctrl == null)
                        {
                            return true;
                        }

                        var m = match(ctrl);
                        if (m)
                        {
                            result = ctrl;
                        }

                        return true;
                    },
                    IntPtr.Zero);
            }

            return result;
        }

    }
}