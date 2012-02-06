using System;
using System.Collections.Generic;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public class Brett : IField
    {
        #region Privates
        private int _width;
        private int _height;
        private char[,] VarField;
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
        public Brett(int width, int height)
        {
            Initialize(width, height);
        }
        #endregion

        #region Interface Methods

        public void Initialize(int width, int height)
        {
            Height = height;
            Width = width;
            Initialize();
        }

        public void Initialize()
        {
            VarField = new char[Width, Height];
            BrettInit();
        }

        private void BrettInit()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    VarField[x, y] = ' ';
        }

        public char GetField(int idx)
        {
            return GetField(Vector2i.IndexToVector(idx, Width, Height));
        }

        public char GetField(Vector2i vect)
        {
            if (IsEntryPointInTheSize(vect))
            {
                return VarField[vect.X, vect.Y];
            }
            else
            {
                return 'n';
            }
        }

        public void SetField(int idx, char value)
        {
            SetField(Vector2i.IndexToVector(idx, Width, Height), value);
        }

        public void SetField(Vector2i vect, char value)
        {
            if (IsEntryPointInTheSize(vect))
            {
                VarField[vect.X, vect.Y] = value;
            }
        }

        public bool IsFieldEmpty(Vector2i vect)
        {
            return GetField(vect) == ' ';
        }

        public bool IsFieldEmpty(int idx)
        {
            return GetField(idx) == ' ';
        }
        #endregion

        #region Methods
        private bool IsEntryPointInTheSize(Vector2i vect)
        {
            return vect > -1 && vect.X < Width && vect.Y < Height;
        }

        public override string ToString()
        {
            return FieldHelper.Calculate(this);
        }
        #endregion
    }
}