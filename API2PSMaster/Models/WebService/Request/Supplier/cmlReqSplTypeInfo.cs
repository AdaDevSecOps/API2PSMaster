using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    /// <summary>
    /// Request supplier type info.
    /// </summary>
    public class cmlReqSplTypeInfo
    {
        ///// <summary>
        ///// Supplier group code.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //[MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptStyCode { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// Supplier type Name.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptStyName { get; set; }

        /// <summary>
        /// Supplier type remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptStyRmk { get; set; }

        /// <summary>
        ///     Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}