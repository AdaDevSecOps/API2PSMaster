using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.System
{
    public class cmlReqSyncData
    {
        public int pnSynSeqNo { get; set; }
        public string ptSynTable { get; set; }
        public Nullable<DateTime> pdSynLast { get; set; }
    }
}