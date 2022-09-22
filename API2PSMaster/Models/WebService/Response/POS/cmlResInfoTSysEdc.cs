using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoTSysEdc
    {
        public string rtSedCode { get; set; }
        public string rtSedModel { get; set; }
        public Nullable<Int64> rnSedAck { get; set; }
        public string rtSedDllVer { get; set; }
        public Nullable<Int64> rnSedTimeOut { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}