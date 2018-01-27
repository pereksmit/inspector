namespace Inspector
{
    using System;
    using System.Drawing;

    public sealed class MouseMoveEventArgs : EventArgs
    {
        public MouseMoveEventArgs(IntPtr hwnd, Rectangle controlRectangle)
        {
            this.Hwnd = hwnd;
            this.ControlRectangle = controlRectangle;
        }

        public IntPtr Hwnd { get; }

        public Rectangle ControlRectangle { get; }
    }
}