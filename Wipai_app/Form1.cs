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
//


    //5-20 注释掉自动上传流程代码，还原以前的设备选择和其他ui操作
namespace Wipai_app
{

    public partial class Form1 : Form
    {
        public string IP;//服务端的IP地址
        public int Port; //服务器端口地址
        //public byte[] buffer = new byte[1008];
        public int perPackageLength = 1008;//每包的长度                                          
        public static int g_datafulllength = 600000; //完整数据包的一个长度
        public static int g_totalPackageCount = 600; //600个包
        Hashtable htClient = new Hashtable(); //创建一个Hashtable实例，保存所有设备信息，key存储是ID，value是DataItem;
        Socket ServerSocket; //The main socket on which the server listens to the clients
        public static int currentUploadGroup = 0;//当前第几组上传
        //public static log4net.ILog DebugLog = log4net.LogManager.GetLogger(typeof(Form1));

        private delegate void ShowMsgHandler(string msg);

        cmdHelper processCMD = new cmdHelper();
        GetDistance getDistance = new GetDistance();

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
        

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //允许跨线程访问
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CmdBox.Items.Add("读取开启和关闭时长");
            CmdBox.Items.Add("读取经纬度");
            CmdBox.Items.Add("读取GPS采样时间");

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
                        Console.WriteLine("本地IP为"+localIP);
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
                    dataitem.byteDeviceID = new byte[4];
                    dataitem.intDeviceID = 0;
                    dataitem.strIP = strIP;
                    dataitem.strPort = strPort;
                    dataitem.datalength = 0;
                    dataitem.socket = clientSocket;
                    dataitem.byteAllData = new byte[g_datafulllength];
                    dataitem.currentsendbulk = 0;
                    dataitem.uploadGroup = 0;
                    dataitem.isChoosed = false;
                    dataitem.CmdStage = 0;
                    dataitem.isSendDataToServer = false;
                    dataitem.SingleBuffer = new byte[perPackageLength];
                    dataitem.strAddress = strAddress;

                    dataitem.byteTimeStamp = new byte[6];//时间戳
                    dataitem.Longitude = 0;//经度，后半段
                    dataitem.Latitude = 0;//纬度， 前半段

                    htClient.Add(strAddress, dataitem);
   
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
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;//此处获取数据大小              

                //获取客户端信息，包括了IP地址、端口
                string strIP = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                string strPort = (clientSocket.RemoteEndPoint as IPEndPoint).Port.ToString();
                //test
                string strAddress = clientSocket.RemoteEndPoint.ToString();

