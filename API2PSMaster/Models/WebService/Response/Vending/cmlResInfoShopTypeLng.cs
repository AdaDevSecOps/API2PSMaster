using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoShopTypeLng
    {
        /// <summary>
        /// รหัสประเภทตู้
        /// </summary>
        public string rtShtCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public int rnLngID { get; set; }

        /// <summary>
        /// ชื่อประเภท Shop
        /// </summary>
        public string rtShtName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string rtShtRemark { get; set; }
    }
}