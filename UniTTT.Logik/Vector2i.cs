using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    [Serializable()]
    public class Vector2i
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Vector2i Zero
        {
            get
            {
                return new Vector2i(0, 0);
            }
        }

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

        public static Vector2i operator *(Vector2i vect1, Vector2i vect2)
        {
            int x = vect1.X * vect2.X;
            int y = vect1.Y * vect2.Y;
            return new Vector2i(x, y);
        }

        public static bool operator <=(Vector2i vect1, Vector2i vect2)
        {
            return vect1.X <= vect2.X && vect1.Y <= vect2.Y;
        }

        public static bool operator <(Vector2i vect1, Vector2i vect2)
        {
            return vect1.X < vect2.X && vect1.Y < vect2.Y;
        }

        public static bool operator <(Vector2i vect1, int vect2)
        {
            return vect1.X < vect2 && vect1.Y < vect2;
        }

        public static bool operator >=(Vector2i vect1, Vector2i vect2)
        {
            return vect1.X >= vect2.X && vect1.Y >= vect2.Y;
        }

        public static bool operator >(Vector2i vect1, Vector2i vect2)
        {
            return vect1.X > vect2.X && vect1.Y > vect2.Y;
        }

        public static bool operator >(Vector2i vect1, int vect2)
        {
            return vect1.X > vect2 && vect1.Y > vect2;
        }

        public static Vector2i IndexToVector(int zug, int width, int height)
        {
            if (zug < 0)
            {
                throw new NullReferenceException();
            }

            Vector2i vect = null;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x * width + y == zug)
                        vect = new Vector2i(x, y);
                }
            }
            return vect;
        }

        public static int VectorToIndex(Vector2i vect, int width)
        {
            return vect.X * width + vect.Y;
        }

        public static Vector2i StringToVector(string value, bool containsCoordinate, char seperator)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value isn't seted.");
            }
            if (containsCoordinate)
            {
                if (value.Contains("X") && value.Contains("Y"))
                {
                    value = value.Remove(0, 1);
                    value = value.Remove(value.IndexOf("Y"), 1);
                }
                if (value.Contains(":"))
                {
                    value = value.Remove(0, 1);
                    value = value.Remove(value.IndexOf(":"), 1);
                }
            }

            if (!value.Contains(seperator))
            {
                int cor;
                if (int.TryParse(value, out cor))
                {
                    return Vector2i.IndexToVector(cor, 3, 3);
                }
            }

            if (value.Contains(seperator))
            {
                string[] splitedValue = value.Split(seperator);
                int x = 0;
                int y = 0;
                if (int.TryParse(splitedValue[0], out x) && int.TryParse(splitedValue[1], out y))
                {
                    return new Vector2i(x, y);
                }
            }
            return null;
        }

        public static Vector2i StringToVector(string value, bool containsCoordinate)
        {
            return StringToVector(value, containsCoordinate, '|');
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X:{0}|Y:{1}", X, Y);
        }
    }
}