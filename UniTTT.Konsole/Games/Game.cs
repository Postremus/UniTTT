﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace UniTTT.Konsole.Games
{
    class Game : Logik.NormalGame
    {
        public Game(int b, int h, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2) : base(b, h, p1, p2, new BrettDarsteller(b, h), new OutputDarsteller()) { }

        public void Start()
        {
            do
            {
                Logik();
            } while (!HasEnd());

            AfterGameActions();
        }

        private void AfterGameActions()
        {
            ODarsteller.WinMessage(player.Spieler, brett.GetGameState(brett.VarBrett, player.Spieler));
            WinCounter();

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
            return Console.ReadLine().Trim().ToUpper(CultureInfo.CurrentCulture) == "J";
        }
    }
}
