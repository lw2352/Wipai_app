using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

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
}
