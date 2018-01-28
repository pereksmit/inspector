namespace Inspector
{
    using System;
    using System.Diagnostics;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var receiver = Process.GetProcessesByName("TestApplication")[0].MainWindowHandle;

            var txt = "Pokus: " + DateTime.Now;
            
            var copyData = new Native.COPYDATASTRUCT();
            copyData.dwData = new IntPtr(2);
            copyData.cbData = txt.Length + 1;
            copyData.lpData = Marshal.StringToHGlobalAnsi(txt);

            // Allocate memory for the data and copy
            var ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
            Marshal.StructureToPtr(copyData, ptrCopyData, false);

            // Send the message
            Native.SendMessage(receiver, Native.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
        }
    }
}