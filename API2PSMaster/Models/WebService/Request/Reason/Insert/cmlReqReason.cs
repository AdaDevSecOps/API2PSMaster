using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Reason.Insert
{
    public class cmlReqReason
    {
        /// <summary>
        /// รหัสเหตุผล
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptRsnCode { get; set; }

        /// <summary>
        /// รหัสกลุ่มเหตุผล
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptRsgCode { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string pnLngID { get; set; }

        /// <summary>
        /// เหตุผล
        /// </summary>
        public string ptRsnName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptRsnRmk { get; set; }

        /// <summary>
        /// กลุ่มเหตุผล
        /// </summary>
        public cmlReqReasonGrpIns poReasonGrp { get; set; }
    }
}