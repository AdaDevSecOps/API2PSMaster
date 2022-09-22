using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtPriDT
    {
        public string rtBchCode { get; set; }
        public string rtXphDocNo { get; set; }
        public Int64 rnXpdSeq { get; set; }
        public string rtPdtCode { get; set; }
        public string rtPunCode { get; set; }
        public Nullable<decimal> rcXpdPriceRet { get; set; }
        public Nullable<decimal> rcXpdPriceWhs { get; set; }
        public Nullable<decimal> rcXpdPriceNet { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}