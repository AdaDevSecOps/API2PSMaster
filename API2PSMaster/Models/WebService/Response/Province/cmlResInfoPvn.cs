using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Province
{
    //[Serializable]
    public class cmlResInfoPvn
    {
        public string rtPvnCode { get; set; }
        public string rtZneCode { get; set; }
        public string rtPvnLatitude { get; set; }
        public string rtPvnLongitude { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}