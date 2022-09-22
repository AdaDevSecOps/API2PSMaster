using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserRole
{
    public class cmlResInfoUsrActRole
    {
        /// <summary>
        ///รหัสหน้าที่
        /// </summary>
        public string rtRolCode { get; set; }

        /// <summary>
        ///รหัสผู้ใช้
        /// </summary>
        public string rtUsrCode { get; set; }

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
        ///ผู้สร่างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
    }
}