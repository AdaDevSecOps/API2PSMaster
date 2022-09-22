using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    //[Serializable]
    public class cmlResInfoSpl
    {
        public string rtSplCode { get; set; }
        public string rtSplTel { get; set; }
        public string rtSplFax { get; set; }
        public string rtSplEmail { get; set; }
        public string rtSplSex { get; set; }
        public Nullable<DateTime> rdSplDob { get; set; }
        public string rtSgpCode { get; set; }
        public string rtStyCode { get; set; }
        public string rtSlvCode { get; set; }
        public string rtVatCode { get; set; }
        public string rtSplStaVATInOrEx { get; set; }
        public string rtSplDiscBillRet { get; set; }
        public string rtSplDiscBillWhs { get; set; }
        public string rtSplDiscBillNet { get; set; }
        public string rtSplBusiness { get; set; }
        public string rtSplStaBchOrHQ { get; set; }
        public string rtSplBchCode { get; set; }
        public string rtSplStaActive { get; set; }
        public string rtUsrCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}