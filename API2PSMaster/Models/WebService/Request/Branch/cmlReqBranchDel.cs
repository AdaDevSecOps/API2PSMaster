using System;
using API2PSMaster.Models.WebService.Request.Image;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using API2PSMaster.Class.Standard;

namespace API2PSMaster.Models.WebService.Request.Branch
{
    public class cmlReqBranchDel
    {
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptBchCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

    }
}