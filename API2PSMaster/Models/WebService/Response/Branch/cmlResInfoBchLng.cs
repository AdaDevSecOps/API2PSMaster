using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Branch
{
    //[Serializable]
    public class cmlResInfoBchLng
    {
        public string rtBchCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtBchName { get; set; }
        public string rtBchRmk { get; set; }
    }
}