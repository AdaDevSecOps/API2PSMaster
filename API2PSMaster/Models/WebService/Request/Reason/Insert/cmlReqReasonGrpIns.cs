using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Reason.Insert
{
    public class cmlReqReasonGrpIns
    {

        /// <summary>
        /// รหัสกลุ่มเหตุผล
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptRsgCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// กลุ่มเหตุผล
        /// </summary>
        public string ptRsgName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptRsgRmk { get; set; }

    }
}