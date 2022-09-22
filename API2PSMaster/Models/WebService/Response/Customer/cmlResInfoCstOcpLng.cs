using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstOcpLng
    {
        public string rtOcpCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtOcpName { get; set; }
        public string rtOcpRmk { get; set; }
    }
}