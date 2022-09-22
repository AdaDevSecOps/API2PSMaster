using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Center
{
    //*Arm 63-01-17 - Create Parameter ใหม่
    public class cmlResInfoAddrLng
    {
        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        /// 1:Branch 2: User 3:Saleman 4:ร้านค้า 5:Agency 6:Pos,7:Merchant
        /// </summary>
        public string rtAddGrpType { get; set; }

        /// <summary>
        /// รหัสอ้างอิง Branch , User ,Saleman , ร้านค้า,เครื่องจุดขาย
        /// </summary>
        public string rtAddRefCode { get; set; }

        /// <summary>
        /// (AUTONUMBER)ลำดับ
        /// </summary>
        public Nullable<Int64> rnAddSeqNo { get; set; }

        /// <summary>
        /// รหัส/ลำดับ อ้างอิง
        /// </summary>
        public string rtAddRefNo { get; set; }

        /// <summary>
        /// ชื่อ
        /// </summary>
        public string rtAddName { get; set; }

        /// <summary>
        /// หมายเลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string rtAddTaxNo { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string rtAddRmk { get; set; }

        /// <summary>
        /// เก็บข้อมูลประเทศ
        /// </summary>
        public string rtAddCountry { get; set; }

        /// <summary>
        /// 1:ใช้งานแบบแยก 2:ใช้งานแบบรวม
        /// </summary>
        public string rtAddVersion { get; set; }

        /// <summary>
        /// บ้านเลขที่
        /// </summary>
        public string rtAddV1No { get; set; }

        /// <summary>
        /// ซอย
        /// </summary>
        public string rtAddV1Soi { get; set; }

        /// <summary>
        /// หมู่บ้าน/อาคาร
        /// </summary>
        public string rtAddV1Village { get; set; }

        /// <summary>
        /// ถนน
        /// </summary>
        public string rtAddV1Road { get; set; }

        /// <summary>
        /// ตำบล/แขวง
        /// </summary>
        public string rtAddV1SubDist { get; set; }

        /// <summary>
        /// รหัสอำเภอ/เขต
        /// </summary>
        public string rtAddV1DstCode { get; set; }

        /// <summary>
        /// รหัสจังหวัด
        /// </summary>
        public string rtAddV1PvnCode { get; set; }

        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string rtAddV1PostCode { get; set; }

        /// <summary>
        /// ทีอยู่1
        /// </summary>
        public string rtAddV2Desc1 { get; set; }

        /// <summary>
        /// ทีอยู่2
        /// </summary>
        public string rtAddV2Desc2 { get; set; }

        /// <summary>
        /// website address (Url)
        /// </summary>
        public string rtAddWebsite { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวตั้ง
        /// </summary>
        public string rtAddLongitude { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวนอน
        /// </summary>
        public string rtAddLatitude { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        /// ผู้ที่สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }

    }
}