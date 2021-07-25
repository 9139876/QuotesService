using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using QuotesService.GUI.Forms;

namespace QuotesService.GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            CommonLibraries.ClientApplication.ProgramUtils.PreparingServiceFactory<Startup>();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(MyCommonExceptionHandlingMethod);


            Application.Run(new MainWindow());
        }



        private static void MyCommonExceptionHandlingMethod(object sender, ThreadExceptionEventArgs t)
        {
            ShowErrorMessage(t.Exception);
        }

        public static void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}");
        }
    }
}
