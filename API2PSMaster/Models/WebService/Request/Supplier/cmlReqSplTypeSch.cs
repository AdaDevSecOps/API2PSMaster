using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplTypeSch
    {
        /// <summary>
        ///     Search type 1:equal to 2:something
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSearchType { get; set; }

        /// <summary>
        ///     Supplier type code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptStyCode { get; set; }

        /// <summary>
        ///     Supplier type naem.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptStyName { get; set; }

        /// <summary>
        ///     Supplier type remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptStyRmk { get; set; }
    }
}