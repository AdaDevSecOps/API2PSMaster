using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstContactLng
    {
        public string rtCstCode { get; set; }
        public Int64 rnLngID { get; set; }
        public Int64 rnCtrSeq { get; set; }
        public string rtCtrName { get; set; }
        public string rtCtrFax { get; set; }
        public string rtCtrTel { get; set; }
        public string rtCtrEmail { get; set; }
        public string rtCtrRmk { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}