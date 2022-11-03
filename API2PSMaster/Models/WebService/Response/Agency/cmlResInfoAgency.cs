using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Agency
{
    public class cmlResInfoAgency
    {
        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string rtAgnCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///รหัสการใช้งาน API
        /// </summary>
        public string rtAgnKeyAPI { get; set; }

        /// <summary>
        ///รหัสผ่าน
        /// </summary>
        public string rtAgnPwd { get; set; }

        /// <summary>
        ///Email
        /// </summary>
        public string rtAgnEmail { get; set; }

        /// <summary>
        ///เบอร์โทรศัพท์
        /// </summary>
        public string rtAgnTel { get; set; }

        /// <summary>
        ///เบอร์โทรสาร
        /// </summary>
        public string rtAgnFax { get; set; }

        /// <summary>
        ///เบอร์โทรศัพท์
        /// </summary>
        public string rtAgnMo { get; set; }

        /// <summary>
        ///สถานะการอนุญาติ 0:อนุญาติ, 1:ไม่อนุญาติ
        /// </summary>
        public string rtAgnStaApv { get; set; }

        /// <summary>
        ///สถานะติดต่อ 1:ติดต่อ, 2:เลิกติดต่อ
        /// </summary>
        public string rtAgnStaActive { get; set; }

        /// <summary>
        ///รหัสประเภท
        /// </summary>
        public string rtAtyCode { get; set; }

        /// <summary>
        ///รหัสกลุ่ม agency
        /// </summary>
        public string rtAggCode { get; set; }

        /// <summary>
        ///อ้างอิงรหัสตัวแทนขาย
        /// </summary>
        public string rtAgnRefCode { get; set; }

        /// <summary>
        ///รหัสช่องทางการจำหน่าย
        /// </summary>
        public string rtChnCode { get; set; }

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
        /// รหัสประเทศ
        /// </summary>
        public string rtCtyCode { get; set; }  //*Arm 65-08-16 [CR-Oversea] เพิ่มรหัสประเทศ
    }
}