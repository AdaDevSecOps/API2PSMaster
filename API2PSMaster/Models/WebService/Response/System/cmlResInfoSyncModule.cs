using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSyncModule
    {
        /// <summary>
        /// รหัส Application
        /// </summary>
        public string rtAppCode { get; set; }

        /// <summary>
        /// ลำดับรายการจากตาราง TSysSyncData
        /// </summary>
        public Nullable<int> rnSynSeqNo { get; set; }
    }
}