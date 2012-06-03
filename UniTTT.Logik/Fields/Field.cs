using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public abstract class Field
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
                    ret.Add(Row(y));
                }
                for (int x = 0; x < Width; x++)
                {
                    ret.Add(Column(x));
                }
                ret.Add(Diagonal(0));
                ret.Add(Diagonal(1));
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

        public virtual char GetField(int idx)
        {
            throw new NotImplementedException();
        }

        public virtual char GetField(Vector2i vect)
        {
            throw new NotImplementedException();
        }

        public virtual void SetField(int idx, char value)
        {

        }

        public virtual void SetField(Vector2i vect, char value)
        {
        }

        public virtual bool IsFieldEmpty(Vector2i vect)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsFieldEmpty(int idx)
        {
            throw new NotImplementedException();
        }

        public FieldRegion Row(int count)
        {
            FieldRegion ret = new FieldRegion();

            for (int y = 0; y < Height; y++)
            {
                ret.Add(count * Width + y, GetField(new Vector2i(count, y)));
            }
            return ret;
        }

        public FieldRegion Column(int count)
        {
            FieldRegion ret = new FieldRegion();
            for (int x = 0; x < Width; x++)
            {
                ret.Add(x * Width + count, GetField(new Vector2i(x, count)));
            }
            return ret;
        }

        public FieldRegion Diagonal(int count)
        {
            FieldRegion ret = new FieldRegion();
            if (count == 0)
            {
                int y = 0;
                for (int x = 0; x < Width; x++)
                {
                    ret.Add(x * Width + y, GetField(new Vector2i(x, y)));
                    y++;
                }
            }
            else
            {
                int y = 0;
                for (int x = Width; x > 0; x--)
                {
                    ret.Add(x * Width + y, GetField(new Vector2i(x - 1, y - 1)));
                    y++;
                }
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
    }
}