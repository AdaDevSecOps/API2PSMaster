using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtUnitDwn
    {
        public List<cmlResInfoPdtUnit> raPdtUnit { get; set; }

        public List<cmlResInfoPdtUnitLng> raPdtUnitLng { get; set; }

    }
}