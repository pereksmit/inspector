namespace Inspector
{
    using System;

    public sealed class MouseDownEventArgs : EventArgs
    {
        public MouseDownEventArgs(IntPtr hwnd)
        {
            this.Hwnd = hwnd;
        }

        public IntPtr Hwnd { get; }
    }
}