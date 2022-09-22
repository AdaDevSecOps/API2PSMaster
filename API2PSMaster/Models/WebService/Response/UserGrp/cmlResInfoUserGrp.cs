using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserGrp
{
    public class cmlResInfoUserGrp
    {
        ///// <summary>
        /////รหัสผู้ใช้
        ///// </summary>
        //public string rtUsrCode { get; set; }

        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtBchCode { get; set; }

        ///// <summary>
        /////สถานะร้านค้า 1:ทุกสาขา 2:เฉพาะสาขา 3:ร้านค้า
        ///// </summary>
        //public string rtUsrStaShop { get; set; }

        ///// <summary>
        /////รหัสร้านค้า
        ///// </summary>
        //public string rtShpCode { get; set; }

        ///// <summary>
        /////วันที่เริ่มดำเนินการ
        ///// </summary>
        //public Nullable<DateTime> rdUsrStart { get; set; }

        ///// <summary>
        /////วันที่สิ้นสุดดำเนินการ
        ///// </summary>
        //public Nullable<DateTime> rdUsrStop { get; set; }




        //*Arm 63-06-15 ปรับ Model ใหม่ตามโครงส้าง Database SKC

        /// <summary>
        ///รหัสผู้ใช้
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string rtAgnCode { get; set; }

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