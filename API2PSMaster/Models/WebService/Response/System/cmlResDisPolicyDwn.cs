using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResDisPolicyDwn
    {
        public List<cmlResInfoDisPolicy> raDisPolicy { get; set; }
        public List<cmlResInfoDisPolicyLng> raDisPolicyLng { get; set; }
        public List<cmlResInfoTPSTDisPolicy> raTPSTDiscPolicy { get; set; }
    }
}