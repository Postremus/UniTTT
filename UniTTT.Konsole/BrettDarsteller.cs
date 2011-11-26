using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Konsole
{
    class BrettDarsteller : Logik.IBrettDarsteller
    {
        public BrettDarsteller(int width, int height)
        {
            Width = width;
            Height = height;
            spielfeld = new char[Width + (Width - 1), Height + (Height - 1)];
        }

        #region Fields
        public int Width { get; private set; }
        public int Height { get; private set; }
        private char[,] spielfeld;
        #endregion

        public void Update(Logik.Fields.IField Field)
        {
            int i = 0, f = 0;

            for (int x = 0; x < Width + (Width - 1); x++)
            {
                // Die ZwischenReihen 
                if (x % 2 == 1)
                    for (int c = 0; c < Height + (Height - 1); c++)
                        spielfeld[x, c] = c % 2 == 1 ? '+' : '|';
                // Die Hauptreihen
                else if (x % 2 == 0)
                {
                    for (int a = 0; a < Height + (Height - 1); a++)
                    {
                        spielfeld[x, a] = a % 2 == 1 ? '-' : Field.GetField(new Logik.Vector2i(i, f));
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
