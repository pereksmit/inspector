namespace Inspector
{
    using System;

    public sealed class MouseMoveEventArgs : EventArgs
    {
        public MouseMoveEventArgs(IntPtr controlHandle, int x, int y)
        {
            this.ControlHandle = controlHandle;
            this.X = x;
            this.Y = y;
        }

        public IntPtr ControlHandle { get; }

        public int X { get; }

        public int Y { get; }
    }
}