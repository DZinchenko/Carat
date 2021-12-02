using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("no such column") || ex.Message.Contains("no such table"))
                {
                    MessageBox.Show("Ви використовуєте стару версію бази даних!");
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
