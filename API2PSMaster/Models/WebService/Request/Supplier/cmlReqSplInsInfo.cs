using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request
{
    /// <summary>
    /// Supplier information for insert.
    /// </summary>
    public class cmlReqSplInsInfo
    {
        private Nullable<DateTime> dC_SplLastCta;
        private Nullable<DateTime> dC_SplLastPay;
        private Nullable<DateTime> dC_SplApply;
        private Nullable<DateTime> dC_SplCrdIssue;
        private Nullable<DateTime> dC_SplCrdExpire;

        /// <summary>
        /// Telephone number.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplTel { get; set; }

        /// <summary>
        /// Fax number.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplFax { get; set; }

        /// <summary>
        /// Email Address.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplEmail { get; set; }

        /// <summary>
        /// Sex 1:Male 2:Female.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptSplSex { get; set; } 

        /// <summary>
        /// Date of birth.
        /// </summary>
        [DefaultValue(typeof(DateTime), "2018-01-01")]
        public Nullable<DateTime> pdSplDob { get; set; }

        /// <summary>
        /// Supplier group code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSgpCode { get; set; }

        /// <summary>
        /// Supplier type code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptStyCode { get; set; }

        /// <summary>
        /// Supplier level code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSlvCode { get; set; }

        /// <summary>
        /// Vat code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptVatCode { get; set; }

        /// <summary>
        /// Shipping type code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptViaCode { get; set; }

        /// <summary>
        /// Shipping time.
        /// </summary>
        public Nullable<Decimal> pcSplLeadTime { get; set; } = 0;

        /// <summary>
        /// Vat 1:Inclusive 2:Exclusive.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptSplStaVATInOrEx { get; set; }

        /// <summary>
        /// Credit Term.
        /// </summary>
        public Nullable<Int64> pnSplCrTerm { get; set; } = 0;

        /// <summary>
        /// Credit Limit.
        /// </summary>
        public Nullable<Decimal> pcSplCrLimit { get; set; } = 0;

        /// <summary>
        /// Discount bill retail.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplDiscBillRet { get; set; }

        /// <summary>
        /// Discount bill wholesale.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplDiscBillWhs { get; set; }

        /// <summary>
        /// Discount bill sale online.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplDiscBillNet { get; set; }

        /// <summary>
        /// Contact day.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplDayCta { get; set; }

        /// <summary>
        /// Last contact.
        /// </summary>
        public Nullable<DateTime> pdSplLastCta
        {
            get{return (dC_SplLastCta == null ? DateTime.Now : dC_SplLastCta);}
            set {dC_SplLastCta = value;}
        }

        /// <summary>
        /// Last payment.
        /// </summary>
        public Nullable<DateTime> pdSplLastPay
        {
            get { return (dC_SplLastPay == null ? DateTime.Now : dC_SplLastPay); }
            set { dC_SplLastPay = value; }
        }

        /// <summary>
        /// Transport Paid 1:Source 2:Destination.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptSplTspPaid { get; set; }

        /// <summary>
        /// Record per bill.
        /// </summary>
        public int pnSplLimitRow { get; set; } = 0;

        /// <summary>
        /// Business 1:Corporate 2:Individual.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptSplBusiness { get; set; }

        /// <summary>
        /// Supplier branch is Headoffice 1:HeadOffice Other:Branch.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptSplStaBchOrHQ { get; set; }

        /// <summary>
        /// Supplier branch code.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplBchCode { get; set; }

        /// <summary>
        /// Date apply by supplier.
        /// </summary>
        public Nullable<DateTime> pdSplApply
        {
            get { return (dC_SplApply == null ? DateTime.Now : dC_SplApply); }
            set { dC_SplApply = value; }
        }

        /// <summary>
        /// Card number by supplier.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplRefExCrdNo { get; set; }

        /// <summary>
        /// Card issue.
        /// </summary>
        public Nullable<DateTime> pdSplCrdIssue
        {
            get { return (dC_SplCrdIssue == null ? DateTime.Now : dC_SplCrdIssue); }
            set { dC_SplCrdIssue = value; }
        }

        /// <summary>
        /// Card Expire.
        /// </summary>
        public Nullable<DateTime> pdSplCrdExpire
        {
            get { return (dC_SplCrdExpire == null ? DateTime.Now : dC_SplCrdExpire); }
            set { dC_SplCrdExpire = value; }
        }

        /// <summary>
        /// Status contact 1:Contact 2:Break up.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptSplStaActive { get; set; }

        /// <summary>
        /// responsible person.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
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
        public string ptSplName { get; set; }

        /// <summary>
        /// Supplier name other.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplNameOth { get; set; }

        /// <summary>
        /// Payment remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplPayRmk { get; set; }

        /// <summary>
        /// Bill remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplBillRmk { get; set; }

        /// <summary>
        /// Shipping remark.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplViaRmk { get; set; }

        /// <summary>
        /// Remark.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSplRmk { get; set; }

        /// <summary>
        /// Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}