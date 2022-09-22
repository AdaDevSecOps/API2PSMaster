using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.BankNote
{
    //[Serializable]
    public class cmlResBankNoteDwn
    {
        public List<cmlResInfoBankNote> raBankNote { get; set; }
        public List<cmlResInfoBankNoteLng> raBankNoteLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}