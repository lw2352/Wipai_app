using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Timers;
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]


//TODO



namespace Wipai_app
{

    public partial class Form1 : Form
    {
        private string IP;//服务端的IP地址
        private int Port; //服务器端口地址
        private int perPackageLength = 1009;//每包的长度                                          
        private static int g_datafulllength = 600000; //完整数据包的一个长度
        private static int g_totalPackageCount = 600; //600个包
        Hashtable htClient = new Hashtable(); //创建一个Hashtable实例，保存所有设备信息，key存储是ID，value是DataItem;
        Socket ServerSocket; //The main socket on which the server listens to the clients
        private static int currentUploadGroup = 0;//当前第几组上传
        //public static log4net.ILog DebugLog = log4net.LogManager.GetLogger(typeof(Form1));

        private delegate void ShowMsgHandler(string msg);

        cmdHelper processCMD = new cmdHelper();
        GPSDistance gpsDistance = new GPSDistance();
        CmdItem cmdItem = new CmdItem();

        #region 更新ui控件
        private void ShowMsg(string msg)
        {
            if (!receiveDatarichTextBox.InvokeRequired)
            {
                receiveDatarichTextBox.AppendText(msg);
            }
            else
            {
                ShowMsgHandler handler = new ShowMsgHandler(ShowMsg);
                BeginInvoke(handler, new object[] { msg });
            }
        }

        private void AddAddress(string msg)
        {
            if (!DeviceCheckedListBox1.InvokeRequired)
            {
                DeviceCheckedListBox1.Items.Add(msg);
            }
            else
            {
                ShowMsgHandler handler = new ShowMsgHandler(AddAddress);
                BeginInvoke(handler, new object[] { msg });
            }

        }

        private void RemoveAddress(string msg)
        {
            if (!DeviceCheckedListBox1.InvokeRequired)
            {
                DeviceCheckedListBox1.Items.Remove(msg);
            }
            else
            {
                ShowMsgHandler handler = new ShowMsgHandler(RemoveAddress);
                BeginInvoke(handler, new object[] { msg });
            }
        }

        private void RefreshIDBox(string msg)
        {
            if (!IDBox.InvokeRequired)
            {
                IDBox.Text = msg;
            }
            else
            {
                ShowMsgHandler handler = new ShowMsgHandler(RefreshIDBox);
                BeginInvoke(handler, new object[] { msg });
            }
        }

        private void ShowProgressBar(string msg)
        {
            if (!progressBar1.InvokeRequired)
            {
                if (msg == null)
                    progressBar1.Value++;
                if(progressBar1.Value == progressBar1.Maximum)
                    progressBar1.Value = 0;
            }
            else
            {
                ShowMsgHandler handler = new ShowMsgHandler(ShowProgressBar);
                BeginInvoke(handler, new object[] { msg });
            }
        }
        #endregion 更新ui控件

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CmdBox.Items.Add("读取开启和关闭时长");
            CmdBox.Items.Add("读取经纬度");
            CmdBox.Items.Add("读取GPS采样时间");
            CmdBox.Items.Add("读取当前开启和关闭时长");

            BtnOpenServer.Enabled = true;
            BtnCloseServer.Enabled = false;

            try
            {
                //获取主机名
                string HostName = Dns.GetHostName();
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        string localIP = IpEntry.AddressList[i].ToString();
                        IPBox.Text = localIP;
                        Console.WriteLine("本地IP为" + localIP);
                        //DebugLog.Debug("本地IP为" + localIP);
                        ShowMsg("本地IP为" + localIP + "\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //DebugLog.Debug(ex);
            }

        }

