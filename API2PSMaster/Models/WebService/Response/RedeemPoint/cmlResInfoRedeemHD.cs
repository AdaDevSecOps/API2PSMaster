using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.RedeemPoint
{
    public class cmlResInfoRedeemHD
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่นแลกคะแนน XXYY-######
        /// </summary>
        public string rtRdhDocNo { get; set; }

        /// <summary>
        ///วันที่เอกสาร
        /// </summary>
        public Nullable<DateTime> rdRdhDocDate { get; set; }

        /// <summary>
        ///ประเภทเอกสาร 1: Redeem แต้ม+เงิน 2: Redeem ส่วนลด
        /// </summary>
        public string rtRdhDocType { get; set; }

        /// <summary>
        ///การคำนวน 1: ส่วนลด(Default)  2: เงินสด (ไม่ re-cal Vat)
        /// </summary>
        public string rtRdhCalType { get; set; }

        /// <summary>
        ///วันที่เริ่ม
        /// </summary>
        public Nullable<DateTime> rdRdhDStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด
        /// </summary>
        public Nullable<DateTime> rdRdhDStop { get; set; }

        /// <summary>
        ///เวลาเริ่ม
        /// </summary>
        public Nullable<DateTime> rdRdhTStart { get; set; }

        /// <summary>
        ///เวลาสิ้นสุด
        /// </summary>
        public Nullable<DateTime> rdRdhTStop { get; set; }

        /// <summary>
        ///หยุดรายการ 0: เปิดใช้  1: หยุด
        /// </summary>
        public string rtRdhStaClosed { get; set; }

        /// <summary>
        ///สถานะเอกสาร ว่าง:ยังไม่สมบูรณ์, 1:สมบูรณ์
        /// </summary>
        public string rtRdhStaDoc { get; set; }

        /// <summary>
        ///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
        /// </summary>
        public string rtRdhStaApv { get; set; }

        /// <summary>
        ///สถานะ prc เอกสาร ว่าง:ยังไม่ทำ, 1:ทำแล้ว
        /// </summary>
        public string rtRdhStaPrcDoc { get; set; }

        /// <summary>
        ///สถานะ เคลื่อนไหว 0:NonActive, 1:Active
        /// </summary>
        public Nullable<int> rnRdhStaDocAct { get; set; }

        /// <summary>
        ///รหัสผู้บันทึก
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///รหัสผู้อนุมัติ
        /// </summary>
        public string rtRdhUsrApv { get; set; }

        /// <summary>
        ///คำนวนรวมสินค้าโปรโมชั่น 1:อนญาตคำนวน 2:ไม่อนญาตคำนวน
        /// </summary>
        public string rtRdhStaOnTopPmt { get; set; }

        /// <summary>
        ///จำนวนครั้งที่อนุญาต/บิล 0: ไม่จำกัด
        /// </summary>
        public Nullable<Int64> rnRdhLimitQty { get; set; }

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