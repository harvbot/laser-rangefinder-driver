namespace Harvbot.Rangefinder.TestApp
{
    partial class MainForm
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
            this.CbPorts = new System.Windows.Forms.ComboBox();
            this.LblComPort = new System.Windows.Forms.Label();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LsMeasurement = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // CbPorts
            // 
            this.CbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbPorts.FormattingEnabled = true;
            this.CbPorts.Location = new System.Drawing.Point(12, 25);
            this.CbPorts.Name = "CbPorts";
            this.CbPorts.Size = new System.Drawing.Size(146, 21);
            this.CbPorts.TabIndex = 0;
            // 
            // LblComPort
            // 
            this.LblComPort.AutoSize = true;
            this.LblComPort.Location = new System.Drawing.Point(12, 9);
            this.LblComPort.Name = "LblComPort";
            this.LblComPort.Size = new System.Drawing.Size(56, 13);
            this.LblComPort.TabIndex = 1;
            this.LblComPort.Text = "COM Port:";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(12, 52);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(146, 23);
            this.BtnStart.TabIndex = 2;
            this.BtnStart.Text = "Start Measurement";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(12, 81);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(146, 23);
            this.BtnStop.TabIndex = 3;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // LsMeasurement
            // 
            this.LsMeasurement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LsMeasurement.FormattingEnabled = true;
            this.LsMeasurement.Location = new System.Drawing.Point(164, 9);
            this.LsMeasurement.Name = "LsMeasurement";
            this.LsMeasurement.Size = new System.Drawing.Size(365, 368);
            this.LsMeasurement.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 384);
            this.Controls.Add(this.LsMeasurement);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.LblComPort);
            this.Controls.Add(this.CbPorts);
            this.Name = "MainForm";
            this.Text = "Laser Rangefinder TestApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CbPorts;
        private System.Windows.Forms.Label LblComPort;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.ListBox LsMeasurement;
    }
}

