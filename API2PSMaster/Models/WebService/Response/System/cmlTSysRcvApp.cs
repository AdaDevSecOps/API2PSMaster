using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlTSysRcvApp
    {
        /// <summary>
        ///รหัสระบบ
        /// <summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> rnAppSeq { get; set; }

        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// <summary>
        public string rtFmtCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อประเภท
        /// <summary>
        public string rtAppName { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ ทำรายการคืนได้  2 :ไม่อนุญาต ให้ ทำรายการคืน
        /// <summary>
        public string rtAppStaAlwRet { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ ทำการยกเลิกรายการได้ 2 :ไม่อนุญาต ให้ ทำการยกเลิกรายการ
        /// <summary>
        public string rtAppStaAlwCancel { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ มีรายการอื่น ต่อท้าย 2 :ไม่อนุญาต ให้ มีรายการอื่น ต่อท้าย
        /// <summary>
        public string rtAppStaPayLast { get; set; }
    }
}