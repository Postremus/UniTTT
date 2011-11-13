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

        public static Vector2i GetVectorOfString(string value)
        {
            int idx = value.IndexOf('.') != -1 ? value.IndexOf('.') : value.IndexOf(',');
            if (idx == -1)
            {
                idx = value.IndexOf(',');
            }
            int x;
            int y;
            int.TryParse(value.Substring(0, idx-1), out x);
            int.TryParse(value.Substring(idx + 1), out y);
            return new Vector2i(x, y);
        }
    }
}
