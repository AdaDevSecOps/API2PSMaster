using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Zone
{
    public class cmlResInfoZoneLng
    {
        /// <summary>
        ///รหัสลูกโซ่ (รหัสกลุ่มรวมกันตามระดับ)
        /// </summary>
        public string rtZneCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อโซน
        /// </summary>
        public string rtZneName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtZneRmk { get; set; }

        /// <summary>
        ///รหัสโซน
        /// </summary>
        public string rtZneChain { get; set; } //*Net 62-12-30 Add ZneChain follow DB

        /// <summary>
        ///ชื่อรหัสลูกโซ่
        /// </summary>
        public string rtZneChainName { get; set; } //*Net 62-12-30 Add ZneChainName follow DB
        
    }
}