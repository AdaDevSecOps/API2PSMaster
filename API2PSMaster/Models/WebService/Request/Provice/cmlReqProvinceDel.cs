using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Provice
{
    public class cmlReqProvinceDel
    {
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptPvnCode { get; set; }

        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }
    }
}