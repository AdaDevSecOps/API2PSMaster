using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplAddrDel
    {
        /// <summary>
        /// Supplier code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplCode { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// Address group type 1:Supplier 2:Contact 3:Ship to.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddGrpType { get; set; }
    }
}