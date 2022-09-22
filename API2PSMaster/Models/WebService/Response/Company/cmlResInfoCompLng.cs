using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Company
{
    //[Serializable]
    public class cmlResInfoCompLng
    {
        public string rtCmpCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtCmpName { get; set; }
        public string rtCmpShop { get; set; }
        public string rtCmpDirector { get; set; }
    }
}