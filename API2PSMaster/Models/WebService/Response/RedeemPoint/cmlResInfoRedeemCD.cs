using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.RedeemPoint
{
    public class cmlResInfoRedeemCD
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่นแลกคะแนน XXYY-######
        /// </summary>
        public string rtRdhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnRdcSeq { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string rtRddGrpName { get; set; }

        /// <summary>
        ///Redeem code
        /// </summary>
        public string rtRdcRefCode { get; set; }

        /// <summary>
        ///(1)ใช้แต้ม (2)ใช้แต้ม
        /// </summary>
        public Nullable<decimal> rcRdcUsePoint { get; set; }

        /// <summary>
        ///(1)ร่วมกับเงิน (2) ได้ส่วนลด
        /// </summary>
        public Nullable<decimal> rcRdcUseMny { get; set; }

        /// <summary>
        ///ยอดขั้นต่ำ/บิลที่อนุญาต / 0 ไม่มีขั้นต่ำ
        /// </summary>
        public Nullable<decimal> rcRdcMinTotBill { get; set; }
    }
}