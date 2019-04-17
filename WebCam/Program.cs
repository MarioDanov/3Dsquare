using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCam
{
    static class Program
    {
        public static List<Point> X { get; private set; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreen splash = new SplashScreen();
            splash.ShowDialog();
            Application.Run(new MainForm());
        }
    }
}
