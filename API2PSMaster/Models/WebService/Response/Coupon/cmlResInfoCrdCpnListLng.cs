using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    //[Serializable]
    public class cmlResInfoCrdCpnListLng
    {
        public string rtCclCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCclName { get; set; }
        public string rtCclPrnCond { get; set; }
    }
}