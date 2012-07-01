using System;
using System.Collections.Generic;
using System.Text;

namespace UniTTT.Logik.Fields
{
    [Serializable()]
    public class Brett : Field
    {
        #region Privates
        private char[,] VarField;
        #endregion

        #region Constructor
        public Brett(int width, int height)
        {
            Initialize(width, height);
        }
        #endregion

        #region Interface Methods

        public override void Initialize()
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

        public override char GetField(int idx)
        {
            return GetField(Vector2i.IndexToVector(idx, Width, Height));
        }

        public override char GetField(Vector2i vect)
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

        public override void SetField(int idx, char value)
        {
            SetField(Vector2i.IndexToVector(idx, Width, Height), value);
        }

        public override void SetField(Vector2i vect, char value)
        {
            if (IsEntryPointInTheSize(vect))
            {
                VarField[vect.X, vect.Y] = value;
            }
        }

        public override bool IsFieldEmpty(Vector2i vect)
        {
            return GetField(vect) == ' ';
        }

        public override bool IsFieldEmpty(int idx)
        {
            return GetField(idx) == ' ';
        }
        #endregion
    }
}