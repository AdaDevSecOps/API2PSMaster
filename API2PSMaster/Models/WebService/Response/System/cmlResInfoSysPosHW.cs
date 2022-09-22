using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysPosHW
    {
        public string rtShwCode { get; set; }
        public string rtShwHWKey { get; set; }
        public string rtShwName { get; set; }
        public string rtShwNameEng { get; set; }
        public string rtShwSystem { get; set; }
        public string rtShwStaAlwPrinter { get; set; }
        public string rtShwStaAlwCom { get; set; }
        public string rtShwStaAlwTCP { get; set; }
        public string rtShwStaAlwBT { get; set; }
    }
}