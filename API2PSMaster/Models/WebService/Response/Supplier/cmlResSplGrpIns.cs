using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    /// <summary>
    /// Response for insert supplier group.
    /// </summary>
    public class cmlResSplGrpIns:cmlResBase
    {
        /// <summary>
        /// Supplier group code.
        /// </summary>
        public string rtSgpCode { get; set; }
    }
}