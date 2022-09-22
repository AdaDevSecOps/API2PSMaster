using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2PSMaster.Models.WebService.Response.Country
{
    public class cmlResCountryDwn
    {
        public List<cmlResInfoCountry> raCountry { get; set; }
        public List<cmlResInfoCountryLng> raCountryLng { get; set; }
    }
}
