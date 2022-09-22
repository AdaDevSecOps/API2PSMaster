using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResSysPrnDwn
    {
        public List<cmlResInfoSysPrn> raSysPrn { get; set; }
        public List<cmlResInfoSysPrnLng> raSysPrnLng { get; set; }
    }
}