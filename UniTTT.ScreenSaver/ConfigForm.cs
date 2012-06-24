using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using UniTTT.Logik;

namespace UniTTT.ScreenSaver
{
    public partial class ConfigForm : Form
    {
        Config _config;
        ConfigStream _confiStream;

        public ConfigForm()
        {
            InitializeComponent();
            FormClosed += Close;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            _confiStream = new ConfigStream("UnITTT.Screensaver", ".ini");
            try
            {
                _config = (Config)_confiStream.Read();

                trackBar1.Value = _config.PlayVelocity;
                trackBar2.Value = _config.MoveVelocity;
            }
            catch
            {
                _config = new Config();
            }
        }

        private void Close(object sender, EventArgs e)
        {
            _confiStream.Write(_config);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _config.PlayVelocity = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _config.MoveVelocity = trackBar2.Value;
        }

        private void matrix_rbtn_CheckedChanged(object sender, EventArgs e)
        {
            _config.Matrix = matrix_rbtn.Checked;
        }
    }
}
