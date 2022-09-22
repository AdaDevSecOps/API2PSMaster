using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.PdtLayout
{
    public class cmlResInfoPdtLayoutLng
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        /// ชั้นที่
        /// </summary>
        public Int64 rnLayRow { get; set; }

        /// <summary>
        /// คอลัมน์
        /// </summary>
        public Int64 rnLayCol { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public Int64 rnLngID { get; set; }

        /// <summary>
        /// ชื่อ Layout
        /// </summary>
        public string rtLayName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string rtLayRemark { get; set; }
    }
}