using API2PSMaster.Models.WebService.Response.Center;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Merchant
{
    public class cmlResMerchant
    {
        public List<cmlTCNMMerchant> raTCNMMerchant { get; set; }
        public List<cmlTCNMMerchant_L> raTCNMMerchant_L { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }  //*Em 62-10-04
    }
}