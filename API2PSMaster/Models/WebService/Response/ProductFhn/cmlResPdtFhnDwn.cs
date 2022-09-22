using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResPdtFhnDwn
    {
        public List<cmlResInfoPdtFhn> raPdt { get; set; }
        public List<cmlResInfoPdtSeason> raPdtSeason { get; set; }
        public List<cmlResInfoPdtSeasonLng> raPdtSeasonLng { get; set; }
        public List<cmlResInfoPdtDCS> raPdtDCS { get; set; }
        public List<cmlResInfoPdtDCSLng> raPdtDCSLng { get; set; }
        public List<cmlResInfoPdtDepart> raPdtDepart { get; set; }
        public List<cmlResInfoPdtDepartLng> raPdtDepartLng { get; set; }
        public List<cmlResInfoPdtClass> raPdtClass { get; set; }
        public List<cmlResInfoPdtClassLng> raPdtClassLng { get; set; }
        public List<cmlResInfoPdtSubClass> raPdtSubClass { get; set; }
        public List<cmlResInfoPdtSubClassLng> raPdtSubClassLng { get; set; }
    }
}