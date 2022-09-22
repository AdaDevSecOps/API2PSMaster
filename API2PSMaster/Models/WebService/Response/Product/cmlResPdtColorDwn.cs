using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtColorDwn
    {
        public List<cmlResInfoPdtColor> raPdtColor { get; set; }
        public List<cmlResInfoPdtColorLng> raPdtColorLng { get; set; }
    }
}