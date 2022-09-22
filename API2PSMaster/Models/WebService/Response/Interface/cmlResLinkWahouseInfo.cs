using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Interface
{
    public class cmlResLinkWahouseInfo
    {
        /// <summary>
        ///รหัสตัวแทนขาย
        /// </summary>
        public string rtAgnCode { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสคลัง
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        ///รหัสอ้างอิงคลังจาก SKC
        /// </summary>
        public string rtWahRefNo { get; set; }

        /// <summary>
        ///Channel 1:Counter , 2:Event , 3:Vansale
        /// </summary>
        public string rtWahStaChannel { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
        
        /// <summary>
        ///
        /// </summary>
        public string rtWahRemark { get; set; } //*Arm 64-01-19
    }
}