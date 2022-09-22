using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtPriHD
    {
        //*Arm 63-06-15 Comment Code
        //public string rtBchCode { get; set; }
        //public string rtXphDocNo { get; set; }
        //public string rtXphDocType { get; set; }
        //public string rtXphStaAdj { get; set; }
        //public Nullable<DateTime> rdPghDocDate { get; set; }
        //public string rtXphDocTime { get; set; }
        //public string rtXphName { get; set; }
        //public string rtPplCode { get; set; }
        //public string rtAggCode { get; set; }
        //public Nullable<DateTime> rdXphDStart { get; set; }
        //public string rtXphTStart { get; set; }
        //public Nullable<DateTime> rdXphDStop { get; set; }
        //public string rtXphTStop { get; set; }
        //public string rtXphPriType { get; set; }
        //public string rtXphStaDoc { get; set; }
        //public string rtXphStaPrcDoc { get; set; }
        //public Nullable<int> rnXphStaDocAct { get; set; }
        //public string rtUsrCode { get; set; }
        //public string rtXphUsrApv { get; set; }
        //public string rtXphZneTo { get; set; }
        //public string rtXphBchTo { get; set; }
        //public string rtXphRmk { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }



        //*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC

        /// <summary>
        ///รหัสสาขาที่สร้างเอกสาร
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// </summary>
        public string rtXphDocNo { get; set; }

        /// <summary>
        ///ประเภทราคา 1:BasePrice(FTPghStaAdj=1), 2:Price Off
        /// </summary>
        public string rtXphDocType { get; set; }

        /// <summary>
        ///ประเภทการปรับ 1:New Price 2:ปรับลด % 3:ปรับลด มูลค่า 4: ปรับเพิ่ม % 5 ปรับเพิ่ม มูลค่า
        /// </summary>
        public string rtXphStaAdj { get; set; }

        /// <summary>
        ///วันที่เอกสาร
        /// </summary>
        public Nullable<DateTime> rdXphDocDate { get; set; }

        /// <summary>
        ///เวลาเอกสาร
        /// </summary>
        public string rtXphDocTime { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่เอกสาร ภายใน เช่นใบรับของ
        /// </summary>
        public string rtXphRefInt { get; set; }

        /// <summary>
        ///อ้างอิง วันที่เอกสาร ภายใน
        /// </summary>
        public Nullable<DateTime> rdXphRefIntDate { get; set; }

        /// <summary>
        ///ชื่อเอกสาร
        /// </summary>
        public string rtXphName { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคาสินค้า(ตามลูกค้าในกลุ่ม / สาขา / ตัวแทนขาย)
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///วันที่เริ่ม
        /// </summary>
        public Nullable<DateTime> rdXphDStart { get; set; }

        /// <summary>
        ///เวลาเริ่ม
        /// </summary>
        public string rtXphTStart { get; set; }

        /// <summary>
        ///วันที่หมดอายุ
        /// </summary>
        public Nullable<DateTime> rdXphDStop { get; set; }

        /// <summary>
        ///เวลาหมดอายุ
        /// </summary>
        public string rtXphTStop { get; set; }

        /// <summary>
        ///สถานะ 1: ขายปลีก 2: ขายส่ง 3 :ขาย online 4:สินค้าเช่า (ลง RT)
        /// </summary>
        public string rtXphPriType { get; set; }

        /// <summary>
        ///สถานะเอกสาร ว่าง:ยังไม่สมบูรณ์, 1:สมบูรณ์
        /// </summary>
        public string rtXphStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// </summary>
        public string rtXphStaApv { get; set; }

        /// <summary>
        ///สถานะ prc เอกสาร ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// </summary>
        public string rtXphStaPrcDoc { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// </summary>
        public Nullable<int> rnXphStaDocAct { get; set; }

        /// <summary>
        ///สถานะ ลบ MQ ว่าง :ยังไม่ทำ, 1:ทำแล้ว
        /// </summary>
        public string rtXphStaDelMQ { get; set; }

        /// <summary>
        ///รหัสผู้บันทึก
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///รหัสผู้อนุมัติ
        /// </summary>
        public string rtXphUsrApv { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtXphRmk { get; set; }

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