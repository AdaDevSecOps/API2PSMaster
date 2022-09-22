using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rcv
{
    public class cmlResInfoRcvLng
    {
        /// <summary>
        ///รหัสรูปแบบการรับชำระเงิน
        /// </summary>
        public string rtRcvCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อประเภทรับชำระ
        /// </summary>
        public string rtRcvName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtRcvRmk { get; set; }
    }
}