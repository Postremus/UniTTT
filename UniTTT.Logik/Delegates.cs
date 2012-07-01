using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public delegate void WindowTitleChangeHandler(string title);
    public delegate void PlayerOutputHandler(string message);
    public delegate void WinMessageHandler(char symbol, GameStates gameState);
    public delegate void ShowMessageHandler(string message);
    public delegate string GetStringHandler();
    public delegate int GetIntHandler();
}