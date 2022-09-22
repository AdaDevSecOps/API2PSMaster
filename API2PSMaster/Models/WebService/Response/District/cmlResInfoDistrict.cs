using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.District
{
    //[Serializable]
    public class cmlResInfoDistrict
    {
        public string rtDstCode { get; set; }
        public string rtDstPost { get; set; }
        public string rtPvnCode { get; set; }
        public string rtDstLatitude { get; set; }
        public string rtDstLongitude { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}