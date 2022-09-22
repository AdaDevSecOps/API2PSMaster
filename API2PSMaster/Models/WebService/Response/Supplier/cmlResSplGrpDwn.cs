using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    /// <summary>
    ///     Response supplier group infomation.
    /// </summary>
    [Serializable]
    public class cmlResSplGrpDwn
    {
       public List<cmlResInfoSplGrp> raSplGrp { get; set; }
       public List<cmlResInfoSplGrpLng> raSplGrpLng { get; set; }
    }
}