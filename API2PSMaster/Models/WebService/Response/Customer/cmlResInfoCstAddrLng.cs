using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstAddrLng
    {
        public string rtCstCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtAddGrpType { get; set; }
        public Int64 rnAddSeqNo { get; set; }
        public string rtAddRefNo { get; set; }
        public string rtAddName { get; set; }
        public string rtAddRmk { get; set; }
        public string rtAddCountry { get; set; }
        public string rtAreCode { get; set; }
        public string rtZneCode { get; set; }
        public string rtAddVersion { get; set; }
        public string rtAddV1No { get; set; }
        public string rtAddV1Soi { get; set; }
        public string rtAddV1Village { get; set; }
        public string rtAddV1Road { get; set; }
        public string rtAddV1SubDist { get; set; }
        public string rtAddV1DstCode { get; set; }
        public string rtAddV1PvnCode { get; set; }
        public string rtAddV1PostCode { get; set; }
        public string rtAddV2Desc1 { get; set; }
        public string rtAddV2Desc2 { get; set; }
        public string rtAddWebsite { get; set; }
        public string rtAddLongitude { get; set; }
        public string rtAddLatitude { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}