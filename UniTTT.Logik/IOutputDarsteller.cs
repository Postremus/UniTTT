using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{

    public interface IOutputDarsteller
    {
        string Title { set; }
        void WinMessage(char player, FieldHelper.GameStates state);
        void PlayerAusgabe(string message);
        void ShowMessage(string message);
        void Wait();
        int Choice();
        bool Choice(string answerTrue, string answerFalse);
        void Clear();
    }
}