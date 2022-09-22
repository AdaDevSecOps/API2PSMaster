using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.SubDistrict
{
    //[Serializable]
    public class cmlResInfoSubDist
    {
        public string rtSudCode { get; set; }
        public string rtDstCode { get; set; }
        public string rtSudLatitude { get; set; }
        public string rtSudLongitude { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}