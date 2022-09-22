using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Role
{
    public class cmlResInfoDocApvRole
    {
        /// <summary>
        /// (AUTONUMBER)รหัส
        /// </summary>
        public Nullable<Int64> rnDarID { get; set; }

        /// <summary>
        /// ชื่อตาราง
        /// </summary>
        public string rtDarTable { get; set; }

        /// <summary>
        /// FNXshDocType
        /// </summary>
        public string rtDarRefType { get; set; }

        /// <summary>
        /// ลำดับ การอนุมัติ
        /// </summary>
        public Nullable<int> rnDarApvSeq { get; set; }

        /// <summary>
        /// Role ที่ใช้สำหรับอนุมัติ อ้างอิง Table TCNMUser
        /// </summary>
        public string rtDarUsrRole { get; set; }

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