using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    public class cmlResSplContactIns:Base.cmlResBase
    {
        /// <summary>
        /// Supplier code.
        /// </summary>
        public string rtSplCode { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        public int rnLngID { get; set; }
    }
}