using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoShopCabinet
    {
        //*Arm 63-01-16 -Create Model cmlResInfoShopCabinet

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        /// ลำดับตู้ Vending (มีการพ่วงมากกว่า 1 ตู้)
        /// </summary>
        public Nullable<int> rnCabSeq { get; set; }

        /// <summary>
        /// จำนวนแถวสูงสุด  / 1 ตู้
        /// </summary>
        public Nullable<Int64> rnCabMaxRow { get; set; }

        /// <summary>
        /// จำนวน Column สูงสุด / 1 ตู้
        /// </summary>
        public Nullable<int> rnCabMaxCol { get; set; }

        /// <summary>
        /// ประเภทตู้ 1=Vending 2=Locker
        /// </summary>
        public Nullable<int> rnCabType { get; set; }

        /// <summary>
        /// ประเภทร้านค้า  อ้างอิง Table TVDMShopType
        /// </summary>
        public string rtShtCode { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        /// วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        /// ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
    }
}