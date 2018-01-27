namespace Inspector
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public sealed class OverlayForm : Form
    {
        private const int BoundFrameWidth = 4;

        private Pen pen;

        public OverlayForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(1, 2, 3); // some exotic color
            this.TransparencyKey = this.BackColor;

            this.pen = new Pen(Color.Red, BoundFrameWidth) {Alignment = PenAlignment.Outset};
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.ControlBounds.IsEmpty)
            {
                return;
            }

            var b = this.NormalizeBoundingRectangle(this.ControlBounds);
            e.Graphics.DrawRectangle(this.pen, b);
        }

        private Rectangle NormalizeBoundingRectangle(Rectangle rect)
        {
            rect.Inflate(BoundFrameWidth, BoundFrameWidth);

            var left = rect.Left;
            var right = rect.Right;
            var top = rect.Top;
            var bottom = rect.Bottom;

            if (left < BoundFrameWidth / 2)
            {
                left = BoundFrameWidth / 2;
            }

            if (bottom > this.Height - BoundFrameWidth / 2)
            {
                bottom = this.Height - BoundFrameWidth / 2;
            }

            if (top < BoundFrameWidth / 2)
            {
                top = BoundFrameWidth / 2;
            }

            if (right > this.Width - BoundFrameWidth / 2)
            {
                right = this.Width - BoundFrameWidth / 2;
            }

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