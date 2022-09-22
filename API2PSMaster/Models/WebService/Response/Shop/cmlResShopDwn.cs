using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Shop
{
    //[Serializable]
    public class cmlResShopDwn
    {
        public List<cmlResInfoShop> raShop { get; set; }
        public List<cmlResInfoShopLng> raShopLng { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}