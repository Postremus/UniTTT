using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniTTT.WinForms
{
    class OutputDarsteller : Logik.IOutputDarsteller
    {
        public OutputDarsteller(ref Form1 f)
        {
            form1 = f;
        }

        Form1 form1 { get; set; }

        public string Title
        {
            set
            {
                form1.Name = value;
            }
        }

        public void WinMessage(char player, Logik.FieldHelper.GameStates state)
        {
            string title = state.ToString();
            if (state == Logik.FieldHelper.GameStates.Gewonnen)
                MessageBox.Show(string.Format("Spieler {0} hat Gewonnen", player), title);
            else
                MessageBox.Show("Keiner hat Gewonnen, Unentschieden.", title);
        }

        public void PlayerAusgabe(string message)
        {

        }

        public void ShowMessage(string message)
        {
        }

        public void Wait()
        {
            MessageBox.Show("");
        }

        public int Choice()
        {
            return 0;
        }

        public bool Choice(string answerTrue, string answerFalse)
        {
            return true;
        }
    }
}
