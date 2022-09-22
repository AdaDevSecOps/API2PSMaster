using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Bank
{
    //[Serializable]
    public class cmlResInfoBankLng
    {
        public string rtBnkCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtBnkName { get; set; }
        public string rtBnkRmk { get; set; }
    }
}