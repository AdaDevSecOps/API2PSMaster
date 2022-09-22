using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    public class cmlResInfoMemAmtActive
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
        ///ยอดซื้อสะสมรวม
        /// </summary>
        public Nullable<decimal> rcTxnBuyTotal { get; set; }

        /// <summary>
        ///วันที่ซื้อล่าสุด
        /// </summary>
        public Nullable<DateTime> rdTxnBuyLast { get; set; }
    }
}