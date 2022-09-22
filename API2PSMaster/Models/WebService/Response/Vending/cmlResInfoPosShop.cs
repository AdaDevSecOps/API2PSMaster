using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoPosShop
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัส Shop
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        /// รหัสเครื่อง Pos/ตู้
        /// </summary>
        public string rtPosCode { get; set; }

        /// <summary>
        /// S/N เครื่อง/Posตู้
        /// </summary>
        public string rtPshPosSN { get; set; }

        /// <summary>
        /// สถานะใช้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string rtPshStaUse { get; set; }

        /// <summary>
        /// รูปแบบการแสดง Layout  1=Queue SO   2. Select Pdt  3. Both
        /// </summary>
        public string rtShpSceLayout { get; set; }  //*Arm 63-01-16 -เพิ่ม FTShpSceLayout

        /// <summary>
        /// วันที่ Update ล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
    }
}