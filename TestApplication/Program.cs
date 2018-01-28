namespace TestApplication
{
    using System;
    using System.Windows.Forms;

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CopyDataHandler.Instance.Hook();
            Application.Run(new TestForm());
        }
    }
}