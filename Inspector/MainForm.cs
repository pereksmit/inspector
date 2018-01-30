namespace Inspector
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private const int MessageIdentifier = 22817; //random number

        private readonly OverlayForm overlay;

        private IntPtr lastPtr;

        public MainForm()
        {
            this.InitializeComponent();

            this.overlay = CreateOverlayForm();

            MouseHook.Instance.Hook();
            MouseHook.Instance.MouseMove += this.MouseHookMouseMove;
        }

        private static OverlayForm CreateOverlayForm()
        {
            var desktop = GetWholeDesktopDimensions();
            
            var res = new OverlayForm
            {
                Left = desktop.Left,
                Top = desktop.Top,
                Width = desktop.Width,
                Height = desktop.Height
            };

            return res;
        }

        private static Rectangle GetWholeDesktopDimensions()
        {
            var minx = int.MaxValue;
            var maxx = int.MinValue;
            var miny = int.MaxValue;
            var maxy = int.MinValue;

            foreach (var screen in Screen.AllScreens)
            {
                var bounds = screen.Bounds;
                minx = Math.Min(minx, bounds.X);
                miny = Math.Min(miny, bounds.Y);
                maxx = Math.Max(maxx, bounds.Right);
                maxy = Math.Max(maxy, bounds.Bottom);
            }

            var result = Rectangle.FromLTRB(minx, miny, maxx, maxy);
            return result;
        }

        private void MouseHookMouseMove(object sender, MouseMoveEventArgs e)
        {
            this.overlay.ControlBounds = e.ControlRectangle;
            this.overlay.Invalidate();

            this.lastPtr = e.Hwnd;
            this.textBox1.Clear();
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
        }

        private static IntPtr IntPtrAlloc<T>(T param) where T : struct
        {
            var retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        private void Send(IntPtr controlHandle)
        {
            var receiver = controlHandle;

            var data = new SendData {ControlHandle = controlHandle};
            var dataPtr = IntPtrAlloc(data);

            var copyData = new Native.COPYDATASTRUCT
            {
                dwData = new IntPtr(MessageIdentifier),
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
                var copyData = (Native.COPYDATASTRUCT) Marshal.PtrToStructure(m.LParam, typeof(Native.COPYDATASTRUCT));
                var dataType = (int) copyData.dwData;
                if (dataType == MessageIdentifier)
                {
                    var sendData = Marshal.PtrToStructure<SendData>(copyData.lpData);
                    if (sendData.ControlHandle == this.lastPtr)
                    {
                        this.textBox1.Text = sendData.ControlName;                        
                    }

                    this.lastPtr = IntPtr.Zero;
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SendData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            public string ControlName;

            public IntPtr ControlHandle;
        }
    }
}