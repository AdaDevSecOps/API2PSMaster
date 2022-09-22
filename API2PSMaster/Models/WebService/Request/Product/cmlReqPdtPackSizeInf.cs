using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Product
{
    /// <summary>
    ///     Product package size information.
    /// </summary>
    public class cmlReqPdtPackSizeInf
    {
        /// <summary>
        ///     Product code.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtCode { get; set; }

        /// <summary>
        ///     Unit code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPunCode { get; set; }

        /// <summary>
        ///     Unit factor.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<double> pcPdtUnitFact { get; set; }

        /// <summary>
        ///     Grade.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtGrade { get; set; }

        /// <summary>
        ///     Weight.
        /// </summary>
        public Nullable<double> pcPdtWeight { get; set; }

        /// <summary>
        ///     Color code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptClrCode { get; set; }

        /// <summary>
        ///     Package size code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPszCode { get; set; }

        /// <summary>
        ///     Product unit dimensions.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtUnitDim { get; set; }

        /// <summary>
        ///     Product package dimensions.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtPkgDim { get; set; }

        /// <summary>
        ///     Status allow pick 1:Allow 2:Not allow
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtStaAlwPick { get; set; }
    }
}