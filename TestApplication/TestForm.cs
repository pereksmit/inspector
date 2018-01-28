namespace TestApplication
{
    using System;
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
    }
}