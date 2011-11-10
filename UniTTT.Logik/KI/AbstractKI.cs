using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class AbstractKI
    {
        protected AbstractKI(char spieler, int b, int h)
        {
            kiplayer = spieler;
            Breite = b;
            Hoehe = h;
            pruefer = new GewinnPruefer(Hoehe < Breite ? Hoehe : Breite);
            Rnd = new Random();
        }

        public virtual void Learn()
        {
            throw new NotImplementedException();
        }

        public virtual int Play(char[,] brett, char spieler)
        {
            throw new NotImplementedException();
        }

        public int Breite { get; protected set; }
        public int Hoehe { get; protected set; }
        public int FelderAnzahl { get { return Breite * Hoehe; } }
        public GewinnPruefer pruefer { get; protected set; }
        public char kiplayer { get; private set; }
        public Random Rnd { get; private set; }

        public override string ToString()
        {
            return "KI";
        }

        protected char PlayerChange(char spieler)
        {
            return spieler == '2' ? '3' : '2';
        }

        public int GetRandomZug(string sitcode)
        {
            int zug = -1;
            do
            {
                zug = Rnd.Next(0, 9);
            } while (sitcode[zug] != '1');
            return zug;
        }

        protected static class SitCodeHelper
        {
            public static string Berechnen(char[,] brett)
            {
                string mom_sit_code = null;
                char chr;
                foreach (char x in brett)
                {
                    chr = x == 'X' ? '2' : x == 'O' ? '3' : '1';
                    mom_sit_code += chr;
                }
                return mom_sit_code;
            }

            public static char PlayertoSitCode(char spieler)
            {
                return spieler == 'X' ? '2' : '3';
            }

            public static char ToPlayer(char spieler)
            {
                return spieler == '2' ? 'X' : 'O';
            }

            public static char[,] ToBrett(string sitCode, int breite, int hoehe)
            {
                char[,] brett = new char[breite, hoehe];
                for (int x = 0; x < breite; x++)
                    for (int y = 0; y < hoehe; y++)
                        brett[x, y] = sitCode[((x) * 3) + (y + 1) - 1] == '1' ? ' ' : sitCode[((x) * 3) + (y + 1) - 1];
                return brett;
            }

            public static string Lerrsetzen(int felderAnzahl)
            {
                string sit_code = string.Empty;
                while (sit_code.Length < felderAnzahl)
                    sit_code += "1";
                return sit_code;
            }
        }
    }
}