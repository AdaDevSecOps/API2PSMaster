using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoAdvMsgLng
    {
        public string rtAdvCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtAdvName { get; set; }
        public string rtAdvMsg { get; set; }
        public string rtAdvRmk { get; set; }
    }
}