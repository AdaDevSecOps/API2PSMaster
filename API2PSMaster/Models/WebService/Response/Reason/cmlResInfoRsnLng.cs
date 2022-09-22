using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Reason
{
    //[Serializable]
    public class cmlResInfoRsnLng
    {
        public string rtRsnCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtRsnName { get; set; }
        public string rtRsnRmk { get; set; }
    }
}