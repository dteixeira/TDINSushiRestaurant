using System;
using System.Windows.Forms;

namespace DeliveryClient
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DeliveryForm());
        }
    }
}