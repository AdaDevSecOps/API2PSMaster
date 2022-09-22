using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoShopSize
    {
        public string rtBchCode { get; set; }
        public string rtShpCode { get; set; }
        public decimal rcLayRowQty { get; set; }
        public decimal rcLayColQty { get; set; }
        public string rtLayStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtCreateBy { get; set; }
    }
}