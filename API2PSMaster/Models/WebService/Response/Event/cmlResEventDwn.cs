using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Event
{
    public class cmlResEventDwn
    {
        public List<cmlResInfoEventHD> raEvnHD { get; set; }
        public List<cmlResInfoEventHDLng> raEvnHDLng { get; set; }
        public List<cmlResInfoEventDT> raEvnDT { get; set; }
        public List<cmlResInfoEventDTLng> raEvnDTLng { get; set; }
    }
}