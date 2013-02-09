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
using System.Threading;

[assembly: CLSCompliant(true)]
namespace UniTTT.Gui
{
    public partial class GameForm : Form
    {
        private Logik.Game.Game _game;
        private Task _playerWaitTask;
        private bool _taskTurn;
        private bool _spieler1Anfang;
        private bool _spieler2Anfang;
        CancellationTokenSource _taskToken;

        public GameForm()
        {
            InitializeComponent();
            MouseClick += MouseNewStart;
            _taskToken = new CancellationTokenSource();
            FormClosed += GameWindowClosedEvent;

            _game = new Logik.Game.Game(new Player('X'), new Player('O'), new BrettDarsteller(3, 3), new Logik.Fields.Brett(3, 3));
            _game.BDarsteller.Enabled = true;
            EnableBrett();
            ((BrettDarsteller)_game.BDarsteller).BrettUpdateEvent += UpdateBrett;
            ((BrettDarsteller)_game.BDarsteller).BrettEnableEvent += EnableBrett;
            _game.PlayerOutputEvent += OutputPlayer;
            _game.WindowTitleChangeEvent += ChangeWindowTitle;
            _game.WinMessageEvent += OutputWinMessage;
            OutputPlayer(string.Format("Spieler {0} ist an der Reihe.", _game.Player.Symbol));
            _game.Initialize();
        }

        private void GameWindowClosedEvent(object sender, EventArgs e)
        {
            _taskToken.Cancel();
        }

        private void WaitForPlayerTask()
        {
            if (_taskTurn)
            {
                Vector2i zug = _game.Player.Play(_game.Field);
                _game.Logik(zug);
                _taskTurn = false;
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
            if (!(_game is Logik.Game.NetworkGame))
            {
                _game.Player = _game.Player1;
                if (_spieler2Anfang)
                {
                    _game.PlayerChange();
                }
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
            }
            OutputPlayer(string.Format("Spieler {0} ist an der Reihe.", _game.Player.Symbol));
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
                label1.Invoke(new Logik.ShowMessageHandler(OutputPlayer), new object[] { message });
                return;
            }

            string spielerName = "Gegner";
            if (IsPlayerGegner())
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
                MessageBox.Show("    " + gameState.ToString() + "   ", gameState.ToString());
            }
            else
            {
                string spielerName = "Gegner";
                if (IsPlayerGegner())
                {
                    spielerName = "Spieler";
                }
                MessageBox.Show(string.Format("Der {0} {1} hat {2}.", spielerName, symbol, gameState), gameState.ToString());
            }

            label1.Location = new Point(37, label1.Location.Y);
            label1.Text = "Klicken Sie zum neustarten irgendwo hin.";
        }

        private bool IsPlayerGegner()
        {
            return _game.Player == _game.Player1;
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
                if (_game.Player is Logik.Player.AIPlayer || _game.Player is Logik.Player.NetworkPlayer)
                {
                    _taskTurn = true;
                    _playerWaitTask = new Task(new Action(WaitForPlayerTask), _taskToken.Token);
                    _playerWaitTask.Start();
                }
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

        private void assistentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartHelper f = new StartHelper();
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
                OutputPlayer(string.Format("Spieler {0} ist an der Reihe.", _game.Player.Symbol));
                _game.Initialize();
                label1.Location = new Point(80, label1.Location.Y);
                if (_game.Player is Logik.Player.AIPlayer || _game.Player is Logik.Player.NetworkPlayer)
                {
                    _taskTurn = true;
                    _playerWaitTask = new Task(new Action(WaitForPlayerTask), _taskToken.Token);
                    _playerWaitTask.Start();
                }
                if (_game is Logik.Game.NetworkGame)
                {
                    ((Logik.Game.NetworkGame)_game).NewGameRequestReceivedEvent += ResetGame;
                }
                //_spieler1Anfang = f.Spieler1Anfang;
                //_spieler2Anfang = f.Spieler2Anfang;
            }
        }

        private void profiModusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfiModeForm f = new ProfiModeForm();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _game = f.GameMode;
                _game.BDarsteller = new BrettDarsteller(_game.Field.Width, _game.Field.Height);
                _game.BDarsteller.Enabled = true;
                EnableBrett();
                ((BrettDarsteller)_game.BDarsteller).BrettUpdateEvent += UpdateBrett;
                ((BrettDarsteller)_game.BDarsteller).BrettEnableEvent += EnableBrett;
                _game.PlayerOutputEvent += OutputPlayer;
                _game.WindowTitleChangeEvent += ChangeWindowTitle;
                _game.WinMessageEvent += OutputWinMessage;
                OutputPlayer(string.Format("Spieler {0} ist an der Reihe.", _game.Player.Symbol));
                _game.Initialize();
                label1.Location = new Point(80, label1.Location.Y);
                if (_game.Player is Logik.Player.AIPlayer || _game.Player is Logik.Player.NetworkPlayer)
                {
                    _taskTurn = true;
                    _playerWaitTask = new Task(new Action(WaitForPlayerTask), _taskToken.Token);
                    _playerWaitTask.Start();
                }
                if (_game is Logik.Game.NetworkGame)
                {
                    ((Logik.Game.NetworkGame)_game).NewGameRequestReceivedEvent += ResetGame;
                }
                //_spieler1Anfang = f.Spieler1Anfang;
                //_spieler2Anfang = f.Spieler2Anfang;
            }
        }
    }
}
