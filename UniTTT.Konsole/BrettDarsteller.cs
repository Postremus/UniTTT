using System;

namespace UniTTT.Konsole
{
    class BrettDarsteller : Logik.IBrettDarsteller
    {
        #region Constructor
        public BrettDarsteller(int width, int height)
        {
            Initialize(width, height);
        }
        #endregion

        #region Fields
        public int Width { get; private set; }
        public int Height { get; private set; }
        private char[,] spielfeld;
        #endregion

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            spielfeld = new char[Width + (Width - 1), Height + (Height - 1)];
        }

        public void Update(Logik.Fields.IField field)
        {
            int x = 0;
            int y = 0;
            for (int Spielfeldy = 0; Spielfeldy < Height + (Height-1); Spielfeldy++)
            {
                //Hauptreihen
                if (Spielfeldy % 2 == 0)
                {
                    for (int Spielfeldx = 0; Spielfeldx < Width + (Width - 1); Spielfeldx++)
                    {
                        if (Spielfeldx % 2 == 0)
                        {
                            spielfeld[Spielfeldx, Spielfeldy] = field.GetField(new Logik.Vector2i(x, y));
                            x++;
                        }
                        else
                        {
                            spielfeld[Spielfeldx, Spielfeldy] = '|';
                        }
                    }
                    y++;
                    x = 0;
                }
                //Zwischenreihen
                else
                {
                    for (int Spielfeldx = 0; Spielfeldx < Width + (Width - 1); Spielfeldx++)
                    {
                        spielfeld[Spielfeldx, Spielfeldy] = Spielfeldx % 2 == 0 ? '-' : '+';
                    }
                }
            }
        }

        public void Draw()
        {
            Console.Clear();
            string tooutput = null;
            for (int y = 0; y < Height+(Height-1); y++)
            {
                for (int x = 0; x < Width + (Width - 1); x++)
                {
                    tooutput += spielfeld[x, y];
                }
                Console.WriteLine(tooutput);
                tooutput = null;
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}