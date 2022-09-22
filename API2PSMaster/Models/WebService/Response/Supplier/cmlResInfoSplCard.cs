using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    //[Serializable]
    public class cmlResInfoSplCard
    {
        public string rtSplCode { get; set; }
        public Nullable<DateTime> rdSplApply { get; set; }
        public string rtSplRefExCrdNo { get; set; }
        public Nullable<DateTime> rdSplCrdIssue { get; set; }
        public Nullable<DateTime> rdSplCrdExpire { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}