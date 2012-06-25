using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public abstract class Field : IEnumerable<FieldPlaceData>
    {
        private int _width;
        private int _height;

        public int Width 
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        public int Height 
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        public int Length { get { return Width * Height; } }
        public List<FieldRegion> Panels
        {
            get
            {
                List<FieldRegion> ret = new List<FieldRegion>();
                for (int y = 0; y < Height; y++)
                {
                    ret.Add(Column(y));
                }
                for (int x = 0; x < Width; x++)
                {
                    ret.Add(Row(x));
                    ret.Add(LeftTopToRightButtom(x));
                    ret.Add(RightTopToLeftButtom(x));
                }
                return ret;
            }
        }

        public virtual void Initialize(int width, int height)
        {
            Height = height;
            Width = width;
            Initialize();
        }

        public virtual void Initialize()
        {
        }

        public abstract char GetField(int idx);

        public abstract char GetField(Vector2i vect);

        public abstract void SetField(int idx, char value);

        public abstract void SetField(Vector2i vect, char value);

        public abstract bool IsFieldEmpty(Vector2i vect);

        public abstract bool IsFieldEmpty(int idx);

        public FieldRegion Row(int x)
        {
            FieldRegion ret = new FieldRegion();

            for (int y = 0; y < Height; y++)
            {
                ret.Add(x * Width + y, GetField(new Vector2i(x, y)));
            }
            return ret;
        }

        public FieldRegion Column(int y)
        {
            FieldRegion ret = new FieldRegion();
            for (int x = 0; x < Width; x++)
            {
                ret.Add(x * Width + y, GetField(new Vector2i(x, y)));
            }
            return ret;
        }

        public FieldRegion LeftTopToRightButtom(int x)
        {
            FieldRegion ret = new FieldRegion();
            int y = 0;
            for (int i = 0; i < Height; i++)
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                {
                    break;
                }
                ret.Add(x * Width + y, GetField(new Vector2i(x, y)));
                x++;
                y++;
            }
            return ret;
        }

        public FieldRegion RightTopToLeftButtom(int x)
        {
            FieldRegion ret = new FieldRegion();
            int y = 0;
            for (int i = 0; i < Height; i++)
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                {
                    break;
                }
                ret.Add(x * Width + y, GetField(new Vector2i(x, y)));
                x--;
                y++;
            }
            return ret;
        }

        public bool IsEntryPointInTheSize(int vect)
        {
            return IsEntryPointInTheSize(Vector2i.IndexToVector(vect, Width, Height));
        }

        public bool IsEntryPointInTheSize(Vector2i vect)
        {
            return vect > -1 && vect.X < Width && vect.Y < Height;
        }

        public override string ToString()
        {
            return FieldHelper.GetString(this);
        }

        public IEnumerator<FieldPlaceData> GetEnumerator()
        {
            List<FieldPlaceData> ret = new List<FieldPlaceData>();
            for (int i = 0; i < Length; i++)
            {
                ret.Add(new FieldPlaceData(i, GetField(i)));
            }
            return (IEnumerator<FieldPlaceData>)ret.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            List<FieldPlaceData> ret = new List<FieldPlaceData>();
            for (int i = 0; i < Length; i++)
            {
                ret.Add(new FieldPlaceData(i, GetField(i)));
            }
            return (System.Collections.IEnumerator)ret.GetEnumerator();
        }
    }
}