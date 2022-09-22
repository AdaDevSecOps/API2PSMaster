using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ตาราง TFNTCouponHD_L
    /// </summary>
    public class cmlResInfoCpnHD_L
    {

        ///<summary>
        ///รหัสสาขา
        ///</summary>
        public string rtBchCode { get; set; }
        ///<summary>
        ///รหัสเอกสารคูปอง
        ///</summary>
        public string rtCphDocNo { get; set; }

        ///<summary>
        ///รหัสภาษา
        ///</summary>
        public Int64 rnLngID { get; set; }

        ///<summary>
        ///ชื่อคูปอง
        ///</summary>
        public string rtCpnName { get; set; }

        ///<summary>
        ///ข้อความแสดงบนคูปองบันทัดที่ 1
        ///</summary>
        public string rtCpnMsg1 { get; set; }

        ///<summary>
        ///ข้อความแสดงบนคูปองบันทัดที่ 2
        ///</summary>
        public string rtCpnMsg2 { get; set; }

        ///<summary>
        ///เงื่อนไขแสดงบนคูปอง
        ///</summary>
        public string rtCpnCond { get; set; }


    }
}