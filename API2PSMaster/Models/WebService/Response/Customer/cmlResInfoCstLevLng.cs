using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstLevLng
    {
        public string rtClvCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtClvName { get; set; }
        public string rtClvRmk { get; set; }
    }
}