using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.SalePerson
{
    public class cmlSalePersonAdr
    {
        /// <summary>
        /// ภาษา
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// 1:Branch 2: User 3:Saleman 4:ร้านค้า 5:Agency
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptGrptype { get; set; }

        /// <summary>
        /// รหัสอ้างอิง Branch , User ,Saleman ,ร้านค้า
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptRefCode { get; set; }

        /// <summary>
        /// รหัส/ลำดับ อ้างอิง
        /// </summary>
        public string ptRefNo { get; set; }

        /// <summary>
        /// ชื่อ
        /// </summary>
        public string ptAddName { get; set; }

        /// <summary>
        /// หมายเลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string ptTaxNo { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptAddRmk { get; set; }

        /// <summary>
        /// เก็บข้อมูลประเทศ
        /// </summary>
        public string ptAddCountry { get; set; }

        /// <summary>
        /// รหัสเขต/ภูมิภาค
        /// </summary>
        public string ptAreCode { get; set; }

        /// <summary>
        /// รหัสโซน
        /// </summary>
        public string ptZneCode { get; set; }

        /// <summary>
        /// 1:ใช้งานแบบแยก 2:ใช้งานแบบรวม
        /// </summary>
        public string ptAddVersion { get; set; }

        /// <summary>
        /// บ้านเลขที่
        /// </summary>
        public string ptAddV1No { get; set; }

        /// <summary>
        /// ซอย
        /// </summary>
        public string ptAddV1Soi { get; set; }

        /// <summary>
        /// หมู่บ้าน/อาคาร
        /// </summary>
        public string ptV1Village { get; set; }

        /// <summary>
        /// ถนน
        /// </summary>
        public string ptV1Road { get; set; }

        /// <summary>
        /// ตำบล/แขวง
        /// </summary>
        public string ptV1SubDist { get; set; }

        /// <summary>
        /// รหัสอำเภอ/เขต
        /// </summary>
        public string ptAddV1DstC { get; set; }

        /// <summary>
        /// รหัสจังหวัด
        /// </summary>
        public string ptV1PvnC { get; set; }

        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string ptV1PostC { get; set; }

        /// <summary>
        /// ทีอยู่1
        /// </summary>
        public string ptAddV2Desc1 { get; set; }

        /// <summary>
        /// ทีอยู่2
        /// </summary>
        public string ptAddV2Desc2 { get; set; }

        /// <summary>
        /// website address (Url)
        /// </summary>
        public string ptAddWebSite { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวตั้ง
        /// </summary>
        public string ptAddLongtitude { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวนอน
        /// </summary>
        public string ptAddLatitude { get; set; }

        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }
    };
}