using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TicTacToe.Konsole.Games
{
    class Game
    {
        public Logik.Player.AbstractPlayer player { get; private set; }
        public Logik.Player.AbstractPlayer player1 { get; private set; }
        public Logik.Player.AbstractPlayer player2 { get; private set; }
        public Logik.Brett brett { get; private set; }
        public Logik.IBrettDarsteller darsteller { get; private set; }

        public Game(int b, int h, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2)
        {
            brett = new Logik.Brett(b, h);
            player1 = p1;
            player2 = p2;
            darsteller = new BrettDarsteller(b, h);
            Console.Title = "TicTacToe - " + this.ToString();
        }

        public void Start()
        {
            darsteller.Update(brett.VarBrett);
            darsteller.Draw();
            do
            {
                SpielerTausch();
                brett = player.Spiele(brett);
                Console.Clear();
                darsteller.Update(brett.VarBrett);
                darsteller.Draw();
            } while (!HasEnd());

            AfterGameActions();
        }

        private void AfterGameActions()
        {
            Logik.Brett.GameStates state = brett.GetGameState(brett.VarBrett, player.Spieler);

            if (state == Logik.Brett.GameStates.Gewonnen)
            {
                player1.WinCounter = player == player1 ? +1 : 0;
                player2.WinCounter = player == player2 ? +1 : 0;
            }
            Console.WriteLine(state);

            if (NeuesSpielFrage())
            {
                NeuesSpiel();
                Start();
            }
            Console.WriteLine("Das Spiel ist beendet.");
            Console.ReadLine();
        }

        private void NeuesSpiel()
        {
            Console.Clear();
            brett = new Logik.Brett(brett.Breite, brett.Hoehe);
            player = null;
        }

        // Fragen, ob eine neue Partie gespielt werden soll
        bool NeuesSpielFrage()
        {
            Console.WriteLine("Wollen Sie eine neue Partie spielen? (J/N)");
            return Console.ReadLine().ToUpper(CultureInfo.CurrentCulture).Trim() == "J";
        }

        private bool HasEnd()
        {
            if (brett.GetGameState(brett.VarBrett, player.Spieler) != Logik.Brett.GameStates.Laufend)
                return true;
            return false;
        }

        private void SpielerTausch()
        {
            player = player1 == player ? player2 : player1;
        }

        public override string ToString()
        {
            return (player1 is Logik.Player.KIPlayer) && (player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
    }
}
