using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.VatRate
{
    /// <summary>
    /// Request value VatRateDelete
    /// </summary>
    public class cmlReqVatRateDelete
    {
        /// <summary>
        /// VatCode
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptVatCode { get; set; }

        /// <summary>
        /// วันที่มีผล
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<DateTime> pdVatStart { get; set; }
    }
}