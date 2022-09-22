using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResCstGrpDwn
    {
        public List<cmlResInfoCstGrp> raCstGrp { get; set; }
        public List<cmlResInfoCstGrpLng> raCstGrpLng { get; set; }
    }
}