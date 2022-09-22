using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.UserGrp
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlReqUsrGrpInsItem
    {
        /// <summary>
        /// รหัสผู้ใช้
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        ///  สถานะร้านค้า 0:ไม่กำหนด 1:กำหนด
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrStaShop { get; set; }

        /// <summary>
        ///  รหัสร้านค้า
        /// </summary>
        public string ptShpCode { get; set; }

        /// <summary>
        /// วันที่เริ่มดำเนินการ
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<DateTime> pdUsrStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดดำเนินการ
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<DateTime> pdUsrStop { get; set; }
    }
}