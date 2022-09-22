using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Warehouse
{
    public class cmlResInfoWahLng
    {
        /// <summary>
        ///รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; } //*Arm 63-01-23

        /// <summary>
        ///รหัสคลังสินค้า
        /// </summary>
        public string rtWahCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อคลังสินค้า
        /// </summary>
        public string rtWahName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtWahRmk { get; set; }
        
    }
}