using System;
using System.Collections.Generic;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public class Brett : IField
    {
        #region Methods
        // Konstruktor
        public Brett(int width, int height)
        {
            Initialize(width, height);
        }

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

        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Length { get { return Width * Height; } }
        private char[,] VarField;

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
            return VarField[vect.X, vect.Y];
        }

        public void SetField(int idx, char value)
        {
            SetField(Vector2i.IndexToVector(idx, Width, Height), value);
        }

        public void SetField(Vector2i vect, char value)
        {
            char[,] x = (char[,])VarField;
            x[vect.X, vect.Y] = value;
            VarField = x;
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
    }
}