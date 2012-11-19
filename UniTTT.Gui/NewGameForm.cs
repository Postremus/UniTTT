using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.PathSystem.Game;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace UniTTT.Gui
{
    public partial class NewGameForm : Form
    {
        private Game _gameMode;
        private bool _spieler1Anfang;
        private bool _spieler2Anfang;

        public Game GameMode
        {
            get
            {
                return _gameMode;
            }
        }

        public bool Spieler1Anfang
        {
            get
            {
                return _spieler1Anfang;
            }
        }

        public bool Spieler2Anfang
        {
            get
            {
                return _spieler2Anfang;
            }
        }

        public NewGameForm()
        {
            InitializeComponent();
        }

        private void online_modus_cbx_CheckedChanged(object sender, EventArgs e)
        {
            protokol_lbl.Enabled = online_modus_cbx.Checked;
            protokoll_lbx.Enabled = online_modus_cbx.Checked;
            host_lbl.Enabled = online_modus_cbx.Checked;
            host_tbx.Enabled = online_modus_cbx.Checked;
            port_lbl.Enabled = online_modus_cbx.Checked;
            port_tbx.Enabled = online_modus_cbx.Checked;
            server_cbx.Enabled = online_modus_cbx.Checked;
            spieler1_anfang_cbx.Enabled = online_modus_cbx.Checked;
            spieler2_anfang_cbx.Enabled = !online_modus_cbx.Checked;
            if (online_modus_cbx.Checked)
            {
                protokoll_lbx.SelectedIndex = 0;
            }
            else
            {
                protokoll_lbx.SelectedIndex = -1;
            }
        }

        private void spieler2_ki_cbx_CheckedChanged_1(object sender, EventArgs e)
        {
            ki_lbl.Enabled = spieler2_ki_cbx.Checked;
            ki_nud.Enabled = spieler2_ki_cbx.Checked;
        }

        private void abbrechen_btn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(spieler1_tbx.Text.Trim()) || String.IsNullOrEmpty(spieler2_tbx.Text.Trim()))
            {
                return;
            }
            PathSystem.Player.Player p1 = new PathSystem.Player.Player(spieler1_tbx.Text[0]);
            PathSystem.Player.Player p2 = new PathSystem.Player.Player(spieler2_tbx.Text[0]);

            if (spieler2_ki_cbx.Checked)
            {
                p2 = new PathSystem.Player.AIPlayer((int)ki_nud.Value, 3, 3, p2.Symbol);
            }

            if (online_modus_cbx.Checked)
            {
                PathSystem.Network.Network client = null;
                if ((string)protokoll_lbx.SelectedItem == "TCP/IP")
                {
                    if (server_cbx.Checked)
                    {
                        client = new PathSystem.Network.TCPServer(host_tbx.Text, int.Parse(port_tbx.Text));
                    }
                    else
                    {
                        client = new PathSystem.Network.TCPClient(host_tbx.Text, int.Parse(port_tbx.Text));
                    }
                }
                else
                {
                    client = new PathSystem.Network.IRCClient(host_tbx.Text, int.Parse(port_tbx.Text), "#UniTTT-play");
                }
                p2 = new PathSystem.Player.NetworkPlayer(p2.Symbol, client);
                _gameMode = new NetworkGame(p1, p2, new BrettDarsteller(3, 3), new PathSystem.Fields.Brett(3, 3), client);
            }
            else
            {
                _gameMode = new Game(p1, p2, new BrettDarsteller(3, 3), new PathSystem.Fields.Brett(3, 3));
            }
            if (spieler2_anfang_cbx.Checked && !online_modus_cbx.Checked)
            {
                _gameMode.PlayerChange();
            }
            PathSystem.WinChecker.WinCondition = (int)gewinnbedingung_nud.Value;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            _spieler1Anfang = spieler1_anfang_cbx.Checked && online_modus_cbx.Checked;
            _spieler2Anfang = spieler2_anfang_cbx.Checked && !online_modus_cbx.Checked;
            Close();
        }
    }
}