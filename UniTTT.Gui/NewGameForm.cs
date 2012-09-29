using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik.Game;

namespace UniTTT.Gui
{
    public partial class NewGameForm : Form
    {
        private Game _gameMode;

        public Game GameMode
        {
            get
            {
                return _gameMode;
            }
        }

        public NewGameForm()
        {
            InitializeComponent();
        }

        private void spieler2_lbl_Click(object sender, EventArgs e)
        {

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
            if (spieler1_tbx == null || spieler2_tbx == null)
            {
                return;
            }
            Logik.Player.Player p = new Logik.Player.Player(spieler1_tbx.Text[0]);
            if (spieler2_ki_cbx.Checked)
            {
                p = new Logik.Player.AIPlayer((int)ki_nud.Value, (int)breite_nud.Value, (int)hoehe_nud.Value, spieler1_tbx.Text[0]);
            }
            if (online_modus_cbx.Checked)
            {
                if (host_tbx == null || spieler2_tbx == null)
                {
                    return;
                }
                Logik.Network.Network client = null;
                if ((string)protokoll_lbx.SelectedItem == "TCP/IP")
                {
                    if (server_cbx.Checked)
                    {
                        client = new Logik.Network.TCPServer(host_tbx.Text, int.Parse(port_tbx.Text));
                    }
                    else
                    {
                        client = new Logik.Network.TCPClient(host_tbx.Text, int.Parse(port_tbx.Text));
                    }
                }
                else
                {
                    client = new Logik.Network.IRCClient(host_tbx.Text, int.Parse(port_tbx.Text), "#UniTTT-play");
                }
                _gameMode = new NetworkGame(p, new BrettDarsteller((int)breite_nud.Value, (int)hoehe_nud.Value), new Logik.Fields.Brett((int)breite_nud.Value, (int)hoehe_nud.Value), client);
                if (p.Symbol.ToString().ToLower() == "o")
                {
                    _gameMode.PlayerChange();
                }
            }
            else
            {
                p.Symbol = spieler2_tbx.Text[0];
                _gameMode = new Game(new Logik.Player.Player(spieler1_tbx.Text[0]), p, new BrettDarsteller((int)breite_nud.Value, (int)hoehe_nud.Value), new Logik.Fields.Brett((int)breite_nud.Value, (int)hoehe_nud.Value));
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}