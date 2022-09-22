using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Base
{
    //[Serializable]
    public class cmlResList<T>:cmlResBase
    {
        public List<T> raItems { get; set; }

        /// <summary>
        /// Current page.
        /// </summary>
        public int rnCurrentPage { get; set; }

        /// <summary>
        /// All pages.
        /// </summary>
        public int rnAllPage { get; set; }
    }
}