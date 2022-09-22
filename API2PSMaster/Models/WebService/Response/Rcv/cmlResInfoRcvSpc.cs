using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rcv
{
    public class cmlResInfoRcvSpc
    {
        /// <summary>
        ///รหัสประเภทการชำระเงิน
        /// </summary>
        public string rtRcvCode { get; set; }

        /// <summary>
        ///รหัส APP
        /// </summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// </summary>
        public Nullable<Int64> rnRcvSeq { get; set; }

        /// <summary>
        ///สาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของดำเนินการ
        /// </summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสกลุ่ม agency
        /// </summary>
        public string rtAggCode { get; set; }

        /// <summary>
        ///หมายเหตุ (Refer external)
        /// </summary>
        public string rtPdtRmk { get; set; }

        ///// <summary>
        /////1: อนุญาต ให้ ทำรายการคืนได้  2 :ไม่อนุญาต ให้ ทำรายการคืน
        ///// </summary>
        //public string rtAppStaAlwRet { get; set; }          //*Arm 63-07-30 Comment Code (ยกเลิกฟิลด์)

        ///// <summary>
        /////1: อนุญาต ให้ ทำการยกเลิกรายการได้ 2 :ไม่อนุญาต ให้ ทำการยกเลิกรายการ
        ///// </summary>
        //public string rtAppStaAlwCancel { get; set; }       //*Arm 63-07-30 Comment Code (ยกเลิกฟิลด์)

        ///// <summary>
        /////1: อนุญาต ให้ มีรายการอื่น ต่อท้าย 2 :ไม่อนุญาต ให้ มีรายการอื่น ต่อท้าย
        ///// </summary>
        //public string rtAppStaPayLast { get; set; }         //*Arm 63-07-30 Comment Code (ยกเลิกฟิลด์)

        /// <summary>
        ///รหัสจุดขาย
        /// </summary>
        public string rtPosCode { get; set; }   //*Arm 63-10-30
    }
}