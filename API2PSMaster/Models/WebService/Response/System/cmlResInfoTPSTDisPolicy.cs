using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoTPSTDisPolicy
    {
        /// <summary>
        ///รหัสลำดับการให้ส่วนลดแนวแกน X
        /// </summary>
        public string rtDpcDisCodeX { get; set; }

        /// <summary>
        ///รหัสลำดับการให้ส่วนลดแนวแกน Y
        /// </summary>
        public string rtDpcDisCodeY { get; set; }

        /// <summary>
        ///สถานะการอนุญาตใช้งาน 1=Yes  2=No
        /// </summary>
        public string rtDpcStaAlw { get; set; }

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