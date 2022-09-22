using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResPdtDCSDwn
    {
        public List<cmlResInfoPdtDCS> raPdtDCS { get; set; }
        public List<cmlResInfoPdtDCSLng> raPdtDCSLng { get; set; }
    }
}