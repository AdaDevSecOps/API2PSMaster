using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    /// <summary>
    ///     Response supplier type infomation.
    /// </summary>
    //[Serializable]
    public class cmlResSplTypeDwn
    {
        public List<cmlResInfoSplType> raSplType { get; set; }
        public List<cmlResInfoSplTypeLng> raSplTypeLng { get; set; }
    }
}