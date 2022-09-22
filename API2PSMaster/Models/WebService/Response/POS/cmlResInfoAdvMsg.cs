using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoAdvMsg
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string rtAdvCode { get; set; }

        /// <summary>
        ///(INDEX)ประเภท 1:ข้อความต้อนรับ  2:ข้อความประชาสัมพันธ์  3:ภาพเคลื่อนไหว 4.ข้อความขอบคุณ 5:เสียงประชาสัมพันธ์ 6:รูปภาพ
        /// <summary>
        public string rtAdvType { get; set; }

        /// <summary>
        ///ลำดับการแสดง
        /// <summary>
        public Nullable<int> rnAdvSeqNo { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1 : ใช้งาน  อื่น ๆ : ไม่ใช้งาน
        /// <summary>
        public string rtAdvStaUse { get; set; }

        /// <summary>
        ///วันที่/เวลาเริ่ม
        /// <summary>
        public Nullable<DateTime> rdAdvStart { get; set; }

        /// <summary>
        ///วันที่/เวลาหมดอายุ
        /// <summary>
        public Nullable<DateTime> rdAdvStop { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}