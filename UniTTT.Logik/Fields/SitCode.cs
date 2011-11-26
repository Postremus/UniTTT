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
        private string VarField;

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            Initialize();
        }

        public void Initialize()
        {
            VarField = SitCodeHelper.Lerrsetzen(Length);
        }

        public char GetField(int idx)
        {
            return SitCodeHelper.ToPlayer(VarField[idx]);
        }

        public char GetField(Vector2i vect)
        {
            return GetField(Vector2i.VectorToIndex(vect));
        }

        public void SetField(int idx, char value)
        {
            VarField = VarField.Remove(idx, 1).Insert(idx, SitCodeHelper.PlayertoSitCode(value).ToString());
        }

        public void SetField(Vector2i vect, char value)
        {
            SetField(Vector2i.VectorToIndex(vect), value);
        }

        public bool IsFieldEmpty(Vector2i vect)
        {
            return IsFieldEmpty(Vector2i.VectorToIndex(vect));
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
    }
} 
