using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public class SitCode : IField
    {
        public SitCode(int width, int height)
        {
            Initialize(width, height);
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get { return Width * Height; } }
        private string VarField { get; set; }

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

        public static SitCode GetInstance(string sitcode, int width, int height)
        {
            SitCode ret = new SitCode(width, height);
            ret.VarField = sitcode;
            return ret;
        }

        private bool IsEntryPointInTheSize(int idx)
        {
            return idx > -1 && idx < Length;
        }

        public override string ToString()
        {
            return FieldHelper.Calculate(this);
        }
    }
} 
