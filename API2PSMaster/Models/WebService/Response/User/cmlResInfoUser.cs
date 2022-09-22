using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.User
{
    //[Serializable]
    public class cmlResInfoUser
    {
        ///// <summary>
        /////รหัสผู้ใช้
        ///// </summary>
        //public string rtUsrCode { get; set; }

        ///// <summary>
        /////รหัสแผนก
        ///// </summary>
        //public string rtDptCode { get; set; }

        ///// <summary>
        /////รหัสหน้าที่
        ///// </summary>
        //public string rtRolCode { get; set; }

        ///// <summary>
        /////เบอร์โทร
        ///// </summary>
        //public string rtUsrTel { get; set; }

        ///// <summary>
        /////รหัสผ่าน
        ///// </summary>
        //public string rtUsrPwd { get; set; }

        ///// <summary>
        /////Email
        ///// </summary>
        //public string rtUsrEmail { get; set; }

        ///// <summary>
        /////ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID
        ///// </summary>
        //public string rtUsrLoginType { get; set; }  //*Arm 63-01-24


        ///// <summary>
        /////วันที่เริ่มต้น
        ///// </summary>
        //public Nullable<DateTime> rdUsrStart { get; set; }  //*Arm 63-01-24

        ///// <summary>
        /////วันที่สิ้นสุด
        ///// </summary>
        //public Nullable<DateTime> rdUsrFinish { get; set; } //*Arm 63-01-24

        ///// <summary>
        /////วันที่ปรับปรุงรายการล่าสุด
        ///// </summary>
        //public Nullable<DateTime> rdLastUpdOn { get; set; }

        ///// <summary>
        /////ผู้ปรับปรุงรายการล่าสุด
        ///// </summary>
        //public string rtLastUpdBy { get; set; }

        ///// <summary>
        /////วันที่สร้างรายการ
        ///// </summary>
        //public Nullable<DateTime> rdCreateOn { get; set; }

        ///// <summary>
        /////ผู้สร้างรายการ
        ///// </summary>
        //public string rtCreateBy { get; set; }



        //*Arm 63-06-15 ปรับ Model ใหม่ตามโครงส้าง Database SKC

        /// <summary>
        ///รหัสผู้ใช้
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///รหัสแผนก
        /// </summary>
        public string rtDptCode { get; set; }

        /// <summary>
        ///Email
        /// </summary>
        public string rtUsrEmail { get; set; }

        /// <summary>
        ///เบอร์โทร
        /// </summary>
        public string rtUsrTel { get; set; }

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

    }
}