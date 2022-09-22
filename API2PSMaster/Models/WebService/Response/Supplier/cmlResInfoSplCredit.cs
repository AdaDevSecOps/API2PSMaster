using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    //[Serializable]
    public class cmlResInfoSplCredit
    {
        public string rtSplCode { get; set; }
        public Nullable<Int64> rnSplCrTerm { get; set; }
        public Nullable<decimal> rcSplCrLimit { get; set; }
        public string rtSplDayCta { get; set; }
        public Nullable<DateTime> rdSplLastCta { get; set; }
        public Nullable<DateTime> rdSplLastPay { get; set; }
        public Nullable<Int64> rnSplLimitRow { get; set; }
        public Nullable<decimal> rcSplLeadTime { get; set; }
        public string rtSplViaRmk { get; set; }
        public string rtViaCode { get; set; }
        public string rtSplTspPaid { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}