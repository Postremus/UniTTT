using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniTTT.Logik;
using System.Drawing;
using System.IO;

[assembly: CLSCompliant(true)]
namespace UniTTT.ScreenSaver
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //args = new string[2];
            //args[1] = "/c";
            //args = Environment.GetCommandLineArgs();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            File.WriteAllLines("log.txt", args);
            if (args.Length > 0)
            {
                try
                {
                    if (args[1].ToLower().Trim().Contains("/c"))
                    {
                        Application.Run(new ConfigForm());
                    }
                    else if (args[1].ToLower().Trim().Contains("/s"))
                    {
                        Application.Run(new ScreenSaverForm(Color.Black));
                    }
                    else if (args[1].ToLower().Trim().Contains("/p"))
                    {
                        Application.Run(new ScreenSaverForm(Color.Transparent));
                    }
                }
                catch
                {
                }
            }
            else
            {
                Application.Run(new ScreenSaverForm(Color.Black));
            }
        } 
    }
}
