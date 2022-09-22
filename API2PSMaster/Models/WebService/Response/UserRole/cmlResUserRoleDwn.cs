using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserRole
{
    //[Serializable]
    public class cmlResUserRoleDwn
    {
        //public List<cmlResInfoUserRole> raUserRole { get; set; }
        //public List<cmlResInfoUserRoleLng> raUserRoleLng { get; set; }
        public List<cmlResInfoUsrActRole> raUsrActRole { get; set; } //*Arm 63-06-15
    }
}