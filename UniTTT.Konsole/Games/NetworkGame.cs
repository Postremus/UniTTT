using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using UniTTT.Logik;

namespace UniTTT.Konsole.Games
{
    public class NetworkGame : UniTTT.Logik.Game.NetworkGame
    {
        public NetworkGame(int width, int height, Logik.Player.AbstractPlayer p1, Logik.Fields.IField field, string ip, int port, Logik.Network.Network client)
            : base(p1, new BrettDarsteller(width, height), field, ip, port, client)
        {
            WinMessageEvent += WinMessage;
            PlayerOutputEvent += PlayerOutput;
            WindowTitleChangeEvent += TitleChange;
            GetStringEvent += Console.ReadLine;
            ShowMessageEvent += Console.WriteLine;
            GetIntEvent += GetInt;
            Initialize();
        }

        public void Run()
        {
            do
            {
                if (!HasStoped)
                {
                    base.LogikLoop();
                }
                OnWinMessageEvent(Player1.Symbol, UniTTT.Logik.FieldHelper.GetGameState(Field, Player, Player1));
                if (Player1.Symbol == 'X')
                {
                    if (NewGameQuestion())
                    {
                        OnNewGameRequestedEvent();
                    }
                }
                else
                {
                    Console.WriteLine("Warte auf neues Spiel..");
                    while (!HasStarted) { };
                }
            } while (true);
        }

        // Fragen, ob eine neue Partie gespielt werden soll
        private bool NewGameQuestion()
        {
            Console.WriteLine("Wollen Sie eine neue Partie spielen? (J/N)");
            return Console.ReadLine().Trim().ToUpper(CultureInfo.CurrentCulture) == "J";
        }

        private void WinMessage(char player, Logik.FieldHelper.GameStates state)
        {
            if (state == UniTTT.Logik.FieldHelper.GameStates.Gewonnen)
                Console.WriteLine("Spieler {0} hat Gewonnen", player);
            else
                Console.WriteLine(state);
        }

        private void PlayerOutput(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        private void TitleChange(string title)
        {
            Console.Title = title;
        }

        private int GetInt()
        {
            int ret;
            int.TryParse(Console.ReadLine(), out ret);
            return ret;
        }
    }
}