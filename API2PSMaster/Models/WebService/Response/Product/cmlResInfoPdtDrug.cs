using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    public class cmlResInfoPdtDrug
    {
        /// <summary>
        /// รหัสสินค้า/รหัสควบคุมสต๊อก
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        /// อายุยา เช่น มีอายุ 2 ปี
        /// </summary>
        public Nullable<decimal> rcPdgAge { get; set; }

        /// <summary>
        /// วันที่ผลิต
        /// </summary>
        public Nullable<DateTime> rdPdgCreate { get; set; }

        /// <summary>
        /// วันที่สิ้นอายุ
        /// </summary>
        public Nullable<DateTime> rdPdgExpired { get; set; }

        /// <summary>
        /// วิธีการใช้ยา/ข้อบ่งใช้  :  ทางบางๆ บนผิวหนังบริเวณที่อักเสบ วันละ 1-3 ครั้ง หรือตามแพทย์สั่ง
        /// </summary>
        public string rtPdgHowtoUse { get; set; }

        /// <summary>
        /// ส่วนประกอบ / สารออกฤทธิ์  : Betametasone (0.1 g)   Neomycin (0.1g)
        /// </summary>
        public string rtPdgActIngredient { get; set; }

        /// <summary>
        /// สรรพคุณ : บรรเทาอาการแพ้ เช่นแพ้ฝุ่น เกสรดอกไม้ ขนสัตว์ และบรรเทาอาการหวัดคัดจมูก ช่วยให้หายใจสดวกขึ้น
        /// </summary>
        public string rtPdgProperties { get; set; }

        /// <summary>
        /// contraindication ข้อห้ามในการใช้ยา  : ไม่ควรรับประทานเกินวันละ 8 เม็ดต่อวัน หรือใช้ติดต่อกันเกิน 5 วัน
        /// </summary>
        public string rtPdgCtd { get; set; }

        /// <summary>
        /// ข้อควรระวังคำแนะนำข้อห้าม ฯลฯ ที่เกี่ยวข้องกับการใช้ยา Ex.  1. ยานี้อาจทำให้ง่วงซึม  2. ไม่ควรรับประทานร่วมกับสุรา 3. ห้ามใช้ในเด็กต่ำกว่า 1 ปี  ผู่มีอาการหอบหืด  โรคต้อหิน ต่อมลูกหมาก 4. ระวังการใช้ย
        /// </summary>
        public string rtPdgWarn { get; set; }

        /// <summary>
        /// หยุดใช้ยาเมื่อ  Ex. มีอาการแดงอักเสบ ระคายเคืองแบบรุนแรง
        /// </summary>
        public string rtPdgStopUse { get; set; }

        /// <summary>
        /// ปริมาณสูงสุดตามประเภทที่ใช้
        /// </summary>
        public Nullable<decimal> rcPdgDoseSchedule { get; set; }

        /// <summary>
        /// ปริมาณที่แนะนำ
        /// </summary>
        public Nullable<decimal> rcPdgMaxIntake { get; set; }

        /// <summary>
        /// ชื่อของรายการนี้เป็นกรรมสิทธิ์ / ชื่อแบรนด์ (เทียบกับชื่อทั่วไป)
        /// </summary>
        public string rtPdgBrandName { get; set; }

        /// <summary>
        /// ชื่อสามัญทางยา
        /// </summary>
        public string rtPdgGenericName { get; set; }

        /// <summary>
        /// ประเภทยา เช่น ยาอันตราย ยาควบคุมพิเศษ
        /// </summary>
        public string rtPdgCategory { get; set; }

        /// <summary>
        /// ชนิดยา เช่น ยาใช้ภายใน , ยาใช้ภายนอก
        /// </summary>
        public string rtPdgType { get; set; }

        /// <summary>
        /// เลขทะเบียนยา Register No. 1A 239/52
        /// </summary>
        public string rtPdgRegNo { get; set; }

        /// <summary>
        /// วิธีเก็บรักษา  : เก็บไว้ในภาชนะปิดสนิด ป้องกันแสง อุณหภูมิต่ำกว่า 30 องศา
        /// </summary>
        public string rtPdgStorage { get; set; }

        /// <summary>
        /// รหัสหน่วยสินค้า : 001/50 ML
        /// </summary>
        public string rtPunCode { get; set; }

        /// <summary>
        /// รูปแบบยา   เช่น ชนิดเม็ด  ชนิดน้ำเชื่อม  แคปซูน  ครีมสำหรับทา
        /// </summary>
        public string rtPdgForm { get; set; }

        /// <summary>
        /// เงื่อนไขควบคุมการจ่ายโดย   : ไม่กำหนด  , พยาบาล , หมอ ,เภสัช
        /// </summary>
        public string rtPdgCtrlRole { get; set; }

        /// <summary>
        /// ผลิตโดย
        /// </summary>
        public string rtPdgManufacturer { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
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
        /// ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
        
    }
}