namespace Wipai_app
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.StatusBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.receiveDatarichTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.BtnSendOnTime = new System.Windows.Forms.Button();
            this.Btn0hooseDevice = new System.Windows.Forms.Button();
            this.DeviceCheckedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.BtnOpenServer = new System.Windows.Forms.Button();
            this.radioBtnchooseTCP = new System.Windows.Forms.RadioButton();
            this.radioBtnchooseUDP = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IPBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnCloseServer = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CmdBox = new System.Windows.Forms.ComboBox();
            this.IDBox = new System.Windows.Forms.TextBox();
            this.BtnSendCmd = new System.Windows.Forms.Button();
            this.BtnGetData = new System.Windows.Forms.Button();
            this.radioBtnWindowShow = new System.Windows.Forms.RadioButton();
            this.radioBtnSaveFile = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.APnameBox = new System.Windows.Forms.TextBox();
            this.APpasswordBox = new System.Windows.Forms.TextBox();
            this.BtnSetAPName = new System.Windows.Forms.Button();
            this.HourBox = new System.Windows.Forms.TextBox();
            this.BtnSetCaptime = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.MinuteBox = new System.Windows.Forms.TextBox();
            this.SecondBox = new System.Windows.Forms.TextBox();
            this.MsBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.BtnSetAPpassword = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.IPtextBox1 = new System.Windows.Forms.TextBox();
            this.PortextBox = new System.Windows.Forms.TextBox();
            this.BtnSetIPname = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxCloseTime = new System.Windows.Forms.TextBox();
            this.BtnSetOpenAndCloseTime = new System.Windows.Forms.Button();
            this.textBoxOpenTime = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.BtnSetPort = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.IPtextBox4 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.IPtextBox3 = new System.Windows.Forms.TextBox();
            this.IPtextBox2 = new System.Windows.Forms.TextBox();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBar1.Location = new System.Drawing.Point(190, 19);
            this.progressBar1.Maximum = 600;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(888, 22);
            this.progressBar1.TabIndex = 5;
            // 
            // StatusBox
            // 
            this.StatusBox.Location = new System.Drawing.Point(47, 20);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(128, 21);
            this.StatusBox.TabIndex = 6;
            this.StatusBox.Text = "null";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "状态：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.receiveDatarichTextBox);
            this.groupBox4.Location = new System.Drawing.Point(204, 203);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(900, 591);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据接收";
            // 
            // receiveDatarichTextBox
            // 
            this.receiveDatarichTextBox.Location = new System.Drawing.Point(16, 21);
            this.receiveDatarichTextBox.Name = "receiveDatarichTextBox";
            this.receiveDatarichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.receiveDatarichTextBox.Size = new System.Drawing.Size(878, 564);
            this.receiveDatarichTextBox.TabIndex = 0;
            this.receiveDatarichTextBox.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.BtnSendOnTime);
            this.groupBox5.Controls.Add(this.Btn0hooseDevice);
            this.groupBox5.Controls.Add(this.DeviceCheckedListBox1);
            this.groupBox5.Location = new System.Drawing.Point(14, 203);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(175, 591);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "设备列表";
            // 
            // BtnSendOnTime
            // 
            this.BtnSendOnTime.Location = new System.Drawing.Point(10, 562);
            this.BtnSendOnTime.Name = "BtnSendOnTime";
            this.BtnSendOnTime.Size = new System.Drawing.Size(145, 23);
            this.BtnSendOnTime.TabIndex = 16;
            this.BtnSendOnTime.Text = "定时发送";
            this.BtnSendOnTime.UseVisualStyleBackColor = true;
            this.BtnSendOnTime.Click += new System.EventHandler(this.BtnSendOnTime_Click);
            // 
            // Btn0hooseDevice
            // 
            this.Btn0hooseDevice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn0hooseDevice.Location = new System.Drawing.Point(10, 495);
            this.Btn0hooseDevice.Name = "Btn0hooseDevice";
            this.Btn0hooseDevice.Size = new System.Drawing.Size(145, 25);
            this.Btn0hooseDevice.TabIndex = 15;
            this.Btn0hooseDevice.Text = "选定设备";
            this.Btn0hooseDevice.UseVisualStyleBackColor = true;
            this.Btn0hooseDevice.Click += new System.EventHandler(this.BtnChooseDevice_Click);
            // 
            // DeviceCheckedListBox1
            // 
            this.DeviceCheckedListBox1.FormattingEnabled = true;
            this.DeviceCheckedListBox1.Location = new System.Drawing.Point(10, 21);
            this.DeviceCheckedListBox1.Name = "DeviceCheckedListBox1";
            this.DeviceCheckedListBox1.Size = new System.Drawing.Size(145, 468);
            this.DeviceCheckedListBox1.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.StatusBox);
            this.groupBox6.Controls.Add(this.progressBar1);
            this.groupBox6.Location = new System.Drawing.Point(14, 800);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1084, 53);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "当前状态";
            // 
            // BtnOpenServer
            // 
            this.BtnOpenServer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnOpenServer.Location = new System.Drawing.Point(17, 29);
            this.BtnOpenServer.Name = "BtnOpenServer";
            this.BtnOpenServer.Size = new System.Drawing.Size(52, 22);
            this.BtnOpenServer.TabIndex = 0;
            this.BtnOpenServer.Text = "打开";
            this.BtnOpenServer.UseVisualStyleBackColor = true;
            this.BtnOpenServer.Click += new System.EventHandler(this.BtnOpenServer_Click);
            // 
            // radioBtnchooseTCP
            // 
            this.radioBtnchooseTCP.AutoSize = true;
            this.radioBtnchooseTCP.Checked = true;
            this.radioBtnchooseTCP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioBtnchooseTCP.Location = new System.Drawing.Point(17, 57);
            this.radioBtnchooseTCP.Name = "radioBtnchooseTCP";
            this.radioBtnchooseTCP.Size = new System.Drawing.Size(41, 16);
            this.radioBtnchooseTCP.TabIndex = 1;
            this.radioBtnchooseTCP.TabStop = true;
            this.radioBtnchooseTCP.Text = "TCP";
            this.radioBtnchooseTCP.UseVisualStyleBackColor = true;
            // 
            // radioBtnchooseUDP
            // 
            this.radioBtnchooseUDP.AutoSize = true;
            this.radioBtnchooseUDP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioBtnchooseUDP.Location = new System.Drawing.Point(105, 57);
            this.radioBtnchooseUDP.Name = "radioBtnchooseUDP";
            this.radioBtnchooseUDP.Size = new System.Drawing.Size(41, 16);
            this.radioBtnchooseUDP.TabIndex = 2;
            this.radioBtnchooseUDP.TabStop = true;
            this.radioBtnchooseUDP.Text = "UDP";
            this.radioBtnchooseUDP.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(17, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "服务器IP：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(17, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "服务器端口：";
            // 
            // IPBox
            // 
            this.IPBox.Location = new System.Drawing.Point(19, 96);
            this.IPBox.Name = "IPBox";
            this.IPBox.Size = new System.Drawing.Size(127, 21);
            this.IPBox.TabIndex = 5;
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(17, 135);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(127, 21);
            this.PortBox.TabIndex = 6;
            this.PortBox.Text = "8085";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnCloseServer);
            this.groupBox1.Controls.Add(this.PortBox);
            this.groupBox1.Controls.Add(this.IPBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioBtnchooseUDP);
            this.groupBox1.Controls.Add(this.radioBtnchooseTCP);
            this.groupBox1.Controls.Add(this.BtnOpenServer);
            this.groupBox1.Location = new System.Drawing.Point(14, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 175);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器设置";
            // 
            // BtnCloseServer
            // 
            this.BtnCloseServer.Location = new System.Drawing.Point(105, 29);
            this.BtnCloseServer.Name = "BtnCloseServer";
            this.BtnCloseServer.Size = new System.Drawing.Size(50, 23);
            this.BtnCloseServer.TabIndex = 7;
            this.BtnCloseServer.Text = "关闭";
            this.BtnCloseServer.UseVisualStyleBackColor = true;
            this.BtnCloseServer.Click += new System.EventHandler(this.BtnCloseServer_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(25, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "当前设备ID：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(25, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "选择指令：";
            // 
            // CmdBox
            // 
            this.CmdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmdBox.FormattingEnabled = true;
            this.CmdBox.Location = new System.Drawing.Point(109, 57);
            this.CmdBox.Name = "CmdBox";
            this.CmdBox.Size = new System.Drawing.Size(190, 20);
            this.CmdBox.TabIndex = 6;
            // 
            // IDBox
            // 
            this.IDBox.Location = new System.Drawing.Point(109, 21);
            this.IDBox.Name = "IDBox";
            this.IDBox.Size = new System.Drawing.Size(265, 21);
            this.IDBox.TabIndex = 7;
            // 
            // BtnSendCmd
            // 
            this.BtnSendCmd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSendCmd.Location = new System.Drawing.Point(305, 50);
            this.BtnSendCmd.Name = "BtnSendCmd";
            this.BtnSendCmd.Size = new System.Drawing.Size(69, 33);
            this.BtnSendCmd.TabIndex = 8;
            this.BtnSendCmd.Text = "发送指令";
            this.BtnSendCmd.UseVisualStyleBackColor = true;
            this.BtnSendCmd.Click += new System.EventHandler(this.BtnSendCmd_Click);
            // 
            // BtnGetData
            // 
            this.BtnGetData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnGetData.Location = new System.Drawing.Point(204, 101);
            this.BtnGetData.Name = "BtnGetData";
            this.BtnGetData.Size = new System.Drawing.Size(72, 60);
            this.BtnGetData.TabIndex = 9;
            this.BtnGetData.Text = "立即采样";
            this.BtnGetData.UseVisualStyleBackColor = true;
            this.BtnGetData.Click += new System.EventHandler(this.BtnGetData_Click);
            // 
            // radioBtnWindowShow
            // 
            this.radioBtnWindowShow.AutoSize = true;
            this.radioBtnWindowShow.Checked = true;
            this.radioBtnWindowShow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioBtnWindowShow.Location = new System.Drawing.Point(109, 103);
            this.radioBtnWindowShow.Name = "radioBtnWindowShow";
            this.radioBtnWindowShow.Size = new System.Drawing.Size(71, 16);
            this.radioBtnWindowShow.TabIndex = 10;
            this.radioBtnWindowShow.TabStop = true;
            this.radioBtnWindowShow.Text = "窗口显示";
            this.radioBtnWindowShow.UseVisualStyleBackColor = true;
            // 
            // radioBtnSaveFile
            // 
            this.radioBtnSaveFile.AutoSize = true;
            this.radioBtnSaveFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioBtnSaveFile.Location = new System.Drawing.Point(109, 140);
            this.radioBtnSaveFile.Name = "radioBtnSaveFile";
            this.radioBtnSaveFile.Size = new System.Drawing.Size(83, 16);
            this.radioBtnSaveFile.TabIndex = 11;
            this.radioBtnSaveFile.TabStop = true;
            this.radioBtnSaveFile.Text = "保存到文件";
            this.radioBtnSaveFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.radioBtnSaveFile);
            this.groupBox2.Controls.Add(this.radioBtnWindowShow);
            this.groupBox2.Controls.Add(this.BtnGetData);
            this.groupBox2.Controls.Add(this.BtnSendCmd);
            this.groupBox2.Controls.Add(this.IDBox);
            this.groupBox2.Controls.Add(this.CmdBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(204, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 175);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "功能操作";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 58);
            this.button1.TabIndex = 12;
            this.button1.Text = "上传数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(6, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "设置采样时间：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(6, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "设置AP名称：";
            // 
            // APnameBox
            // 
            this.APnameBox.Location = new System.Drawing.Point(89, 62);
            this.APnameBox.Name = "APnameBox";
            this.APnameBox.Size = new System.Drawing.Size(70, 21);
            this.APnameBox.TabIndex = 2;
            // 
            // APpasswordBox
            // 
            this.APpasswordBox.Location = new System.Drawing.Point(90, 89);
            this.APpasswordBox.Name = "APpasswordBox";
            this.APpasswordBox.Size = new System.Drawing.Size(69, 21);
            this.APpasswordBox.TabIndex = 3;
            // 
            // BtnSetAPName
            // 
            this.BtnSetAPName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSetAPName.Location = new System.Drawing.Point(165, 63);
            this.BtnSetAPName.Name = "BtnSetAPName";
            this.BtnSetAPName.Size = new System.Drawing.Size(38, 23);
            this.BtnSetAPName.TabIndex = 4;
            this.BtnSetAPName.Text = "确定";
            this.BtnSetAPName.UseVisualStyleBackColor = true;
            this.BtnSetAPName.Click += new System.EventHandler(this.BtnSetAPName_Click);
            // 
            // HourBox
            // 
            this.HourBox.Location = new System.Drawing.Point(90, 35);
            this.HourBox.Name = "HourBox";
            this.HourBox.Size = new System.Drawing.Size(37, 21);
            this.HourBox.TabIndex = 5;
            // 
            // BtnSetCaptime
            // 
            this.BtnSetCaptime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSetCaptime.Location = new System.Drawing.Point(314, 33);
            this.BtnSetCaptime.Name = "BtnSetCaptime";
            this.BtnSetCaptime.Size = new System.Drawing.Size(52, 23);
            this.BtnSetCaptime.TabIndex = 8;
            this.BtnSetCaptime.Text = "确定";
            this.BtnSetCaptime.UseVisualStyleBackColor = true;
            this.BtnSetCaptime.Click += new System.EventHandler(this.BtnSetCaptime_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(88, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "小时";
            // 
            // MinuteBox
            // 
            this.MinuteBox.Location = new System.Drawing.Point(151, 35);
            this.MinuteBox.Name = "MinuteBox";
            this.MinuteBox.Size = new System.Drawing.Size(37, 21);
            this.MinuteBox.TabIndex = 10;
            // 
            // SecondBox
            // 
            this.SecondBox.Location = new System.Drawing.Point(206, 36);
            this.SecondBox.Name = "SecondBox";
            this.SecondBox.Size = new System.Drawing.Size(37, 21);
            this.SecondBox.TabIndex = 11;
            // 
            // MsBox
            // 
            this.MsBox.Location = new System.Drawing.Point(261, 35);
            this.MsBox.Name = "MsBox";
            this.MsBox.Size = new System.Drawing.Size(37, 21);
            this.MsBox.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(149, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "分钟";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(213, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "秒";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(259, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 15;
            this.label11.Text = "毫秒";
            // 
            // BtnSetAPpassword
            // 
            this.BtnSetAPpassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSetAPpassword.Location = new System.Drawing.Point(165, 89);
            this.BtnSetAPpassword.Name = "BtnSetAPpassword";
            this.BtnSetAPpassword.Size = new System.Drawing.Size(38, 23);
            this.BtnSetAPpassword.TabIndex = 16;
            this.BtnSetAPpassword.Text = "确定";
            this.BtnSetAPpassword.UseVisualStyleBackColor = true;
            this.BtnSetAPpassword.Click += new System.EventHandler(this.BtnSetAPpassword_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(7, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 17;
            this.label12.Text = "设置AP密码：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(7, 120);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 18;
            this.label13.Text = "设置IP：";
            // 
            // IPtextBox1
            // 
            this.IPtextBox1.Location = new System.Drawing.Point(58, 116);
            this.IPtextBox1.Name = "IPtextBox1";
            this.IPtextBox1.Size = new System.Drawing.Size(33, 21);
            this.IPtextBox1.TabIndex = 19;
            // 
            // PortextBox
            // 
            this.PortextBox.Location = new System.Drawing.Point(89, 146);
            this.PortextBox.Name = "PortextBox";
            this.PortextBox.Size = new System.Drawing.Size(209, 21);
            this.PortextBox.TabIndex = 20;
            // 
            // BtnSetIPname
            // 
            this.BtnSetIPname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSetIPname.Location = new System.Drawing.Point(314, 114);
            this.BtnSetIPname.Name = "BtnSetIPname";
            this.BtnSetIPname.Size = new System.Drawing.Size(52, 23);
            this.BtnSetIPname.TabIndex = 21;
            this.BtnSetIPname.Text = "确定";
            this.BtnSetIPname.UseVisualStyleBackColor = true;
            this.BtnSetIPname.Click += new System.EventHandler(this.BtnSetIPnameAndPort_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.textBoxCloseTime);
            this.groupBox3.Controls.Add(this.BtnSetOpenAndCloseTime);
            this.groupBox3.Controls.Add(this.textBoxOpenTime);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.BtnSetPort);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.IPtextBox4);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.IPtextBox3);
            this.groupBox3.Controls.Add(this.IPtextBox2);
            this.groupBox3.Controls.Add(this.BtnSetIPname);
            this.groupBox3.Controls.Add(this.PortextBox);
            this.groupBox3.Controls.Add(this.IPtextBox1);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.BtnSetAPpassword);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.MsBox);
            this.groupBox3.Controls.Add(this.SecondBox);
            this.groupBox3.Controls.Add(this.MinuteBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.BtnSetCaptime);
            this.groupBox3.Controls.Add(this.HourBox);
            this.groupBox3.Controls.Add(this.BtnSetAPName);
            this.groupBox3.Controls.Add(this.APpasswordBox);
            this.groupBox3.Controls.Add(this.APnameBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(602, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(491, 175);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "采样设置";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label20.Location = new System.Drawing.Point(385, 74);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 43;
            this.label20.Text = "关闭时长";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(333, 74);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 42;
            this.label19.Text = "开启时长";
            // 
            // textBoxCloseTime
            // 
            this.textBoxCloseTime.Location = new System.Drawing.Point(387, 91);
            this.textBoxCloseTime.Name = "textBoxCloseTime";
            this.textBoxCloseTime.Size = new System.Drawing.Size(46, 21);
            this.textBoxCloseTime.TabIndex = 41;
            // 
            // BtnSetOpenAndCloseTime
            // 
            this.BtnSetOpenAndCloseTime.Location = new System.Drawing.Point(443, 92);
            this.BtnSetOpenAndCloseTime.Name = "BtnSetOpenAndCloseTime";
            this.BtnSetOpenAndCloseTime.Size = new System.Drawing.Size(42, 23);
            this.BtnSetOpenAndCloseTime.TabIndex = 40;
            this.BtnSetOpenAndCloseTime.Text = "确定";
            this.BtnSetOpenAndCloseTime.UseVisualStyleBackColor = true;
            this.BtnSetOpenAndCloseTime.Click += new System.EventHandler(this.BtnSetOpenAndCloseTime_Click_1);
            // 
            // textBoxOpenTime
            // 
            this.textBoxOpenTime.Location = new System.Drawing.Point(335, 91);
            this.textBoxOpenTime.Name = "textBoxOpenTime";
            this.textBoxOpenTime.Size = new System.Drawing.Size(46, 21);
            this.textBoxOpenTime.TabIndex = 36;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(204, 94);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(125, 12);
            this.label18.TabIndex = 30;
            this.label18.Text = "设置开启和关闭时长：";
            // 
            // BtnSetPort
            // 
            this.BtnSetPort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSetPort.Location = new System.Drawing.Point(314, 143);
            this.BtnSetPort.Name = "BtnSetPort";
            this.BtnSetPort.Size = new System.Drawing.Size(52, 23);
            this.BtnSetPort.TabIndex = 29;
            this.BtnSetPort.Text = "确定";
            this.BtnSetPort.UseVisualStyleBackColor = true;
            this.BtnSetPort.Click += new System.EventHandler(this.BtnSetPort_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(7, 149);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 28;
            this.label17.Text = "设置端口：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(236, 111);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(20, 31);
            this.label16.TabIndex = 27;
            this.label16.Text = ".";
            // 
            // IPtextBox4
            // 
            this.IPtextBox4.Location = new System.Drawing.Point(262, 116);
            this.IPtextBox4.Name = "IPtextBox4";
            this.IPtextBox4.Size = new System.Drawing.Size(36, 21);
            this.IPtextBox4.TabIndex = 26;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(168, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 31);
            this.label15.TabIndex = 25;
            this.label15.Text = ".";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(97, 113);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(20, 31);
            this.label14.TabIndex = 24;
            this.label14.Text = ".";
            // 
            // IPtextBox3
            // 
            this.IPtextBox3.Location = new System.Drawing.Point(194, 116);
            this.IPtextBox3.Name = "IPtextBox3";
            this.IPtextBox3.Size = new System.Drawing.Size(36, 21);
            this.IPtextBox3.TabIndex = 23;
            // 
            // IPtextBox2
            // 
            this.IPtextBox2.Location = new System.Drawing.Point(123, 116);
            this.IPtextBox2.Name = "IPtextBox2";
            this.IPtextBox2.Size = new System.Drawing.Size(36, 21);
            this.IPtextBox2.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 856);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "漏点定位传感器数据采集平台";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox StatusBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox receiveDatarichTextBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckedListBox DeviceCheckedListBox1;
        private System.Windows.Forms.Button Btn0hooseDevice;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button BtnOpenServer;
        private System.Windows.Forms.RadioButton radioBtnchooseTCP;
        private System.Windows.Forms.RadioButton radioBtnchooseUDP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IPBox;
        private System.Windows.Forms.TextBox PortBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CmdBox;
        private System.Windows.Forms.TextBox IDBox;
        private System.Windows.Forms.Button BtnSendCmd;
        private System.Windows.Forms.Button BtnGetData;
        private System.Windows.Forms.RadioButton radioBtnWindowShow;
        private System.Windows.Forms.RadioButton radioBtnSaveFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox APnameBox;
        private System.Windows.Forms.TextBox APpasswordBox;
        private System.Windows.Forms.Button BtnSetAPName;
        private System.Windows.Forms.TextBox HourBox;
        private System.Windows.Forms.Button BtnSetCaptime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox MinuteBox;
        private System.Windows.Forms.TextBox SecondBox;
        private System.Windows.Forms.TextBox MsBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button BtnSetAPpassword;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox IPtextBox1;
        private System.Windows.Forms.TextBox PortextBox;
        private System.Windows.Forms.Button BtnSetIPname;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox IPtextBox3;
        private System.Windows.Forms.TextBox IPtextBox2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox IPtextBox4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button BtnSetPort;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnCloseServer;
        private System.Windows.Forms.Button BtnSendOnTime;
        //private System.Windows.Forms.Button BtnSetOpenTime;
        //private System.Windows.Forms.TextBox textBoxCloseTime;
        //private System.Windows.Forms.TextBox textBoxOpenTime;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxOpenTime;
        private System.Windows.Forms.Button BtnSetOpenAndCloseTime;
        private System.Windows.Forms.TextBox textBoxCloseTime;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
    }
}

