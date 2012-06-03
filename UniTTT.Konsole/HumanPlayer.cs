using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT;

namespace UniTTT.Konsole
{
    class HumanPlayer : Logik.Player.Player
    {
        public HumanPlayer(char startPlayer) : base(startPlayer) { }

        public override Logik.Vector2i Play(Logik.Fields.Field brett)
        {
            Logik.Vector2i ret;
            do
            {
                ret = abfragexy();
                if (brett.IsFieldEmpty(ret))
                    return ret;
                else
                {
                    Console.WriteLine("Feld bereits besetzt.");
                }
            } while (true);
        }

        public override void Learn()
        {
            Console.WriteLine("Brettspiel");
            Console.WriteLine("3 in einer Reihe zum Gewinnen.");
            Console.WriteLine("Abwechselnd gespielt.");
            Console.WriteLine("Spieler werden durch Symbole dargestellt");
            Console.WriteLine("Meistens X und O");
            Console.WriteLine("Taste drücken zum Beenden");
            Console.ReadLine();
        }

        //Zeile und Spalte abfragen.
        private Logik.Vector2i abfragexy()
        {
            do
            {
                try
                {
                    Console.WriteLine("In welcher Nullbasierenden Zeile und Spalte, soll das {0} gesetzt werden? (X.Y)", Symbol);
                    return Logik.Vector2i.StringToVector(Console.ReadLine(), false, '.');
                }
                catch
                {
                    Console.WriteLine("Irgendetwas wurde falsch eingegeben..");
                }
            } while (true);
        }

        public override string ToString()
        {
            return "Human";
        }
    }
}