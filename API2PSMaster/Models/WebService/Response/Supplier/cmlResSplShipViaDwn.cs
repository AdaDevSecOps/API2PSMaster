using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    //[Serializable]
    public class cmlResSplShipViaDwn
    {
        public List<cmlResInfoSplShipVia> raSplShipVia { get; set; }
        public List<cmlResInfoSplShipViaLng> raSplShipViaLng { get; set; }
    }
}