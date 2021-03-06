﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;

[assembly: CLSCompliant(true)]
namespace UniTTT.Plugin.FourConnect
{
    public class FourConnect : Logik.Fields.Field, Logik.Plugin.IFieldPlugin
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

        public bool ForceFieldSize
        {
            get { return true; }
        }

        public FourConnect()
        {
            Width = 7;
            Height = 6;
            _varField = new char[7, 6];
            InitBrett();
        }

        private void InitBrett()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _varField[x, y] = ' ';
                }
            }
        }

        public override char GetField(Vector2i vect)
        {
            return _varField[vect.X, vect.Y];
        }

        public override char GetField(int idx)
        {
            return GetField(Vector2i.FromIndex(idx, Width, Height));
        }

        public override bool IsFieldEmpty(int idx)
        {
            return IsFieldEmpty(Vector2i.FromIndex(idx, Width, Height));
        }

        public override bool IsFieldEmpty(Vector2i vect)
        {
            return _varField[vect.X, 0] == ' ';
        }

        public override void SetField(Vector2i vect, char value)
        {
            for (int y = 0; y < Height; y++)
            {
                if (IsFieldEmpty(new Vector2i(vect.X, y)))
                {
                    _varField[vect.X, y] = value;
                    break;
                }
            }
        }

        public override void SetField(int idx, char value)
        {
            SetField(Vector2i.FromIndex(idx, Width, Height), value);
        }
    }
}