using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Product
{
    /// <summary>
    ///     Product barcode information.
    /// </summary>
    public class cmlReqPdtBarInf
    {
        /// <summary>
        ///     Product code.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtCode { get; set; }

        /// <summary>
        ///     Product barcode.
        /// </summary>
        [MaxLength(25, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBarCode { get; set; }

        /// <summary>
        ///     Unit code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPunCode { get; set; }

        /// <summary>
        ///     barcode status 1:Used
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBarStaUse { get; set; }

        /// <summary>
        ///     Status allow sale 1:Used
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBarStaAlwSale { get; set; }

        /// <summary>
        ///     Status by Generate barcode 1:Barcode from Generate
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBarStaByGen { get; set; }

        /// <summary>
        ///     Product location code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPlcCode { get; set; }

        /// <summary>
        ///     Sequence in product location.
        /// </summary>
        public Nullable<int> pnPldSeq { get; set; }
    }
}