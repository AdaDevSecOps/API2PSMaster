using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtTouchGrpDwn
    {
        public List<cmlResInfoPdtTouchGrp> raPdtTouchGrp { get; set; }

        public List<cmlResInfoPdtTouchGrpLng> raPdtTouchGrpLng { get; set; }

        public List<cmlResInfoImgPdt> raPdtTouchGrpImg { get; set; }
    }
}