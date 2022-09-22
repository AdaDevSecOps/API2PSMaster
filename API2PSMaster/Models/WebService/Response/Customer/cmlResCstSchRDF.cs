using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    public class cmlResCstSchRDF
    {
        public string FTCstCode { get; set; }
        public string FTCstName { get; set; }
        public string FTCstCardID { get; set; }
        public string FTCstTel { get; set; }
        public string FTCstEmail { get; set; }
        public string FTCstSex { get; set; }
        public Nullable<DateTime> FDCstDob { get; set; }
        public string FTPplCodeRet { get; set; }
        public string FTCstDiscRet { get; set; }
        public string FTCrdCode { get; set; }
    }
}