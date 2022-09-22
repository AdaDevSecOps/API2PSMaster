using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Branch
{
    public class cmlReqAddress
    {
        /// <summary>
        /// ภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// 1:Branch 2: User 3:Saleman 4:ร้านค้า 5:Agency
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptAddGrpType { get; set; }

        /// <summary>
        /// รหัสอ้างอิง Branch , User ,Saleman ,ร้านค้า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptAddRefCode { get; set; }

        /// <summary>
        /// รหัส/ลำดับ อ้างอิง
        /// </summary>
        public string ptAddRefNo { get; set; }

        /// <summary>
        /// ชื่อ
        /// </summary>
        public string ptAddName { get; set; }

        /// <summary>
        /// หมายเลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string ptAddTaxNo { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptAddRmk { get; set; }

        /// <summary>
        /// ข้อมูลประเทศ
        /// </summary>
        public string ptAddCountry { get; set; }

        /// <summary>
        /// รหัสเขต/ภูมิภาค
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptAreCode { get; set; }

        /// <summary>
        /// รหัสโซน
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptZneCode { get; set; }

        /// <summary>
        /// 1:ใช้งานแบบแยก 2:ใช้งานแบบรวม
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptVersion { get; set; }

        /// <summary>
        /// บ้านเลขที่
        /// </summary>
        public string ptV1No { get; set; }
        
        /// <summary>
        /// ซอย
        /// </summary>
        public string ptV1Soi { get; set; }

        /// <summary>
        /// หมู่บ้าน อาคาร
        /// </summary>
        public string ptV1Village { get; set; }

        /// <summary>
        /// ถนน
        /// </summary>
        public string ptV1Road { get; set; }

        /// <summary>
        /// ตำบล แขวง
        /// </summary>
        public string ptV1SubDist { get; set; }

        /// <summary>
        /// รหัสอำเภอ / เขต
        /// </summary>
        public string ptV1DstCode { get; set; }

        /// <summary>
        /// รหัสจังหวัด
        /// </summary>
        public string ptV1PvnCode { get; set; }

        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string ptV1PostCode { get; set; }

        /// <summary>
        /// ที่อยู่ 1 
        /// </summary>
        public string ptV2Desc1 { get; set; }

        /// <summary>
        /// ที่อยู่ 2 
        /// </summary>
        public string ptV2Desc2 { get; set; }

        /// <summary>
        /// Website
        /// </summary>
        public string ptAddWebsite { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวตั้ง
        /// </summary>
        public string ptAddLongtitude { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวนอน
        /// </summary>
        public string ptAddLatitude { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }



    }
}