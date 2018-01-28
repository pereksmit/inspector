namespace Inspector
{
    using System;
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
    }
}