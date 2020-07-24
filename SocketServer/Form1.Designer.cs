namespace SocketServer
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.lb_server = new System.Windows.Forms.ListBox();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.lb_port = new System.Windows.Forms.Label();
            this.lb_ip = new System.Windows.Forms.Label();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start server SOCKET";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lb_server
            // 
            this.lb_server.Dock = System.Windows.Forms.DockStyle.Top;
            this.lb_server.FormattingEnabled = true;
            this.lb_server.HorizontalScrollbar = true;
            this.lb_server.Location = new System.Drawing.Point(0, 0);
            this.lb_server.Name = "lb_server";
            this.lb_server.ScrollAlwaysVisible = true;
            this.lb_server.Size = new System.Drawing.Size(383, 407);
            this.lb_server.TabIndex = 1;
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(138, 422);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(100, 20);
            this.tb_port.TabIndex = 5;
            this.tb_port.Text = "8090";
            // 
            // lb_port
            // 
            this.lb_port.AutoSize = true;
            this.lb_port.Location = new System.Drawing.Point(112, 424);
            this.lb_port.Name = "lb_port";
            this.lb_port.Size = new System.Drawing.Size(25, 13);
            this.lb_port.TabIndex = 6;
            this.lb_port.Text = "port";
            // 
            // lb_ip
            // 
            this.lb_ip.AutoSize = true;
            this.lb_ip.Location = new System.Drawing.Point(116, 450);
            this.lb_ip.Name = "lb_ip";
            this.lb_ip.Size = new System.Drawing.Size(15, 13);
            this.lb_ip.TabIndex = 8;
            this.lb_ip.Text = "ip";
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(138, 448);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(100, 20);
            this.tb_ip.TabIndex = 7;
            this.tb_ip.Text = "localhost";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(274, 424);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 44);
            this.button2.TabIndex = 9;
            this.button2.Text = "Start server TCP";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 492);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Text";
            // 
            // tb_text
            // 
            this.tb_text.Location = new System.Drawing.Point(138, 489);
            this.tb_text.Name = "tb_text";
            this.tb_text.Size = new System.Drawing.Size(100, 20);
            this.tb_text.TabIndex = 10;
            this.tb_text.Text = "hello from server";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 521);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_text);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lb_ip);
            this.Controls.Add(this.tb_ip);
            this.Controls.Add(this.lb_port);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.lb_server);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lb_server;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label lb_port;
        private System.Windows.Forms.Label lb_ip;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_text;
    }
}

