using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.RedeemPoint
{
    public class cmlResInfoRedeemDT
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
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnRddSeq { get; set; }

        /// <summary>
        ///ประเภทกลุ่ม 1:กลุ่มร่วมรายการ 2:กลุ่มยกเว้น
        /// </summary>
        public string rtRddStaType { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string rtRddGrpName { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///รหัสหน่วย
        /// </summary>
        public string rtPunCode { get; set; }

        /// <summary>
        ///รหัสบาร์โค้ด ณ. บันทึก
        /// </summary>
        public string rtRddBarCode { get; set; }
    }
}