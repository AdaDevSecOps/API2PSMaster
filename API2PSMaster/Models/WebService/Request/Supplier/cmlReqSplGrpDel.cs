﻿using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplGrpDel
    {
        /// <summary>
        /// Supplier group code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSgpCode { get; set; }
    }
}