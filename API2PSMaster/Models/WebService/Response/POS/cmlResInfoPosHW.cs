using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoPosHW
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; } //*Arm 63-01-23

        public string rtPhwCode { get; set; }
        public string rtPosCode { get; set; }
        public string rtShwCode { get; set; }
        public string rtPhwCodeRef { get; set; }
        public Nullable<Int64> rnPhwSeq { get; set; }
        public string rtPhwName { get; set; }
        public string rtPhwConnType { get; set; }
        public string rtPhwConnRef { get; set; }
        public string rtPhwCustom { get; set; }
        public Nullable<Int64> rnPhwTimeOut { get; set; }
    }
}