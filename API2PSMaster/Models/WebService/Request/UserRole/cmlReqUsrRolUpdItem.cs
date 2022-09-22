using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.UserRole
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlReqUsrRolUpdItem
    {
        /// <summary>
        /// Role Code
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptRolCode { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnRolLevel { get; set; }
    }
}