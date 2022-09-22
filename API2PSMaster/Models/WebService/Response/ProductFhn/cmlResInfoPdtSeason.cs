using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResInfoPdtSeason
    {
        public string rtPgpCode { get; set; }
        public Int64 rnPgpLevel { get; set; }
        public string rtPgpParent { get; set; }
        public string rtPgpChain { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}