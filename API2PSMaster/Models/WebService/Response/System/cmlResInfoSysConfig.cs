using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysConfig
    {
        //public string rtSysCode { get; set; }
        //public string rtSysApp { get; set; }
        //public string rtSysKey { get; set; }
        //public string rtSysSeq { get; set; }
        //public string rtGmnCode { get; set; }
        //public string rtSysStaAlwEdit { get; set; }
        //public string rtSysStaDataType { get; set; }
        //public Nullable<int> rnSysMaxLength { get; set; }
        //public string rtSysStaDefValue { get; set; }
        //public string rtSysStaDefRef { get; set; }
        //public string rtSysStaUsrValue { get; set; }
        //public string rtSysStaUsrRef { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }





        /// <summary>
        ///รหัสระบบ/Option
        /// </summary>
        public string rtSysCode { get; set; }

        /// <summary>
        ///ชื่อโปรแกรม
        /// </summary>
        public string rtSysApp { get; set; }

        /// <summary>
        ///กำหนดกลุ่มระบบ/หน้าจอ
        /// </summary>
        public string rtSysKey { get; set; }

        /// <summary>
        ///รหัสลำดับ
        /// </summary>
        public string rtSysSeq { get; set; }

        /// <summary>
        ///กลุ่มเมนู จาก TSysMenuGrp
        /// </summary>
        public string rtGmnCode { get; set; }

        /// <summary>
        ///แก้ไข 0:ไม่อนุญาต, 1:อนุญาต
        /// </summary>
        public string rtSysStaAlwEdit { get; set; }

        /// <summary>
        ///ประเภทแก้ไข 0:Text 1:Int 2:Double 3:Date 4:Yes/No 5:Combo 6:Brows from master
        /// </summary>
        public string rtSysStaDataType { get; set; }

        /// <summary>
        ///ความกว้างและค่าสูงสุด
        /// </summary>
        public Nullable<int> rnSysMaxLength { get; set; }

        /// <summary>
        ///ค่าสำคัญ Default
        /// </summary>
        public string rtSysStaDefValue { get; set; }

        /// <summary>
        ///ส่วนอ้างอิง Default
        /// </summary>
        public string rtSysStaDefRef { get; set; }

        /// <summary>
        ///ค่าสำคัญ ผู้ใช้กำหนด
        /// </summary>
        public string rtSysStaUsrValue { get; set; }

        /// <summary>
        ///ส่วนอ้างอิง ผู้ใช้กำหนด
        /// </summary>
        public string rtSysStaUsrRef { get; set; }

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