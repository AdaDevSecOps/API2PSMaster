using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.BankNote
{
    //[Serializable]
    public class cmlResInfoBankNote
    {
        public string rtRteCode { get; set; }
        public string rtBntCode { get; set; }
        public string rtBntStaShw { get; set; }
        public Nullable<decimal> rcBntRateAmt { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}