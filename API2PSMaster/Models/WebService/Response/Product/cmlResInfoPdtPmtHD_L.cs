using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    public class cmlResInfoPdtPmtHD_L
    {
        
        //*Arm 63-03-26

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น
        /// </summary>
        public string rtPmhName { get; set; }

        /// <summary>
        ///ชื่อโปรโมชั่น(แบบย่อ)
        /// </summary>
        public string rtPmhNameSlip { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtPmhRmk { get; set; }

    }
}