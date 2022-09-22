using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Response.UserGrp;
using API2PSMaster.Models.WebService.Response.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.User
{
    //[Serializable]
    public class cmlResUserDwn
    {
        public List<cmlResInfoUser> raUser { get; set; }
        public List<cmlResInfoUserLng> raUserLng { get; set; }
        public List<cmlResInfoImgPerson> raImage { get; set; }
        public List<cmlResInfoUserGrp> raUserGrp { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
        public List<cmlResTCNMUsrLogin> raTCNMUsrLogin { get; set; }    //*Em 62-08-29
        public List<cmlResInfoUsrActRole> raTCNMUsrActRole { get; set; }
    }
}