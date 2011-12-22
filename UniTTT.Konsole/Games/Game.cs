﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using UniTTT.Logik;

namespace UniTTT.Konsole.Games
{
    class Game : Logik.NormalGame
    {
        public Game(int width, int height, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2) : base(p1, p2, new BrettDarsteller(width, height), new OutputDarsteller(), null) { }

        public void Start()
        {
            LogikLoop();
            AfterGameActions();
        }

        private void AfterGameActions()
        {
            if (IsODarstellerValid())
            {
                ODarsteller.WinMessage(Player.Symbol, UniTTT.Logik.FieldHelper.GetGameState(Field, Player.Symbol));
                WinCounter();
            }
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
    }
}
