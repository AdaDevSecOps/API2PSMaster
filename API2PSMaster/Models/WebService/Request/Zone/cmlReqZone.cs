using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Zone
{
    /// <summary>
    /// Model Zone
    /// </summary>
    public class cmlReqZone
    {
        /// <summary>
        /// รหัสลูกโซ่ (รหัสกลุ่มรวมกันตามระดับ)
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptZneChain { get; set; }

        /// <summary>
        /// รหัสโซน
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptZneCode { get; set; }

        /// <summary>
        /// ระดับความลึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnZneLevel { get; set; }

        /// <summary>
        /// รหัส Parent
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptZneParent { get; set; }

        /// <summary>
        /// รหัสเขตการซื้อ/ขาย
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptAreCode { get; set; }

        /// <summary>
        /// รหัสผู้ปรับปรุงล่าสุด
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }


        //For TCNMWaHouse_L
        /// <summary>
        /// รหัสภาษา
        /// </summary>

        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ชื่อโซน
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptZneName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptZneRmk { get; set; }

        /// <summary>
        /// ชื่อลูกโซ่
        /// </summary>
        public string ptZneChainName { get; set; }

    }
}