using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstTypeLng
    {
        public string rtCtyCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCtyName { get; set; }
        public string rtCtyRmk { get; set; }
    }
}