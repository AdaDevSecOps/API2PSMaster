using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.User
{

    /// <summary>
    /// Model User Respone Insert
    /// </summary>
    public class cmlResUsrInsItem : cmlResBase
    {
        /// <summary>
        ///  Code User
        /// </summary>
        public string rtUsrCode { get; set; }
    }
}