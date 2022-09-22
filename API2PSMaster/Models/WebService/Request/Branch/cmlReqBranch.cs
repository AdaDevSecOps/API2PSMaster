using API2PSMaster.Class.Standard;
using API2PSMaster.Models.WebService.Request.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Branch
{
    public class cmlReqBranch
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสคลังตัด Stock
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWahCode { get; set; }

        /// <summary>
        /// ประเภทสาขา 1:สาขา 2:FC 3:DC
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchType { get; set; }

        /// <summary>
        /// ระดับความสำคัญ  1: Blank,N/A  2:Very Low  3:Low  4:Medium  5:High  6:Very High
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchPriority { get; set; }

        /// <summary>
        /// รหัสสาขาที่จดทะเบียนไว้กับสรรพากร
        /// </summary>
        public string ptBchRegNo { get; set; }

        /// <summary>
        /// รหัสอ้างอิง
        /// </summary>
        public string ptBchRefID { get; set; }

        /// <summary>
        /// วันที่เริ่มดำเนินการ
        /// </summary>
        public Nullable<DateTime> pdBchStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดดำเนินการ
        /// </summary>
        public Nullable<DateTime> pdBchStop { get; set; }

        /// <summary>
        /// วันที่เริมขาย
        /// </summary>
        public Nullable<DateTime> pdBchSaleStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดการขาย
        /// </summary>
        public Nullable<DateTime> pdBchSaleStop { get; set; }

        /// <summary>
        /// สถานะ ว่าง Null: ยังไม่เปิดใช้งาน 1: เปิดใช้งาน
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchStaActive { get; set; }

        /// <summary>
        /// Who Update
        /// </summary>
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// ภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ชื่อสาขา
        /// </summary>
        public string ptBchName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptBchRmk { get; set; }
        
        /// <summary>
        /// Upload Image
        /// </summary>
        public List<cmlReqImgList> roImgUpl { get; set; }
    }
}