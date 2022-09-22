using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.UserDep
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlReqUsrDepDelItem
    {
        /// <summary>
        /// Department Code
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptDptCode { get; set; }
    }
}