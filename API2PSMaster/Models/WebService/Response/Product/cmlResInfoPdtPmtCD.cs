using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtPmtCD
    {
        public string rtBchCode { get; set; }
        public string rtPmhCode { get; set; }
        public Int64 rnPmcSeq { get; set; }
        public string rtSpmCode { get; set; }
        public string rtPmcGrpName { get; set; }
        public string rtPmcStaGrpCond { get; set; }
        public Nullable<decimal> rcPmcPerAvgDis { get; set; }
        public Nullable<decimal> rcPmcBuyAmt { get; set; }
        public Nullable<decimal> rcPmcBuyQty { get; set; }
        public Nullable<decimal> rcPmcBuyMinQty { get; set; }
        public Nullable<decimal> rcPmcBuyMaxQty { get; set; }
        public Nullable<DateTime> rdPmcBuyMinTime { get; set; }
        public Nullable<DateTime> rdPmcBuyMaxTime { get; set; }
        public Nullable<decimal> rcPmcGetCond { get; set; }
        public Nullable<decimal> rcPmcGetValue { get; set; }
        public Nullable<decimal> rcPmcGetQty { get; set; }
        public string rtSpmStaBuy { get; set; }
        public string rtSpmStaRcv { get; set; }
        public string rtSpmStaAllPdt { get; set; }
        public string rtPmcGrpCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}