using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Zone
{
    //[Serializable]
    public class cmlResZoneDwn
    {
        public List<cmlResInfoZone> raZone { get; set; }
        public List<cmlResInfoZoneLng> raZoneLng { get; set; }
    }
}