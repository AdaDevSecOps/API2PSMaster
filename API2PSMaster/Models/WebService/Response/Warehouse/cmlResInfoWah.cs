using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Warehouse
{
    //[Serializable]
    public class cmlResInfoWah
    {

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; } //*Arm 63-01-23

        /// <summary>
        ///รหัสคลังสินค้า
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        ///ประเภทคลัง 1:มาตรฐาน 2:คลังทั่วไป 3 :คลังสาขา(ยกเลิก) 4 :คลังฝากขาย/ร้านค้า 5:คลังหน่วยรถ 6:Vending
        /// </summary>
        public string rtWahStaType { get; set; }

        /// <summary>
        ///ประเภทคลัง 4:Shop 5:Sale Man 6:Pos
        /// </summary>
        public string rtWahRefCode { get; set; }

        /// <summary>
        ///ใช้ตรวจสอบในขั้นตอนการขาย 1: ไม่เช็ค (ขายติดลบได้)  2: เช็ค Local  (Vending)  3: Check Online   API (Ada,3Party)
        /// </summary>
        public string rtWahStaChkStk { get; set; }      //*Arm 63-06-23

        /// <summary>
        ///ใช้ตรวจสอบในขั้นตอนประมวลผลตอนจบบิล 1: ไม่ตัดสต๊อก  2.ดัดสต๊อก
        /// </summary>
        public string rtWahStaPrcStk { get; set; }      //*Arm 63-06-23

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
        
    }
}