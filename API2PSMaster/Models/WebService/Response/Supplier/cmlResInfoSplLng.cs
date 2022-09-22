using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    //[Serializable]
    public class cmlResInfoSplLng
    {
        public string rtSplCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtSplName { get; set; }
        public string rtSplNameOth { get; set; }
        public string rtSplPayRmk { get; set; }
        public string rtSplBillRmk { get; set; }
        public string rtSplViaRmk { get; set; }
        public string rtSplRmk { get; set; }
    }
}