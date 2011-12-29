using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniTTT.WinForms
{
    class BrettDarsteller : Logik.IGraphicalBrettDarsteller
    {
        public BrettDarsteller(int width, int height, ref Form1 f)
        {
            Width = width;
            Height = height;
            form1 = f;
            Btn = new Button[width, height];
            spielfeld = new char[width, height];
            Create();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        private char[,] spielfeld { get; set; }
        private Form1 form1 { get; set; }
        private Button[,] Btn { get; set; }

        public void Update(Logik.Fields.IField field)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    spielfeld[x, y] = field.GetField(new Logik.Vector2i(x, y));
                }
            }
        }

        public void Draw()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Btn[x, y].Text = spielfeld[x, y].ToString();
                }
            }
        }

        public void Lock()
        {
            foreach (var item in Btn)
            {
                item.Visible = false;
            }
        }

        public void DeLock()
        {
            foreach (var item in Btn)
            {
                item.Visible = true;
            }
        }

        public void Create()
        {
            int count = 0;
            Padding padd = new Padding();
            padd.Left = 148;
            padd.Top = 20;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Btn[x, y] = new Button();
                    Btn[x, y].Margin = padd;
                    Btn[x, y].Height = 55;
                    Btn[x, y].Width = 70;
                    Btn[x, y].Name = "btn_" + count;
                    form1.Controls.Add(Btn[x, y]);

                    padd.Top += 61;
                    count++;
                }
                padd.Top = 20;
                padd.Left += 76;
            }
        }
    }
}
