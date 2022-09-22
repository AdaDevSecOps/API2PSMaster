using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.CreditCard
{
    //[Serializable]
    public class cmlResCreditCardDwn
    {
        public List<cmlResInfoCreditCard> raCreditCard { get; set; }
        public List<cmlResInfoCreditCardLng> raCreditCardLng { get; set; }
    }
}