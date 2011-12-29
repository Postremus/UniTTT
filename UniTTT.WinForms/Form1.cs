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
    public partial class Form1 : Form
    {
        Logik.NormalGame Game;
        Form1 form1;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Form1 f)
        {
            if (f == null)
                throw new NullReferenceException();
            form1 = f;
            InitializeComponent();
            Game = new Logik.NormalGame(new Logik.Player.HumanPlayer('X'), new Logik.Player.HumanPlayer('O'), new BrettDarsteller(3, 3, ref form1), new OutputDarsteller(ref form1), null);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            int idx = int.Parse(b.Name.Substring(0, 4));
            Game.Logik(idx);
        }
    }
}
