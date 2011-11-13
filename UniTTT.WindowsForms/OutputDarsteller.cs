using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniTTT.WindowsForms
{
    class OutputDarsteller : Logik.IOutputDarsteller
    {
        public OutputDarsteller(ref Form1 form1)
        {
            F1 = form1;
        }

        private Form1 F1;

        public string Title
        {
            set 
            {
                F1.Text = value;
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
            System.Windows.Forms.MessageBox.Show(message);
        }

        public void ThrowMessage(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
