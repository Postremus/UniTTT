using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniTTT.Logik;

namespace UniTTT.Gui
{
    public partial class ProfiModeForm : Form
    {
        private Logik.Game.Game _gameMode;

        public Logik.Game.Game GameMode
        {
            get
            {
                return _gameMode;
            }
        }

        public ProfiModeForm()
        {
            InitializeComponent();
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            string[] args = parameter_tbx.Text.Split(' ');
            _gameMode = GameFactory.CreateGame(args);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
