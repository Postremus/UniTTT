using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
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
