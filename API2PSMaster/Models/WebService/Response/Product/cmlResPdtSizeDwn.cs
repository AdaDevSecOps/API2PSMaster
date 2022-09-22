using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
   //[Serializable]
    public class cmlResPdtSizeDwn
    {
        public List<cmlResInfoPdtSize> raPdtSize { get; set; }
        public List<cmlResInfoPdtSizeLng> raPdtSizeLng { get; set; }
    }
}