using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    /// <summary>
    ///     Response supplier level infomation.
    /// </summary>
    [Serializable]
    public class cmlResSplLevelDwn
    {
        public List<cmlResInfoSplLev> raSplLev { get; set; }
        public List<cmlResInfoSplLevLng> raSplLevLng { get; set; }
    }
}