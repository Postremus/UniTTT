namespace UniTTT.Gui
{
    partial class ProfiModeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.parameter_tbx = new System.Windows.Forms.TextBox();
            this.ok_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Geben Sie die Parameter für den Start von UniTTT ein";
            // 
            // parameter_tbx
            // 
            this.parameter_tbx.Location = new System.Drawing.Point(15, 54);
            this.parameter_tbx.Name = "parameter_tbx";
            this.parameter_tbx.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.parameter_tbx.Size = new System.Drawing.Size(257, 20);
            this.parameter_tbx.TabIndex = 1;
            // 
            // ok_btn
            // 
            this.ok_btn.Location = new System.Drawing.Point(101, 80);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(75, 23);
            this.ok_btn.TabIndex = 2;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // ProfiModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 138);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.parameter_tbx);
            this.Controls.Add(this.label1);
            this.Name = "ProfiModeForm";
            this.Text = "ProfiModeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox parameter_tbx;
        private System.Windows.Forms.Button ok_btn;
    }
}