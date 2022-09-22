using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Product
{
    /// <summary>
    ///     Request for insert product.
    /// </summary>
    public class cmlReqPdtItemIns
    {
        /// <summary>
        ///     Branch code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        ///     Product information.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public cmlReqPdtInfIns poPdtInf { get; set; }

        /// <summary>
        ///     Product package size information.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public cmlReqPdtPackSizeInf poPdtPackSizeInf { get; set; }

        /// <summary>
        ///     Product barcode information.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public cmlReqPdtBarInf poPdtBar { get; set; }

        /// <summary>
        ///     Price product for retail.
        /// </summary>
        public Nullable<double> pcPgdNewPriRet { get; set; } = 0;

        /// <summary>
        ///     Price product for wholesale.
        /// </summary>
        public Nullable<double> pcPgdNewPriWhs { get; set; } = 0;

        /// <summary>
        ///     Price product for online.
        /// </summary>
        public Nullable<double> pcPgdNewPriNet { get; set; } = 0;

        /// <summary>
        ///     Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }

     }
}