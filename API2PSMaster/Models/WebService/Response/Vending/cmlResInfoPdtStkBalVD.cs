using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoPdtStkBalVD
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสคลังสินค้า
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        /// ลำดับ Cabinet
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
        /// รหัสควบคุมสต๊อก
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        /// จำนวนสินค้า
        /// </summary>
        public Nullable<decimal> rcStkQty { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
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
    }
}