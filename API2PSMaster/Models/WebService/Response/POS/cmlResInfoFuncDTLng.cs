using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoFuncDTLng
    {
        public string rtGhdCode { get; set; }
        public string rtSysCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtGdtName { get; set; }
    }
}