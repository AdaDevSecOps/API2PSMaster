using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.UserDep
{
    /// <summary>
    /// Information User Department 
    /// </summary>
    public class cmlResUsrDepInsItem : cmlResBase
    {
        /// <summary>
        ///  User Departmnet Code
        /// </summary>
        public string rtDptCode { get; set; }
    }
}