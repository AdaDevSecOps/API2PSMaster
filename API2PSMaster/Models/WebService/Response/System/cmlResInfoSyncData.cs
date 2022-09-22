using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    //[Serializable]
    public class cmlResInfoSyncData
    {
        public int rnSynSeqNo { get; set; }
        public string rtSynGroup { get; set; }
        public string rtSynTable { get; set; }
        public string rtSynTable_L { get; set; }
        public string rtSynType { get; set; }
        public Nullable<DateTime> rdSynLast { get; set; }
        public Nullable<int> rnSynSchedule { get; set; }
        public string rtSynStaUse { get; set; }
        public string rtSynUriDwn { get; set; }
        public string rtSynUriUld { get; set; }
    }
}