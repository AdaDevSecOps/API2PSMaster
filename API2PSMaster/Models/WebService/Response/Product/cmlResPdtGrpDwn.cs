using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtGrpDwn
    {
        public List<cmlResInfoPdtGrp> raPdtGrp { get; set; }

        public List<cmlResInfoPdtGrpLng> raPdtGrpLng { get; set; }

        public List<cmlResInfoImgPdt> raPdtGrpImg { get; set; }
    }
}