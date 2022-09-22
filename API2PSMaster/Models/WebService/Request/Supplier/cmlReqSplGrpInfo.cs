using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    /// <summary>
    /// Supplier group infomation.
    /// </summary>
    public class cmlReqSplGrpInfo
    {
        /// <summary>
        /// Language ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// Supplier group Name.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSgpName { get; set; }

        /// <summary>
        /// Supplier group remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSgpRmk { get; set; }

        /// <summary>
        ///     Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}