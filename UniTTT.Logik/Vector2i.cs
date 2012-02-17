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

        private static Vector2i GetVectorOfString(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                throw new NullReferenceException();
            Vector2i ret = null;
            if (value.Length > 2)
            {
                int idx = value.IndexOf('.') != -1 ? value.IndexOf('.') : value.IndexOf(',');
                if (idx == -1)
                {
                    idx = value.IndexOf(',');
                }
                if (idx > -1)
                {
                    int x;
                    int y;
                    if (int.TryParse(value.Substring(0, idx), out x) && int.TryParse(value.Substring(idx + 1), out y))
                    {
                        ret = new Vector2i(x, y);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
            }
            else
            {
                int count = 0;
                string str = null;
                if (value.Contains(".") || value.Contains(","))
                {
                    str = value.Substring(0, value.Length - 1);
                }
                else
                {
                    str = value.Substring(0, value.Length);
                }
                if (int.TryParse(str, out count))
                {
                    if (count < 0)
                    {
                        throw new NullReferenceException();
                    }
                    ret = new Vector2i(count, count);
                }
                else
                {
                    throw new FormatException();
                }
            }
            return ret;
        }

        public static Vector2i IndexToVector(int zug, int width, int height)
        {
            Vector2i vect = null;
            if (zug < 0)
            {
                throw new NullReferenceException();
            }
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

        public static int VectorToIndex(Vector2i vect, int width)
        {
            return ((vect.X) * width) + (vect.Y + 1) - 1;
        }

        public static Vector2i StringToVector(string value, bool containsCoordinate)
        {
            if (containsCoordinate == true)
            {
                if (value.Contains(' '))
                    value = value.Trim();
                Vector2i vect = null;
                string sub = value.Substring(value.IndexOf(':')+1, value.IndexOf('|') - (value.IndexOf(':')+1));
                int x = int.Parse(sub);
                value = value.Remove(0, value.IndexOf('|'));
                sub = value.Substring(value.LastIndexOf(':')+1, value.Length - (value.LastIndexOf(':')+1));
                int y = int.Parse(sub);
                vect = new Vector2i(x, y);
                return vect;
            }
            else
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException();
                Vector2i ret = null;
                if (value.Length > 2)
                {
                    int idx = value.IndexOf('.') != -1 ? value.IndexOf('.') : value.IndexOf(',');
                    if (idx == -1)
                    {
                        idx = value.IndexOf(',');
                    }
                    if (idx > -1)
                    {
                        int x;
                        int y;
                        if (int.TryParse(value.Substring(0, idx), out x) && int.TryParse(value.Substring(idx + 1), out y))
                        {
                            ret = new Vector2i(x, y);
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                }
                else
                {
                    int count = 0;
                    string str = null;
                    if (value.Contains(".") || value.Contains(","))
                    {
                        str = value.Substring(0, value.Length - 1);
                    }
                    else
                    {
                        str = value.Substring(0, value.Length);
                    }
                    if (int.TryParse(str, out count))
                    {
                        if (count < 0)
                        {
                            throw new NullReferenceException();
                        }
                        ret = new Vector2i(count, count);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                return ret;
            }
        }

        public static Vector2i StringToVector(string str, char seperator, bool containsCoordinate)
        {
            return StringToVector(str.Replace(seperator, '|'), containsCoordinate);
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