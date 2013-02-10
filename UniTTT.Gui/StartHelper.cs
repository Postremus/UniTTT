using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik;
using UniTTT.Logik.Network;

namespace UniTTT.Gui
{
    public partial class StartHelper : WizardFramework.WizardBaseClass
    {
        private List<int> _indexes;
        private int _currIndex;
        private Logik.Game.Game _gameMode;
        private Logik.Player.Player _p1;
        private Logik.Player.Player _p2;
        private Logik.IBrettDarsteller _brettDarsteller;
        private Logik.Fields.Field _field;
        private Network _client;

        public Logik.Game.Game GameMode
        {
            get
            {
                return _gameMode;
            }
        }

        public StartHelper()
        {
            InitializeComponent();
            base.Cancel = cancel_btn;
            base.Finish = finish_btn;
            base.Next = next_btn;
            base.Previous = previous_btn;
            base.MainTabControl = mainTabControl;
            this.Load += base.WizardForm_Load;
            base.AllowTitleChange(false);

            base.Next.Click += base.NextBtn_Click;
            base.Previous.Click += base.PreviousBtn_Click;
            _indexes = new List<int>();
            _indexes.Add(0);
            _currIndex = 0;
            ai_selection_lbx.SelectedIndex = 0;
            network_mode_lbx.SelectedIndex = 0;
            network_role_lbx.SelectedIndex = 0;

            game_mode_lbx.SelectedIndex = 1;

            _brettDarsteller = new BrettDarsteller(3, 3);
            _field = new Logik.Fields.Brett(3, 3);
        }

        public override int ForwardOffset(int piCurrentPage)
        {
            if (_currIndex != _indexes.Count - 1)
            {
                _currIndex = _currIndex + 1;
            }
            return _indexes[_currIndex];
        }

        public override int PreviousOffset(int piCurrentPage)
        {
            _indexes.RemoveAt(_currIndex);
            _currIndex = _currIndex - 1;
            return _indexes[_currIndex];
        }

        protected override bool ValidatePage(int piPageNumber)
        {
            if (piPageNumber == 0)
            {
                if (game_mode_lbx.SelectedIndex == 0)
                {
                    _indexes.Add(1);
                }
                else if (game_mode_lbx.SelectedIndex == 1)
                {
                    _indexes.Add(2);
                }
                else
                {
                    _indexes.Add(3);
                }
            }
            else if (piPageNumber == 1)
            {
                if (human_symbol_player_tbx.Text == string.Empty || human_symbol_enemie_tbx.Text == string.Empty)
                {
                    return false;
                }

                _p1 = new Logik.Player.Player(human_symbol_player_tbx.Text[0]);
                if (game_mode_lbx.SelectedIndex == 0)
                {
                    _p2 = new Logik.Player.Player(human_symbol_enemie_tbx.Text[0]);
                    if (human_start_player_lbx.SelectedIndex == 0)
                    {
                        Logik.Player.Player temp = _p1;
                        _p1 = _p2;
                        _p2 = temp;
                    }
                }
                else if (game_mode_lbx.SelectedIndex == 1)
                {
                    _p2 = new Logik.Player.AIPlayer((string)ai_selection_lbx.SelectedItem, 3, 3, human_symbol_enemie_tbx.Text[0]);
                    if (human_start_player_lbx.SelectedIndex == 0)
                    {
                        Logik.Player.Player temp = _p1;
                        _p1 = _p2;
                        _p2 = temp;
                    }
                }
                else
                {
                    Network client;
                    if (network_mode_lbx.SelectedIndex == 0)
                    {
                        client = new IRCClient(network_server_adress_tbx.Text, int.Parse(network_server_port_tbx.Text), "#UniTTT-play", network_nick_tbx.Text);
                    }
                    else
                    {
                        client = new TCPServer("", int.Parse(network_server_port_tbx.Text), network_nick_tbx.Text);
                    }
                    _p2 = new Logik.Player.NetworkPlayer(human_symbol_enemie_tbx.Text[0], ref _client);
                    _gameMode = new Logik.Game.NetworkGame(_p1, _p2, _brettDarsteller, _field, ref client, true);
                    _indexes.Add(6);
                    return true;
                }
                _gameMode = new Logik.Game.Game(_p1, _p2, _brettDarsteller, _field);
                _indexes.Add(7);
            }
            else if (piPageNumber == 2)
            {
                _indexes.Add(1);
            }
            else if (piPageNumber == 3)
            {
                if (network_nick_tbx.Text == string.Empty)
                {
                    return false;
                }
                _indexes.Add(4);
            }
            else if (piPageNumber == 4)
            {
                if (network_server_adress_tbx.Text == string.Empty && network_server_port_tbx.Text == string.Empty)
                {
                    return false;
                }

                if (network_role_lbx.SelectedIndex == 0)
                {
                    _indexes.Add(1);
                    return true;
                }
                if (_client != null && network_mode_lbx.SelectedIndex == 0 && network_role_lbx.SelectedIndex == 1)
                {
                    _indexes.Add(5);
                    return ((IRCClient)_client).IsConnected;
                }
            }
            else if (piPageNumber == 5)
            {
                if (listBox4.SelectedItem == null)
                {
                    return false;
                }
                _indexes.Add(7);
            }
            else if (piPageNumber == 6)
            {
            }
            else if (piPageNumber == 7)
            {

            }
            return true;
        }

