using API2PSMaster.Models.WebService.Response.Center;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.SalePerson
{
    //[Serializable]
    public class cmlResSpnDwn
    {
        public List<cmlResInfoSpn> raSpn { get; set; }
        public List<cmlResInfoSpnLng> raSpnLng { get; set; }
        public List<cmlResInfoSpnGrp> raSpnGrp { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
    }
}