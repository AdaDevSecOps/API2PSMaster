using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResAdvMsgDwn
    {
        public List<cmlResInfoAdvMsg> raAdvMsg { get; set; }
        public List<cmlResInfoAdvMsgLng> raAdvMsgLng { get; set; }
        public List<resTCNMMediaObj> raTCNMMediaObj { get; set; }       //*Em 62-08-16
        public List<cmlResInfoImgObj> raTCNMImgObj { get; set; }    //*Em 62-09-11
    }
}