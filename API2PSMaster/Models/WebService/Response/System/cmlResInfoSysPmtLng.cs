using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysPmtLng
    {
        //public string rtSpmCode { get; set; }
        //public Int64 rnLngID { get; set; }
        //public string rtSpmName { get; set; }
        //public string rtSpmRmk { get; set; }

        /// <summary>
        ///รหัสรูปแบบโปรโมชั่น
        /// </summary>
        public string rtSpmCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น
        /// </summary>
        public string rtSpmName { get; set; }

        /// <summary>
        ///Remark
        /// </summary>
        public string rtSpmRmk { get; set; }
    }
}