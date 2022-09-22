using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResInfoCstRFIDLng
    {
        public string rtCstCode { get; set; }
        public string rtCstID { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCrfName { get; set; }
    }
}