                DataItem dataitem = (DataItem)htClient[strAddress];//取出address对应的dataitem
                //获取接收的数据长度,注意此处的停止接收，后面必须继续接收，否则不会接收数据的。
                int bytesRead = clientSocket.EndReceive(ar);//接收到的数据长度 !!!如果设备掉线，此处会throw错误
                if (bytesRead > 0 && radioBtnWindowShow.Checked == true)     //打印数据
                {                  
                    string str = byteToHexStr(dataitem.SingleBuffer);
                    string strrec = str.Substring(0, bytesRead * 2);
                    //string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    ShowMsg(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "从硬件" + strAddress + "设备号--"+ dataitem.intDeviceID + "接收到的数据长度是" + bytesRead.ToString() + "数据是" + strrec + "\n");
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "从硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "接收到的数据长度是" + bytesRead.ToString() + "数据是" + strrec + "\n");
                    if (checkIsHeartPackage(dataitem.SingleBuffer))  //检查一下是否是心跳包
                    {
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

                                //DataItem dataitem1 = new DataItem();
                                dataitem.byteDeviceID = ID;
                                dataitem.intDeviceID = intdeviceID;
                                dataitem.strIP = strIP;
                                dataitem.strPort = strPort;
                                dataitem.datalength = olddataitem.datalength;//继承旧属性
                                dataitem.socket = clientSocket;
                                dataitem.byteAllData = olddataitem.byteAllData;//继承旧属性
                                dataitem.currentsendbulk = olddataitem.currentsendbulk;//继承旧属性
                                dataitem.uploadGroup = 0;
                                dataitem.isChoosed = false;
                                dataitem.CmdStage = olddataitem.CmdStage;//继承旧属性
                                dataitem.isSendDataToServer = olddataitem.isSendDataToServer;//继承旧属性
                                dataitem.SingleBuffer = new byte[perPackageLength];
                                dataitem.strAddress = strAddress;

                                dataitem.byteTimeStamp = olddataitem.byteTimeStamp;//时间戳
                                dataitem.Longitude = olddataitem.Longitude;//经度，后半段
                                dataitem.Latitude = olddataitem.Latitude;//纬度， 前半段

                                htClient.Remove(oldAddress);//删除旧地址的键值对
                                string OldAddress = oldAddress + "--" + dataitem.intDeviceID.ToString();
                                DeviceCheckedListBox1.Items.Remove(OldAddress);

                                htClient[strAddress] = dataitem;//把设备的IP和设备的dataitem对应地更新进哈希表
                                string newAddress = strAddress + "--" + dataitem.intDeviceID.ToString();
                                DeviceCheckedListBox1.Items.Add(newAddress);
                            }
                            else
                            {
                                //若不存在，属于全新地址，更新ID号
                                dataitem.intDeviceID = intdeviceID;
                                dataitem.byteDeviceID = ID;

                                string newAddress = strAddress + "--" + dataitem.intDeviceID.ToString();
                                DeviceCheckedListBox1.Items.Add(newAddress);
                            }
                        }//if (dataitem.intDeviceID == 0)
                    }//end if (checkIsHeartPackage())   

                    //设定GPS采样时间成功
                    if (checkIsGPSsetOK(dataitem.SingleBuffer))
                    {
                        //StatusBox.Text = dataitem.intDeviceID + "设定GPS采样时间成功";
                        //DebugLog.Debug("设备ID为" + dataitem.intDeviceID + "设定GPS采样时间成功");
                        string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--设定GPS采样时间成功" + "\n";
                        Console.WriteLine(msg);
                        ShowMsg(msg);
                    }
                    if (checkIsADstart(dataitem.SingleBuffer))
                    {
                        //StatusBox.Text = dataitem.intDeviceID + "AD采样开始";
                        //DebugLog.Debug("设备ID为" + dataitem.intDeviceID + "AD采样开始");
                        string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--AD采样开始"+"\n";
                        Console.WriteLine(msg);
                        ShowMsg(msg);
                    }
                    if (checkIsADfinished(dataitem.SingleBuffer))
                    {
                        dataitem.CmdStage = 1;
                        //StatusBox.Text = dataitem.intDeviceID + "AD采样结束";
                        //DebugLog.Debug("设备ID为" + dataitem.intDeviceID + "AD采样结束");
                        string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--AD采样结束"+"\n";
                        Console.WriteLine(msg);
                        ShowMsg(msg);

                    }

                    if (checkISGpsData(dataitem.SingleBuffer) != null)
                    {
                        int[] gpsData = new int[23];
                        gpsData = checkISGpsData(dataitem.SingleBuffer);
                                               
                        dataitem.Latitude = (gpsData[1] - 0x30) * 10.0 + (gpsData[2] - 0x30);
                           
                        dataitem.Latitude += ((gpsData[3] - 0x30) * 10 + (gpsData[4] - 0x30)) / 60.0;

                        dataitem.Latitude += ((gpsData[6] - 0x30) / 10.0 + (gpsData[7] - 0x30)/100.0 + (gpsData[8] - 0x30) / 1000.0 + (gpsData[9] - 0x30) / 10000.0 + (gpsData[10] - 0x30) / 100000.0) / 60.0;

                        dataitem.Longitude = (gpsData[12] - 0x30) * 100 + (gpsData[13] - 0x30)*10 + (gpsData[14] - 0x30);

                        dataitem.Longitude += ((gpsData[15] - 0x30) * 10 + (gpsData[16] - 0x30)) / 60.0;

                        dataitem.Longitude += ((gpsData[18] - 0x30) / 10.0 + (gpsData[19] - 0x30) / 100.0 + (gpsData[20] - 0x30) / 1000.0 + (gpsData[21] - 0x30) / 10000.0 + (gpsData[22] - 0x30) / 100000.0) / 60.0;
                    }

