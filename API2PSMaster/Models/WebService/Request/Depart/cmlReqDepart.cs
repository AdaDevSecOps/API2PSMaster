using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Depart
{
    public class cmlReqDepart

    {
        /// <summary>
        /// รหัสแผนก
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptDptCode { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// 
        /// </summary>
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// ชื่อแผนก
        /// </summary>
        public string ptDptName { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptDptRmk { get; set; }
    }
}