using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoPos
    {
        
        /// <summary>
        /// สถานะรวมรายการสินค้าตอนสแกน 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string rtPosStaSumScan { get; set; }     //*Arm 63-05-05   

        /// <summary>
        /// สถานะรวมรายการสินค้าตอนพิมพ์ 1:อนุญาต 2: ไม่อนุญาต
        /// </summary>
        public string rtPosStaSumPrn { get; set; }    //*Arm 63-05-05     
        
        //*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง POS
        /// </summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///ประเภทของเครื่องจุดขาย 1:จุดขาย/ร้านค้า 2:จุดเติมเงิน 3:จุดตรวจสอบมูลค่า 4:ตู้ขายสินค้า (Vending)  5: ตู้ฝากสินค้า (Smart Locker) 6. Vansale
        /// </summary>
        public string rtPosType { get; set; }

        /// <summary>
        ///หมายเลขจดทะเบียน
        /// </summary>
        public string rtPosRegNo { get; set; }

        /// <summary>
        ///รหัสหัวท้ายใบเสร็จ
        /// </summary>
        public string rtSmgCode { get; set; }

        /// <summary>
        ///สถานะ เครื่อง Null or 1:ขายปลีก, 2:ขายส่ง
        /// </summary>
        public string rtPosStaRorW { get; set; }

        /// <summary>
        ///สถานะพิมพ์ EJ 1:มี, 2:ไม่มี
        /// </summary>
        public string rtPosStaPrnEJ { get; set; }

        /// <summary>
        ///สถานะ ส่งภาษี 1:ส่ง, 2:ไม่ส่ง
        /// </summary>
        public string rtPosStaVatSend { get; set; }

        /// <summary>
        ///สถานะ ทำงาน 1:ใช้งาน, 2:ไม่ใช้งาน
        /// </summary>
        public string rtPosStaUse { get; set; }

        /// <summary>
        ///สถานะ เปิดรอบ 1:manual, 2:Auto
        /// </summary>
        public string rtPosStaShift { get; set; }

        /// <summary>
        ///สถานะวันที่เปิดรอบเครื่องจุดขาย 1:เลือกวันที่ได้, 2:System Date
        /// </summary>
        public string rtPosStaDate { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string rtPrgRegToken { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }

        /// <summary>
        ///รหัสช่องทางการจำหน่าย
        /// </summary>
        public string rtChnCode { get; set; }  //*Net 63-12-25 เพิ่ม FTChnCode
    }
}