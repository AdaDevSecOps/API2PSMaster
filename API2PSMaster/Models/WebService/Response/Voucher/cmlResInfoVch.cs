using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Voucher
{
    public class cmlResInfoVch
    {
        //public string rtVocCode { get; set; }
        //public string rtVocBarCode { get; set; }
        //public Nullable<DateTime> rdVocExpired { get; set; }
        //public string rtVotCode { get; set; }
        //public Nullable<decimal> rcVocValue { get; set; }
        //public Nullable<decimal> rcVocSalePri { get; set; }
        //public Nullable<decimal> rcVocBalance { get; set; }
        //public string rtVocComBook { get; set; }
        //public string rtVocStaBook { get; set; }
        //public string rtVocStaSale { get; set; }
        //public string rtVocStaUse { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }



        /// <summary>
        ///รหัสคูปอง
        /// </summary>
        public string rtVocCode { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด
        /// </summary>
        public string rtVocBarCode { get; set; }

        /// <summary>
        ///วันที่หมดอายุ
        /// </summary>
        public Nullable<DateTime> rdVocExpired { get; set; }

        /// <summary>
        ///รหัสประเภท
        /// </summary>
        public string rtVotCode { get; set; }

        /// <summary>
        ///มูลค่า
        /// </summary>
        public Nullable<decimal> rcVocValue { get; set; }

        /// <summary>
        ///ราคาขาย
        /// </summary>
        public Nullable<decimal> rcVocSalePri { get; set; }

        /// <summary>
        ///คงเหลือ
        /// </summary>
        public Nullable<decimal> rcVocBalance { get; set; }

        /// <summary>
        ///ชื่อเครื่องจอง
        /// </summary>
        public string rtVocComBook { get; set; }

        /// <summary>
        ///สถานะ 1:จอง, 2 or นอกนั้น:ยังไม่จอง
        /// </summary>
        public string rtVocStaBook { get; set; }

        /// <summary>
        ///สถานะ 1:ขายแล้ว, 2 or นอกนั้น:ยังไม่ขาย
        /// </summary>
        public string rtVocStaSale { get; set; }

        /// <summary>
        ///สถานะ 1:Active, 2 or นอกนั้น:ไม่ Active
        /// </summary>
        public string rtVocStaUse { get; set; }

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