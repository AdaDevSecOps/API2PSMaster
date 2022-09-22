using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.SalePerson
{
    //[Serializable]
    public class cmlResInfoSpn
    {
        public string rtSpnCode { get; set; }
        public string rtSpnTel { get; set; }
        public Nullable<decimal> rcSpnSleAmt { get; set; }
        public string rtSpnEmail { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}