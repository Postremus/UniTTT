using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class Vector2i
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2i operator +(Vector2i vect1, Vector2i vect2)
        {
            int x = vect1.X + vect2.X;
            int y = vect1.Y + vect2.Y;
            return new Vector2i(x, y);
        }

        public static Vector2i operator -(Vector2i vect1, Vector2i vect2)
        {
            int x = vect1.X - vect2.X;
            int y = vect1.Y - vect2.Y;
            return new Vector2i(x, y);
        }

        public static Vector2i GetVectorOfString(string value)
        {
            int idx = value.IndexOf('.') != -1 ? value.IndexOf('.') : value.IndexOf(',');
            if (idx == -1)
            {
                idx = value.IndexOf(',');
            }
            int x;
            int y;
            int.TryParse(value.Substring(0, idx), out x);
            int.TryParse(value.Substring(idx +1), out y);
            return new Vector2i(x, y);
        }

        public static Vector2i IndexToVector(int zug, int width, int height)
        {
            Vector2i vect = null;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (((x) * width) + (y + 1) - 1 == zug)
                        vect = new Vector2i(x, y);
                }
            }
            return vect;
        }

        public static int VectorToIndex(Vector2i vect)
        {
            return ((vect.X) * 3) + (vect.Y + 1) - 1;
        }
    }
}
