using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Online
{
    /// <summary>
    /// Information Is online ตรวจสอบ connection
    /// </summary>
    public class cmlResIsOnline : cmlResBase
    {
        /// <summary>
        /// Response result
        /// </summary>
        public string rtResult { get; set; }
    }
}