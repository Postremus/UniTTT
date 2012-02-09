using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using UniTTT.Logik;

namespace UniTTT.Konsole.Games
{
    public class NetworkGame : UniTTT.Logik.NetworkGame
    {
        public NetworkGame(int width, int height, Logik.Player.AbstractPlayer p1, Logik.Fields.IField field, string ip, int port) : base(p1, new BrettDarsteller(width, height), new OutputDarsteller(), field, ip, port) { }

        public void Run()
        {
            base.LogikLoop();
            AfterGameActions();
        }

        private void AfterGameActions()
        {
            if (IsODarstellerValid())
            {
                ODarsteller.WinMessage(Player.Symbol, UniTTT.Logik.FieldHelper.GetGameState(Field, Player1.Symbol));
                WinCounter();
            }
            if (NewGameQuestion())
            {
                NewGame();
                Run();
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
    }
}