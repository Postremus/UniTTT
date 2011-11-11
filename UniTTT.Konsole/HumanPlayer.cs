﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT;

namespace UniTTT.Konsole
{
    class HumanPlayer : Logik.Player.AbstractPlayer
    {
        public HumanPlayer(char startspieler) : base(startspieler) { }

        public override Logik.Vector2i Spiele(Logik.Brett brett)
        {
            Logik.Vector2i ret;
            do
            {
                ret = abfragexy();
                if (brett.IsFieldEmpty(ret))
                    return ret;
                else
                {
                    Console.WriteLine("Feld bereits besetzt. (Taste drücken)");
                    Console.ReadLine();

                }
            } while (true);
        }

        //Zeile und Spalte abfragen.
        private Logik.Vector2i abfragexy()
        {
            Console.WriteLine("In welcher Zeile und Spalte soll das {0} gesetzt werden? (z.B. 0.0)", Spieler);
            return Logik.Vector2i.GetVectorOfString(Console.ReadLine());
        }

        public override string ToString()
        {
            return "Human";
        }
    }
}