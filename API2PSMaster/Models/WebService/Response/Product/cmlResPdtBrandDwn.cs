using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtBrandDwn
    {
        public List<cmlResInfoPdtBrand> raPdtBrand { get; set; }
        public List<cmlResInfoPdtBrandLng> raPdtBrandLng { get; set; }
    }
}