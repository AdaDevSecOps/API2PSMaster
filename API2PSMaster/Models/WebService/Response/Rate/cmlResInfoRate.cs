using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    //[Serializable]
    public class cmlResInfoRate
    {
        public string rtRteCode { get; set; }
        public Nullable<decimal> rcRteRate { get; set; }
        public Nullable<decimal> rcRteFraction { get; set; }
        public string rtRteType { get; set; }
        public string rtRteTypeChg { get; set; }
        public string rtRteSign { get; set; }
        public string rtRteStaLocal { get; set; }
        public string rtRteStaUse { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
        public string rtAgnCode { get; set; }  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์
        public Nullable<decimal> rcRteLastRate { get; set; }  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์
        public string rtRteIsoCode { get; set; }  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์
        public string rtRteStaAlwChange { get; set; }  //*Arm 65-09-03 -[CR-Oversea] เพิ่มฟิลด์
        public Nullable<decimal> rcRteMaxUnit { get; set; }  //*Arm 65-09-05 -[CR-Oversea] เพิ่มฟิลด์
        public Nullable<DateTime> rdRteLastUpdOn { get; set; }  //*Arm 65-09-08 -[CR-Oversea] เพิ่มฟิลด์
    }
}