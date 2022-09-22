using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rcv
{
    public class cmlResInfoRcvSpcConfig
    {
        /// <summary>
        ///รหัสประเภทการชำระเงิน 
        /// </summary>
        public string rtRcvCode { get; set; }

        /// <summary>
        ///ลำดับรายการของการกำหนดพิเศษ
        /// </summary>
        public Nullable<Int64> rnRcvSeq { get; set; }

        /// <summary>
        ///รหัสลำดับ
        /// </summary>
        public Nullable<int> rnSysSeq { get; set; }

        /// <summary>
        ///ตัวแปร
        /// </summary>
        public string rtSysKey { get; set; }
        
        /// <summary>
        ///ค่า ตัวแปร
        /// </summary>
        public string rtSysStaUsrValue { get; set; }

        /// <summary>
        ///ค่า อ้างอิง
        /// </summary>
        public string rtSysStaUsrRef { get; set; }
    }
}