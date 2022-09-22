﻿using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplTypeIns:cmlReqSplTypeInfo
    {
        /// <summary>
        /// Branch code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// Supplier group code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptStyCode { get; set; }
    }
}