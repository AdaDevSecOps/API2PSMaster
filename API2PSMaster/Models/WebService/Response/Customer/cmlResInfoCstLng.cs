using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstLng
    {
        public string rtCstCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCstName { get; set; }
        public string rtCstNameOth { get; set; }
        public string rtCstRmk { get; set; }
    }
}