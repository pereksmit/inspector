namespace TestApplication
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public partial class TestForm : Form
    {
        public TestForm()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tbFormHwnd.Text = this.Handle.ToString("X");
            this.tbTextBoxHwnd.Text = this.textBox.Handle.ToString("X");
            this.tbGroupBoxHwnd.Text = this.groupBox.Handle.ToString("X");
        }

        private static int WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;

            public int cbData;

            public IntPtr lpData;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                // Extract the file name
                var copyData = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                var dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    var txt = Marshal.PtrToStringAnsi(copyData.lpData);
                    this.textBox.Text = txt;
                }
                else
                {
                    MessageBox.Show($"Unknown data type {dataType}");
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}