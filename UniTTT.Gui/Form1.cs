﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik;

[assembly: CLSCompliant(true)]
namespace UniTTT.Gui
{
    public partial class Form1 : Form
    {
        private Logik.Game.Game _game;

        public Form1()
        {
            InitializeComponent();
            _game = new Logik.Game.Game(new Logik.Player.Player('X'), new Logik.Player.Player('O'), new BrettDarsteller(3, 3), new Logik.Fields.Brett(3, 3));
            ((BrettDarsteller)_game.BDarsteller).BrettUpdateEvent += UpdateBrett;
            ((BrettDarsteller)_game.BDarsteller).BrettEnableEvent += EnableBrett;
            _game.PlayerOutputEvent += OutputPlayer;
            _game.WindowTitleChangeEvent += ChangeWindowTitle;
            _game.WinMessageEvent += OutputWinMessage;
            OutputPlayer(_game.Player1.Ausgabe());
            _game.Initialize();
            MouseClick += MouseNewStart;
        }

        private void neustartenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _game.NewGame();
            OutputPlayer(_game.Player1.Ausgabe());
        }

        private void MouseNewStart(object sender, MouseEventArgs e)
        {
            if (_game.HasEnd())
            {
                _game.NewGame();
                OutputPlayer(_game.Player1.Ausgabe());
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ChangeWindowTitle(string title)
        {
            this.Text = title;
        }

        public void OutputPlayer(string message)
        {
            label1.Text = message;
        }

        public void OutputWinMessage(char symbol, GameStates gameState)
        {
            MessageBox.Show(string.Format("Spieler {0} hat {1}.", symbol, gameState), gameState.ToString());
            label1.Text = "Klicken Sie zum neustarten irgendwo hin.";
        }

        public string GetString()
        {
            MessageBox.Show("Bitte geben Sie einen Text ein.", "Texteingabe erforderlich");
            return null;
        }

        public int GetInt()
        {
            return 0;
        }

        public void UpdateBrett()
        {
            button1.Text = _game.Field.GetField(0).ToString();
            button2.Text = _game.Field.GetField(1).ToString();
            button3.Text = _game.Field.GetField(2).ToString();

            button4.Text = _game.Field.GetField(3).ToString();
            button5.Text = _game.Field.GetField(4).ToString();
            button6.Text = _game.Field.GetField(5).ToString();

            button7.Text = _game.Field.GetField(6).ToString();
            button8.Text = _game.Field.GetField(7).ToString();
            button9.Text = _game.Field.GetField(8).ToString();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button send = sender as Button;
            int idx = int.Parse(send.Name.Substring(send.Name.Length - 1));
            idx--;
            if (_game.Field.IsFieldEmpty(idx))
            {
                _game.Logik(Vector2i.FromIndex(idx, 3, 3));
            }
        }

        public void EnableBrett()
        {
            button1.Enabled = _game.BDarsteller.Enabled;
            button2.Enabled = _game.BDarsteller.Enabled;
            button3.Enabled = _game.BDarsteller.Enabled;

            button4.Enabled = _game.BDarsteller.Enabled;
            button5.Enabled = _game.BDarsteller.Enabled;
            button6.Enabled = _game.BDarsteller.Enabled;

            button7.Enabled = _game.BDarsteller.Enabled;
            button8.Enabled = _game.BDarsteller.Enabled;
            button9.Enabled = _game.BDarsteller.Enabled;
        }

        private void neuesSpielToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameForm f = new NewGameForm();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _game = f.GameMode;
                _game.BDarsteller.Enabled = true;
                EnableBrett();
                ((BrettDarsteller)_game.BDarsteller).BrettUpdateEvent += UpdateBrett;
                ((BrettDarsteller)_game.BDarsteller).BrettEnableEvent += EnableBrett;
                _game.PlayerOutputEvent += OutputPlayer;
                _game.WindowTitleChangeEvent += ChangeWindowTitle;
                _game.WinMessageEvent += OutputWinMessage;
                OutputPlayer(_game.Player1.Ausgabe());
                _game.Initialize();
            }
        }
    }
}