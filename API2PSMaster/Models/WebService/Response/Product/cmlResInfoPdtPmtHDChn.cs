using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    /// <summary>
    /// ตาราง TCNTPdtPmtHDChn
    /// </summary>
    public class cmlResInfoPdtPmtHDChn
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///รหัสช่องทางการจำหน่าย
        /// </summary>
        public string rtChnCode { get; set; }

        /// <summary>
        ///1:Include 2:ยกเว้น
        /// </summary>
        public string rtPmhStaType { get; set; }



    }
}