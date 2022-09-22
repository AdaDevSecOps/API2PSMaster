using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtPmtDT
    {

        //*Arm 63-03-25

        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสโปรโมชั่น XXYY-######
        /// </summary>
        public string rtPmhDocNo { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnPmdSeq { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// </summary>
        public string rtPmdStaType { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string rtPmdGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string rtPmdRefCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string rtPmdSubRef { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด ณ. บันทึก
        /// </summary>
        public string rtPmdBarCode { get; set; }

        //++++++++++++++++++



        
    }
}