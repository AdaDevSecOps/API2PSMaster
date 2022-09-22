using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplItemUpd
    {
        /// <summary>
        /// Supplier code.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplCode { get; set; }

        /// <summary>
        /// Telephone number.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplTel { get; set; }

        /// <summary>
        /// Fax number.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplFax { get; set; }

        /// <summary>
        /// Email Address.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplEmail { get; set; }

        /// <summary>
        /// Sex 1:Male 2:Female.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplSex { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        [DefaultValue("M")]
        public Nullable<DateTime> pdSplDob { get; set; }

        /// <summary>
        /// Supplier group code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSgpCode { get; set; }

        /// <summary>
        /// Supplier type code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptStyCode { get; set; }

        /// <summary>
        /// Supplier level code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSlvCode { get; set; }

        /// <summary>
        /// Vat code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptVatCode { get; set; }

        /// <summary>
        /// Shipping type code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("D")]
        public string ptViaCode { get; set; }

        /// <summary>
        /// Shipping time.
        /// </summary>
        [DefaultValue("D")]
        public Nullable<Decimal> pcSplLeadTime { get; set; } = 0;

        /// <summary>
        /// Vat 1:Inclusive 2:Exclusive.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplStaVATInOrEx { get; set; }

        /// <summary>
        /// Credit Term.
        /// </summary>
        [DefaultValue("D")]
        public Nullable<Int64> pnSplCrTerm { get; set; } = 0;

        /// <summary>
        /// Credit Limit.
        /// </summary>
        [DefaultValue("D")]
        public Nullable<Decimal> pcSplCrLimit { get; set; } = 0;

        /// <summary>
        /// Discount bill retail.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplDiscBillRet { get; set; }

        /// <summary>
        /// Discount bill wholesale.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplDiscBillWhs { get; set; }

        /// <summary>
        /// Discount bill sale online.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplDiscBillNet { get; set; }

        /// <summary>
        /// Contact day.
        /// </summary>
        [MaxLength(10, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("D")]
        public string ptSplDayCta { get; set; }

        /// <summary>
        /// Last contact.
        /// </summary>
        [DefaultValue("D")]
        public Nullable<DateTime> pdSplLastCta { get; set; }

        /// <summary>
        /// Last payment.
        /// </summary>
        [DefaultValue("D")]
        public Nullable<DateTime> pdSplLastPay { get; set; }

        /// <summary>
        /// Transport Paid 1:Source 2:Destination.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("D")]
        public string ptSplTspPaid { get; set; }

        /// <summary>
        /// Record per bill.
        /// </summary>
        [DefaultValue("D")]
        public int pnSplLimitRow { get; set; } = 0;

        /// <summary>
        /// Business 1:Corporate 2:Individual.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplBusiness { get; set; }

        /// <summary>
        /// Supplier branch is Headoffice 1:HeadOffice Other:Branch.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplStaBchOrHQ { get; set; }

        /// <summary>
        /// Supplier branch code.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplBchCode { get; set; }

        /// <summary>
        /// Date apply by supplier.
        /// </summary>
        [DefaultValue("C")]
        public Nullable<DateTime> pdSplApply { get; set; }

        /// <summary>
        /// Card number by supplier.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("C")]
        public string ptSplRefExCrdNo { get; set; }

        /// <summary>
        /// Card issue.
        /// </summary>
        [DefaultValue("C")]
        public Nullable<DateTime> pdSplCrdIssue { get; set; }

        /// <summary>
        /// Card Expire.
        /// </summary>
        [DefaultValue("C")]
        public Nullable<DateTime> pdSplCrdExpire { get; set; }

        /// <summary>
        /// Status contact 1:Contact 2:Break up.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptSplStaActive { get; set; }

        /// <summary>
        /// responsible person.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("M")]
        public string ptUsrCode { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Int64 pnLngID { get; set; }

        /// <summary>
        /// Supplier name.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptSplName { get; set; }

        /// <summary>
        /// Supplier name other.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptSplNameOth { get; set; }

        /// <summary>
        /// Payment remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptSplPayRmk { get; set; }

        /// <summary>
        /// Bill remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptSplBillRmk { get; set; }

        /// <summary>
        /// Shipping remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L,D")]
        public string ptSplViaRmk { get; set; }

        /// <summary>
        /// Remark.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("L")]
        public string ptSplRmk { get; set; }

        /// <summary>
        /// Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}