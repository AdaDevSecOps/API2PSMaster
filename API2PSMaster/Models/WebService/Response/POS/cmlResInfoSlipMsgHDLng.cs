using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoSlipMsgHDLng
    {
        public string rtSmgCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtSmgTitle { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
        public string rtFonts { get; set; } //Net 65-09-06
    }
}