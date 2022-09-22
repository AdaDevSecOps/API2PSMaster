using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Branch
{
    //[Serializable]
    public class cmlResInfoBch
    {
        //public string rtBchCode { get; set; }
        ////public string rtWahCode { get; set; }

        ///// <summary>
        ///// รหัสกลุ่มราคา
        ///// </summary>
        //public string rtPplCode { get; set; }   //*Arm 63-02-20

        //public string rtBchType { get; set; }
        //public string rtBchPriority { get; set; }
        //public string rtBchRegNo { get; set; }
        //public string rtBchRefID { get; set; }
        //public Nullable<DateTime> rdBchStart { get; set; }
        //public Nullable<DateTime> rdBchStop { get; set; }
        //public Nullable<DateTime> rdBchSaleStart { get; set; }
        //public Nullable<DateTime> rdBchSaleStop { get; set; }
        //public string rtBchStaActive { get; set; }
        //public string rtBchUriSrvMQ { get; set; }
        //public string rtBchUriSrvSG { get; set; }
        //public string rtBchStaHQ { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string rtWahCode { get; set; }   //*Arm 63-03-27

        ///// <summary>
        ///// 
        ///// </summary>
        //public Nullable<int> rnBchDefLang { get; set; }    //*Arm 63-03-27



        //*Arm 63-06-23

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///ประเภทสาขา 1:สาขา 2:FC 3:DC
        /// </summary>
        public string rtBchType { get; set; }

        /// <summary>
        ///ระดับความสำคัญ  1: Blank,N/A  2:Very Low  3:Low  4:Medium  5:High  6:Very High
        /// </summary>
        public string rtBchPriority { get; set; }

        /// <summary>
        ///รหัสสาขาที่จดทะเบียนไว้กับสรรพากร
        /// </summary>
        public string rtBchRegNo { get; set; }

        /// <summary>
        ///รหัสอ้างอิง
        /// </summary>
        public string rtBchRefID { get; set; }

        /// <summary>
        ///วันที่เริ่มดำเนินการ
        /// </summary>
        public Nullable<DateTime> rdBchStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดดำเนินการ
        /// </summary>
        public Nullable<DateTime> rdBchStop { get; set; }

        /// <summary>
        ///วันที่เริมขาย
        /// </summary>
        public Nullable<DateTime> rdBchSaleStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุดการขาย
        /// </summary>
        public Nullable<DateTime> rdBchSaleStop { get; set; }

        /// <summary>
        ///ประเภทสาขาสำนักงานใหญ่ 1: สำนักงานใหญ่ , ว่าง Null :สาขา
        /// </summary>
        public string rtBchStaHQ { get; set; }

        /// <summary>
        ///สถานะ ว่าง Null: ยังไม่เปิดใช้งาน 1: เปิดใช้งาน
        /// </summary>
        public string rtBchStaActive { get; set; }

        /// <summary>
        ///รหัสคลังสินค้า
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        ///ภาษา Default ของสาขา
        /// </summary>
        public Nullable<int> rnBchDefLang { get; set; }

        /// <summary>
        ///Store URL Rabbit MQ Server
        /// </summary>
        public string rtBchUriSrvMQ { get; set; }

        /// <summary>
        ///Store URL SignalR Server
        /// </summary>
        public string rtBchUriSrvSG { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string rtAgnCode { get; set; }

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

        public string rtCtyCode { get; set; }           //*Nick 65-09-07
    }
}