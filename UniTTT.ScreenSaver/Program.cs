using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniTTT.Logik;
using System.Drawing;

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
            if (args.Length > 0)
            {
                try
                {
                    if (args[1].ToLower().Trim().Substring(0, 2) == "/c")
                    {
                        Application.Run(new ConfigForm());
                    }
                    else if (args[1].ToLower() == "/s")
                    {
                        Application.Run(new ScreenSaverForm(Color.Black));
                    }
                    else if (args[1].ToLower() == "/p")
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
