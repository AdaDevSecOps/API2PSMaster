using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.PdtCost
{
    public class cmlTCNMPdtCostFIFO
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///เลขที่เอกสารอ้างอิง
        /// <summary>
        public string rtPdtDocRef { get; set; }

        /// <summary>
        ///วันที่มีผล
        /// <summary>
        public Nullable<DateTime> rdPdtAffect { get; set; }

        /// <summary>
        ///ต้นทุน/หน่วยเล็ก ไม่รวมภาษี(ปรับตอนประมวลผลต้นทุน)
        /// <summary>
        public Nullable<decimal> rcPdtCostEx { get; set; }

        /// <summary>
        ///ต้นทุน/หน่วยเล็ก รวมภาษี(ปรับตอนประมวลผลต้นทุน)
        /// <summary>
        public Nullable<decimal> rcPdtCostIn { get; set; }

        /// <summary>
        ///ราคาต้นทุน ซื้อครั้งสุดท้าย
        /// <summary>
        public Nullable<decimal> rcPdtCostLast { get; set; }

        /// <summary>
        ///จำนวนชิ้นต่อเอกสาร
        /// <summary>
        public Nullable<decimal> rcPdtDocQtyAll { get; set; }

        /// <summary>
        ///จำนวนที่ใช้ไป
        /// <summary>
        public Nullable<decimal> rcPdtQtyAllUse { get; set; }

        /// <summary>
        ///ต้นทุนรวม ตามจำนวนคงเหลือ/เอกสาร(ปรับตอนประมวลผลสต๊อก,ต้นทุน)
        /// <summary>
        public Nullable<decimal> rcPdtCostAmt { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }
    }
}