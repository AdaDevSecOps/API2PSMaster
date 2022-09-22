using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResShopTypeDwn
    {
        public List<cmlResInfoShopType> raShopType { get; set; }
        public List<cmlResInfoShopTypeLng> raShopTypeLng { get; set; }
    }
}