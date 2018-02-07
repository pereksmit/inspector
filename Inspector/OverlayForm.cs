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

            this.pen = new Pen(Color.Red, BoundFrameWidth) {Alignment = PenAlignment.Inset};
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

        public void SetTopMost()
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
        }
    }
}