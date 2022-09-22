using API2PSMaster.Class.Standard;
using API2PSMaster.Models.WebService.Request.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Company
{
    public class cmlReqCompany
    {
        /// <summary>
        /// รหัสบริษัท
        /// </summary>
        /// 
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string  ptCmpCode { get; set; }

        /// <summary>
        /// เบอร์โทรศัพท์
        /// </summary> 
        public string ptCmpTel { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        public string ptCmpFax { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchcode { get; set; }

        /// <summary>
        /// ภาษีขายส่ง
        /// </summary>
        /// 
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCmpWhsInOrEx { get; set; }

        /// <summary>
        /// ภาษีขายปลีก
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCmpRetInOrEx { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string ptCmpEmail { get; set; }

        /// <summary>
        /// รหัสสกุลเงิน
        /// </summary>
        public string ptRteCode { get; set; }

        /// <summary>
        /// รหัสภาษี
        /// </summary>
        public string ptVatCode { get; set; }

        /// <summary>
        /// Who Update
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
        /// Company Name
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptCmpName { get; set; }

        /// <summary>
        /// ชื่อร้านค้า
        /// </summary>
        public string ptCmpShop { get; set; }

        /// <summary>
        /// ชื่อผู้ประกอบการ/เจ้าของ
        /// </summary>
        public string ptCmpDirector { get; set; }

        /// <summary>
        /// รูปภาพ
        /// </summary>
        public List<cmlReqImgList> roImgUpl { get; set; } 

    }
}