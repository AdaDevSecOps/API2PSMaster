using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysPmt
    {
        //public string rtSpmCode { get; set; }
        //public string rtSpmType { get; set; }
        //public string rtSpmStaGrpBuy { get; set; }
        //public string rtSpmStaBuy { get; set; }
        //public string rtSpmStaGrpRcv { get; set; }
        //public string rtSpmStaRcv { get; set; }
        //public string rtSpmStaGrpBoth { get; set; }
        //public string rtSpmStaGrpReject { get; set; }
        //public string rtSpmStaAllPdt { get; set; }
        //public string rtSpmStaExceptPmt { get; set; }
        //public string rtSpmStaGetNewPri { get; set; }
        //public string rtSpmStaGetDisAmt { get; set; }
        //public string rtSpmStaGetDisPer { get; set; }
        //public string rtSpmStaGetPoint { get; set; }
        //public string rtSpmStaRcvFree { get; set; }
        //public string rtSpmStaAlwOffline { get; set; }
        //public string rtSpmStaChkLimitGet { get; set; }
        //public string rtSpmStaChkCst { get; set; }
        //public string rtSpmStaChkCstDOB { get; set; }
        //public string rtSpmStaUseRange { get; set; }
        //public Nullable<Int64> rnSpmLimitGrpRcv { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }




        /// <summary>
        ///รหัสรูปแบบโปรโมชั่น
        /// </summary>
        public string rtSpmCode { get; set; }

        /// <summary>
        ///ประเภทโปรโมชั่น 1:ตามสินค้า 2:ตามบิล
        /// </summary>
        public string rtSpmType { get; set; }

        /// <summary>
        ///สถานะใช้งาน การกำหนดสินค้า เฉพาะกลุ่มซื้อ (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGrpBuy { get; set; }

        /// <summary>
        ///1:ผลรวมมูลค่าเฉพาะกลุ่ม,2:ผลรวมมูลค่ากลุ่มซื้อทั้งหมด,3:ผลรวมจำนวนเฉพาะกลุ่ม,4:ผลรวมจำนวนกลุ่มซื้อทั้งหมด
        /// </summary>
        public string rtSpmStaBuy { get; set; }

        /// <summary>
        ///สถานะใช้งาน การกำหนดสินค้า เฉพาะกลุ่มได้รับ (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGrpRcv { get; set; }

        /// <summary>
        ///1:ได้รับเงื่อนไขเฉพาะกลุ่ม,2:ได้รับเงื่อนไขกลุ่มได้รับทั้งหมด,3:ได้รับเงื่อนไขเป็นแต้ม
        /// </summary>
        public string rtSpmStaRcv { get; set; }

        /// <summary>
        ///สถานะใช้งาน การกำหนดสินค้าที่เป็นทั้งกลุ่มซื้อและกลุ่มได้รับ (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGrpBoth { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนดสินค้ากลุ่มยกเว้น (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGrpReject { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนดสินค้าทั้งร้าน (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaAllPdt { get; set; }

        /// <summary>
        ///สถานะใช้งาน การคำนวนรวมสินค้าโปรโมชั่น (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaExceptPmt { get; set; }

        /// <summary>
        ///สถานะใช้งาน รูปแบบเงื่อนไขพิเศษแบบ ปรับราคา (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGetNewPri { get; set; }

        /// <summary>
        ///สถานะใช้งาน รูปแบบเงื่อนไขพิเศษแบบ ลดเป็นมูลค่า (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGetDisAmt { get; set; }

        /// <summary>
        ///สถานะใช้งาน รูปแบบเงื่อนไขพิเศษแบบ ลดเป็น% (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGetDisPer { get; set; }

        /// <summary>
        ///สถานะใช้งาน รูปแบบเงื่อนไขพิเศษแบบ ได้แต้ม (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaGetPoint { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด รับของแถมที่จุดขาย (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaRcvFree { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด อนุญาตใช้งานเมื่อ Offline (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaAlwOffline { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด จำนวนครั้ง ต่อวัน ต่อเดือน (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaChkLimitGet { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด เงื่อนไขเฉพาะสมาชิก (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaChkCst { get; set; }

        /// <summary>
        ///สถานะใช้งาน กำหนด เงื่อนไขเฉพาะสมาชิกที่ตรงกับวันเกิด (1:ใช้งาน,2:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaChkCstDOB { get; set; }

        /// <summary>
        ///สถานะรูปแบบ Promotion แบบขั้นบรรได  (1:ช่วงจำนวน,2:ช่วงเวลา,3:ไม่ใช้งาน)
        /// </summary>
        public string rtSpmStaUseRange { get; set; }

        /// <summary>
        ///Maximum จำนวนกลุ่มได้รับโปรโมชั่น
        /// </summary>
        public Nullable<Int64> rnSpmLimitGrpRcv { get; set; }

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
    }
}