using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResPdtClassDwn
    {
        public List<cmlResInfoPdtClass> raPdtClass {get;set;}
        public List<cmlResInfoPdtClassLng> raPdtClassLng { get; set; }
    }
}