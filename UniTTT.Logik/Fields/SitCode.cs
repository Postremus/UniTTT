using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    [Serializable()]
    public class SitCode : IField
    {

        #region Privates
        private int _width;
        private int _height;
        private string VarField { get; set; }
        #endregion

        #region Interface Propertys
        public int Width 
        {
            get
            {
                return _width;
            }
            private set
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
        #endregion

        #region Constructor
        public SitCode(int width, int height)
        {
            Initialize(width, height);
        }
        #endregion

        #region interface Methods
        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            Initialize();
        }

        public void Initialize()
        {
            VarField = SitCodeHelper.SetEmpty(Length);
        }

        public char GetField(int idx)
        {
            if (IsEntryPointInTheSize(idx))
            {
                return SitCodeHelper.ToPlayer(VarField[idx]);
            }
            else
            {
                return 'n';
            }
        }

        public char GetField(Vector2i vect)
        {
            return GetField(Vector2i.VectorToIndex(vect, Width));
        }

        public void SetField(int idx, char value)
        {
            if (IsEntryPointInTheSize(idx))
            {
                VarField = VarField.Remove(idx, 1).Insert(idx, SitCodeHelper.PlayertoSitCode(value).ToString());
            }
        }

        public void SetField(Vector2i vect, char value)
        {
            SetField(Vector2i.VectorToIndex(vect, Width), value);
        }

        public bool IsFieldEmpty(Vector2i vect)
        {
            return IsFieldEmpty(Vector2i.VectorToIndex(vect, Width));
        }

        public bool IsFieldEmpty(int idx)
        {
            return VarField[idx] == '1';
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">0 = Oben Links -> unten Rechts; 1 = Oben Rechts -> unten Links</param>
        /// <returns></returns>
        public FieldRegion Diagonal(int count)
        {
            FieldRegion ret = new FieldRegion();
            if (count == 0)
            {
                int y = 0;
                for (int x = 0; x < Width; x++)
                {
                    ret.Add(x *Width + y, GetField(new Vector2i(x, y)));
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
        #endregion

        public static SitCode GetInstance(string sitcode, int width, int height)
        {
            SitCode ret = new SitCode(width, height);
            ret.VarField = sitcode;
            return ret;
        }

        #region Methods
        private bool IsEntryPointInTheSize(int idx)
        {
            return idx > -1 && idx < Length;
        }

        public override string ToString()
        {
            return FieldHelper.Calculate(this);
        }
        #endregion
    }
} 