        protected override void ActivatePage(int piPageNumber)
        {
            if (piPageNumber == 4)
            {
                collect_server_adress_lbl.Enabled = true;
                network_server_adress_lbl.Enabled = true;
                network_server_adress_tbx.Enabled = true;
                collect_server_adress_lbl.Text = "Auf welchen Server soll gespielt werden?";
                network_server_adress_tbx.Text = "";
                network_server_port_tbx.Text = "";
                if (network_mode_lbx.SelectedIndex == 0)
                {
                    collect_server_adress_lbl.Text = "Wo befindet sich die Lobby?";
                    network_server_adress_tbx.Text = "wolfe.freenode.net";
                    network_server_port_tbx.Text = "6665";
                }
                else
                {
                    if (network_role_lbx.SelectedIndex == 0)
                    {
                        collect_server_adress_lbl.Enabled = false;
                        network_server_adress_lbl.Enabled = false;
                        network_server_adress_tbx.Enabled = false;
                        network_server_port_tbx.Text = "5000";
                    }
                    else
                    {
                        network_server_port_tbx.Text = "5000";
                    }
                }
            }
            if (piPageNumber == 5)
            {
                _p1 = new Logik.Player.Player('X');
                _p2 = new Logik.Player.NetworkPlayer('O', ref _client);
                _gameMode = new Logik.Game.NetworkGame(_p1, _p2, _brettDarsteller, null, ref _client, false);
                ((Logik.Game.NetworkGame)_gameMode).ServerListSizeChangedEvent += ServerListSizeChanged;
                ((Logik.Game.NetworkGame)_gameMode).UpdateServerListStarter();
            }
            if (piPageNumber == 6)
            {
                ((Logik.Game.NetworkGame)_gameMode).JoinRequestListSizeChangedEvent += JoinRequestListSizeChanged;
                finish_btn.Enabled = true;
            }
            if (piPageNumber == 7)
            {
                ((Logik.Game.NetworkGame)_gameMode).SetEnemyNick((string)listBox4.SelectedItem);
                ((Logik.Game.NetworkGame)_gameMode).SendJoinRequest();
                _gameMode.GameReadyStateChangedEvent += EnableFinishButton;

                finish_btn.Enabled = true;
            }
        }

        private void ServerListSizeChanged()
        {
            if (listBox4.InvokeRequired)
            {
                listBox4.BeginInvoke(new MethodInvoker(ServerListSizeChanged));
                return;
            }
            listBox4.DataSource = ((Logik.Game.NetworkGame)_gameMode).Servers;
            listBox4.Refresh();
        }

        private void JoinRequestListSizeChanged()
        {
            if (network_server_enemy_lbx.InvokeRequired)
            {
                network_server_enemy_lbx.BeginInvoke(new MethodInvoker(JoinRequestListSizeChanged));
                return;
            }
            network_server_enemy_lbx.DataSource = ((Logik.Game.NetworkGame)_gameMode).JoinRequests;
            network_server_enemy_lbx.Refresh();
        }

        private void finish_btn_Click(object sender, EventArgs e)
        {
            if (_indexes.Last() == 7)
            {
                if (network_server_enemy_lbx.SelectedItem == null)
                {
                    return;
                }
                ((Logik.Game.NetworkGame)_gameMode).SetEnemyNick((string)network_server_enemy_lbx.SelectedItem);
                ((Logik.Game.NetworkGame)_gameMode).SendJoinAnswer();
            }
            for (int i = 0; i < _indexes.Count; i++)
            {
                if (!ValidatePage(_indexes[i]))
                {
                    return;
                }
            }
            if (!_indexes.Contains(7))
            {
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void network_server_connect_btn_Click(object sender, EventArgs e)
        {
            connect_to_server_lbl.Text = "Verbinde mit dem Server..";
            connect_to_server_lbl.Enabled = true;
            try
            {
                if (network_mode_lbx.SelectedIndex == 0)
                {
                    _client = new IRCClient(network_server_adress_tbx.Text, int.Parse(network_server_port_tbx.Text), "#UniTTT-play", network_nick_tbx.Text);
                    if (network_role_lbx.SelectedIndex == 1)
                    {
                        _client.Connect();
                    }
                }
                else
                {
                    _client = new TCPClient(network_server_adress_tbx.Text, int.Parse(network_server_port_tbx.Text), network_nick_tbx.Text);
                    _indexes.Add(7);
                }
                connect_to_server_lbl.Text += "  Erfolgreich";
            }
            catch (Exception)
            {
                connect_to_server_lbl.Text += "  Konnte keine Verbindung herstellen. Sind alle Daten korrekt?";
            }
        }

        private void EnableFinishButton(bool currState)
        {
            finish_btn.Enabled = currState;
        }

        private void refresh_listbox4_Click(object sender, EventArgs e)
        {
            if (!(_gameMode is Logik.Game.NetworkGame))
                return;
            ((Logik.Game.NetworkGame)_gameMode).UpdateServerListStarter();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
