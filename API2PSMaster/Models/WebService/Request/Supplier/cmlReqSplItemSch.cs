using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplItemSch
    {
        /// <summary>
        ///     Search type 1:equal to 2:something
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSearchType { get; set; }

        /// <summary>
        ///     Languague ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        ///     Supplier code.
        /// </summary>
        [DefaultValue("M")]
        public string ptSplCode { get; set; }

        /// <summary>
        ///     Supplier name.
        /// </summary>
        [DefaultValue("L")]
        public string ptSplName { get; set; }

        /// <summary>
        ///     Telephone number.
        /// </summary>
        [DefaultValue("M")]
        public string ptSplTel { get; set; }

        /// <summary>
        ///     Fax number.
        /// </summary>
        [DefaultValue("M")]
        public string ptSplFax { get; set; }

        /// <summary>
        ///     Email address.
        /// </summary>
        [DefaultValue("M")]
        public string ptSplEmail { get; set; }

    }
}