using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Channel
{
    /// <summary>
    /// ตาราง TCNMChannel
    /// </summary>
    public class cmlResInfoChannel
    {
        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสช่องทางการจำหน่าย
        /// </summary>
        public string rtChnCode { get; set; }

        /// <summary>
        ///รหัสแอปพลิเคชั่น
        /// </summary>
        public string rtAppCode { get; set; }

        ///// <summary>
        /////รหัสกลุ่มช่องทางจำหน่าย
        ///// </summary>
        //public string rtChnGroup { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<int> rnChnSeq { get; set; }

        /// <summary>
        ///สถานะการใช้งาน 1=ใช้ 2=ไม่ใช้งาน
        /// </summary>
        public string rtChnStaUse { get; set; }

        ///// <summary>
        /////สถานะ ค่า Default 1=default   2=ปกติ
        ///// </summary>
        //public string rtChnAppDef { get; set; }

        /// <summary>
        ///ค่าอ้างอิง
        /// </summary>
        public string rtChnRefCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

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

        ///// <summary>
        /////รหัส Agency
        ///// </summary>
        //public string rtAgnCode { get; set; }

        /// <summary>
        ///รหัสคลัง
        /// </summary>
        public string rtWahCode { get; set; }

    }
}