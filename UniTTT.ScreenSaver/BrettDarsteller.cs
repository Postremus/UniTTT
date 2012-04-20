using System;
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
        private Random _rnd;

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

        public BrettDarsteller(int width, int height, int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            Initialize(width, height);
        }

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            Image = new Bitmap(_screenWidth, _screenHeight);
            _rnd = new Random();
        }

        public void Update(Logik.Fields.IField field)
        {
            UpdateImageLetters(new Font("consolas", _screenHeight / 4), field);
        }

        private void UpdateImageLetters(Font font, Logik.Fields.IField field)
        {
            SolidBrush xBrush = new SolidBrush(Color.Green);
            SolidBrush oBrush = new SolidBrush(Color.Blue);
            Bitmap tmpImg = new Bitmap(_screenWidth, _screenHeight);
            Point posi = new Point(0, 0);
            Graphics graphics = Graphics.FromImage(tmpImg);
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

            Image = tmpImg;
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
