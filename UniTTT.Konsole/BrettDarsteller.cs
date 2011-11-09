using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Konsole
{
    class BrettDarsteller : Logik.IBrettDarsteller
    {
        public BrettDarsteller(int breite, int hoehe)
        {
            Breite = breite;
            Hoehe = hoehe;
            spielfeld = new char[Breite + (Breite - 1), Hoehe + (Hoehe - 1)];
        }

        #region Fields
        public int Breite { get; private set; }
        public int Hoehe { get; private set; }
        private char[,] spielfeld;
        #endregion

        public void Update(char[,] brett)
        {
            int i = 0, f = 0;

            for (int x = 0; x < Breite + (Breite - 1); x++)
            {
                // Die ZwischenReihen 
                if (x % 2 == 1)
                    for (int c = 0; c < Hoehe + (Hoehe - 1); c++)
                        spielfeld[x, c] = c % 2 == 1 ? '+' : '|';
                // Die Hauptreihen
                else if (x % 2 == 0)
                {
                    for (int a = 0; a < Hoehe + (Hoehe - 1); a++)
                    {
                        spielfeld[x, a] = a % 2 == 1 ? '-' : brett[i, f];
                        if (a % 2 == 0)
                            f++;
                    }
                    i++;
                    f = 0;
                }
            }
        }

        public void Draw()
        {
            Console.Clear();
            for (int y = 0; y < spielfeld.GetUpperBound(1)+1; y++)
            {
                for (int x = 0; x < spielfeld.GetUpperBound(0)+1; x++)
                    Console.Write(spielfeld[x, y]);
                Console.Write(System.Environment.NewLine);
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
