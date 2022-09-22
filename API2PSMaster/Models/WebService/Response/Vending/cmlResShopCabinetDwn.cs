using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResShopCabinetDwn
    {
        public List<cmlResInfoShopCabinet> raShopCabinet { get; set; }
        public List<cmlResInfoShopCabinetLng> raShopCabinetLng { get; set; }
    }
}