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
        Config c;

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
                c = (Config)serializer.Deserialize(stream);
                stream.Close();

                trackBar1.Value = c.PlayVelocity;
                trackBar2.Value = c.MoveVelocity;
            }
            catch
            {
                c = new Config();
            }
        }

        private void Close(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            FileStream stream = new FileStream("UniTTT.Screensaver.ini", FileMode.Create);
            serializer.Serialize(stream, c);
            stream.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            c.PlayVelocity = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            c.MoveVelocity = trackBar2.Value;
        }
    }
}
