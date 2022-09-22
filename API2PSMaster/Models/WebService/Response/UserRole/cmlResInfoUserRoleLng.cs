using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserRole
{
    public class cmlResInfoUserRoleLng
    {
        /// <summary>
        ///รหัสแผนก
        /// </summary>
        public string rtRolCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อแผนก
        /// </summary>
        public string rtRolName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtRolRmk { get; set; }
    }
}