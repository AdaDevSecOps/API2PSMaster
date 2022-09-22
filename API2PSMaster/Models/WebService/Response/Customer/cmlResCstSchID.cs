using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    public class cmlResCstSchID
    {
        /// <summary>
        /// Customer code.
        /// </summary>
        public string rtCstCode { get; set; }

        /// <summary>
        /// Customer card ID.
        /// </summary>
        public string rtCstCardID { get; set; }

        /// <summary>
        /// Customer telephone.
        /// </summary>
        public string rtCstTel { get; set; }

        /// <summary>
        /// Customer fax.
        /// </summary>
        public string rtCstFax { get; set; }

        /// <summary>
        /// Customer email.
        /// </summary>
        public string rtCstEmail { get; set; }

        /// <summary>
        /// Customer sex.<br/>
        /// 1: male, 2: femail.
        /// </summary>
        public string rtCstSex { get; set; }

        /// <summary>
        /// Customer date of birth.<br/>
        /// <b>Format. yyyy-MM-dd.</b>
        /// </summary>
        public Nullable<DateTime> rdCstDob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCgpCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtClvCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtPplCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtPmgCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtOcpCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtSpnCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstDiscWhs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstDiscRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstBusiness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstBchHQ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstBchCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstStaActive { get; set; }

        /// <summary>
        /// Customer languageg ID.
        /// </summary>
        public Nullable<long> rnLngID { get; set; }

        /// <summary>
        /// Customer name.
        /// </summary>
        public string rtCstName { get; set; }

        /// <summary>
        /// Customer name other.
        /// </summary>
        public string rtCstNameOth { get; set; }

        /// <summary>
        /// Customer remark.
        /// </summary>
        public string rtCstRmk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> rdDateUpd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtTimeUpd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtWhoUpd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> rdDateIns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtTimeIns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtWhoIns { get; set; }
    }
}