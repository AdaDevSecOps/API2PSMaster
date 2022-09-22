using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rcv
{
    //[Serializable]
    public class cmlResInfoRcv
    {
        //public string rtRcvCode { get; set; }
        //public string rtFmtCode { get; set; }
        //public string rtRcvStaUse { get; set; }
        //public Nullable<DateTime> rdLastUpdOn { get; set; }
        //public Nullable<DateTime> rdCreateOn { get; set; }
        //public string rtLastUpdBy { get; set; }
        //public string rtCreateBy { get; set; }

        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// </summary>
        public string rtRcvCode { get; set; }

        /// <summary>
        ///รหัสรูปแบบการรับชำระเงิน
        /// </summary>
        public string rtFmtCode { get; set; }

        /// <summary>
        ///สถานะใฃ้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string rtRcvStaUse { get; set; }

        /// <summary>
        ///สถานะใฃ้งาน 1:ใช้งาน 2:ไม่ใช้งาน
        /// </summary>
        public string rtRcvStaShwInSlip { get; set; }

        /// <summary>
        ///รหัสรูปแบบการรับชำระเงินกรณีทำรายการ คืน
        /// </summary>
        public string rtRcv4Ret { get; set; }

        /// <summary>
        ///รหัสรูปแบบการรับชำระเงินกรณีทำรายการ checkout
        /// </summary>
        public string rtRcv4ChkOut { get; set; }

        /// <summary>
        ///1: อนุญาต ให้ ทำรายการคืนได้  2 :ไม่อนุญาต ให้ ทำรายการคืน
        /// </summary>
        public string rtAppStaAlwRet { get; set; }      //*Arm 63-07-30

        /// <summary>
        ///1: อนุญาต ให้ ทำการยกเลิกรายการได้ 2 :ไม่อนุญาต ให้ ทำการยกเลิกรายการ
        /// </summary>
        public string rtAppStaAlwCancel { get; set; }   //*Arm 63-07-30

        /// <summary>
        ///1: อนุญาต ให้ มีรายการอื่น ต่อท้าย 2 :ไม่อนุญาต ให้ มีรายการอื่น ต่อท้าย
        /// </summary>
        public string rtAppStaPayLast { get; set; }     //*Arm 63-07-30

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }

        public string rtRcvStaAllRte { get; set; }      //*Nick 65-08-23

        public string rtRcvRefRate { get; set; }        //*Nick 65-08-26
    }
}