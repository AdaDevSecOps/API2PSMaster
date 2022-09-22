using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.BankNote
{
    //[Serializable]
    public class cmlResInfoBankNoteLng
    {
        public string rtRteCode { get; set; }
        public string rtBntCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtBntName { get; set; }
        public string rtBntRmk { get; set; }
    }
}