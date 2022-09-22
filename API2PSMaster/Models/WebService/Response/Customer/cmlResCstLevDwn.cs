using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResCstLevDwn
    {
        public List<cmlResInfoCstLev> raCstLev { get; set; }
        public List<cmlResInfoCstLevLng> raCstLevLng { get; set; }
    }
}