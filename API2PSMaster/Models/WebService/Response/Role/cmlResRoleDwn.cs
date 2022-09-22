using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Role
{
    //[Serializable]
    public class cmlResRoleDwn
    {
        public List<cmlResInfoFuncRpt> raFuncRpt { get; set; }
        public List<cmlResInfoUsrMenu> raUsrMenu { get; set; }
    }
}