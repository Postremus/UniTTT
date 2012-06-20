﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;
using System.Drawing;
using System.Threading;

namespace UniTTT.ScreenSaver
{
    public class BrettDarsteller : IBrettDarsteller
    {
        private int _width;
        private int _height;
        private int _screenWidth;
        private int _screenHeight;
        private bool _matrix;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public event EventHandler DrawEvent;
        public Bitmap Image;

        public BrettDarsteller(int width, int height, int screenWidth, int screenHeight, bool matrix)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _matrix = matrix;
            Initialize(width, height);
        }

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            Image = new Bitmap(_screenWidth, _screenHeight);
        }

        public void Update(Logik.Fields.IField field)
        {
            int fontHeight = 0;
            if (_matrix)
            {
                fontHeight = _screenHeight / 10;
            }
            else
            {
                fontHeight = _screenHeight / 4;
            }
            UpdateImageLetters(new Font("consolas", fontHeight), field);
        }

        private void UpdateImageLetters(Font font, Logik.Fields.IField field)
        {
            Bitmap tmpImage = new Bitmap(_screenWidth, _screenHeight);
            if (_matrix)
            {
                for (int i = 0; i < 10; i++)
                {
                    Point posi = new Point((_screenWidth / 10) * i);
                    DrawBrettOnBitMap(tmpImage, font, field, posi);
                }
            }
            else
            {
                DrawBrettOnBitMap(tmpImage, font, field, new Point(0, 0));
            }
            Image = tmpImage;
        }

        private Bitmap DrawBrettOnBitMap(Bitmap image, Font font, Logik.Fields.IField field, Point posi)
        {
            SolidBrush xBrush = new SolidBrush(Color.Green);
            SolidBrush oBrush = new SolidBrush(Color.Blue);
            Graphics graphics = Graphics.FromImage(image);
            for (int x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    if (field.GetField(new Vector2i(x, y)) == 'X')
                    {
                        graphics.DrawString(field.GetField(new Vector2i(x, y)).ToString(), font, xBrush, posi);
                    }
                    else
                    {
                        graphics.DrawString(field.GetField(new Vector2i(x, y)).ToString(), font, oBrush, posi);
                    }
                    posi.Y += _screenHeight / 3;
                }
                posi.X += _screenWidth / 4;
                posi.Y = 0;
            }
            return image;
        }

        public void Draw()
        {
            OnDrawEvent();
            
        }

        private void OnDrawEvent()
        {
            EventHandler drawEvent = DrawEvent;
            if (drawEvent != null)
            {
                drawEvent(this, EventArgs.Empty);
            }
        }
    }
}
