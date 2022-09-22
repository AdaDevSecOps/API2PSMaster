using System;
using API2PSMaster.Models.WebService.Request.Image;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using API2PSMaster.Class.Standard;

namespace API2PSMaster.Models.WebService.Request.Shop
{
    /// <summary>
    /// ร้านค้า
    /// </summary>
    public class cmlReqShop
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptShpCode { get; set; }

        /// <summary>
        /// ชื่อร้านค้า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptShpName { get; set; }

        /// <summary>
        /// รหัสคลังร้านค้า
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWahCode { get; set; }

        /// <summary>
        /// ประเภท 1:ร้านค้า 2:ฝากขาย 3: Partner เช่น MOL
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptShpType { get; set; }

        /// <summary>
        /// รหัสสาขาร้านค้าที่จดทะเบียนไว้กับสรรพากร
        /// </summary>
        public string ptShpRegNo { get; set; }

        /// <summary>
        /// รหัสอ้างอิง
        /// </summary>
        public string ptShpRefID { get; set; }

        /// <summary>
        /// วันที่เริ่มดำเนินการ
        /// </summary>
        public Nullable<DateTime> pdShpStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดดำเนินการ
        /// </summary>
        public Nullable<DateTime> pdShpStop { get; set; }

        /// <summary>
        /// วันที่เริมขาย
        /// </summary>
        public Nullable<DateTime> pdShpSaleStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดการขาย
        /// </summary>
        public Nullable<DateTime> pdShpSaleStop { get; set; }

        /// <summary>
        /// สถานะ ว่าง Null: ยังไม่เปิดใช้งาน 1: เปิดใช้งาน
        /// </summary>
        public string ptShpStaActive { get; set; }

        /// <summary>
        /// ฝากขาย 1:คำนวณแบบ ปิดบิลลด,2: คำนวณแบบ ปิดบิลเต็ม
        /// </summary>
        public string ptShpStaClose { get; set; }

        /// <summary>
        /// ผู้ปรับปรุง
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// ภาษา
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptShpRmk { get; set; }

        /// <summary>
        /// รูปภาพที่อัพโหลด
        /// </summary>
        public List<cmlReqImgList> roImgUpl { get; set; }
    }
}