        //接收来自客户端的请求
        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = ServerSocket.EndAccept(ar);
                //Start listening for more clients
                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), ServerSocket);
                //add 5-6
                string strIP = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                string strPort = (clientSocket.RemoteEndPoint as IPEndPoint).Port.ToString();
                string strAddress = clientSocket.RemoteEndPoint.ToString();
                if (!htClient.ContainsKey(strAddress))
                {
                    DataItem dataitem = new DataItem();

                    dataitem.strIP = strIP;
                    dataitem.strPort = strPort;
                    dataitem.socket = clientSocket;
                    dataitem.SingleBuffer = new byte[perPackageLength];
                    dataitem.strAddress = strAddress;

                    dataitem.byteDeviceID = new byte[4];
                    dataitem.intDeviceID = 0;

                    dataitem.datalength = 0;
                    dataitem.byteAllData = new byte[g_datafulllength];
                    dataitem.currentsendbulk = 0;

                    dataitem.isSendDataToServer = false;
                    dataitem.isChoosed = false;
                    dataitem.CmdStage = 0;
                    dataitem.uploadGroup = 0;

                    dataitem.byteTimeStamp = new byte[6];//时间戳
                    dataitem.Longitude = 0;//经度，后半段
                    dataitem.Latitude = 0;//纬度， 前半段                  

                    htClient.Add(strAddress, dataitem);

                    ShowMsg(DateTime.Now.ToString() + "收到客户端" + strAddress + "的连接请求" + "\n");
                    //Once the client connects then start receiving the commands from her
                    //开始从连接的socket异步接收数据
                    clientSocket.BeginReceive(dataitem.SingleBuffer, 0, dataitem.SingleBuffer.Length, SocketFlags.None,
                    new AsyncCallback(OnReceive), clientSocket);
                }
                //DebugLog.Debug("收到客户端的连接请求");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message + "---" + DateTime.Now.ToLongTimeString() + "出错信息：" + "\n");
                //DebugLog.Debug(ex);
            }
        }

        //接收数据
        private void OnReceive(IAsyncResult ar)
        {
            byte[] ID = new byte[4];//设备的ID号
            int intdeviceID = 0;
            string msg = "";
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;//此处获取数据大小              

                //获取客户端信息，包括了IP地址、端口
                string strIP = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                string strPort = (clientSocket.RemoteEndPoint as IPEndPoint).Port.ToString();
                //test
                string strAddress = clientSocket.RemoteEndPoint.ToString();

                DataItem dataitem = (DataItem)htClient[strAddress];//取出address对应的dataitem

                //获取接收的数据长度,注意此处的停止接收，后面必须继续接收，否则不会接收数据的
                int bytesRead = clientSocket.EndReceive(ar);//接收到的数据长度 !!!如果设备掉线，此处会throw错误
                if (bytesRead > 0 )     //打印数据
                {
                    string str = byteToHexStr(dataitem.SingleBuffer);
                    string strrec = str.Substring(0, bytesRead * 2);

                    msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "从硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "接收到的数据长度是" + bytesRead.ToString() + "数据是" + strrec + "\n";
                    ShowMsg(msg);
                    Console.WriteLine(msg);

                    switch (dataitem.SingleBuffer[2])
                    {
                        case 0x21:
                            if (dataitem.SingleBuffer[7] == 0x00)
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--读取开启时长和关闭时长成功" + "\n";
                                Console.WriteLine(msg);
                                ShowMsg(msg);
                            }
                            if (dataitem.SingleBuffer[7] == 0x01)
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定开启时长和关闭时长成功" + "\n";
                                Console.WriteLine(msg);
                                ShowMsg(msg);
                            }
                            break;

                        case 0x22:
                            if (dataitem.SingleBuffer[9] == 0xAA)
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--AD采样开始" + "\n";
                            }
                            else if (dataitem.SingleBuffer[9] == 0x55)
                            {
                                dataitem.CmdStage = 1;
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--AD采样结束" + "\n";
                            }
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                            break;

                        case 0x25:
                            if (dataitem.SingleBuffer[9] == 0x55)
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定GPS采样时间成功" + "\n";
                                Console.WriteLine(msg);
                                ShowMsg(msg);
                            }
                            
                            break;

                        case 0x26:
                            if (dataitem.SingleBuffer[7] == 0x01)
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定开启时长和关闭时长成功" + "\n";
                                Console.WriteLine(msg);
                                ShowMsg(msg);
                            }
                            break;

                        case 0x27:
                            int[] gpsData = new int[23];
                            for (int i = 0; i < 23; i++)
                            {
                                gpsData[i] = dataitem.SingleBuffer[9 + i];
                            }
                            gpsDistance.getGPSData(gpsData, out dataitem.Latitude, out dataitem.Longitude);
                            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--经度为："+ dataitem.Longitude + "纬度为：" + dataitem.Latitude + "\n";
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                            break;

                        case 0x29:
                            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定服务器IP成功" + "\n";
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                            break;

                        case 0x30:
                            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定服务器端口号成功" + "\n";
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                            break;

                        case 0x31:
                            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定AP名称成功" + "\n";
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                            break;

                        case 0x32:
                            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定AP密码成功" + "\n";
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                            break;

                        case 0x23:
                            if (bytesRead == perPackageLength)
                            {
                                if (dataitem.isSendDataToServer == true)
                                {
                                    dataitem.currentsendbulk++;

                                    ShowProgressBar(null);

                                    for (int i = 7; i < perPackageLength - 2; i++)//将上传的包去掉头和尾的两个字节后，暂时存储在TotalData[]中
                                    {
                                        dataitem.byteAllData[dataitem.datalength++] = dataitem.SingleBuffer[i];
                                    }

                                    if (dataitem.datalength == g_datafulllength)//1000*600 = 600000;
                                    {
                                        StoreDataToFile(dataitem.intDeviceID, dataitem.byteAllData);

                                        dataitem.currentsendbulk = 0;
                                        dataitem.isSendDataToServer = false;
                                        dataitem.CmdStage = 3;

                                        msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--数据上传完毕" + "\n";
                                        Console.WriteLine(msg);
                                        ShowMsg(msg);
                                    }
                                }
                                else
                                {
                                    for (int i = 368, j = 0; i <= 373; i++, j++)//将上传的包去掉头和尾的两个字节后，暂时存储在TotalData[]中
                                    {
                                        dataitem.byteTimeStamp[j] = (byte)(Convert.ToInt32(dataitem.SingleBuffer[i]) - 0x30);
                                    }
                                    msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + dataitem.strAddress + "设备号--" + dataitem.intDeviceID + "--时间戳是:" + byteToHexStr(dataitem.byteTimeStamp) + "\n";
                                    Console.WriteLine(msg);
                                    ShowMsg(msg);
                                }
                            }
                            break;

                        case 0xFF:
                            if (dataitem.intDeviceID == 0)//只判断新地址的心跳包，避免重复检测
                            {
                                //设备的ID字符串
                                ID[0] = dataitem.SingleBuffer[3];
                                ID[1] = dataitem.SingleBuffer[4];
                                ID[2] = dataitem.SingleBuffer[5];
                                ID[3] = dataitem.SingleBuffer[6];
                                intdeviceID = byteToInt(ID);

                                string oldAddress = checkIsHaveID(intdeviceID);//得到当前ID对应的旧地址

                                if (oldAddress != null)//若存在，把旧地址的属性复制到新地址上
                                {//！！！由于掉线，新dataitem属性要继承旧设备，只需要更新网络属性，如IP、port、socket等
                                    DataItem olddataitem = (DataItem)htClient[oldAddress];//取出当前数据IP对应的dataitem

                                    dataitem.strIP = strIP;
                                    dataitem.strPort = strPort;
                                    dataitem.socket = clientSocket;
                                    dataitem.SingleBuffer = new byte[perPackageLength];
                                    dataitem.strAddress = strAddress;

                                    dataitem.datalength = olddataitem.datalength;//继承旧属性
                                    dataitem.byteAllData = olddataitem.byteAllData;//继承旧属性
                                    dataitem.currentsendbulk = olddataitem.currentsendbulk;//继承旧属性

                                    dataitem.byteDeviceID = ID;
                                    dataitem.intDeviceID = intdeviceID;

                                    dataitem.isSendDataToServer = olddataitem.isSendDataToServer;//继承旧属性
                                    dataitem.isChoosed = false;
                                    dataitem.CmdStage = olddataitem.CmdStage;//继承旧属性
                                    dataitem.uploadGroup = 0;

                                    dataitem.byteTimeStamp = olddataitem.byteTimeStamp;//时间戳，继承旧属性
                                    dataitem.Longitude = olddataitem.Longitude;//经度，后半段，继承旧属性
                                    dataitem.Latitude = olddataitem.Latitude;//纬度， 前半段，继承旧属性

                                    htClient.Remove(oldAddress);//删除旧地址的键值对
                                    string OldAddress = oldAddress + "--" + dataitem.intDeviceID.ToString();
                                    RemoveAddress(OldAddress);

                                    htClient[strAddress] = dataitem;//把设备的IP和设备的dataitem对应地更新进哈希表
                                    string newAddress = strAddress + "--" + dataitem.intDeviceID.ToString();
                                    AddAddress(newAddress);
                                }
                                else
                                {
                                    //若不存在，属于全新地址，更新ID号
                                    dataitem.intDeviceID = intdeviceID;
                                    dataitem.byteDeviceID = ID;

                                    string newAddress = strAddress + "--" + dataitem.intDeviceID.ToString();
                                    AddAddress(newAddress);
                                }
                            }//if (dataitem.intDeviceID == 0)
                            break;

                        default:
                            break;
                    }

                    //把数据上传到服务器
                    if (dataitem.isSendDataToServer == true)
                    {

                        SendCmdSingle(SetADcmd(dataitem.currentsendbulk), dataitem.byteDeviceID, dataitem.socket);//发送下一包的命令    

                        msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "从硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--第" + dataitem.currentsendbulk + "包" + "\n";
                        ShowMsg(msg);
                    }

                    //选择是否自动测试
                    if (checkBoxAutoTest.Checked)
                    {
                        if (checkIsAllCmdStage1())//所有设备采样完成，关闭定时器，可以进行上传了
                        {
                           UploadADdataByGroup(currentUploadGroup);//在UploadADdataByGroup函数中将stage置2
                        }

                        if (checkIsAllCmdStage3())
                        {
                            foreach (DictionaryEntry de in htClient)
                            {
                                DataItem dataitem1 = (DataItem)de.Value;
                                if (dataitem1.CmdStage == 3)//add 5-13
                                    dataitem.CmdStage = 0;
                            }
                            ShowMsg("所有设备数据传完,CmdStage置0；设置当前组的采样时间\n");
                            SetCapTimeByGroup(5, currentUploadGroup);
                        }
                    }

                    //自动上传
                    if (checkBoxAutoUpload.Checked)
                    {
                        if (checkIsAllCmdStage3())
                        {
                            foreach (DictionaryEntry de in htClient)
                            {
                                DataItem dataitem1 = (DataItem)de.Value;
                                if (dataitem1.CmdStage == 3)//add 5-13
                                    dataitem.CmdStage = 0;
                            }
                            UploadADdataByGroup(currentUploadGroup);
                        }
                    }

                }
                else if (bytesRead == 0)//设备自己关闭socket
                {
                    //必须关闭,因为断开后socket不会立即关闭,会无限循环卡死
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "设备--" + dataitem.intDeviceID + "--设备自己关闭socket";
                    ShowMsg(msg);
                }

                clientSocket.BeginReceive(dataitem.SingleBuffer, 0, dataitem.SingleBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        #region 自动测试
        //检查所有设备处于第几命令阶段--stage1--除上传外的命令已发完，可以进行上传了
        private bool checkIsAllCmdStage1()
        {
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.CmdStage != 1 && dataitem.intDeviceID != 0)//add 5-9
                    return false;
            }
            return true;
        }


        //检查所有设备处于第几命令阶段--stage3--所有命令发完，可以进行数据分析了
        private bool checkIsAllCmdStage3()
        {
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.CmdStage != 3 && dataitem.intDeviceID != 0)//add 5-9
                    return false;
            }
            return true;
        }

        //向一组设备发送上传命令
        private void UploadADdataByGroup(int group)
        {
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.uploadGroup == group && dataitem.intDeviceID != 0)
                {
                    dataitem.isSendDataToServer = true;
                    dataitem.datalength = 0;
                    dataitem.currentsendbulk = 0;
                    dataitem.CmdStage = 2;//stage置2，分完组准备上传

                    SendCmdSingle(SetADcmd(0), dataitem.byteDeviceID, dataitem.socket);
                }
            }
        }

        //向一组设备设置采样时间
        private void SetCapTimeByGroup(int minute, int group)
        {
            byte[] cmd = cmdItem.CmdSetCapTime;
            if (DateTime.Now.Minute + minute <= 59)
            {
                cmd[9] = (byte)DateTime.Now.Hour;
                cmd[10] = (byte)(DateTime.Now.Minute + minute);//当前时刻加5分钟
            }
            else
            { //分钟数大于60
                cmd[9] = (byte)(DateTime.Now.Hour + 1);
                cmd[10] = (byte)(DateTime.Now.Minute + minute - 60);
            }
            try
            {//此处进行遍历操作
                foreach (DictionaryEntry de in htClient)
                {
                    DataItem dataitem = (DataItem)de.Value;
                    if (dataitem.uploadGroup == group && dataitem.intDeviceID != 0)
                    {
                        SendCmdSingle(cmd, dataitem.byteDeviceID, dataitem.socket);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="ar">IAsyncResult</param>
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSserverTCP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 字节数组转16进制字符串 
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>string</returns>
        private static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (long i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 字节数组转int值，转换ID号
        /// </summary>
        /// <parambytes>byte[]</param>
        /// <returns>int型的ID号</returns>
        private static int byteToInt(byte[] bytes)
        {
            int returnInt = 0;
            if (bytes != null)
            {
                for (long i = 0; i < bytes.Length; i++)
                {
                    if (i == 3)
                        returnInt += (int)bytes[i];
                    else if (i == 2)
                        returnInt += (int)bytes[i] * 256;
                    else if (i == 1)
                        returnInt += (int)bytes[i] * 65536;
                    else if (i == 0)
                        returnInt += (int)bytes[i] * 16777216;
                }
            }
            return returnInt;
        }

        /// <summary>
        /// 字符串转数组(1-->49)
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>byte[]</returns>
        private static byte[] strToByte(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = Convert.ToByte(str[i]);
            }
            return bytes;
        }

        /// <summary>
        /// 将int数值转换为占byte数组
        /// </summary>
        /// <param name="value">int</param>
        /// <returns>byte[]</returns>
        private static byte[] intToBytes(int value)
        {
            byte[] src = new byte[2];

            src[0] = (byte)((value >> 8) & 0xFF);
            src[1] = (byte)(value & 0xFF);
            return src;
        }

        /// <summary>
        /// 检查哈希表中是否已存在当前ID
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>string or null</returns>
        private string checkIsHaveID(int id)
        {
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.intDeviceID == id)//设备掉线后address改变而ID号不变
                    return dataitem.strAddress;
            }
            return null;
        }

        //(多个)将命令从服务端发送到每一个选中的设备（普通命令）
        private void SendCmdAll(byte[] cmd)
        {
            byte[] ChoosedDeviceID = new byte[4]; //已选择设备的ID号
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.isChoosed == true && dataitem.intDeviceID != 0)
                {
                    ChoosedDeviceID = dataitem.byteDeviceID;
                    cmd[3] = ChoosedDeviceID[0];
                    cmd[4] = ChoosedDeviceID[1];
                    cmd[5] = ChoosedDeviceID[2];
                    cmd[6] = ChoosedDeviceID[3];
                    try
                    {
                        dataitem.socket.BeginSend(cmd, 0, cmd.Length, SocketFlags.None, new AsyncCallback(OnSend), dataitem.socket);
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");    
                        ShowMsg(timestamp + "向设备" + dataitem.intDeviceID + "发送命令是：" + byteToHexStr(cmd) + "\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        //(单个)将命令从服务端发送到特定的设备（数据采集）
        private void SendCmdSingle(byte[] cmd, byte[] id, Socket deviceSocket)
        {

            cmd[3] = id[0];
            cmd[4] = id[1];
            cmd[5] = id[2];
            cmd[6] = id[3];
            string deviceid = byteToHexStr(id);
            try
            {
                deviceSocket.BeginSend(cmd, 0, cmd.Length, SocketFlags.None, new AsyncCallback(OnSend), deviceSocket);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                ShowMsg(timestamp + "向设备" + deviceid + "发送命令是：" + byteToHexStr(cmd) + "\n");
            }
            catch (Exception ex)
            {
                //DebugLog.Debug(ex);
                Console.WriteLine(ex);
            }


        }

        /// <summary>
        /// 构造AD采样命令
        /// </summary>
        /// <param name="bulkCount">包数</param>
        /// <returns>string</returns>
        private byte[] SetADcmd(int bulkCount)
        {
            byte[] Cmd = cmdItem.CmdADPacket;
            byte[] bytesbulkCount = new byte[2];
            bytesbulkCount = intToBytes(bulkCount);
            
            Cmd[11] = bytesbulkCount[0];
            Cmd[12] = bytesbulkCount[1];
            
            string strtest = byteToHexStr(Cmd);
            return (Cmd);
        }

        //保存文件，16进制，封装开头是0xAA，结尾是0x55
        private void StoreDataToFile(int intDeviceID, byte[] bytes)
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + "--" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "--" + intDeviceID.ToString();//以日期时间命名，避免文件名重复
            byte[] fileStartAndEnd = new byte[2] { 0xAA, 0x55 };//保存文件的头是AA，尾是55
            string url = "D:\\Data";

            if (!Directory.Exists(url))//如果不存在就创建file文件夹　　             　　                
            {
                Directory.CreateDirectory(url);//创建该文件夹　

                string path = @"D:\\Data\\" + filename + ".dat";
                FileStream F = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                F.Write(fileStartAndEnd, 0, 1);
                F.Write(bytes, 0, bytes.Length);
                F.Write(fileStartAndEnd, 1, 1);
                F.Flush();
                F.Close();
                ShowMsg("文件保存成功");
            }
            else
            {
                string path = @"D:\\Data\\" + filename + ".dat";
                FileStream F = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                F.Write(fileStartAndEnd, 0, 1);
                F.Write(bytes, 0, bytes.Length);
                F.Write(fileStartAndEnd, 1, 1);
                F.Flush();
                F.Close();
                ShowMsg("文件保存成功");
            }
        }




    }//对应public partial class Form1 : Form

}//对应namespace Wipai_app
