using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoEdcLng
    {
        public string rtEdcCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtEdcName { get; set; }
        public string rtEdcRmk { get; set; }
    }
}