using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace UniTTT.Konsole.Games
{
    class Game : Logik.AbstractGame
    {
        public Game(int b, int h, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2) : base(b, h, p1, p2, new BrettDarsteller(b, h))
        {
            Console.Title = "TicTacToe - " + this.ToString();
        }

        public void Start()
        {
            do
            {
                PlayerChange();
                brett.Setzen(player.Spieler, player.Spiele(brett));
                Console.Clear();
                darsteller.Update(brett.VarBrett);
                darsteller.Draw();
            } while (!HasEnd());

            AfterGameActions();
        }

        private void AfterGameActions()
        {
            Logik.Brett.GameStates state = brett.GetGameState(brett.VarBrett, player.Spieler);

            if (state == UniTTT.Logik.Brett.GameStates.Gewonnen)
            {
                player1.WinCounter = player == player1 ? +1 : 0;
                player2.WinCounter = player == player2 ? +1 : 0;
                Console.WriteLine("Spieler {0} hat {1}", player.Spieler, state);
            }

            if (NeuesSpielFrage())
            {
                NewGame();
                Console.Clear();
                Start();
            }
            else
            {
                Console.WriteLine("Das Spiel ist beendet.");
                Console.ReadLine();
            }
        }

        // Fragen, ob eine neue Partie gespielt werden soll
        private bool NeuesSpielFrage()
        {
            Console.WriteLine("Wollen Sie eine neue Partie spielen? (J/N)");
            return Console.ReadLine().ToUpper(CultureInfo.CurrentCulture).Trim() == "J";
        }

        private bool HasEnd()
        {
            if (brett.GetGameState(brett.VarBrett, player.Spieler) != UniTTT.Logik.Brett.GameStates.Laufend)
                return true;
            return false;
        }

        public override string ToString()
        {
            return (player1 is Logik.Player.KIPlayer) && (player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
    }
}
