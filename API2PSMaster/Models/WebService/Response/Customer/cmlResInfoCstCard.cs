using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstCard
    {
        public string rtCstCode { get; set; }
        public Nullable<DateTime> rdCstApply { get; set; }
        public string rtCstCrdNo { get; set; }
        public string rtBchCode { get; set; }
        public Nullable<DateTime> rdCstCrdIssue { get; set; }
        public Nullable<DateTime> rdCstCrdExpire { get; set; }
        public string rtCstStaAge { get; set; }
    }
}