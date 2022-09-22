using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResPdtDepartDwn
    {
        public List<cmlResInfoPdtDepart> raPdtDepart { get; set; }
        public List<cmlResInfoPdtDepartLng> raPdtDepartLng { get; set; }
    }
}