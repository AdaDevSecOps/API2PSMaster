using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    public class cmlResInfoPdtSpcWah
    {
        //*Arm 63-01-17 - [Create Model] : TCNMPdtSpcWah

        /// <summary>
        /// รหัสสินค้า/รหัสควบคุมสต๊อก
        /// </summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        /// สาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสคลัง
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        /// จำนวนต่ำสุด
        /// </summary>
        public Nullable<decimal> rcSpwQtyMin { get; set; }

        /// <summary>
        /// จำนวนสูงสุด
        /// </summary>
        public Nullable<decimal> rcSpwQtyMax { get; set; }

        /// <summary>
        /// หมายเหตุ (Refer external)
        /// </summary>
        public string rtSpwRmk { get; set; }
    }
}