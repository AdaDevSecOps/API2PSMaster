using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ตาราง TFNMCouponType_L
    /// </summary>
    public class cmlResInfoCpnType_L
    {
        ///<summary>
        ///รหัสประเภทคูปอง
        ///</summary>
        public string rtCptCode { get; set; }

        ///<summary>
        ///รหัสภาษา
        ///</summary>
        public Int64 rnLngID { get; set; }

        ///<summary>
        ///ชื่อประเภท
        ///</summary>
        public string rtCptName { get; set; }

        ///<summary>
        ///หมายเหตุ
        ///</summary>
        public string rtCptRemark { get; set; }





        //*Net 63-03-05 Rearrange by script
        /*
        public string rtCptCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCptName { get; set; }
        public string rtCptRemark { get; set; }*/
    }
}