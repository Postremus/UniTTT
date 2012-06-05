using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;

namespace UniTTT.Plugin.FourConnect
{
    public class FourConnect : Logik.Fields.Field, Logik.Plugin.IPlugin
    {
        private char[,] _varField;

        public string PluginName
        {
            get { return "4connect"; }
        }

        public Logik.Plugin.PluginTypes PluginType
        {
            get { return Logik.Plugin.PluginTypes.Field; }
        }

        public FourConnect()
        {
            Width = 7;
            Height = 6;
            _varField = InitBrett();
        }

        private char[,] InitBrett()
        {
            char[,] ret = null;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    ret[x, y] = ' ';
                }
            }
            return ret;
        }

        public override char GetField(Vector2i vect)
        {
            return _varField[vect.X, vect.Y];
        }

        public override char GetField(int idx)
        {
            return GetField(Vector2i.IndexToVector(idx, Width, Height));
        }

        public override bool IsFieldEmpty(int idx)
        {
            return IsFieldEmpty(Vector2i.IndexToVector(idx, Width, Height));
        }

        public override bool IsFieldEmpty(Vector2i vect)
        {
            return _varField[vect.X, vect.Y] == ' ';
        }

        public override void SetField(Vector2i vect, char value)
        {
            int set = 0;
            for (int y = 0; y < Height; y++)
            {
                if (IsFieldEmpty(new Vector2i(vect.X, y)))
                {
                    set++;
                }
                else
                {
                    set--;
                    break;
                }
            }
            _varField[vect.X, set] = value;
        }

        public override void SetField(int idx, char value)
        {
            SetField(Vector2i.IndexToVector(idx, Width, Height), value);
        }
    }
}
