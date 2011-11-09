using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UniTTT.Wpf
{
    class OutputDarsteller : Logik.IOutputDarsteller
    {
        public OutputDarsteller(ref MainWindow mainwindow)
        {
            MWindow = mainwindow;
        }

        private MainWindow MWindow;

        public string Title
        {
            set
            {
                MWindow.Title = value;
            }
        }

        public void WinMessage(char player, Logik.BrettHelper.GameStates state)
        {
            string title = state.ToString();
            string message = state == Logik.BrettHelper.GameStates.Gewonnen ?
                string.Format("Spieler {0} hat Gewonnen.", player) :
                "Keiner hat Gewonnen, Unentschieden.";
            MessageBox.Show(message, title);
        }

        public void PlayerAusgabe(string message)
        {
            MWindow.lbl_1.Content = message;
        }

        public void ThrowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
