using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysRcvFmtLng
    {
        //public string rtFmtCode { get; set; }
        //public Int64 rnLngID { get; set; }
        //public string rtFmtName { get; set; }


        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// </summary>
        public string rtFmtCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อประเภท
        /// </summary>
        public string rtFmtName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtRcvRmk { get; set; }    //*Arm 63-01-24

    }
}