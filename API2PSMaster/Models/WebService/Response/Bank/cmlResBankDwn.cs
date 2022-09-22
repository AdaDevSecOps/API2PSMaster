using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Bank
{
    //[Serializable]
    public class cmlResBankDwn
    {
        public List<cmlResInfoBank> raBank { get; set; }
        public List<cmlResInfoBankLng> raBankLng { get; set; }
    }
}