using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Zone
{
    public class cmlResInfoZone
    {
        /// <summary>
        ///รหัสลูกโซ่ (รหัสกลุ่มรวมกันตามระดับ)
        /// </summary>
        public string rtZneChain { get; set; }

        /// <summary>
        ///รหัสโซน
        /// </summary>
        public string rtZneCode { get; set; }

        /// <summary>
        ///ระดับความลึก
        /// </summary>
        public Nullable<int> rnZneLevel { get; set; }

        /// <summary>
        ///รหัส Parent
        /// </summary>
        public string rtZneParent { get; set; }

        /// <summary>
        ///รหัสเขตการซื้อ/ขาย
        /// </summary>
        public string rtAreCode { get; set; }

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