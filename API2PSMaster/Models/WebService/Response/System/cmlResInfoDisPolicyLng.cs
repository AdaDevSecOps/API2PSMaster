using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoDisPolicyLng
    {
        /// <summary>
        ///รหัส Discount policy
        /// </summary>
        public string rtDisCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///คำอธิบาย
        /// </summary>
        public string rtDisName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtDisRemark { get; set; }
    }
}