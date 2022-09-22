using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.PdtLayout
{
    public class cmlResInfoPdtLayout
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสกลุ่มร้านค้า
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        /// ลำดับตู้ Vending/Locker
        /// </summary>
        public Nullable<int> rnCabSeq { get; set; }     //*Arm 63-01-16 -เพิ่ม FNCabSeq

        /// <summary>
        /// ชั้นที่
        /// </summary>
        public Nullable<Int64> rnLayRow { get; set; }

        /// <summary>
        /// คอลัมน์
        /// </summary>
        public Nullable<Int64> rnLayCol { get; set; }

        /// <summary>
        /// สถานะสั่งหมุนเกลียว  1: หมุนเกลียวเพื่อปล่อยสินค้า  (Default)  2.ไม่ต้องหมุนเกลียว
        /// </summary>
        public string rtLayStaCtrlXY { get; set; }      //*Arm 63-01-16 -เพิ่ม FTLayStaCtrlXY

        /// <summary>
        /// รหัสสินค้า
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        /// รองรับจำนวนชิ้น
        /// </summary>
        public Nullable<decimal> rcLayColQtyMax { get; set; }

        /// <summary>
        /// ความลึก
        /// </summary>
        public Nullable<decimal> rcLayDim { get; set; }

        /// <summary>
        /// ความสูง
        /// </summary>
        public Nullable<decimal> rcLayHigh { get; set; }

        /// <summary>
        /// ความกว้าง
        /// </summary>
        public Nullable<decimal> rcLayWide { get; set; }

        /// <summary>
        /// สถานะใช้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string rtLayStaUse { get; set; }

        /// <summary>
        /// คลังสินค้า : Default คลัง ตาม Shop  แต่เปลี่ยนได้
        /// </summary>
        public string rtWahCode { get; set; }       //*Arm 63-01-16 -เพิ่ม FTWahCode

        /// <summary>
        /// วันที่ Update ล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
    }
}