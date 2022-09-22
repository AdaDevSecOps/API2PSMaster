using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtPriListDwn
    {
        public List<cmlResInfoPdtPriList> raPdtPriList { get; set; }
        public List<cmlResInfoPdtPriListLng> raPdtPriListLng { get; set; }
    }
}