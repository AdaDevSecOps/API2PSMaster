using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.VatRate
{
    //[Serializable]
    public class cmlResInfoVatRate
    {

        //public string rtVatCode { get; set; }
        //public Nullable<DateTime> rdVatStart { get; set; }
        //public Nullable<decimal> rcVatRate { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }

        /// <summary>
        ///รหัสภาษี
        /// </summary>
        public string rtVatCode { get; set; }

        /// <summary>
        ///เริมใช้งาน
        /// </summary>
        public Nullable<DateTime> rdVatStart { get; set; }

        /// <summary>
        ///อัตราภาษี เช่น 7
        /// </summary>
        public Nullable<decimal> rcVatRate { get; set; }

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