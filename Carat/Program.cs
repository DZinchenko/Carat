using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new MainForm());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception.Message.Contains("no such column") || e.Exception.Message.Contains("no such table"))
            {
                MessageBox.Show("Ви використовуєте стару версію бази даних!");
                Application.Restart();
            }
            else
            {
                MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
                Application.Exit();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = (e.ExceptionObject as Exception).Message;
            if (message.Contains("no such column") || message.Contains("no such table"))
            {
                MessageBox.Show("Ви використовуєте стару версію бази даних!");
                Application.Restart();
            }
            else
            {
                MessageBox.Show(message, "Unhandled UI Exception");
                Application.Exit();
            }
        }
    }
}
