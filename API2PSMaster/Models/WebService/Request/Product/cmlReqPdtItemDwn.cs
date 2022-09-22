using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Product
{
    public class cmlReqPdtItemDwn
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสตัวแทนขาน (AD)
        /// </summary>
        public string ptAgnCode { get; set; }

        /// <summary>
        /// รหัสคลัง
        /// </summary>
        public string ptWahCode { get; set; }

        /// <summary>
        /// วันที่
        /// </summary>
        public Nullable<DateTime> pdDate { get; set; }
    }
}