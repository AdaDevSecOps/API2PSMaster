using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Document
{
    //[Serializable]
    public class cmlResInfoPdtTnfHDRef
    {
        /// <summary>
        ///สาขาสร้าง
        /// <summary>
        public string FTBchCode { get; set; }

        /// <summary>
        ///เลขที่เอกสาร
        /// <summary>
        public string rtXthDocNo { get; set; }

        /// <summary>
        ///ชื่อผู้ตืดต่อ
        /// <summary>
        public string rtXthCtrName { get; set; }

        /// <summary>
        ///วันที่ส่งของ
        /// <summary>
        public Nullable<DateTime> rdXthTnfDate { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่ ใบขนส่ง
        /// <summary>
        public string rtXthRefTnfID { get; set; }

        /// <summary>
        ///อ้างอิง เลขที่ ยานพาหนะ ขนส่ง
        /// <summary>
        public string rtXthRefVehID { get; set; }

        /// <summary>
        ///จำนวนและลักษณะหีบห่อ
        /// <summary>
        public string rtXthQtyAndTypeUnit { get; set; }

        /// <summary>
        ///อ้างอิง ที่อยู่ ส่งของ null หรือ 0 ไม่กำหนด
        /// <summary>
        public Nullable<Int64> rnXthShipAdd { get; set; }

        /// <summary>
        ///รหัสการขนส่ง
        /// <summary>
        public string rtViaCode { get; set; }
    }
}