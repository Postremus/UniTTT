using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public interface IOutputDarsteller
    {
        string Title { set; }
        void WinMessage(char player, BrettHelper.GameStates state);
        void PlayerAusgabe(string message);
        void ThrowMessage(string message);
    }
}
