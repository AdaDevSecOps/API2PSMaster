using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstGrpLng
    {
        public string rtCgpCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCgpName { get; set; }
        public string rtCgpRmk { get; set; }
    }
}