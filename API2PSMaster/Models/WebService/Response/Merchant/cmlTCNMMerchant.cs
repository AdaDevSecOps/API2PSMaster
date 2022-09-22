using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Merchant
{
    public class cmlTCNMMerchant
    {
        /// <summary>
        ///รหัสตัวแทน/เจ้าของกำเนินการ
        /// <summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสกลุ่มราคา
        /// <summary>
        public string rtPplCode { get; set; } // *Arm 63-06-17

        /// <summary>
        ///Email
        /// <summary>
        public string rtMerEmail { get; set; }

        /// <summary>
        ///เบอร์โทรศัพท์
        /// <summary>
        public string rtMerTel { get; set; }

        /// <summary>
        ///เบอร์โทรสาร
        /// <summary>
        public string rtMerFax { get; set; }

        /// <summary>
        ///เบอร์โทรศัพท์
        /// <summary>
        public string rtMerMo { get; set; }

        /// <summary>
        ///สถานะติดต่อ 1:ติดต่อ, 2:เลิกติดต่อ
        /// <summary>
        public string rtMerStaActive { get; set; }

        ///<summary>
        ///รหัสอ้างอิงตัวแทน
        ///<summary>
        public string rtMerRefCode { get; set; } // *Arm 63-06-17

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}