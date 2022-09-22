using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Supplier
{
    public class cmlResSplAddrDel
    {
        /// <summary>
        /// Supplier code.
        /// </summary>
        public string rtSplCode { get; set; }

        /// <summary>
        /// Language ID.
        /// </summary>
        public int rnLngID { get; set; }

        /// <summary>
        /// Address group type 1:Supplier 2:Contact 3:Ship to.
        /// </summary>
        public string rtAddGrpType { get; set; }
    }
}