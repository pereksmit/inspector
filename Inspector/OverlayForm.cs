namespace Inspector
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public sealed class OverlayForm : Form
    {
        private const int BoundFrameWidth = 4;

        private const int BoundFramePadding = 6;

        private IntPtr winEventHook;

        // this field exists because of hungry GC can collect delegate, which leads to a exception
        // Managed Debugging Assistant 'CallbackOnCollectedDelegate'
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Native.WinEventDelegate procDelegate;

        private Pen pen;

        public OverlayForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(1, 2, 3); // some exotic color
            this.TransparencyKey = this.BackColor;

            this.pen = new Pen(Color.Red, BoundFrameWidth) { Alignment = PenAlignment.Inset };

            this.procDelegate = this.ProcDelegate;
            this.winEventHook = Native.SetWinEventHook(
                Native.EVENT_SYSTEM_FOREGROUND,
                Native.EVENT_OBJECT_CREATE,
                IntPtr.Zero,
                this.procDelegate,
                0,
                0,
                Native.WINEVENT_OUTOFCONTEXT);
        }

        public Rectangle ControlBounds { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ExStyle |= Native.WS_EX_TRANSPARENT;
                return createParams;
            }
        }

        private void ProcDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            this.SetTopMost();
        }

        private void SetTopMost()
        {
            Native.SetWindowPos(this.Handle, Native.HWND_TOPMOST, 0, 0, 0, 0, Native.TOPMOST_FLAGS);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.ControlBounds.IsEmpty)
            {
                return;
            }

            var b = this.ControlBounds;
            b.Offset(-this.Left, -this.Top);

            b = this.NormalizeBoundingRectangle(b);
            e.Graphics.DrawRectangle(this.pen, b);
        }

        private Rectangle NormalizeBoundingRectangle(Rectangle rect)
        {
            rect.Inflate(BoundFramePadding, BoundFramePadding);

            var left = rect.Left;
            var right = rect.Right;
            var top = rect.Top;
            var bottom = rect.Bottom;

            left = Math.Max(left, 0);
            right = Math.Min(right, this.Right - this.Left);

            top = Math.Max(top, 0);
            bottom = Math.Min(bottom, this.Bottom - this.Top);

            var result = Rectangle.FromLTRB(left, top, right, bottom);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.pen.Dispose();
            this.pen = null;

            if (this.winEventHook != IntPtr.Zero)
            {
                Native.UnhookWinEvent(this.winEventHook);
                this.winEventHook = IntPtr.Zero;
            }
        }
    }
}