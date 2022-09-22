using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Area
{
    //[Serializable]
    public class cmlResAreaDwn
    {
        public List<cmlResInfoArea> raArea { get; set; }
        public List<cmlResInfoAreaLng> raAreaLng { get; set; }
    }
}