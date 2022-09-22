using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.User
{
    /// <summary>
    /// Model User Update
    /// </summary>
    public class cmlReqUsrUpdItem
    {
        /// <summary>
        /// User Code
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrCode { get; set; }

        /// <summary>
        /// Departmente Code
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptDptCode { get; set; }

        /// <summary>
        ///  User Tel
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrTel { get; set; }

        /// <summary>
        ///  User Password
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrPwd { get; set; }

        /// <summary>
        ///  User Email
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrEmail { get; set; }

        /// <summary>
        ///  Role Code
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptRolCode { get; set; }
    }
}