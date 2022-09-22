using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResPdtSubClassDwn
    {
        public List<cmlResInfoPdtSubClass> raPdtSubClass { get; set; }
        public List<cmlResInfoPdtSubClassLng> raPdtSubClassLng { get; set; }
    }
}