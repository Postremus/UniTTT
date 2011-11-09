using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.XNA
{
    class OutputDarsteller : Logik.IOutputDarsteller
    {
        public string Title { set { } }
        public void WinMessage(char player, Logik.BrettHelper.GameStates state) { }
        public void PlayerAusgabe(string message) { }
        public void ThrowMessage(string message) { }
    }
}
