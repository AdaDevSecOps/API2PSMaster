using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Province
{
    //[Serializable]
    public class cmlResPvnDwn
    {
        public List<cmlResInfoPvn> raPvn { get; set; }
        public List<cmlResInfoPvnLng> raPvnLng { get; set; }
    }
}