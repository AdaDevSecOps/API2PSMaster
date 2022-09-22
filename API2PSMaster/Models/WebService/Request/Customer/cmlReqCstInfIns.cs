using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public class cmlReqCstInfIns
    {
        private Nullable<DateTime> dC_CstDob;

        /// <summary>
        /// Customer code.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstCode { get; set; }

        /// <summary>
        /// Customer card ID.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstCardID { get; set; }

        /// <summary>
        /// Customer telephone.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstTel { get; set; }

        /// <summary>
        /// Customer fax.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstFax { get; set; }

        /// <summary>
        /// Customer email.
        /// </summary>
        [MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstEmail { get; set; }

        /// <summary>
        /// Customer sex.<br/>
        /// 1: male, 2: femail.
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptCstSex { get; set; }

        /// <summary>
        /// Customer date of birth.<br/>
        /// <b>Format. yyyy-MM-dd.</b>
        /// </summary>
        [Description("yyyy-MM-dd")]
        public Nullable<DateTime> pdCstDob
        {
            get
            {
                return (dC_CstDob == null) ? DateTime.Now : dC_CstDob;
            }
            set
            {
                dC_CstDob = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCgpCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCtyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptClvCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPplCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptPmgCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptOcpCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSpnCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptUsrCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstDiscWhs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstDiscRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("2")]
        public string ptCstBusiness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptCstBchHQ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCstBchCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [DefaultValue("1")]
        public string ptCstStaActive { get; set; }
    }
}