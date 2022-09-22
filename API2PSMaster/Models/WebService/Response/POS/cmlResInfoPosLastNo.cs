using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoPosLastNo
    {
        public string rtPosCode { get; set; }
        public Nullable<Int32> rnPosDocType { get; set; }
        public string rtPosComName { get; set; }
        public Nullable<Int64> rnPosLastNo { get; set; }
        public Nullable<DateTime> rdPosLastSale { get; set; }
    }
}