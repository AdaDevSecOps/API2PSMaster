using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoPrinter
    {
        public string rtPrnCode { get; set; }
        public string rtPrnSrcType { get; set; }
        public string rtSppCode { get; set; }
        public string rtPrnDriver { get; set; }
        public string rtPrnType { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}