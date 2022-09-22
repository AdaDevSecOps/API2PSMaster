using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtItemDwn
    {
        public List<cmlResInfoPdt> raPdt { get; set; }
        public List<cmlResInfoPdtLng> raPdtLng { get; set; }
        public List<cmlResInfoPdtPackSize> raPdtPackSize { get; set; }
        public List<cmlResInfoPdtBar> raPdtBar { get; set; }
        public List<cmlResInfoPdtBrand> raPdtBrand { get; set; }
        public List<cmlResInfoPdtBrandLng> raPdtBrandLng { get; set; }
        public List<cmlResInfoPdtGrp> raPdtGrp { get; set; }
        public List<cmlResInfoPdtGrpLng> raPdtGrpLng { get; set; }
        public List<cmlResInfoPdtModel> raPdtModel { get; set; }
        public List<cmlResInfoPdtModelLng> raPdtModelLng { get; set; }
        public List<cmlResInfoPdtPriHD> raPdtPriHD { get; set; }
        public List<cmlResInfoPdtPriDT> raPdtPriDT { get; set; }
        public List<cmlResInfoPdtTouchGrp> raPdtTouchGrp { get; set; }
        public List<cmlResInfoPdtTouchGrpLng> raPdtTouchGrpLng { get; set; }
        public List<cmlResInfoPdtType> raPdtType { get; set; }
        public List<cmlResInfoPdtTypeLng> raPdtTypeLng { get; set; }
        public List<cmlResInfoPdtUnit> raPdtUnit { get; set; }
        public List<cmlResInfoPdtUnitLng> raPdtUnitLng { get; set; }
        public List<cmlResInfoImgPdt> raImgPdt { get; set; }
        public List<cmlTCNMPdtAge> raTCNMPdtAge { get; set; }   //*Em 62-08-17
        public List<cmlResTCNMPdtSpcBch> raTCNMPdtSpcBch { get; set; }  //*Em 62-09-09
        public List<cmlResInfoPdtDrug> raTCNMPdtDrug { get; set; }  //*Arm 63-01-17
        public List<cmlResInfoPdtSpcWah> raTCNMPdtSpcWah { get; set; }  //*Arm 63-01-17

    }
}