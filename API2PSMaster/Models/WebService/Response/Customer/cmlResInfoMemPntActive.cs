using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    public class cmlResInfoMemPntActive
    {
        /// <summary>
        ///รหัสกลุ่มบริษัท
        /// </summary>
        public string rtCgpCode { get; set; }

        /// <summary>
        ///รหัสลูกค้า
        /// </summary>
        public string rtMemCode { get; set; }

        /// <summary>
        ///จำนวนแต้มล่าสุด (ที่ Active)
        /// </summary>
        public Nullable<decimal> rcTxnPntQty { get; set; }

        /// <summary>
        ///จำนวนแต้มที่จะหมดอายุภายในสิ้นปี
        /// </summary>
        public Nullable<decimal> rcTxnPnt2ExpYear { get; set; }

        /// <summary>
        ///วันที่คำนวนล่าสุด
        /// </summary>
        public Nullable<DateTime> rdTxnPntLast { get; set; }
    }
}