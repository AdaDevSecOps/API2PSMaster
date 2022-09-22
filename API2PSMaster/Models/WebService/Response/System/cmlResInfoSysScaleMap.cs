using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysScaleMap
    {
        public string rtSslCode { get; set; }
        public int rnSsmSeq { get; set; }
        public string rtSsmDesc { get; set; }
        public Nullable<Int64> rnSsmFixLen { get; set; }
        public string rtSsmChrSplit { get; set; }
        public string rtSsmChrExtra { get; set; }
        public string rtsmKey { get; set; }
    }
}