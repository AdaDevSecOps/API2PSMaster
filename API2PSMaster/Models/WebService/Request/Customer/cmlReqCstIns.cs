using API2PSMaster.Class.Standard;
using API2PSMaster.Models.WebService.Request.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public class cmlReqCstIns
    {
        /// <summary>
        /// Branch code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

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
        public cmlReqCstInfIns poCstInf { get; set; }

        /// <summary>
        /// Customer language.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public cmlReqCstLngIns poCstLng { get; set; }

    }
}