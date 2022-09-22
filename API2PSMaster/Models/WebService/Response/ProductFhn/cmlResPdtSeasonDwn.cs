using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResPdtSeasonDwn
    {
        public List<cmlResInfoPdtSeason> raPdtSeason { get; set; }
        public List<cmlResInfoPdtSeasonLng> raPdtSeasonLng { get; set; }
    }
}