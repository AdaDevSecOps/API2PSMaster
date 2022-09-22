using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    /// <summary>
    /// Customer language.
    /// </summary>
    public class cmlReqCstLngIns
    {
        /// <summary>
        /// Customer code.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstCode { get; set; }

        /// <summary>
        /// Customer languageg ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<long> pnLngID { get; set; }

        /// <summary>
        /// Customer name.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstName { get; set; }

        /// <summary>
        /// Customer name other.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstNameOth { get; set; }

        /// <summary>
        /// Customer remark.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstRmk { get; set; }
    }
}