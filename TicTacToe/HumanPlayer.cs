using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe.Konsole
{
    class HumanPlayer : Logik.Player.AbstractPlayer
    {
        public HumanPlayer(char startspieler) : base(startspieler) { }

        public override Logik.Brett Spiele(Logik.Brett brett)
        {
            int spalte = abfragexy("Spalte", brett);
            Console.WriteLine();
            int zeile = abfragexy("Zeile", brett);

            if (brett.IsFieldEmpty(zeile, spalte))
                brett.Setzen(Spieler, new Logik.Vector(spalte, zeile));
            else
            {
                Console.WriteLine("Feld bereits besetzt. (Taste drücken)");
                Console.ReadLine();
                Spiele(brett);
            }
            return brett;
        }

        //Zeile und Spalte abfragen.
        private int abfragexy(string bezeichner, Logik.Brett b)
        {
            int position = 0;

            Console.WriteLine("In welcher {0} soll das {1} gesetzt werden? (1 - {2})", bezeichner, Spieler, b.Breite > b.Hoehe ? b.Breite : b.Hoehe);
            position = Convert.ToInt32(Console.ReadLine(), System.Globalization.CultureInfo.CurrentCulture) - 1;
            if ((position > -1) && (position < (b.Hoehe | b.Breite)))
                return position;
            else
            {
                Console.WriteLine("Fehlerhafte Eingabe ({0}).",
                   position < 0 ? "Eingabe zu klein" : position > (b.Breite | b.Hoehe) ? "Eingabe zu Groß" : "Unbekannter Fehler.");
                return abfragexy(bezeichner, b);
            }
        }

        public override string ToString()
        {
            return "Human";
        }
    }
}