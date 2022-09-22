using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Product
{
    /// <summary>
    ///     Product information.
    /// </summary>
    public class cmlReqPdtInfIns
    {
        /// <summary>
        ///     Product code.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtCode { get; set; }

        /// <summary>
        ///     Stock factor.
        /// </summary>
        [DefaultValue(1)]
        public Nullable<double> pcPdtStkFac { get; set; }

        /// <summary>
        ///     Control stock 1:Yes 2:No
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtStkControl { get; set; }

        /// <summary>
        ///     Special control 1:Control 2:No Control
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("2")]
        public string ptPdtGrpControl { get; set; } 

        /// <summary>
        ///     For system 1:POS 2:Restaurant 3:TICKET 4:RENTAL
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtForSystem { get; set; }

        /// <summary>
        ///     Quantity for purchase.
        /// </summary>
        public Nullable<double> pcPdtQtyOrdBuy { get; set; }

        /// <summary>
        ///     Cost Average.
        /// </summary>
        public Nullable<double> pcPdtCostAvg { get; set; }

        /// <summary>
        ///     Cost FIFO.
        /// </summary>
        public Nullable<double> pcPdtCostFiFo { get; set; }

        /// <summary>
        ///     Cost Last.
        /// </summary>
        public Nullable<double> pcPdtCostLast { get; set; }

        /// <summary>
        ///     Cost Defind.
        /// </summary>
        public Nullable<double> pcPdtCostDef { get; set; }

        /// <summary>
        ///     Cost other.
        /// </summary>
        public Nullable<double> pcPdtCostOth { get; set; }

        /// <summary>
        ///     Cost summary.
        /// </summary>
        public Nullable<double> pcPdtCostAmt { get; set; }

        /// <summary>
        ///     Cost standard.
        /// </summary>
        public Nullable<double> pcPdtCostStd { get; set; }

        /// <summary>
        ///     Minimum quantity.
        /// </summary>
        public Nullable<double> pcPdtMin { get; set; }

        /// <summary>
        ///     Maximum quantity.
        /// </summary>
        public Nullable<double> pcPdtMax { get; set; }

        /// <summary>
        ///     Point give 1:Point give ,Not to point.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtPoint { get; set; }

        /// <summary>
        ///     Point for redeem.
        /// </summary>
        public Nullable<double> pcPdtPointTime { get; set; } = 0;

        /// <summary>
        ///     Product type 1:General 2:Service 3:Other 4:Free 5:Special
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtType { get; set; }

        /// <summary>
        ///     Price used 1:Sale Price 2:Open PLU 3:Scales 4:Weight 
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtSaleType { get; set; }

        /// <summary>
        ///     1:General Product 2:Product set 3:Product serial 4:Product serial set
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtSetOrSN { get; set; }

        /// <summary>
        ///     Set price used 1:Set price 2:Item price
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtStaSetPri { get; set; }

        /// <summary>
        ///     Show list item 1:Show 2:Not show
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtStaSetShwDT { get; set; }

        /// <summary>
        ///     Allow discount/Change 1:Allow 2:Not Allow
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtStaAlwDis { get; set; }

        /// <summary>
        ///     Allow return 1:Allow 2:Not Allow
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtStaAlwReturn { get; set; }

        /// <summary>
        ///     Vat status 1:Vat 2:Non vat
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtStaVat { get; set; }

        /// <summary>
        ///     Active status 1:Active 2:Not active
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptPdtStaActive { get; set; }

        /// <summary>
        ///     allow recalculate option 1:Repack 2:Step price 3:Not Allow
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtStaAlwReCalOpt { get; set; }

        /// <summary>
        ///     product type 1:Buy absent 2:Consignment
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtStaCsm { get; set; }

        /// <summary>
        ///     Shop code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptShpCode { get; set; }

        /// <summary>
        ///     Referance product code from shop.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPdtRefShop { get; set; }

        /// <summary>
        ///     Touch group code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptTcgCode { get; set; }

        /// <summary>
        ///     Product group.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPgpChain { get; set; }

        /// <summary>
        ///     Type code of prodouct.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPtyCode { get; set; }

        /// <summary>
        ///     Product brand code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPbnCode { get; set; }

        /// <summary>
        ///     Product model code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPmoCode { get; set; }

        /// <summary>
        ///     Vat code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptVatCode { get; set; }

        /// <summary>
        ///     Event code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptEvnCode { get; set; }

        /// <summary>
        ///     Sale date start.
        /// </summary>
        public Nullable<DateTime> pdPdtSaleStart { get; set; }

        /// <summary>
        ///     Sale date stop.
        /// </summary>
        public Nullable<DateTime> FDPdtSaleStop { get; set; }
    }
}