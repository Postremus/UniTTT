using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using UniTTT.Logik;

namespace UniTTT.Konsole.Games
{
    class Game : Logik.Game.NormalGame
    {
        public Game(int width, int height, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.Fields.IField field)
            : base(p1, p2, new BrettDarsteller(width, height), field)
        {
            WinMessageEvent += WinMessage;
            PlayerOutputEvent += PlayerOutput;
            WindowTitleChangeEvent += TitleChange;
            Initialize();
        }

        public void Start()
        {
            LogikLoop();
            AfterGameActions();
        }

        private void AfterGameActions()
        {
            OnWinMessageEvent(Player1.Symbol, UniTTT.Logik.FieldHelper.GetGameState(Field, Player, Player1));
            WinCounter();
            if (NewGameQuestion())
            {
                NewGame();
                Start();
            }
            else
            {
                Console.WriteLine("Das Spiel ist beendet.");
                Console.ReadLine();
            }
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

        public void PlayerOutput(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        public void TitleChange(string title)
        {
            Console.Title = title;
        }
    }
}
