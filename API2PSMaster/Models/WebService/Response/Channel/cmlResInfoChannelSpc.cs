using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Channel
{
    public class cmlResInfoChannelSpc
    {
        /// <summary>
        ///รหัสช่องทางการจำหน่าย
        /// </summary>
        public string rtChnCode { get; set; }

        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string rtAgnCode { get; set; }

        /// <summary>
        ///รหัสแอปพลิเคชั่น
        /// </summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<int> rnChnSeq { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของดำเนินการ
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///สาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสจุดขาย
        /// </summary>
        public string rtPosCode { get; set; }
    }
}