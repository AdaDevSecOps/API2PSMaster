using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoPrinterLng
    {
        public string rtPrnCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtPrnName { get; set; }
        public string rtPrnRmk { get; set; }
    }
}