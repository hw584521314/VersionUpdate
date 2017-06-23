namespace VersionUpdateManager
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.isAutoStart = new System.Windows.Forms.CheckBox();
            this.cancel = new System.Windows.Forms.Button();
            this.Update = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.logWin = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "状态：";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.status.Location = new System.Drawing.Point(60, 240);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(55, 14);
            this.status.TabIndex = 1;
            this.status.Text = "等待更新";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(23, 13);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(378, 100);
            this.checkedListBox1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logWin);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.isAutoStart);
            this.panel1.Controls.Add(this.cancel);
            this.panel1.Controls.Add(this.Update);
            this.panel1.Controls.Add(this.checkedListBox1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(532, 225);
            this.panel1.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(23, 192);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(378, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            // 
            // isAutoStart
            // 
            this.isAutoStart.AutoSize = true;
            this.isAutoStart.Checked = true;
            this.isAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isAutoStart.Location = new System.Drawing.Point(408, 192);
            this.isAutoStart.Name = "isAutoStart";
            this.isAutoStart.Size = new System.Drawing.Size(108, 16);
            this.isAutoStart.TabIndex = 5;
            this.isAutoStart.Text = "升级后启动程序";
            this.isAutoStart.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(434, 43);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "退出";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // Update
            // 
            this.Update.Location = new System.Drawing.Point(434, 13);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(75, 23);
            this.Update.TabIndex = 3;
            this.Update.Text = "开始更新";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // logWin
            // 
            this.logWin.FormattingEnabled = true;
            this.logWin.ItemHeight = 12;
            this.logWin.Location = new System.Drawing.Point(23, 120);
            this.logWin.Name = "logWin";
            this.logWin.Size = new System.Drawing.Size(378, 64);
            this.logWin.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 261);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "升级管理器";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox isAutoStart;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox logWin;
    }
}

