using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResShopSizeDwn
    {
        public List<cmlResInfoShopSize> raShopSize { get; set; }
        public List<cmlResInfoShopSizeLng> raShopSizeLng { get; set; }
    }
}