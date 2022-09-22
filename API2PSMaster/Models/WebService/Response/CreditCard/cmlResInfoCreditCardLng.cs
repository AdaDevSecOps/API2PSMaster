using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.CreditCard
{
    //[Serializable]
    public class cmlResInfoCreditCardLng
    {
        public string rtCrdCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCrdName { get; set; }
        public string rtCrdRmk { get; set; }
    }
}