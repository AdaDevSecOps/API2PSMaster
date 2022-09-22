using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysDocApv
    {
        /// <summary>
        /// (AUTONUMBER)รหัส
        /// </summary>
        public Nullable<Int64> rnDapID { get; set; }

        /// <summary>
        /// ชื่อตาราง
        /// </summary>
        public string rtDapTable { get; set; }

        /// <summary>
        /// FNXshDocType
        /// </summary>
        public string rtDapRefType { get; set; }

        /// <summary>
        /// ลำดับ การอนุมัติ
        /// </summary>
        public Nullable<int> rnDapSeq { get; set; }

        /// <summary>
        /// ชื่อผู้อนุมัติ
        /// </summary>
        public string rtDapName { get; set; }

        /// <summary>
        /// ชื่อผู้อนุมัติ
        /// </summary>
        public string rtDapNameOth { get; set; }
    }
}