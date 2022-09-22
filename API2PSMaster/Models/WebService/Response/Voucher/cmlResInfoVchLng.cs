using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Voucher
{
    public class cmlResInfoVchLng
    {
        //public string rtVocCode { get; set; }
        //public Int64 rnLngID { get; set; }
        //public string rtVocName { get; set; }
        //public string rtVocRemark { get; set; }

        /// <summary>
        ///รหัส Voucher
        /// </summary>
        public string rtVocCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// </summary>
        public string rtVocName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtVocRemark { get; set; }
    }
}