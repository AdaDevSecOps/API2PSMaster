using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    /// <summary>
    /// Response delete supplier
    /// </summary>
    public class cmlResSplDelItem:cmlResBase
    {
        /// <summary>
        /// Supplier code.
        /// </summary>
        public string rtSplCode { get; set; }
    }
}