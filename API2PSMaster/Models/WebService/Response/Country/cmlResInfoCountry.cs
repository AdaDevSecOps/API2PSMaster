using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2PSMaster.Models.WebService.Response.Country
{
    public class cmlResInfoCountry
    {
        /// <summary>
        ///รหัสประเทศ
        /// </summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        ///รหัสภาษี
        /// </summary>
        public string rtVatCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ตำแหน่งบนแผนที่ แนวตั้ง
        /// </summary>
        public string rtCtyLongitude { get; set; }

        /// <summary>
        ///ตำแหน่งบนแผนที่ แนวนอน
        /// </summary>
        public string rtCtyLatitude { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1 : ใช้งาน  อื่น ๆ : ไม่ใช้งาน
        /// </summary>
        public string rtCtyStaUse { get; set; }

        /// <summary>
        ///รหัส ISO Code (สกุลเงิน)
        /// </summary>
        //public string rtCurCode { get; set; }
        public string rtRteIsoCode { get; set; }  //*Arm 65-08-18 -[CR-Oversea] เพิ่มฟิลด์

        /// <summary>
        ///สถานะควบคุม Exchange rate รายวัน 1:ควบคุม อื่น ๆ :ไม่ควบคุม (Default ไม่ควบคุม)
        /// </summary>
        public string rtCtyStaCtrlRate { get; set; }

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
        ///รหัสอ้างอิง
        /// </summary>
        public string rtCtyRefID { get; set; }
    }
}
