namespace AuditRecordSender
{
    partial class AuditMessageSender
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
            this.components = new System.ComponentModel.Container();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(35, 21);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 0;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(12, 28);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(17, 13);
            this.lblIP.TabIndex = 1;
            this.lblIP.Text = "IP";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(157, 24);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(198, 21);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 3;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(315, 24);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(34, 13);
            this.lblMode.TabIndex = 4;
            this.lblMode.Text = "Mode";
            // 
            // cmbMode
            // 
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Location = new System.Drawing.Point(365, 20);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(121, 21);
            this.cmbMode.TabIndex = 6;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(492, 18);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(15, 75);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(551, 175);
            this.txtMessage.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "No. of times to send";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(132, 49);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(100, 20);
            this.txtCount.TabIndex = 12;
            // 
            // AuditMessageSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 262);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cmbMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.txtIP);
            this.Name = "AuditMessageSender";
            this.Text = "AuditMessageSender";
            this.Load += new System.EventHandler(this.AuditMessageSender_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCount;
    }
}