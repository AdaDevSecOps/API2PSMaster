using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ตาราง TFNTCouponHDPdt
    /// </summary>
    public class cmlResInfoCpnHDPdt
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสเอกสาร
        /// </summary>
        public string rtCphDocNo { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string rtPunCode { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// </summary>
        public string rtCphStaType { get; set; }



    }
}