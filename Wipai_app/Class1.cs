using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;

namespace Wipai_app
{
    class DataItem
    {
        public int datalength;
        public string strIP;
        public string strPort;      //设备的端口号(掉线后改变)
        public byte[] byteAllData; //所有数据，算一个完整的数据。
        public byte[] byteDeviceID; //这个是设备的ID的byte数组
        //public string deviceMac;    //转化为string后的ID
        public int intDeviceID;
        public int currentsendbulk; //当前发送的包数
        //public int totalsendbulk; //总发送的包数
        public Socket socket;   //Socket of the client
        public bool isSendDataToServer;//发送数据到服务器
        public bool isChoosed;

        public int CmdStage;//0-没发命令；1-除上传以外的命令发完；2-分段时间设定完；3-数据上传并存储完
        public int uploadGroup;//用来判断对哪一组设备发送上传命令

        public byte[] SingleBuffer;
        public string strAddress;

        public byte[] byteTimeStamp;//时间戳
        public double Longitude;//经度，后半段
        public double Latitude;//纬度， 前半段
    }

    class GetDistance
    {
        private const double EARTH_RADIUS = 6378.137 * 1000;//地球半径，单位为米
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public double caculateDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }

    class cmdHelper
    {
        private string CmdPath = @"C:\Windows\System32\cmd.exe";

        /// <summary>
        /// 执行cmd命令
        /// 多命令请使用批处理命令连接符：
        /// <![CDATA[
        /// &:同时执行两个命令
        /// |:将上一个命令的输出,作为下一个命令的输入
        /// &&：当&&前的命令成功时,才执行&&后的命令
        /// ||：当||前的命令失败时,才执行||后的命令]]>
        /// 其他请百度
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="output"></param>
        public void RunCmd(string cmd, out string output)
        {
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }
    }
}
