using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    /// <summary>
    /// Request supplier level info.
    /// </summary>
    public class cmlReqSplLevInfo
    {
        ///// <summary>
        ///// Supplier level code.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //[MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptSlvCode { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// Supplier level Name.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSlvName { get; set; }

        /// <summary>
        /// Supplier level remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSlvRmk { get; set; }

        /// <summary>
        ///     Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}