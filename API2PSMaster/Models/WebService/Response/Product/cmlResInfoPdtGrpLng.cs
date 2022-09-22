using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResInfoPdtGrpLng
    {
        public string rtPgpChain { get; set; }
        ////public Int64 rnPgpLevel { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtPgpName { get; set; }
        public string rtPgpChainName { get; set; }
        public string rtPgpRmk { get; set; }
    }
}