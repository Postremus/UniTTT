using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace UniTTT.ScreenSaver
{
    public partial class ConfigForm : Form
    {
        Config _config;

        public ConfigForm()
        {
            InitializeComponent();
            FormClosed += Close;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                FileStream stream = new FileStream("UniTTT.Screensaver.ini", FileMode.Open);
                _config = (Config)serializer.Deserialize(stream);
                stream.Close();

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
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            FileStream stream = new FileStream("UniTTT.Screensaver.ini", FileMode.Create);
            serializer.Serialize(stream, _config);
            stream.Close();
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
