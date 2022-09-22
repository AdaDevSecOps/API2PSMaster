using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public class cmlReqCstUpd
    {
        /// <summary>
        /// Customer code.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptCstCode { get; set; }

        /// <summary>
        /// Customer languageg ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<long> pnLngID { get; set; }

        /// <summary>
        /// User code save data.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// Customer detail.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public cmlReqCstInfUpd poCstInf { get; set; }

        /// <summary>
        /// Customer language.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public cmlReqCstLngUpd poCstLng { get; set; }
    }
}