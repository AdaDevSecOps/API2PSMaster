using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResCstOcpDwn
    {
        public List<cmlResInfoCstOcp> raCstOcp { get; set; }
        public List<cmlResInfoCstOcpLng> raCstOcpLng { get; set; }
    }
}