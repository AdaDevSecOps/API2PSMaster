using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserRole
{
    public class cmlResInfoUserRole
    {
        /// <summary>
        ///รหัสหน้าที่
        /// </summary>
        public string rtRolCode { get; set; }

        /// <summary>
        ///ระดับ (สำหรับ กำหนดสิทธ์ แบบ ระดับ)
        /// </summary>
        public Nullable<Int64> rnRolLevel { get; set; }

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