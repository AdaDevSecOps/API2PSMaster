using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.District
{
    //[Serializable]
    public class cmlResDistrictDwn
    {
        public List<cmlResInfoDistrict> raDistrinct { get; set; }
        public List<cmlResInfoDistrictLng> raDistrinctLng { get; set; }
    }
}