using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    public class cmlResSplTypeIns:cmlResBase
    {
        /// <summary>
        /// Supplier type code.
        /// </summary>
        public string rtStyCode { get; set; }
    }
}