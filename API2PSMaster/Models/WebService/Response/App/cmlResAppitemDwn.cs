using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.App
{
    /// <summary>
    /// Class model response App download item
    /// </summary>
    public class cmlResAppitemDwn
    {
        /// <summary>
        /// Model TCNMAppModule
        /// </summary>
        public List<cmlResAppModule> raTCNMAppModule { get; set; }

        /// <summary>
        /// Model TSysApp
        /// </summary>
        public List<cmlResSysApp> raTSysApp { get; set; }

        /// <summary>
        /// Model TSysApp_L
        /// </summary>
        public List<cmlResSysApp_L> raTSysApp_L { get; set; }
    }
}