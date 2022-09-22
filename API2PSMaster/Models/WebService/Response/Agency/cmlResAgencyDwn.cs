using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Agency
{
    public class cmlResAgencyDwn
    {
        public List<cmlResInfoAgency> raAgency { get; set; }
        public List<cmlResInfoAgencyLng> raAgencyLng { get; set; }
    }
}