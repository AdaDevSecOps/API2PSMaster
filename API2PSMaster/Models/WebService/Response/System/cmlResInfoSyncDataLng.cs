using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    //[Serializable]
    public class cmlResInfoSyncDataLng
    {
        public int rnSynSeqNo { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtSynName { get; set; }
        public string rtSynRmk { get; set; }
    }
}