using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtPmtCG
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
        public Nullable<Int64> rnPgtSeq { get; set; }

        /// <summary>
        ///ชื่อกลุ่มจัดรายการ
        /// </summary>
        public string rtPmdGrpName { get; set; }

        /// <summary>
        ///1:ตามคำนวน 2:ตามช่วง 3:ตามกลุ่ม 4:ทุกกลุ่ม
        /// </summary>
        public string rtPgtStaGetEffect { get; set; }

        /// <summary>
        ///1:ลดบาท 2:ลด% 3:ปรับราคา 4:.ใช้กลุ่มราคา 5:แถม(Free) 6:ไม่กำหนด
        /// </summary>
        public string rtPgtStaGetType { get; set; }

        /// <summary>
        ///เงื่อนไขการเลือกสินค้ากรณี>1รายการในบิล 1:ราคามากกว่า 2:ราคาน้อยกว่า 3:user เลือก
        /// </summary>
        public string rtPgtStaGetPdt { get; set; }

        /// <summary>
        ///กลุ่มที่มีสิทธิอนุมัติ  ว่าง: ได้ Auto  ไม่ว่าง: popup user login
        /// </summary>
        public string rtRolCode { get; set; }

        /// <summary>
        ///มูลค่า แปรผันตามรูปแบบส่วนลด
        /// </summary>
        public Nullable<decimal> rcPgtGetvalue { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        ///จำนวนที่จะได้รับ
        /// </summary>
        public Nullable<decimal> rcPgtGetQty { get; set; }

        /// <summary>
        ///%เฉลี่ย รวมกันต้องเท่ากับ 100
        /// </summary>
        public Nullable<decimal> rcPgtPerAvgDis { get; set; }

        /// <summary>
        ///การให้แต้ม 1:ไม่กำหนด 2:.ให้แต้ม
        /// </summary>
        public string rtPgtStaPoint { get; set; }

        /// <summary>
        ///การคำนวณตาม 1:มูลค่า 2:จำนวน
        /// </summary>
        public string rtPgtStaPntCalType { get; set; }

        /// <summary>
        ///ประเภทสินค้าร่วมรายการ 1:Product 2:Brand
        /// </summary>
        public string rtPgtStaPdtDT { get; set; }

        /// <summary>
        ///อัตราส่วนแต้มที่จะได้รับ  (ต้นทางจากBuy)
        /// </summary>
        public Nullable<Int64> rnPgtPntGet { get; set; }

        /// <summary>
        ///อัตราส่วนแต้มที่จะได้รับ
        /// </summary>
        public Nullable<Int64> rnPgtPntBuy { get; set; }

        /// <summary>
        ///การให้สิทธิ์ 1:ไม่กำหนด 2:.ให้สิทธิ์คูปอง 3:ข้อความ
        /// </summary>
        public string rtPgtStaCoupon { get; set; }

        /// <summary>
        ///ข้อความ ท้ายบิล
        /// </summary>
        public string rtPgtCpnText { get; set; }

        /// <summary>
        ///รหัสเอกสารคูปอง (Bar in DT)
        /// </summary>
        public string rtCphDocNo { get; set; }

    }
}