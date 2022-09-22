using API2PSMaster.Class.Standard;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.VatRate
{
    /// <summary>
    /// VatRate Request Model
    /// </summary>
    public class cmlReqVatRate
    {
        /// <summary>
        /// VatCode
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptVatCode { get; set; }
      
        /// <summary>
        /// VatRate
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcVatRate { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpdate { get; set; }

    }
}