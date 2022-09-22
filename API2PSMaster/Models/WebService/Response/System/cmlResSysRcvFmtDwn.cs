using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResSysRcvFmtDwn
    {
        public List<cmlResInfoSysRcvFmt> raSysRcvFmt { get; set; }
        public List<cmlResInfoSysRcvFmtLng> raSysRcvFmtLng { get; set; }
    }
}