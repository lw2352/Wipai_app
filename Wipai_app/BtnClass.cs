using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Wipai_app
{
    partial class Form1
    {
        //开启服务
        private void BtnOpenServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnchooseTCP.Checked == true)//选择建立TCP
                {
                    ServerSocket = new Socket(AddressFamily.InterNetwork,
                                                 SocketType.Stream,
                                                 ProtocolType.Tcp);
                }
                if (radioBtnchooseUDP.Checked == true)//选择建立UDP
                {
                    ServerSocket = new Socket(AddressFamily.InterNetwork,
                                                 SocketType.Dgram,
                                                 ProtocolType.Udp);
                }

                IP = IPBox.Text;
                Port = Convert.ToInt32(PortBox.Text);

                //Assign the any IP of the machine and listen on port number 8080
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);

                //Bind and listen on the given address
                ServerSocket.Bind(ipEndPoint);
                ServerSocket.Listen(8080);

                //Accept the incoming clients
                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), ServerSocket);

                BtnOpenServer.Enabled = false;
                BtnCloseServer.Enabled = true;

                //DebugLog.Debug("socket监听服务打开");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "error",MessageBoxButtons.OK, MessageBoxIcon.Error); 
                Console.WriteLine(ex.Message + "---" + DateTime.Now.ToLongTimeString() + "出错信息：" + "\n");
                //DebugLog.Debug(ex);
            }
        }

        //关闭服务
        public void BtnCloseServer_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOpenServer.Enabled = true;
                BtnCloseServer.Enabled = false;
                receiveDatarichTextBox.Clear();
                DeviceCheckedListBox1.Items.Clear();
                progressBar1.Value = 0;

                foreach (DictionaryEntry de in htClient)
                {
                    DataItem dataitem = (DataItem)de.Value;
                    dataitem.socket.Shutdown(SocketShutdown.Both);
                    dataitem.socket.Close();
                }

                htClient.Clear();//清除哈希表
                ServerSocket.Close();

                //DebugLog.Debug("socket监听服务关闭");
            }
            catch (Exception ex)
            {
                string error = DateTime.Now.ToString() + "出错信息：" + "---" + ex.Message + "\n";
                System.Diagnostics.Debug.WriteLine(error);
            }
        }

        //下拉菜单中的命令
        private void BtnSendCmd_Click(object sender, EventArgs e)
        {
            byte[] Cmd1 = cmdItem.CmdSetOpenAndCloseTime;//设置开启和关闭时长
            Cmd1[7] = 0x00;

            byte[] Cmd2 = cmdItem.CmdReadGPSData;

            byte[] Cmd3 = cmdItem.CmdSetCapTime;
            Cmd3[7] = 0x00;

            switch (this.CmdBox.SelectedIndex)//根据下拉框当前选择的第几行文本来选择指令
            {
                //读取开启和关闭时长
                case 0:
                    SendCmdAll(Cmd1);
                    break;
                //读取经纬度
                case 1:
                    SendCmdAll(Cmd2);
                    break;
                //读取GPS采样时间
                case 2:
                    SendCmdAll(Cmd3);
                    break;
                default:
                    ShowMsg("请选择一条读取指令");
                    break;
            }

        }

        /// <summary>
        /// 选定设备并显示设备ID,并把进度条最大值设为600*num
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChooseDevice_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                dataitem.isChoosed = false;//先复位选中状态
            }
            //int ChoosedDeviceID;//当前已选择的设备
            string ChoosedAddress;
            string IDString = "";
            int num = 0;
            for (int i = 0; i < DeviceCheckedListBox1.Items.Count; i++)
            {
                if (DeviceCheckedListBox1.GetItemChecked(i))
                {
                    ChoosedAddress = DeviceCheckedListBox1.GetItemText(DeviceCheckedListBox1.Items[i]);
                    foreach (DictionaryEntry de in htClient)
                    {
                        DataItem dataitem = (DataItem)de.Value;
                        //if (dataitem.intDeviceID == ChoosedDeviceID)
                        if (String.Compare(dataitem.strAddress + "--" + dataitem.intDeviceID.ToString(), ChoosedAddress) == 0)
                        {
                            dataitem.isChoosed = true;
                            num++;
                            IDString += dataitem.intDeviceID + ";";//显示设备ID,用";"隔开
                        }
                    }
                }
            }
            progressBar1.Maximum = num * g_totalPackageCount;
            IDBox.Text = (IDString);
        }

        //设置采样时间
        private void BtnSetCaptime_Click(object sender, EventArgs e)
        {
            byte[] Cmd = cmdItem.CmdSetCapTime;
            Cmd[7] = 0x01;
            Cmd[9] = Convert.ToByte(HourBox.Text);
            Cmd[10] = Convert.ToByte(MinuteBox.Text);
            Cmd[11] = Convert.ToByte(SecondBox.Text);
            Cmd[12] = Convert.ToByte(MsBox.Text);
            SendCmdAll(Cmd);
        }
        //设置AP名(ssid)
        private void BtnSetAPName_Click(object sender, EventArgs e)
        {
            byte[] Cmd = cmdItem.CmdSetAPssid;
            Cmd[7] = 0x01;
            byte[] SetAPName = strToByte(APnameBox.Text);//转换成字符型
            for (int i = 0, j = 9; i < SetAPName.Length; i++)
            {
                Cmd[j++] = SetAPName[i];
            }
            SendCmdAll(Cmd);
        }
        //设置AP的密码
        private void BtnSetAPpassword_Click(object sender, EventArgs e)
        {
            byte[] Cmd = cmdItem.CmdSetAPpassword;
            Cmd[7] = 0x01;
            byte[] SetAPpassword = strToByte(APpasswordBox.Text);
            for (int i = 0, j = 9; i < SetAPpassword.Length; i++)
            {
                Cmd[j++] = SetAPpassword[i];
            }
            SendCmdAll(Cmd);
        }
        //设置服务端IP地址
        private void BtnSetIPnameAndPort_Click(object sender, EventArgs e)
        {

            byte[] SetIPname1 = strToByte(IPtextBox1.Text);
            byte[] SetIPname2 = strToByte(IPtextBox2.Text);
            byte[] SetIPname3 = strToByte(IPtextBox3.Text);
            byte[] SetIPname4 = strToByte(IPtextBox4.Text);
            byte[] Cmd = cmdItem.CmdSetServerIP;
            Cmd[7] = 0x01;
            for (int i = 0, j = 9; i < SetIPname1.Length; i++)
            {
                Cmd[j++] = SetIPname1[i];
            }
            Cmd[9 + SetIPname1.Length] = 0x2E;

            for (int i = 0, j = 10 + SetIPname1.Length; i < SetIPname2.Length; i++)
            {
                Cmd[j++] = SetIPname2[i];
            }
            Cmd[10 + SetIPname1.Length + SetIPname2.Length] = 0x2E;

            for (int i = 0, j = 11 + SetIPname1.Length + SetIPname2.Length; i < SetIPname3.Length; i++)
            {
                Cmd[j++] = SetIPname3[i];
            }
            Cmd[11 + SetIPname1.Length + SetIPname2.Length + SetIPname3.Length] = 0x2E;

            for (int i = 0, j = 12 + SetIPname1.Length + SetIPname2.Length + SetIPname3.Length; i < SetIPname4.Length; i++)
            {
                Cmd[j++] = SetIPname4[i];
            }
            SendCmdAll(Cmd);
        }
        //设置Port
        private void BtnSetPort_Click(object sender, EventArgs e)
        {
            byte[] Cmd = cmdItem.CmdSetServerPort;
            Cmd[7] = 0x01;
            byte[] SetPort = strToByte(PortextBox.Text);
            for (int i = 0, j = 9; i < SetPort.Length; i++)
            {
                Cmd[j++] = SetPort[i];
            }
            SendCmdAll(Cmd);
        }

        //让设备立即采样
        private void BtnGetData_Click(object sender, EventArgs e)
        {
            byte[] cmd = new byte[] { 0xA5, 0xA5, 0x25, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x04, 0x0A, 0x1E, 0x00, 0x00, 0xFF, 0x5A, 0x5A };

            if (DateTime.Now.Minute + 5 <= 59)
            {
                cmd[9] = (byte)DateTime.Now.Hour;
                cmd[10] = (byte)(DateTime.Now.Minute + 5);//当前时刻加5分钟
            }
            else
            { //分钟数大于60
                cmd[9] = (byte)(DateTime.Now.Hour + 1);
                cmd[10] = (byte)(DateTime.Now.Minute + 5 - 60);
            }
            try
            {//此处进行遍历操作
                foreach (DictionaryEntry de in htClient)
                {
                    DataItem dataitem = (DataItem)de.Value;
                    if (dataitem.isChoosed == true)
                    {
                        SendCmdSingle(cmd, dataitem.byteDeviceID, dataitem.socket);
                    }
                    //return "OK";
                }
                //return "Fail";
            }
            catch (Exception ex)
            {
                //DebugLog.Debug(ex);
                //return "Fail";
                Console.WriteLine(ex);
            }
        }

        //让设备上传数据到服务器
        private void button1_Click(object sender, EventArgs e)
        {
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.isChoosed == true)
                {
                    dataitem.isSendDataToServer = true;
                    dataitem.datalength = 0;
                    dataitem.currentsendbulk = 0;
                    SendCmdSingle(SetADcmd(0), dataitem.byteDeviceID, dataitem.socket);//发送第0包的命令
                }
            }
        }

        
        //设置开始和关闭时长
        private void BtnSetOpenAndCloseTime_Click_1(object sender, EventArgs e)
        {
            byte[] CmdSetOpenAndCloseTime = cmdItem.CmdSetOpenAndCloseTime;//设置开启时长
            CmdSetOpenAndCloseTime[7] = 0x01;
            int OpenTime = 2 * Convert.ToInt32(textBoxOpenTime.Text);
            int CloseTime = 2 * Convert.ToInt32(textBoxCloseTime.Text);
            CmdSetOpenAndCloseTime[9] = (byte)(OpenTime >> 8);
            CmdSetOpenAndCloseTime[10] = (byte)(OpenTime & 0xFF);
            CmdSetOpenAndCloseTime[11] = (byte)(CloseTime >> 8);
            CmdSetOpenAndCloseTime[12] = (byte)(CloseTime & 0xFF);
            SendCmdAll(CmdSetOpenAndCloseTime);
        }

        //获取时间戳
        private void btn_GetTimeStamp_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.isChoosed == true && dataitem.intDeviceID != 0)
                {
                    SendCmdSingle(SetADcmd(655), dataitem.byteDeviceID, dataitem.socket);
                }
            }
        }

        //计算距离
        private void button2_Click(object sender, EventArgs e)
        {
            double lat1 = 0;
            double lng1 = 0;
            double lat2 = 0;
            double lng2 = 0;

            int num = 0;

            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.isChoosed == true && dataitem.intDeviceID != 0)
                {
                    num++;
                    if (num == 1)
                    {
                        lat1 = dataitem.Latitude;
                        lng1 = dataitem.Longitude;
                    }
                    else if (num == 2)
                    {
                        lat2 = dataitem.Latitude;
                        lng2 = dataitem.Longitude;
                    }
                    else break;
                }
            }

            double length = gpsDistance.getGpsDistance(lat1, lng1, lat2, lng2);
            ShowMsg("两点间的距离是：" + length.ToString() + "米" + "\n");
        }

        //打开热点
        private void OpenVirtualWIFI_Click(object sender, EventArgs e)
        {
            string output = "";
            string cmd = "netsh wlan set hostednetwork mode = allow ssid = " + ssidBox.Text + " key = " + passWordBox.Text;
            processCMD.RunCmd(cmd, out output);
            //MessageBox.Show(output);

            cmd = "netsh wlan start hostednetwork";
            processCMD.RunCmd(cmd, out output);
            MessageBox.Show(output);
        }

        //关闭热点
        private void CloseVirtualWIFI_Click(object sender, EventArgs e)
        {
            string output = "";
            string cmd = "netsh wlan stop hostednetwork";
            processCMD.RunCmd(cmd, out output);
            MessageBox.Show(output);
        }

        private void btn_ChangeVirtualIP_Click(object sender, EventArgs e)
        {
            string output = "";
            string cmd = "netsh interface ip set address \"" + textBoxVirtuaName.Text + "\" static " + textBoxVirtualIP.Text + " " + textBoxSubnetMask.Text + " " + textBoxGateWay.Text + " 1";

            processCMD.RunCmd(cmd, out output);
            MessageBox.Show(output);
        }

        private void ReadAPName_Click(object sender, EventArgs e)
        {
            byte[] CmdReadAPName = cmdItem.CmdSetAPssid;
            CmdReadAPName[7] = 0x00;
            SendCmdAll(CmdReadAPName);
        }

        private void ReadAPPassword_Click(object sender, EventArgs e)
        {
            byte[] CmdReadAPPassword = cmdItem.CmdSetAPpassword;
            CmdReadAPPassword[7] = 0x00;
            SendCmdAll(CmdReadAPPassword);
        }

        private void ReadServerIP_Click(object sender, EventArgs e)
        {
            byte[] CmdReadServerIP = cmdItem.CmdSetServerIP;
            CmdReadServerIP[7] = 0x00;
            SendCmdAll(CmdReadServerIP);
        }

        private void ReadServerPort_Click(object sender, EventArgs e)
        {
            byte[] CmdReadServerPort = cmdItem.CmdSetServerPort;
            CmdReadServerPort[7] = 0x00;
            SendCmdAll(CmdReadServerPort);
        }


        private void Btn_Test_Click(object sender, EventArgs e)
        {
            byte[] Testcmd = { 0xA5, 0xA5, 0x22, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x00, 0xFF, 0xFF, 0x5A, 0x5A };
            SendCmdAll(Testcmd);
        }
    }
}
