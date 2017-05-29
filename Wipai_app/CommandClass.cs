﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wipai_app
{
    class CmdItem
    {
        //(数据位用0xFF填充)
        //上传AD数据包--0x23
        public byte[] CmdADPacket            = new byte[] { 0xA5, 0xA5, 0x23, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x06, 0x02, 0x57, 0xFF, 0xFF, 0x03, 0xE8, 0xFF, 0x5A, 0x5A };
        //设定GPS采样时间,byte[9]是小时，byte[10]是分钟--0x25
        public byte[] CmdSetCapTime          = new byte[] { 0xA5, 0xA5, 0x25, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x04, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x5A, 0x5A };
        //设置开启和关闭时长--0x26
        public byte[] CmdSetOpenAndCloseTime = new byte[] { 0xA5, 0xA5, 0x26, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x04, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x5A, 0x5A };
        //读取经纬度--0x27
        public byte[] CmdReadGPSData         = new byte[] { 0xA5, 0xA5, 0x27, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xFF, 0x5A, 0x5A };


        //(数据位用0x00填充)
        //设置服务端IP地址--0x29
        public byte[] CmdSetServerIP         = new byte[] { 0xA5, 0xA5, 0x29, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x5A, 0x5A };
        //设置服务端Port端口--0x30
        public byte[] CmdSetServerPort       = new byte[] { 0xA5, 0xA5, 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x5A, 0x5A };
        //设置AP名--0x31
        public byte[] CmdSetAPssid           = new byte[] { 0xA5, 0xA5, 0x31, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x5A, 0x5A };
        //设置AP的密码--0x32
        public byte[] CmdSetAPpassword       = new byte[] { 0xA5, 0xA5, 0x32, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x5A, 0x5A };

    }
}
