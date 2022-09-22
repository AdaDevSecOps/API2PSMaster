using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Pos
{
    public class cmlReqPosRegister
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสเครื่องจุดขาย
        /// </summary>
        public string ptPosCode { get; set; }

        /// <summary>
        /// Mac. Address เครื่อง POS
        /// </summary>
        public string ptMacAddress { get; set; }

        /// <summary>
        /// Computer Name
        /// </summary>
        public string ptCompName { get; set; }

    }
}