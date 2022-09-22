using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public class cmlResCstSch
    {
        /// <summary>
        /// Customer code.
        /// </summary>
        public string rtCstCode { get; set; }

        /// <summary>
        /// Customer name.
        /// </summary>
        public string rtCstName { get; set; }

        /// <summary>
        /// Customer name other.
        /// </summary>
        public string rtCstNameOth { get; set; }

        /// <summary>
        /// Customer telephone.
        /// </summary>
        public string rtCstTel { get; set; }

        /// <summary>
        /// Customer email.
        /// </summary>
        public string rtCstEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstCardID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstSex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> rdCstDob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtPplCodeRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstDiscRet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstCrdNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstTaxNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<DateTime> rdCstCrdExpire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> rtTxnPntQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> rtTxnBuyTotal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string rtCstStaAlwPosCalSo { get; set; }
    }
}