﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace UniTTT.ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        private Logik.Game.Game game;
        private Point prevPos;
        private Thread loopThread;
        private Point moveTo;
        private Random rnd;
        private int screenWidth;
        private int screenHeight;
        private Config _config;
        private ConfigStream _confiStream;

        public ScreenSaverForm(Color backColor)
        {
            InitializeComponent();
            LoadConfig();
            game = new Logik.Game.Game(new Logik.Player.AIPlayer(3, 3, 3, 'X'), new Logik.Player.AIPlayer(3, 3, 3, 'O'), new BrettDarsteller(3, 3, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, _config.Matrix), null);
            loopThread = new Thread(Loop);
            rnd = new Random();
            moveTo = new Point(0, 0);
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;

            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            Cursor.Hide();
            TopMost = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.TransparencyKey = SystemColors.Control;
            BackColor = backColor;

            MouseMove += MouseMoveCheck;
            MouseDown += DoWakeUp;
            KeyDown += DoWakeUp;
            FormClosed += AbortThreads;
            ((BrettDarsteller)game.BDarsteller).DrawEvent += Draw;
            loopThread.Start();
        }

        private void AbortThreads(object sender, EventArgs e)
        {
            loopThread.Abort();
        }

        private void Loop()
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            TimeSpan elapsed = TimeSpan.Zero;
            bool locupdate = false;
            while (true)
            {
                if (game.HasEnd())
                {
                    game.NewGame();
                }
                if (elapsed.Seconds != st.Elapsed.Seconds && st.Elapsed.Seconds % _config.PlayVelocity / 2 == 0)
                {
                    game.Logik();
                    if (game.HasEnd())
                    {
                        game.NewGame();
                    }
                }
                else if (st.Elapsed.Milliseconds % 41 == 0)
                {
                    locupdate = true;
                }
                else if (locupdate)
                {
                    pictureBox1.Invoke(new MethodInvoker(UpdateImageLocation));
                    locupdate = false;
                }
                elapsed = st.Elapsed;
            }
        }

        private void Draw(object sender, EventArgs e)
        {
            pictureBox1.Invoke(new MethodInvoker(DrawImage));
        }

        private void DrawImage()
        {
            pictureBox1.Image = ((BrettDarsteller)game.BDarsteller).Image;
        }

        private void UpdateImageLocation()
        {
            if (pictureBox1.Location == moveTo)
            {
                moveTo = new Point(rnd.Next(screenWidth - 200), rnd.Next(screenHeight - 200));
            }
            Point currLocation = pictureBox1.Location;
            if (pictureBox1.Location.X < moveTo.X)
            {
                currLocation.X += _config.MoveVelocity;
            }
            else if (pictureBox1.Location.X > moveTo.Y)
            {
                currLocation.X -= _config.MoveVelocity;
            }
            if (pictureBox1.Location.Y < moveTo.Y)
            {
                currLocation.Y += _config.MoveVelocity;
            }
            else if (pictureBox1.Location.Y > moveTo.Y)
            {
                currLocation.Y -= _config.MoveVelocity;
            }
            pictureBox1.Location = currLocation;
        }

        private void MouseMoveCheck(object sender, MouseEventArgs e)
        {
            if (!prevPos.IsEmpty)
            {
                if (e.X != prevPos.X || e.Y != prevPos.Y)
                {
                    DoWakeUp(sender, e);
                }
            }
            prevPos = new Point(e.X, e.Y);
        }

        private void DoWakeUp(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadConfig()
        {
            _confiStream = new ConfigStream("UniTTT.Screensaver", ".ini");
            try
            {
                _config = (Config)_confiStream.Read();
            }
            catch
            {
                _config = new Config();
                _config.PlayVelocity = 1;
                _config.MoveVelocity = 1;
            }
        }
    }
}