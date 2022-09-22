using API2PSMaster.Models.WebService.Response.Branch;
using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Company
{
    //[Serializable]
    public class cmlResCompDwn
    {
        public List<cmlResInfoComp> raComp { get; set; }
        public List<cmlResInfoCompLng> raCompLng { get; set; }
        public List<cmlResInfoImgObj> raImage { get; set; }
        public List<cmlResTCNTUrlObject> raUrlObject { get; set; }  //*Arm 63-08-17
    }
}