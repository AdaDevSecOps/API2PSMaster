using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtModelDwn
    {
        public List<cmlResInfoPdtModel> raPdtModel { get; set; }
        public List<cmlResInfoPdtModelLng> raPdtModelLng { get; set; }
    }
}