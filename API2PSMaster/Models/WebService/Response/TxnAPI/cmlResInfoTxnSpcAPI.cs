using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.TxnAPI
{
    public class cmlResInfoTxnSpcAPI
    {
        /// <summary>
        ///รหัสเส้น API ที่ Interface
        /// </summary>
        public string rtApiCode { get; set; }

        /// <summary>
        ///รหัสบริษัท
        /// </summary>
        public string rtCmpCode { get; set; }

        /// <summary>
        ///รหัสตัวแทนขาย
        /// </summary>
        public string rtAgnCode { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มธุรกิจ
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่องจุดขาย
        /// </summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///รหัสรูปแบบของ API
        /// </summary>
        public string rtApiFmtCode { get; set; }

        /// <summary>
        ///เก็บ URL ของเส้น Interface กรณีที่ Interface นั้นต้องใช้ Webservice
        /// </summary>
        public string rtApiURL { get; set; }

        /// <summary>
        ///รหัส User สำหรับ API authen
        /// </summary>
        public string rtSpaUsrCode { get; set; }

        /// <summary>
        ///รหัส Password
        /// </summary>
        public string rtSpaUsrPwd { get; set; }

        /// <summary>
        ///รหัส Token key
        /// </summary>
        public string rtSpaApiKey { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtSpaRmk { get; set; }

        /// <summary>
        ///วันที่สร้าง
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้าง
        /// </summary>
        public string rtCreateBy { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }
    }
}