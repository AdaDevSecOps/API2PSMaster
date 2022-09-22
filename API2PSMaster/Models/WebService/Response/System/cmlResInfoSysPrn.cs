using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysPrn
    {
        public string rtSppCode { get; set; }
        public string rtSppValue { get; set; }
        public string rtSppRef { get; set; }
        public string rtSppType { get; set; }
        public string rtSppStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}