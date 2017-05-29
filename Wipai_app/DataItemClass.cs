using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;

namespace Wipai_app
{
    /// <summary>
    /// 设备的属性
    /// 17个
    /// </summary>
    class DataItem
    {
        public string strIP;
        public string strPort;      //设备的端口号(掉线后改变)
        public Socket socket;   //Socket of the client
        public byte[] SingleBuffer;
        public string strAddress;
     
        public byte[] byteDeviceID; //这个是设备的ID的byte数组
        public int intDeviceID;

        public int datalength;
        public byte[] byteAllData; //所有数据，算一个完整的数据
        public int currentsendbulk; //当前发送的包数
       
        public bool isSendDataToServer;//发送数据到服务器
        public bool isChoosed;
        public int CmdStage;//0-没发命令；1-除上传以外的命令发完；2-分段时间设定完；3-数据上传并存储完
        public int uploadGroup;//用来判断对哪一组设备发送上传命令

        public byte[] byteTimeStamp;//时间戳
        public double Longitude;//经度，后半段
        public double Latitude;//纬度， 前半段
    }
   
}
