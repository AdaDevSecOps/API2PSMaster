using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    /// <summary>
    /// Request for supplier contact.
    /// </summary>
    public class cmlReqSplContactInfo
    {
        ///// <summary>
        ///// Supplier code.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //[MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptSplCode { get; set; }

        ///// <summary>
        ///// Language ID.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //public int pnLngID { get; set; }

        ///// <summary>
        ///// Address sequence no.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //public int pnAddSeqNo { get; set; }

        /// <summary>
        /// Contact name.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCtrName { get; set; }

        /// <summary>
        /// Contact Fax.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCtrFax { get; set; }

        /// <summary>
        /// Contact Telephone.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCtrTel { get; set; }

        /// <summary>
        /// Contact Email.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCtrEmail { get; set; }

        /// <summary>
        /// Contact remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCtrRmk { get; set; }

        /// <summary>
        ///     Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}