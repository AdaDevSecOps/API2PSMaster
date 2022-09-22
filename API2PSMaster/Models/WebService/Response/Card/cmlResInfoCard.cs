using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Card
{
    //[Serializable]
    public class cmlResInfoCard
    {
        public string rtCrdCode { get; set; }
        public Nullable<DateTime> rdCrdStartDate { get; set; }
        public Nullable<DateTime> rdCrdExpireDate { get; set; }
        public Nullable<DateTime> rdCrdResetDate { get; set; }
        public Nullable<DateTime> rdCrdLastTopup { get; set; }
        public string rtCtyCode { get; set; }
        public Nullable<decimal> rcCrdValue { get; set; }
        public Nullable<decimal> rcCrdDeposit { get; set; }
        public string rtCrdHolderID { get; set; }
        public string rtCrdRefID { get; set; }
        public string rtCrdStaType { get; set; }
        public string rtCrdStaLocate { get; set; }
        public string rtCrdStaActive { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }


        /// <summary>
        ///รหัสแผนก
        /// </summary>
        public string rtDptCode { get; set; }                   //*Arm 63-01-28

        /// <summary>
        ///มูลค่ายอดมัดจำสินค้า
        /// </summary>
        public Nullable<decimal> rcCrdDepositPdt { get; set; }  //*Arm 63-01-28

        /// <summary>
        ///สถานะเบิกบัตร 1 : ยังไม่ถูกเบิก, 2 : เบิกไปแล้ว
        /// </summary>
        public string rtCrdStaShift { get; set; }               //*Arm 63-01-28

        /// <summary>
        ///จำนวน Transaction ทำรายการ offline (ได้จาก wrishband)
        /// </summary>
        public Nullable<Int64> rnCrdTxnOffline { get; set; }    //*Arm 63-01-28

        /// <summary>
        ///จำนวน Transaction ที่ ประมวลผลตัดยอดแล้ว  (ได้จาก การประมวลผลตัดยอดที่ละรายการ) 
        /// </summary>
        public Nullable<Int64> rnCrdTxnPrcAdj { get; set; }     //*Arm 63-01-28

    }
}