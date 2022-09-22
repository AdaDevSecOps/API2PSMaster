using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    //[Serializable]
    public class cmlResRateDwn
    {
        public List<cmlResInfoRate> raRate { get; set; }
        public List<cmlResInfoRateLng> raRateLng { get; set; }
        public List<cmlResInfoRateUnit> raRateUnit { get; set; }

        public List<cmlResInfoImgObj> raImgObj { get; set; }     //*Nick 65-08-24
        public List<cmlResInfoSysRateLng> raSysRateLng { get; set; } //*Arm 65-09-16
    }
}