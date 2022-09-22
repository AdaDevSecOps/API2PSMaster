using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstPoint
    {
        public string rtBchCode { get; set; }
        public string rtCstCode { get; set; }
        public string rtPntRefDoc { get; set; }
        public string rtSplCode { get; set; }
        public Nullable<DateTime> rdPntDate { get; set; }
        public Nullable<decimal> rcPntOptBuyAmt { get; set; }
        public Nullable<decimal> rcPntOptGetAmt { get; set; }
        public Nullable<Int64> rnPntB4Bill { get; set; }
        public Nullable<decimal> rcPntBillAmt { get; set; }
        public Nullable<Int64> rnPntBillQty { get; set; }
        public string rtPntExpired { get; set; }
        public string rtPntStaPrcDoc { get; set; }
        public string rtPntCardType { get; set; }
        public string rtCptJDate { get; set; }
        public string rtCptTime { get; set; }
        public Nullable<DateTime> rdPntSplStart { get; set; }
        public Nullable<DateTime> rdPntSplExpired { get; set; }
        public string rtPntStaExpired { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}