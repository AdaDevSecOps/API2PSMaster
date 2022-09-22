using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoShopType
    {
        ///// <summary>
        ///// รหัสสาขา
        ///// </summary>
        //public string rtBchCode { get; set; }

        ///// <summary>
        ///// รหัสร้านค้า
        ///// </summary>
        //public string rtShpCode { get; set; }

        /// <summary>
        /// ประเภทตู้ 1:เย็น 2:ตู้อุ่น 3: เย็น+อุ่น
        /// </summary>
        public string rtShtType { get; set; }

        /// <summary>
        /// อุณภูมิเฉลี่ย
        /// </summary>
        public int rnShtValue { get; set; }

        /// <summary>
        /// อุณภูมิต่ำสุด
        /// </summary>
        public int rnShtMin { get; set; }

        /// <summary>
        /// อุณภูมิสูงสุด
        /// </summary>
        public int rnShtMax { get; set; }

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
        /// ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }

        /// <summary>
        /// รหัสประเภทตู้
        /// </summary>
        public string rtShtCode { get; set; }       // *Arm 63-01-16 - เพิ่ม FTShtCode
    }
}