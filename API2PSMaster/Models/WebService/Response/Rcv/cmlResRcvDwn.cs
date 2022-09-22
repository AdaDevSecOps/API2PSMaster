using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rcv
{
    //[Serializable]
    public class cmlResRcvDwn
    {
        public List<cmlResInfoRcv> raRcv { get; set; }
        public List<cmlResInfoRcvLng> raRcvLng { get; set; }
        public List<cmlResInfoRcvSpc> raRcvSpc { get; set; }    //*Arm 63-04-24
        public List<cmlResInfoRcvSpcConfig> raRcvSpcConfig { get; set; }  //*Arm 63-10-30
    }
}