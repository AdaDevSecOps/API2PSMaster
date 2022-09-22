using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.SubDistrict
{
    //[Serializable]
    public class cmlResSubDistDwn
    {
        public List<cmlResInfoSubDist> raSubDist { get; set; }
        public List<cmlResInfoSubDistLng> raSubDistLng { get; set; }
    }
}