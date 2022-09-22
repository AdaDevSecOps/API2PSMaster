using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Shop
{
    //*Arm 63-01-17 - Create Parameter ใหม่
    public class cmlResInfoShopLng
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        public string rtShpCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        /// ชื่อร้าน
        /// </summary>
        public string rtShpName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string rtShpRmk { get; set; }
    }
}