                    //add 5-3
                   /* if (checkIsAllCmdStage1())//采样完成
                    {
                        UploadADdataByGroup(currentUploadGroup);
                        //DebugLog.Debug("所有已选择设备都处于stage1--采样完成，准备上传");
                        string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "--所有已选择设备都处于stage1--采样完成，准备上传" + "\n";
                        Console.WriteLine(msg);
                        ShowMsg(msg);
                    }*/

                    /*if (checkIsAllCmdStage3())//stored ok
                    {
                        htClient.Clear();
                    }*/

                    //处理AD数据（收到纯数据时保存到一个60万大小的数组中）
                    if (checkIsPureData(dataitem.SingleBuffer))
                    {
                        if (dataitem.isSendDataToServer == true)
                        {
                            dataitem.currentsendbulk++;
                            //progressBar1.Value = progressBar1.Value + dataitem.currentsendbulk;
                            progressBar1.Value++;

                            for (int i = 7; i < perPackageLength - 1; i++)//将上传的包去掉头和尾的两个字节后，暂时存储在TotalData[]中
                            {
                                dataitem.byteAllData[dataitem.datalength++] = dataitem.SingleBuffer[i];
                            }

                            if (dataitem.datalength == g_datafulllength)//1000*600 = 600000;
                            {
                                StoreDataToFile(dataitem.intDeviceID, dataitem.byteAllData);
                                //dataitem.totalsendbulk = 0;
                                dataitem.currentsendbulk = 0;
                                dataitem.isSendDataToServer = false;
                                dataitem.CmdStage = 3;

                                string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--数据上传完毕" + "\n";
                                Console.WriteLine(msg);
                                ShowMsg(msg);

                                //StatusBox.Text = "数据采集完毕";
                                //!!!待测
                                if (progressBar1.Value == progressBar1.Maximum)
                                {
                                    progressBar1.Value = 0;
                                }
                                //DebugLog.Debug("设备ID为" + dataitem.intDeviceID + "数据采集完毕");
                            }
                        }
                        else
                        {
                            for (int i = 368,j = 0; i <= 373; i++, j++)//将上传的包去掉头和尾的两个字节后，暂时存储在TotalData[]中
                            {
                                dataitem.byteTimeStamp[j] = (byte)(Convert.ToInt32(dataitem.SingleBuffer[i]) - 0x30);
                            }
                            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "硬件" + dataitem.strAddress + "设备号--" + dataitem.intDeviceID + "--时间戳是:" + byteToHexStr(dataitem.byteTimeStamp) + "\n";
                            Console.WriteLine(msg);
                            ShowMsg(msg);
                        }
                    }

                    //把数据上传到服务器
                    if (dataitem.isSendDataToServer == true)
                    {

                        SendCmdSingle(SetADcmd(dataitem.currentsendbulk), dataitem.byteDeviceID, dataitem.socket);//发送下一包的命令    
                        ShowMsg(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "从硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--第" + dataitem.currentsendbulk + "包" + "\n");                                                                                                                            
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "从硬件" + strAddress + "设备号--" + dataitem.intDeviceID + "--第" + dataitem.currentsendbulk + "包" + "\n");

