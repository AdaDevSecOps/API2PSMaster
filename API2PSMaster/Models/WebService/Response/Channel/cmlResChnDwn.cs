using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Channel
{
    /// <summary>
    /// Download ข้อมูล Channel
    /// </summary>
    public class cmlResChnDwn
    {
        public List<cmlResInfoChannel> raChannel { get; set; }
        public List<cmlResInfoChannelLng> raChannelLng { get; set; }
        public List<cmlResInfoChannelSpc> raChannelSpc { get; set; } //*Arm 64-01-14
    }
}