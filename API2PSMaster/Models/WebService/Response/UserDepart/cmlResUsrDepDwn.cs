using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserDepart
{
    //[Serializable]
    public class cmlResUsrDepDwn
    {
        public List<cmlResInfoUsrDep> raUsrDep { get; set; }
        public List<cmlResInfoUsrDepLng> raUsrDepLng { get; set; }
    }
}