using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.User
{
    public class cmlResInfoUserLng
    {
        /// <summary>
        ///รหัสผู้ใช้
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// </summary>
        public string rtUsrName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtUsrRmk { get; set; }
    }
}