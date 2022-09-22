using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtLng
    {
        public string rtPdtCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtPdtName { get; set; }
        public string rtPdtNameOth { get; set; }
        public string rtPdtNameABB { get; set; }
        public string rtPdtRmk { get; set; }
    }
}