using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoDisPolicy
    {
        /// <summary>
        ///รหัส Discount policy
        /// </summary>
        public string rtDisCode { get; set; }

        /// <summary>
        ///กลุ่มส่วยลด  1=ราย Item 2=Promotion 3=ท้ายบิล
        /// </summary>
        public string rtDisGroup { get; set; }

        /// <summary>
        ///ระดับขอ Policy ส่วนลด ถ้า Level ที่ลดแล้ว > Level Function ปัจจุบัน ต้องทำงานไม่ได้
        /// </summary>
        public Nullable<Int64> rnDisLevel { get; set; }

        /// <summary>
        ///สถานะการใช้งาน  1=ใช้งาน  2=ไม่ใช้งาน
        /// </summary>
        public string rtDisStaUse { get; set; }

        /// <summary>
        ///ชื่อ Function ใน POS
        /// </summary>
        public string rtDisPosFunc { get; set; }

        /// <summary>
        ///ยอดที่ใช้คำนวน  1= Full Price/Set Price   2=Net Peice
        /// </summary>
        public string rtDisStaPrice { get; set; }

        /// <summary>
        ///รหัสอ้างอิง
        /// </summary>
        public string rtDisCodeRef { get; set; }

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