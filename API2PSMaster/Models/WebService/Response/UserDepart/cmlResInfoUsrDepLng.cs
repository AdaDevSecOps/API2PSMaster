using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserDepart
{
    public class cmlResInfoUsrDepLng
    {
        /// <summary>
        ///รหัสแผนก
        /// </summary>
        public string rtDptCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อแผนก
        /// </summary>
        public string rtDptName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtDptRmk { get; set; }
    }
}