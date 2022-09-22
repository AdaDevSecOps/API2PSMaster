using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Card
{
    //[Serializable]
    public class cmlResInfoCardType
    {
        public string rtCtyCode { get; set; }
        public Nullable<decimal> rcCtyDeposit { get; set; }
        public Nullable<decimal> rcCtyTopupAuto { get; set; }
        public Nullable<Int64> rnCtyExpirePeriod { get; set; }
        public Nullable<int> rnCtyExpiredType { get; set; }
        public string rtCtyStaAlwRet { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }

        /// <summary>
        ///สถานะการชำระ 1:เติมเงินก่อน 2: จ่ายทีหลัง
        /// </summary>
        public string rtCtyStaPay { get; set; }                 //*Arm 63-01-28

        /// <summary>
        ///วงเงิน
        /// </summary>
        public Nullable<decimal> rcCtyCreditLimit { get; set; } //*Arm 63-01-28
    }
}