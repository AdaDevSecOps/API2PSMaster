using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtPmtGrpDwn
    {
        public List<cmlResInfoPdtPmtGrp> raPdtPmtGrp { get; set; }
        public List<cmlResInfoPdtPmtGrpLng> raPdtPmtGrpLng { get; set; }
    }
}