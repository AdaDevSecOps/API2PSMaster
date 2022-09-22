using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Reason
{
    //[Serializable]
    public class cmlResRsnDwn
    {
        public List<cmlResInfoRsn> raRsn { get; set; }
        public List<cmlResInfoRsnLng> raRsnLng { get; set; }
        public List<cmlResInfoRsnGrpLng> raRsnGrpLng { get; set; }
    }
}