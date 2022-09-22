using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Branch
{
    //[Serializable]
    public class cmlResBchDwn
    {
        public List<cmlResInfoBch> raBch { get; set; }
        public List<cmlResInfoBchLng> raBchLng { get; set; }
        public List<cmlResInfoAddrLng> raAddrLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
        public List<cmlResTCNTUrlObject> raTCNTUrlObject { get; set; }  //*Em 62-09-04
        public List<cmlResTCNTUrlObjectLogin> raTCNTUrlObjectLogin { get; set; }    //*Em 62-09-04
    }
}