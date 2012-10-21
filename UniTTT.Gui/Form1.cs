using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik;
using UniTTT.Logik.Player;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]
namespace UniTTT.Gui
{
    public partial class Form1 : Form
    {
        private Logik.Game.Game _game;
        private Task _playerWaitTask;
        private bool _taskTurn;
        private bool _isGameWindowClosed;
        private bool _spieler1Anfang;
        private bool _spieler2Anfang;

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
            _playerWaitTask = new Task(new Action(WaitForPlayerTask));
            _playerWaitTask.Start();
            _isGameWindowClosed = false;
            FormClosed += GameWindowClosedEvent;
        }

        private void GameWindowClosedEvent(object sender, EventArgs e)
        {
            _isGameWindowClosed = true;
        }

        private void WaitForPlayerTask()
        {
            while (!_isGameWindowClosed)
            {
                if (_taskTurn)
                {
                    Vector2i zug = _game.Player.Play(_game.Field);
                    _game.Logik(zug);
                    _taskTurn = false;
                }
            }
        }

        private void neustartenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewStart();
        }

        private void MouseNewStart(object sender, MouseEventArgs e)
        {
            if (_game.HasEnd())
            {
                NewStart();
            }
        }

        private void NewStart()
        {
            _game.OnNewGameEvent();
            ResetGame();
        }

        private void ResetGame()
        {
            if (InvokeRequired)
            {
                Invoke(new Logik.Network.NewGameRequestReceived(ResetGame));
            }
            label1.Location = new Point(80, label1.Location.Y);
            if (_game.GetType() != typeof(Logik.Game.NetworkGame))
            {
                _game.Player = _game.Player1;
            }
            else
            {
                if (_spieler1Anfang && _game is Logik.Game.NetworkGame)
                {
                    if (_game.Player1 is NetworkPlayer)
                    {
                        _game.Player = _game.Player2;
                    }
                    else
                    {
                        _game.Player = _game.Player1;
                    }
                }
                else
                {
                    if (_game.Player1 is NetworkPlayer)
                    {
                        _game.Player = _game.Player1;
                    }
                    else
                    {
                        _game.Player = _game.Player2;
                    }
                    _taskTurn = true;
                }

                if (_spieler2Anfang && !(_game is Logik.Game.NetworkGame))
                {
                    _game.PlayerChange();
                    _game.PlayerChange();
                }
            }
            OutputPlayer(_game.Player.Ausgabe());
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
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Logik.ShowMessageHandler(OutputPlayer), new object[] {message});
                return;
            }

            string spielerName = "Gegner";
            if (_game.Player == _game.Player1)
            {
                spielerName = "Spieler";
            }

            label1.Text = string.Format("Der {0} {1} ist an der Reihe", spielerName, _game.Player.Symbol);
        }

        public void OutputWinMessage(char symbol, GameStates gameState)
        {
            if (InvokeRequired)
            {
                Invoke(new Logik.WinMessageHandler(OutputWinMessage), new object[] { symbol, gameState });
                return;
            }
            if (gameState == GameStates.Unentschieden)
            {
                MessageBox.Show(gameState.ToString(), gameState.ToString());
            }
            else
            {
                MessageBox.Show(string.Format("Spieler {0} hat {1}.", symbol, gameState), gameState.ToString());
            }

            label1.Location = new Point(37, label1.Location.Y);
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
            if (InvokeRequired)
            {
                Invoke(new Logik.Network.NewGameRequestReceived(UpdateBrett));
                return;
            }

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
            if (_taskTurn)
            {
                return;
            }
            Button send = sender as Button;
            int idx = int.Parse(send.Name.Substring(send.Name.Length - 1));
            idx--;
            if (_game.Field.IsFieldEmpty(idx))
            {
                _game.Logik(Vector2i.FromIndex(idx, 3, 3));
                _taskTurn = _game.Player.GetType() == typeof(Logik.Player.AIPlayer) || _game.Player.GetType() == typeof(Logik.Player.NetworkPlayer);
            }
        }

        public void EnableBrett()
        {
            if (InvokeRequired)
            {
                Invoke(new Logik.Network.NewGameRequestReceived(EnableBrett));
                return;
            }
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
                OutputPlayer(_game.Player.Ausgabe());
                _game.Initialize();
                label1.Location = new Point(80, label1.Location.Y);
                if (_game.Player is Logik.Player.AIPlayer || _game.Player is Logik.Player.NetworkPlayer)
                {
                    _taskTurn = true;
                }
                if (_game is Logik.Game.NetworkGame)
                {
                    ((Logik.Game.NetworkGame)_game).newGameRequestReceivedEvent += ResetGame;
                }
                _spieler1Anfang = f.Spieler1Anfang;
                _spieler2Anfang = f.Spieler2Anfang;
            }
        }
    }
}
