using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using System.IO;

namespace UniTTT.ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        private NormalGame game;
        private Point prevPos;
        private Thread loopThread;
        private Point moveTo;
        private Random rnd;
        private int screenWidth;
        private int screenHeight;
        private Config c;

        public ScreenSaverForm(Color backColor)
        {
            InitializeComponent();
            game = new NormalGame(new Logik.Player.KIPlayer(3, 3, 3, 'X'), new Logik.Player.KIPlayer(3, 3, 3, 'O'), new BrettDarsteller(3, 3, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), null, null);
            loopThread = new Thread(Loop);
            rnd = new Random();
            moveTo = new Point(0, 0);
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;

            //MouseMove += MouseMoveCheck;
            //MouseDown += DoWakeUp;
            //KeyDown += DoWakeUp;
            FormClosed += AbortThreads;

            //FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.Bounds = Screen.PrimaryScreen.Bounds;
            //Cursor.Hide();
            //TopMost = true;
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.TransparencyKey = SystemColors.Control;
            BackColor = backColor;
            LoadConfig();

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
                if (elapsed.Seconds != st.Elapsed.Seconds && st.Elapsed.Seconds % c.PlayVelocity / 2 == 0)
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
                currLocation.X += c.MoveVelocity;
            }
            else if (pictureBox1.Location.X > moveTo.Y)
            {
                currLocation.X -= c.MoveVelocity;
            }
            if (pictureBox1.Location.Y < moveTo.Y)
            {
                currLocation.Y += c.MoveVelocity;
            }
            else if (pictureBox1.Location.Y > moveTo.Y)
            {
                currLocation.Y -= c.MoveVelocity;
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
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                FileStream stream = new FileStream("UniTTT.Screensaver.ini", FileMode.Open);
                c = (Config)serializer.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                c = new Config();
                c.PlayVelocity = 1;
                c.MoveVelocity = 1;
            }
        }
    }
}