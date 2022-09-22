using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Voucher
{
    public class cmlResInfoVchTypeLng
    {
        //public string rtVotCode { get; set; }
        //public Int64 rnLngID { get; set; }
        //public string rtVotName { get; set; }
        //public string rtVotRemark { get; set; }

        /// <summary>
        ///รหัสvoucher
        /// </summary>
        public string rtVotCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อประเภท
        /// </summary>
        public string rtVotName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtVotRemark { get; set; }
    }
}