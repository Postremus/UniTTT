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
        private int spielFeldWidth;
        private int spielFeldHeight;
        #endregion

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            spielFeldWidth = Width + (Width - 1);
            spielFeldHeight = Height + (Height - 1);
            spielfeld = new char[spielFeldWidth, spielFeldHeight];
        }

        public void Update(Logik.Fields.IField field)
        {
            int realX= 0;
            int realY = 0;
            for (int Spielfeldy = 0; Spielfeldy < spielFeldHeight; Spielfeldy++)
            {
                //Hauptreihen
                if (Spielfeldy % 2 == 0)
                {
                    for (int Spielfeldx = 0; Spielfeldx < spielFeldWidth; Spielfeldx++)
                    {
                        if (Spielfeldx % 2 == 0)
                        {
                            spielfeld[Spielfeldx, Spielfeldy] = field.GetField(new Logik.Vector2i(realX, realY));
                            realX++;
                        }
                        else
                        {
                            spielfeld[Spielfeldx, Spielfeldy] = '|';
                        }
                    }
                    realY++;
                    realX = 0;
                }
                //Zwischenreihen
                else
                {
                    for (int Spielfeldx = 0; Spielfeldx < spielFeldWidth; Spielfeldx++)
                    {
                        if (Spielfeldx % 2 == 0)
                        {
                            spielfeld[Spielfeldx, Spielfeldy] = '-';
                        }
                        else
                        {
                            spielfeld[Spielfeldx, Spielfeldy] = '+';
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            Console.Clear();
            string tooutput = null;
            for (int y = 0; y < spielFeldHeight; y++)
            {
                for (int x = 0; x < spielFeldWidth; x++)
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