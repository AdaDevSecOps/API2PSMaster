using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResSlipMsgDwn
    {
        public List<cmlResInfoSlipMsgHDLng> raSlipMsgHDLng { get; set; }
        public List<cmlResInfoSlipMsgDTLng> raSlipMsgDTLng { get; set; }
    }
}