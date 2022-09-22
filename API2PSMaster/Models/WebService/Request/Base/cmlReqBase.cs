using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Base
{
    public class cmlReqBase
    {
        /// <summary>
        /// User code save data.
        /// </summary>
        public string ptWhoUpd { get; set; }

        public string ptBchCode { get; set; }
    }
}