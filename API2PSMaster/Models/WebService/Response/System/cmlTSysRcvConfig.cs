using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlTSysRcvConfig
    {
        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string rtFmtCode { get; set; }

        /// <summary>
        ///รหัสลำดับ
        /// <summary>
        public Nullable<int> rnSysSeq { get; set; }

        /// <summary>
        ///ตัวแปร
        /// <summary>
        public string rtSysKey { get; set; }

        /// <summary>
        ///ค่า ตัวแปร
        /// <summary>
        public string rtSysStaUsrValue { get; set; }

        /// <summary>
        ///ค่า อ้างอิง
        /// <summary>
        public string rtSysStaUsrRef { get; set; }

        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string rtBchCode { get; set; }
    }
}