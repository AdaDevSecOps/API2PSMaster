using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Warehouse
{
    //[Serializable]
    public class cmlResWahDwn
    {
        public List<cmlResInfoWah> raWah { get; set; }
        public List<cmlResInfoWahLng> raWahLng { get; set; }
    }
}