using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Event
{
    public class cmlResInfoEventDT
    {
        /// <summary>
        ///รหัส
        /// <summary>
        public string rtEvhCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> rnEvdSeqNo { get; set; }

        /// <summary>
        ///ประเภทเหตุการณ์ 1:ตามช่วงเวลา 2:ตามช่วงวันที่
        /// <summary>
        public string rtEvdType { get; set; }

        /// <summary>
        ///เวลาเริ่มต้น
        /// <summary>
        public string rtEvdTStart { get; set; }

        /// <summary>
        ///วันที่เริ่มต้น
        /// <summary>
        public Nullable<DateTime> rdEvdDStart { get; set; }

        /// <summary>
        ///เวลาสิ้นสุด
        /// <summary>
        public string rtEvdTFinish { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด
        /// <summary>
        public Nullable<DateTime> rdEvdDFinish { get; set; }

        /// <summary>
        ///สถานะ 1 :ใช้งาน 2:ไม่ใช้งาน
        /// <summary>
        public string rtEvdStaUse { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }
    }
}