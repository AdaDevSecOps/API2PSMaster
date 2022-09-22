using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Provice
{
    /// <summary>
    /// Model Provice
    /// </summary>
    public class cmlReqProvince
    {
        /// <summary>
        /// รหัสจังหวัด
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptPvnCode { get; set; }

        /// <summary>
        /// รหัสโซน
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptZneCode { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวนอน
        /// </summary>
        public string ptPvnLatitude { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวตั้ง
        /// </summary>
        public string ptPvnLongitude { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// ภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ชื่อจังหวัด
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptPvmName { get; set; }

    }
}