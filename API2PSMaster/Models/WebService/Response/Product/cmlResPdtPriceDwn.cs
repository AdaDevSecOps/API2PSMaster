using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtPriceDwn
    {
        public List<cmlResInfoPdtPriHD> raPdtPriHD { get; set; }
        public List<cmlResInfoPdtPriDT> raPdtPriDT { get; set; }
    }
}