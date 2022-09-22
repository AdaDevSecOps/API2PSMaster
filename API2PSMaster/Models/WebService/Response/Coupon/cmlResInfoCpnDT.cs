using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ตาราง TFNTCouponDT
    /// </summary>
    public class cmlResInfoCpnDT
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string rtCphDocNo { get; set; }

        /// <summary>
        ///คูปองบาร์ (หมายเลขคูปอง)
        /// </summary>
        public string rtCpdBarCpn { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnCpdSeqNo { get; set; }

        /// <summary>
        ///จำนวนอนุญาตใช้งาน 0 : ครั้งเดียว
        /// </summary>
        public Nullable<Int64> rnCpdAlwMaxUse { get; set; }

    }
}