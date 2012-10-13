namespace UniTTT.Gui
{
    partial class NewGameForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewGameForm));
            this.gewinnbedingung_nud = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.hoehe_nud = new System.Windows.Forms.NumericUpDown();
            this.breite_nud = new System.Windows.Forms.NumericUpDown();
            this.hoehe_lbl = new System.Windows.Forms.Label();
            this.breite_lbl = new System.Windows.Forms.Label();
            this.ki_lbl = new System.Windows.Forms.Label();
            this.ki_nud = new System.Windows.Forms.NumericUpDown();
            this.spieler2_ki_cbx = new System.Windows.Forms.CheckBox();
            this.spieler2_tbx = new System.Windows.Forms.TextBox();
            this.spieler1_tbx = new System.Windows.Forms.TextBox();
            this.spieler2_lbl = new System.Windows.Forms.Label();
            this.spieler1_lbl = new System.Windows.Forms.Label();
            this.online_modus_cbx = new System.Windows.Forms.CheckBox();
            this.protokol_lbl = new System.Windows.Forms.Label();
            this.protokoll_lbx = new System.Windows.Forms.ListBox();
            this.host_lbl = new System.Windows.Forms.Label();
            this.host_tbx = new System.Windows.Forms.TextBox();
            this.abbrechen_btn = new System.Windows.Forms.Button();
            this.ok_btn = new System.Windows.Forms.Button();
            this.port_lbl = new System.Windows.Forms.Label();
            this.port_tbx = new System.Windows.Forms.TextBox();
            this.server_cbx = new System.Windows.Forms.CheckBox();
            this.spieler2_anfang_cbx = new System.Windows.Forms.CheckBox();
            this.spieler1_anfang_cbx = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gewinnbedingung_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hoehe_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.breite_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ki_nud)).BeginInit();
            this.SuspendLayout();
            // 
            // gewinnbedingung_nud
            // 
            this.gewinnbedingung_nud.Location = new System.Drawing.Point(125, 197);
            this.gewinnbedingung_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gewinnbedingung_nud.Name = "gewinnbedingung_nud";
            this.gewinnbedingung_nud.Size = new System.Drawing.Size(31, 20);
            this.gewinnbedingung_nud.TabIndex = 21;
            this.gewinnbedingung_nud.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Gewinnbedingung:";
            // 
            // hoehe_nud
            // 
            this.hoehe_nud.Location = new System.Drawing.Point(67, 180);
            this.hoehe_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hoehe_nud.Name = "hoehe_nud";
            this.hoehe_nud.Size = new System.Drawing.Size(25, 20);
            this.hoehe_nud.TabIndex = 20;
            this.hoehe_nud.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // breite_nud
            // 
            this.breite_nud.Location = new System.Drawing.Point(67, 157);
            this.breite_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.breite_nud.Name = "breite_nud";
            this.breite_nud.Size = new System.Drawing.Size(25, 20);
            this.breite_nud.TabIndex = 19;
            this.breite_nud.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // hoehe_lbl
            // 
            this.hoehe_lbl.AutoSize = true;
            this.hoehe_lbl.Location = new System.Drawing.Point(24, 182);
            this.hoehe_lbl.Name = "hoehe_lbl";
            this.hoehe_lbl.Size = new System.Drawing.Size(36, 13);
            this.hoehe_lbl.TabIndex = 21;
            this.hoehe_lbl.Text = "Höhe:";
            // 
            // breite_lbl
            // 
            this.breite_lbl.AutoSize = true;
            this.breite_lbl.Location = new System.Drawing.Point(24, 159);
            this.breite_lbl.Name = "breite_lbl";
            this.breite_lbl.Size = new System.Drawing.Size(37, 13);
            this.breite_lbl.TabIndex = 20;
            this.breite_lbl.Text = "Breite:";
            // 
            // ki_lbl
            // 
            this.ki_lbl.AutoSize = true;
            this.ki_lbl.Enabled = false;
            this.ki_lbl.Location = new System.Drawing.Point(38, 131);
            this.ki_lbl.Name = "ki_lbl";
            this.ki_lbl.Size = new System.Drawing.Size(20, 13);
            this.ki_lbl.TabIndex = 19;
            this.ki_lbl.Text = "KI:";
            // 
            // ki_nud
            // 
            this.ki_nud.Enabled = false;
            this.ki_nud.Location = new System.Drawing.Point(64, 129);
            this.ki_nud.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ki_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ki_nud.Name = "ki_nud";
            this.ki_nud.Size = new System.Drawing.Size(28, 20);
            this.ki_nud.TabIndex = 18;
            this.ki_nud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spieler2_ki_cbx
            // 
            this.spieler2_ki_cbx.AutoSize = true;
            this.spieler2_ki_cbx.Location = new System.Drawing.Point(28, 79);
            this.spieler2_ki_cbx.Name = "spieler2_ki_cbx";
            this.spieler2_ki_cbx.Size = new System.Drawing.Size(122, 17);
            this.spieler2_ki_cbx.TabIndex = 17;
            this.spieler2_ki_cbx.Text = "Spieler 2, sei eine KI";
            this.spieler2_ki_cbx.UseVisualStyleBackColor = true;
            this.spieler2_ki_cbx.CheckedChanged += new System.EventHandler(this.spieler2_ki_cbx_CheckedChanged_1);
            // 
            // spieler2_tbx
            // 
            this.spieler2_tbx.Location = new System.Drawing.Point(79, 53);
            this.spieler2_tbx.Name = "spieler2_tbx";
            this.spieler2_tbx.Size = new System.Drawing.Size(14, 20);
            this.spieler2_tbx.TabIndex = 16;
            this.spieler2_tbx.Text = "O";
            // 
            // spieler1_tbx
            // 
            this.spieler1_tbx.Location = new System.Drawing.Point(79, 27);
            this.spieler1_tbx.Name = "spieler1_tbx";
            this.spieler1_tbx.Size = new System.Drawing.Size(14, 20);
            this.spieler1_tbx.TabIndex = 15;
            this.spieler1_tbx.Text = "X";
            // 
            // spieler2_lbl
            // 
            this.spieler2_lbl.AutoSize = true;
            this.spieler2_lbl.Location = new System.Drawing.Point(25, 56);
            this.spieler2_lbl.Name = "spieler2_lbl";
            this.spieler2_lbl.Size = new System.Drawing.Size(48, 13);
            this.spieler2_lbl.TabIndex = 14;
            this.spieler2_lbl.Text = "Spieler 2";
            // 
            // spieler1_lbl
            // 
            this.spieler1_lbl.AutoSize = true;
            this.spieler1_lbl.Location = new System.Drawing.Point(25, 27);
            this.spieler1_lbl.Name = "spieler1_lbl";
            this.spieler1_lbl.Size = new System.Drawing.Size(48, 13);
            this.spieler1_lbl.TabIndex = 13;
            this.spieler1_lbl.Text = "Spieler 1";
            // 
            // online_modus_cbx
            // 
            this.online_modus_cbx.AutoSize = true;
            this.online_modus_cbx.Location = new System.Drawing.Point(27, 233);
            this.online_modus_cbx.Name = "online_modus_cbx";
            this.online_modus_cbx.Size = new System.Drawing.Size(91, 17);
            this.online_modus_cbx.TabIndex = 22;
            this.online_modus_cbx.Text = "Online Modus";
            this.online_modus_cbx.UseVisualStyleBackColor = true;
            this.online_modus_cbx.CheckedChanged += new System.EventHandler(this.online_modus_cbx_CheckedChanged);
            // 
            // protokol_lbl
            // 
            this.protokol_lbl.AutoSize = true;
            this.protokol_lbl.Enabled = false;
            this.protokol_lbl.Location = new System.Drawing.Point(25, 256);
            this.protokol_lbl.Name = "protokol_lbl";
            this.protokol_lbl.Size = new System.Drawing.Size(51, 13);
            this.protokol_lbl.TabIndex = 27;
            this.protokol_lbl.Text = "Protokoll:";
            // 
            // protokoll_lbx
            // 
            this.protokoll_lbx.Enabled = false;
            this.protokoll_lbx.FormattingEnabled = true;
            this.protokoll_lbx.Items.AddRange(new object[] {
            "IRC",
            "TCP/IP"});
            this.protokoll_lbx.Location = new System.Drawing.Point(82, 256);
            this.protokoll_lbx.Name = "protokoll_lbx";
            this.protokoll_lbx.Size = new System.Drawing.Size(104, 30);
            this.protokoll_lbx.Sorted = true;
            this.protokoll_lbx.TabIndex = 23;
            // 
            // host_lbl
            // 
            this.host_lbl.AutoSize = true;
            this.host_lbl.Enabled = false;
            this.host_lbl.Location = new System.Drawing.Point(26, 299);
            this.host_lbl.Name = "host_lbl";
            this.host_lbl.Size = new System.Drawing.Size(32, 13);
            this.host_lbl.TabIndex = 29;
            this.host_lbl.Text = "Host:";
            // 
            // host_tbx
            // 
            this.host_tbx.Enabled = false;
            this.host_tbx.Location = new System.Drawing.Point(64, 292);
            this.host_tbx.Name = "host_tbx";
            this.host_tbx.Size = new System.Drawing.Size(100, 20);
            this.host_tbx.TabIndex = 24;
            // 
            // abbrechen_btn
            // 
            this.abbrechen_btn.Location = new System.Drawing.Point(152, 404);
            this.abbrechen_btn.Name = "abbrechen_btn";
            this.abbrechen_btn.Size = new System.Drawing.Size(75, 23);
            this.abbrechen_btn.TabIndex = 28;
            this.abbrechen_btn.Text = "Abbrechen";
            this.abbrechen_btn.UseVisualStyleBackColor = true;
            this.abbrechen_btn.Click += new System.EventHandler(this.abbrechen_btn_Click);
            // 
            // ok_btn
            // 
            this.ok_btn.Location = new System.Drawing.Point(71, 404);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(75, 23);
            this.ok_btn.TabIndex = 27;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // port_lbl
            // 
            this.port_lbl.AutoSize = true;
            this.port_lbl.Enabled = false;
            this.port_lbl.Location = new System.Drawing.Point(25, 325);
            this.port_lbl.Name = "port_lbl";
            this.port_lbl.Size = new System.Drawing.Size(29, 13);
            this.port_lbl.TabIndex = 33;
            this.port_lbl.Text = "Port:";
            // 
            // port_tbx
            // 
            this.port_tbx.Enabled = false;
            this.port_tbx.Location = new System.Drawing.Point(64, 325);
            this.port_tbx.Name = "port_tbx";
            this.port_tbx.Size = new System.Drawing.Size(100, 20);
            this.port_tbx.TabIndex = 25;
            // 
            // server_cbx
            // 
            this.server_cbx.AutoSize = true;
            this.server_cbx.Enabled = false;
            this.server_cbx.Location = new System.Drawing.Point(29, 351);
            this.server_cbx.Name = "server_cbx";
            this.server_cbx.Size = new System.Drawing.Size(57, 17);
            this.server_cbx.TabIndex = 26;
            this.server_cbx.Text = "Server";
            this.server_cbx.UseVisualStyleBackColor = true;
            // 
            // spieler2_anfang_cbx
            // 
            this.spieler2_anfang_cbx.AutoSize = true;
            this.spieler2_anfang_cbx.Location = new System.Drawing.Point(29, 102);
            this.spieler2_anfang_cbx.Name = "spieler2_anfang_cbx";
            this.spieler2_anfang_cbx.Size = new System.Drawing.Size(109, 17);
            this.spieler2_anfang_cbx.TabIndex = 34;
            this.spieler2_anfang_cbx.Text = "Spieler 2 fängt an";
            this.spieler2_anfang_cbx.UseVisualStyleBackColor = true;
            this.spieler2_anfang_cbx.CheckedChanged += new System.EventHandler(this.spieler2_anfang_CheckedChanged);
            // 
            // spieler1_anfang_cbx
            // 
            this.spieler1_anfang_cbx.AutoSize = true;
            this.spieler1_anfang_cbx.Enabled = false;
            this.spieler1_anfang_cbx.Location = new System.Drawing.Point(29, 375);
            this.spieler1_anfang_cbx.Name = "spieler1_anfang_cbx";
            this.spieler1_anfang_cbx.Size = new System.Drawing.Size(70, 17);
            this.spieler1_anfang_cbx.TabIndex = 35;
            this.spieler1_anfang_cbx.Text = "1. Spieler";
            this.spieler1_anfang_cbx.UseVisualStyleBackColor = true;
            // 
            // NewGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(239, 439);
            this.Controls.Add(this.spieler1_anfang_cbx);
            this.Controls.Add(this.spieler2_anfang_cbx);
            this.Controls.Add(this.server_cbx);
            this.Controls.Add(this.port_tbx);
            this.Controls.Add(this.port_lbl);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.abbrechen_btn);
            this.Controls.Add(this.host_tbx);
            this.Controls.Add(this.host_lbl);
            this.Controls.Add(this.protokoll_lbx);
            this.Controls.Add(this.protokol_lbl);
            this.Controls.Add(this.online_modus_cbx);
            this.Controls.Add(this.gewinnbedingung_nud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hoehe_nud);
            this.Controls.Add(this.breite_nud);
            this.Controls.Add(this.hoehe_lbl);
            this.Controls.Add(this.breite_lbl);
            this.Controls.Add(this.ki_lbl);
            this.Controls.Add(this.ki_nud);
            this.Controls.Add(this.spieler2_ki_cbx);
            this.Controls.Add(this.spieler2_tbx);
            this.Controls.Add(this.spieler1_tbx);
            this.Controls.Add(this.spieler2_lbl);
            this.Controls.Add(this.spieler1_lbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewGameForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Neues Spiel";
            ((System.ComponentModel.ISupportInitialize)(this.gewinnbedingung_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hoehe_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.breite_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ki_nud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown gewinnbedingung_nud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown hoehe_nud;
        private System.Windows.Forms.NumericUpDown breite_nud;
        private System.Windows.Forms.Label hoehe_lbl;
        private System.Windows.Forms.Label breite_lbl;
        private System.Windows.Forms.Label ki_lbl;
        private System.Windows.Forms.NumericUpDown ki_nud;
        private System.Windows.Forms.CheckBox spieler2_ki_cbx;
        private System.Windows.Forms.TextBox spieler2_tbx;
        private System.Windows.Forms.TextBox spieler1_tbx;
        private System.Windows.Forms.Label spieler2_lbl;
        private System.Windows.Forms.Label spieler1_lbl;
        private System.Windows.Forms.CheckBox online_modus_cbx;
        private System.Windows.Forms.Label protokol_lbl;
        private System.Windows.Forms.ListBox protokoll_lbx;
        private System.Windows.Forms.Label host_lbl;
        private System.Windows.Forms.TextBox host_tbx;
        private System.Windows.Forms.Button abbrechen_btn;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Label port_lbl;
        private System.Windows.Forms.TextBox port_tbx;
        private System.Windows.Forms.CheckBox server_cbx;
        private System.Windows.Forms.CheckBox spieler2_anfang_cbx;
        private System.Windows.Forms.CheckBox spieler1_anfang_cbx;



    }
}