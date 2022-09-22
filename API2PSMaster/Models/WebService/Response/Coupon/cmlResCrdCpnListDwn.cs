using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    //[Serializable]
    public class cmlResCrdCpnListDwn
    {
        public List<cmlResInfoCrdCpnList> raCrdCpnList { get; set; }
        public List<cmlResInfoCrdCpnListLng> raCrdCpnListLng { get; set; }
    }
}