                        //DebugLog.Debug("设备ID为" + dataitem.intDeviceID + "第" + dataitem.currentsendbulk + "包");
                        //StatusBox.Text = dataitem.currentsendbulk.ToString();
                        //progressBar1.Value = dataitem.currentsendbulk;

                    }


                }
                else if (bytesRead == 0)//设备自己关闭socket
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    //DebugLog.Debug("设备自己关闭socket");
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "设备--" + dataitem.intDeviceID + "--设备自己关闭socket");

                }
              
                clientSocket.BeginReceive(dataitem.SingleBuffer, 0, dataitem.SingleBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "SGSserverTCP", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                Console.WriteLine(ex.Message + "---" + DateTime.Now.ToLongTimeString() + "出错信息：" + "\n");
                //DebugLog.Debug(ex);
            }
        }

        //发送数据
        public void OnSend(IAsyncResult ar)
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

        // 字节数组转16进制字符串 
        public static string byteToHexStr(byte[] bytes)
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

        //字节数组转int值
        public static int byteToInt(byte[] bytes)
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

        // 字符串转16进制字节数组(1-->1,没用到)
        private static byte[] strToHexByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length];
            for (int i = 0; i < returnBytes.Length; i++)
            { returnBytes[i] = Convert.ToByte(hexString.Substring(i, 1), 16); }
            return returnBytes;
        }
        //字符串转数组(1-->49)
        private static byte[] strToByte(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = Convert.ToByte(str[i]);
            }
            return bytes;
        }

        //将int数值转换为占byte数组
        public static byte[] intToBytes(int value)
        {
            byte[] src = new byte[2];

            src[0] = (byte)((value >> 8) & 0xFF);
            src[1] = (byte)(value & 0xFF);
            return src;
        }

        private int[] checkISGpsData(byte[] buffer)
        {
            int[] gpsData = new int[23];
            if (buffer[0] == 0xA5 && buffer[1] == 0xA5 && buffer[2] == 0x27 && buffer[32] == 0xFF && buffer[33] == 0x5A && buffer[34] == 0x5A)
            {
                for (int i = 0; i < 23; i++)
                {
                    gpsData[i] = buffer[9+i];
                }
                return gpsData;
            }
            else
            {
                return null;
            }
        }

        //检查是否是心跳包，主要检查命令码即可
        private bool checkIsHeartPackage(byte[] buffer)
        {
            if (buffer[0] == 0xA5 && buffer[1] == 0xA5 && buffer[2] == 0xFF && buffer[9] == 0xFF && buffer[10] == 0x5A && buffer[11] == 0x5A)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //检查AD采样结束是否开始
        private bool checkIsADstart(byte[] buffer)
        {
            if (buffer[0] == 0xA5 && buffer[1] == 0xA5 && buffer[2] == 0x22 && buffer[9] == 0xAA && buffer[10] == 0xFF && buffer[11] == 0x5A && buffer[12] == 0x5A)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //检查AD采样结束是否结束
        private bool checkIsADfinished(byte[] buffer)
        {
            if (buffer[0] == 0xA5 && buffer[1] == 0xA5 && buffer[2] == 0x22 && buffer[9] == 0x55 && buffer[10] == 0xFF && buffer[11] == 0x5A && buffer[12] == 0x5A)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //设定GPS采样时间成功
        private bool checkIsGPSsetOK(byte[] buffer)
        {
            if (buffer[0] == 0xA5 && buffer[1] == 0xA5 && buffer[2] == 0x25 && buffer[9] == 0x55 && buffer[10] == 0xFF && buffer[11] == 0x5A && buffer[12] == 0x5A)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //是否完全是采集数据，而不是命令码或者应答或者心跳包之类的。
        private bool checkIsPureData(byte[] buffer)
        {
            int DataLength = buffer.Length;
            if (buffer[0] == 0xA5 && buffer[1] == 0xA5 && buffer[2] == 0xAA && buffer[DataLength - 1] == 0x55 && DataLength == perPackageLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //5-3
        //检查所有设备处于第几命令阶段--stage1--除上传外的命令已发完，可以进行上传了
        private bool checkIsAllCmdStage1()
        {
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if ((dataitem.CmdStage != 1) && dataitem.intDeviceID != 0)
                    return false;
            }
            return true;
        }

        private bool checkIsAllCmdStage3()
        {
            //此处进行遍历操作
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if ((dataitem.CmdStage != 3) && dataitem.intDeviceID != 0)
                    return false;
            }
            return true;
        }

        //检查哈希表中是否已存在当前ID
        public string checkIsHaveID(int id)
        {
            foreach (DictionaryEntry de in htClient)
            {
                DataItem dataitem = (DataItem)de.Value;
                if (dataitem.intDeviceID == id)//设备掉线后address改变而ID号不变
                    return dataitem.strAddress;
            }
            return null;
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
                    dataitem.CmdStage = 2;
                    dataitem.isSendDataToServer = true;
                    dataitem.datalength = 0;
                    dataitem.currentsendbulk = 0;
                    SendCmdSingle(SetADcmd(0), dataitem.byteDeviceID, dataitem.socket);//发送第0包的命令;采集AD数据时第四个参数默认为1
                    //DebugLog.Debug("向设备--"+ dataitem.byteDeviceID + "--发送第0包的命令,并设置上传属性(自动上传)");
                }
            }
        }

        //选定设备并显示设备ID(ok)
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
                        if (String.Compare(dataitem.strAddress+"--" + dataitem.intDeviceID.ToString(), ChoosedAddress) == 0)
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
            //DebugLog.Debug("选定设备ID有："+ IDString);
        }

        //下拉菜单中的命令
        private void BtnSendCmd_Click(object sender, EventArgs e)
        {
            byte[] Cmd = new byte[16];
            switch (this.CmdBox.SelectedIndex)//根据下拉框当前选择的第几行文本来选择指令
            {
                //读取开启和关闭时长
                case 0:
                    Cmd = new byte[] { 0xA5, 0xA5, 0x26, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x04, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x5A, 0x5A };//设置开启时长
                    break;
                //读取经纬度
                case 1:
                    Cmd = new byte[] { 0xA5, 0xA5, 0x27, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0x5A, 0x5A };
                    break;
                //读取GPS采样时间
                case 2:
                    //Cmd = 0xA5 A5 25 00 00 00 01 00 04 FF FF FF FF FF 5A 5A;//长度是16
                    Cmd[0] = 0xA5;
                    Cmd[1] = 0xA5;
                    Cmd[2] = 0x25;
                    //Cmd[3] = ID[0];
                    //Cmd[4] = ID[1];
                    //Cmd[5] = ID[2];
                    //Cmd[6] = ID[3];
                    Cmd[7] = 0x00;
                    Cmd[8] = 0x04;
                    Cmd[9] = 0xFF;
                    Cmd[10] = 0xFF;
                    Cmd[11] = 0xFF;
                    Cmd[12] = 0xFF;
                    Cmd[13] = 0xFF;
                    Cmd[14] = 0x5A;
                    Cmd[15] = 0x5A;
                    //CmdLength = 16;
                    break;
            }

            SendCmdAll(Cmd);

        }
        
        //(多个)将命令从服务端发送到每一个选中的设备（普通命令）
        public void SendCmdAll(byte[] cmd)
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
                        //receiveDatarichTextBox.AppendText(timestamp + "向设备" + dataitem.intDeviceID + "发送命令是：" + byteToHexStr(cmd) + "\n");
                        ShowMsg(timestamp + "向设备" + dataitem.intDeviceID + "发送命令是：" + byteToHexStr(cmd) + "\n");
                        }
                        catch (Exception ex)
                        {
                            //DebugLog.Debug(ex);
                            Console.WriteLine(ex);
                        }
                        

                    }
                }                     
        }

        //(单个)将命令从服务端发送到特定的设备（数据采集）
        public void SendCmdSingle(byte[] cmd, byte[] id, Socket deviceSocket)
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
                //receiveDatarichTextBox.AppendText(timestamp + "向设备" + deviceid + "发送命令是：" + byteToHexStr(cmd) + "\n");
                ShowMsg(timestamp + "向设备" + deviceid + "发送命令是：" + byteToHexStr(cmd) + "\n");
                }
                catch (Exception ex)
                {
                    //DebugLog.Debug(ex);
                    Console.WriteLine(ex);
                }
                
            
        }

        //设置采样时间
        private void BtnSetCaptime_Click(object sender, EventArgs e)
        {
            StatusBox.Text = "正在设置采样时间";
            byte[] Cmd = new byte[16];
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            Cmd[2] = 0x25;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x01;
            Cmd[8] = 0x04;
            Cmd[9] = Convert.ToByte(HourBox.Text);
            Cmd[10] = Convert.ToByte(MinuteBox.Text);
            Cmd[11] = Convert.ToByte(SecondBox.Text);
            Cmd[12] = Convert.ToByte(MsBox.Text);
            Cmd[13] = 0xFF;
            Cmd[14] = 0x5A;
            Cmd[15] = 0x5A;
            //CmdLength = 16;
            SendCmdAll(Cmd);
        }
        //设置AP名(ssid)
        private void BtnSetAPName_Click(object sender, EventArgs e)
        {
            byte[] Cmd = new byte[27];
            //byte[] SetAPName = strToHexByte(APnameBox.Text);
            byte[] SetAPName = strToByte(APnameBox.Text);//转换成字符型
                                                         //byte[] APname1= APnameBox.Text.
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            Cmd[2] = 0x31;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x01;
            Cmd[8] = 0x0F;

            for (int i = 0, j = 9; i < SetAPName.Length; i++)
            {
                Cmd[j++] = SetAPName[i];
            }
            /*Cmd[9] = SetAPName[0];
            Cmd[10] = SetAPName[1];
            Cmd[11] = SetAPName[2];
            Cmd[12] = SetAPName[3];
            Cmd[13] = SetAPName[4];
            Cmd[14] = SetAPName[5];
            Cmd[15] = SetAPName[6];
            Cmd[16] = SetAPName[7];
            Cmd[17] = SetAPName[8];
            Cmd[18] = SetAPName[9];
            Cmd[19] = SetAPName[10];
            Cmd[20] = SetAPName[11];
            Cmd[21] = SetAPName[12];
            Cmd[22] = SetAPName[13];
            Cmd[23] = SetAPName[14];*/
            Cmd[24] = 0xFF;
            Cmd[25] = 0x5A;
            Cmd[26] = 0x5A;
            //CmdLength = 27;
            SendCmdAll(Cmd);
        }
        //设置AP的密码
        private void BtnSetAPpassword_Click(object sender, EventArgs e)
        {
            byte[] Cmd = new byte[23];
            byte[] SetAPpassword = strToByte(APpasswordBox.Text);
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            Cmd[2] = 0x32;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x01;
            Cmd[8] = 0x0C;
            for (int i = 0, j = 9; i < SetAPpassword.Length; i++)
            {
                Cmd[j++] = SetAPpassword[i];
            }
            /*Cmd[9] = SetAPpassword[0];
			Cmd[10] = SetAPpassword[1];
			Cmd[11] = SetAPpassword[2];
			Cmd[12] = SetAPpassword[3];
			Cmd[13] = SetAPpassword[4];
			Cmd[14] = SetAPpassword[5];
			Cmd[15] = SetAPpassword[6];
			Cmd[16] = SetAPpassword[7];
			Cmd[17] = SetAPpassword[8];
			Cmd[18] = SetAPpassword[9];
			Cmd[19] = SetAPpassword[10];
			Cmd[20] = SetAPpassword[11];*/
            Cmd[20] = 0xFF;
            Cmd[21] = 0x5A;
            Cmd[22] = 0x5A;
            //CmdLength = 23;
            SendCmdAll(Cmd);
        }
        //设置服务端IP地址
        private void BtnSetIPnameAndPort_Click(object sender, EventArgs e)
        {

            byte[] SetIPname1 = strToByte(IPtextBox1.Text);
            byte[] SetIPname2 = strToByte(IPtextBox2.Text);
            byte[] SetIPname3 = strToByte(IPtextBox3.Text);
            byte[] SetIPname4 = strToByte(IPtextBox4.Text);
            byte[] Cmd = new byte[32];
            //设置IP
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            //Cmd[2] = 0x2A;
            Cmd[2] = 0x29;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x01;
            Cmd[8] = 0x14;
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
            /*Cmd[9] = SetIPname[0];
			Cmd[10] = SetIPname[1];
			Cmd[11] = SetIPname[2];
			Cmd[12] = SetIPname[3];
			Cmd[13] = SetIPname[4];
			Cmd[14] = SetIPname[5];
			Cmd[15] = SetIPname[6];
			Cmd[16] = SetIPname[7];
			Cmd[17] = SetIPname[8];
			Cmd[18] = SetIPname[9];
			Cmd[19] = SetIPname[10];
			Cmd[20] = SetIPname[11];
			Cmd[21] = SetIPname[12];
			Cmd[22] = SetIPname[13];
			Cmd[23] = SetIPname[14];
			Cmd[24] = SetIPname[15];
			Cmd[25] = SetIPname[16];
			Cmd[26] = SetIPname[17];
			Cmd[27] = SetIPname[18];
			Cmd[28] = SetIPname[19];*/
            Cmd[29] = 0xFF;
            Cmd[30] = 0x5A;
            Cmd[31] = 0x5A;
            //CmdLength = 32;
            SendCmdAll(Cmd);
        }
        //设置Port
        private void BtnSetPort_Click(object sender, EventArgs e)
        {
            byte[] Cmd = new byte[20];
            byte[] SetPort = strToByte(PortextBox.Text);
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            //Cmd[2] = 0x27;
            Cmd[2] = 0x30;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x01;
            Cmd[8] = 0x0C;
            for (int i = 0, j = 9; i < SetPort.Length; i++)
            {
                Cmd[j++] = SetPort[i];
            }
            /*Cmd[9] = SetPort[0];
			Cmd[10] = SetPort[1];
			Cmd[11] = SetPort[2];
			Cmd[12] = SetPort[3];
			Cmd[13] = SetPort[4];
			Cmd[14] = SetPort[5];
			Cmd[15] = SetPort[6];
			Cmd[16] = SetPort[7];*/
            Cmd[17] = 0xFF;
            Cmd[18] = 0x5A;
            Cmd[19] = 0x5A;
            //CmdLength = 20;
            SendCmdAll(Cmd);
        }

        //让设备立即采样
        private void BtnGetData_Click(object sender, EventArgs e)
        {
            byte[] cmd = new byte[] { 0xA5, 0xA5, 0x25, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x04, 0x0A, 0x1E, 0x00, 0x00, 0xFF, 0x5A, 0x5A };

            if (DateTime.Now.Minute + 5 <= 60)
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

            /*byte[] Cmd = new byte[16];
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            Cmd[2] = 0x22;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x01;
            Cmd[8] = 0x01;
            Cmd[9] = 0xFF;
            Cmd[13] = 0xFF;
            Cmd[14] = 0x5A;
            Cmd[15] = 0x5A;
            //CmdLength = 16;
            SendCmdAll(Cmd);*/
        }

        //返回ADcmd
        public byte[] SetADcmd(int bulkCount)
        {
            byte[] Cmd = new byte[18];
            byte[] bytesbulkCount = new byte[2];
            bytesbulkCount = intToBytes(bulkCount);
            Cmd[0] = 0xA5;
            Cmd[1] = 0xA5;
            Cmd[2] = 0x23;
            //Cmd[3] = ID[0];
            //Cmd[4] = ID[1];
            //Cmd[5] = ID[2];
            //Cmd[6] = ID[3];
            Cmd[7] = 0x00;
            Cmd[8] = 0x06;
            Cmd[9] = 0x02;//0x0257等于599
            Cmd[10] = 0x57;//0x003B==59
            Cmd[11] = bytesbulkCount[0];
            Cmd[12] = bytesbulkCount[1];
            Cmd[13] = 0x03;//1000字节
            Cmd[14] = 0xE8;
            //Cmd[13] = 0x04;//1024字节
            //Cmd[14] = 0x00;
            Cmd[15] = 0xFF;
            Cmd[16] = 0x5A;
            Cmd[17] = 0x5A;
            //CmdLength = 18;
            if (Cmd[11] == 0x00 && Cmd[12] == 0x3A)
            {
                Cmd[11] = 0x03;
                Cmd[12] = 0x0A;
            }
            else if (Cmd[11] == 0x01 && Cmd[12] == 0x3A)
            {
                Cmd[11] = 0x13;
                Cmd[12] = 0x0A;
            }
            else if (Cmd[11] == 0x02 && Cmd[12] == 0x3A)
            {
                Cmd[11] = 0x23;
                Cmd[12] = 0x0A;
            }
            string strtest = byteToHexStr(Cmd);
            //receiveDatarichTextBox.AppendText("发送把采集的数据回传到服务器端的命令的数据是"+strtest);
            return (Cmd);
        }

        //保存文件，16进制，封装开头是0xAA，结尾是0x55
        public void StoreDataToFile(int intDeviceID, byte[] bytes)
        {
            /*   int flash_addr = 0;
               int sum1 = 0;
               int sum2 = 0;
               int FlashData = 0;
               int Data = 0;
               for (flash_addr = 0; flash_addr < g_datafulllength; flash_addr++)//校验00~FF的数据，理论值和实际值是否一致
               {
                   FlashData = bytes[flash_addr];
                   Data = flash_addr & 0xff;
                   if (Data != FlashData)
                       sum1++;
                   if (Data == FlashData)
                       sum2++;
               }
               MessageBox.Show(intDeviceID+"差异+"+sum1.ToString());*/

            string filename = DateTime.Now.ToString("yyyy-MM-dd") + "--" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "--" + intDeviceID.ToString();//以日期时间命名，避免文件名重复
            byte[] fileStartAndEnd = new byte[2] { 0xAA, 0x55 };//保存文件的头是AA，尾是55
            string path = @"D:\\Data\\" + filename + ".dat";
            FileStream F = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            F.Write(fileStartAndEnd, 0, 1);
            F.Write(bytes, 0, bytes.Length);
            F.Write(fileStartAndEnd, 1, 1);
            F.Flush();
            F.Close();
            //StatusBox.Text = "文件保存成功";
            ShowMsg("文件保存成功");

            //DebugLog.Debug("设备ID为" + intDeviceID + "文件保存成功");
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
                    //StatusBox.Text = dataitem.currentsendbulk.ToString();
                    //DebugLog.Debug("向设备--" + dataitem.byteDeviceID + "--发送第0包的命令,并设置上传属性(手动上传)");
                }
            }
        }

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

        private void BtnSetOpenAndCloseTime_Click_1(object sender, EventArgs e)
        {
            byte[] CmdSetOpenAndCloseTime = new byte[] { 0xA5, 0xA5, 0x26, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x04, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x5A, 0x5A };//设置开启时长
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

            double length = getDistance.caculateDistance(lat1, lng1, lat2, lng2);
            ShowMsg("两点间的距离是：" + length.ToString() + "米" + "\n");
        }

        //打开热点
        private void OpenVirtualWIFI_Click(object sender, EventArgs e)
        {
            string output = "";
            string cmd = "netsh wlan set hostednetwork mode = allow ssid = "+ ssidBox.Text + " key = " + passWordBox.Text;
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
            string cmd = "netsh interface ip set address \"" + textBoxVirtuaName.Text + "\" static "+ textBoxVirtualIP.Text+ " " +textBoxSubnetMask.Text + " " + textBoxGateWay.Text + " 1";

            processCMD.RunCmd(cmd, out output);
            MessageBox.Show(output);
        }

        private void ReadAPName_Click(object sender, EventArgs e)
        {
            byte[] CmdReadAPName = new byte[] { 0xA5, 0xA5, 0x31, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0x5A, 0x5A };
            SendCmdAll(CmdReadAPName);
        }

        private void ReadAPPassword_Click(object sender, EventArgs e)
        {
            byte[] CmdReadAPPassword = new byte[] { 0xA5, 0xA5, 0x32, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0x5A, 0x5A };
            SendCmdAll(CmdReadAPPassword);
        }

        private void ReadServerIP_Click(object sender, EventArgs e)
        {
            byte[] CmdReadServerIP = new byte[] { 0xA5, 0xA5, 0x29, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0x5A, 0x5A };
            SendCmdAll(CmdReadServerIP);
        }

        private void ReadServerPort_Click(object sender, EventArgs e)
        {
            byte[] CmdReadServerPort = new byte[] { 0xA5, 0xA5, 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0x5A, 0x5A };
            SendCmdAll(CmdReadServerPort);
        }
    }//对应public partial class Form1 : Form

}//对应namespace Wipai_app
