using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCst
    {
        public string rtCstCode { get; set; }
        public string rtCstCardID { get; set; }
        public string rtCstTaxNo { get; set; }
        public string rtCstTel { get; set; }
        public string rtCstFax { get; set; }
        public string rtCstEmail { get; set; }
        public string rtCstSex { get; set; }
        public Nullable<DateTime> rdCstDob { get; set; }
        public string rtCgpCode { get; set; }
        public string rtCtyCode { get; set; }
        public string rtClvCode { get; set; }
        public string rtPplCodeRet { get; set; }
        public string rtPplCodeWhs { get; set; }
        public string rtPplCodenNet { get; set; }
        public string rtPmgCode { get; set; }
        public string rtOcpCode { get; set; }
        public string rtSpnCode { get; set; }
        public string rtUsrCode { get; set; }
        public string rtCstDiscWhs { get; set; }
        public string rtCstDiscRet { get; set; }
        public string rtCstBusiness { get; set; }
        public string rtCstBchHQ { get; set; }
        public string rtCstBchCode { get; set; }
        public string rtCstStaActive { get; set; }

        /// <summary>
        /// อนุญาตคำนวณใบสั่งขายใหม่ 1:อนุญาต , 2:ไม่อนุญาต (default)
        /// </summary>
        public string rtCstStaAlwPosCalSo { get; set; }     //*Arm 63-02-20

        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}