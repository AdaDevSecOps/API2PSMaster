using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Vending
{
    public class cmlResInfoShopCabinetLng
    {
        /// <summary>
        ///ลำดับ CabinetSeq
        /// </summary>
        public Nullable<int> rnCabSeq { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<int> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ Cabinet
        /// </summary>
        public string rtCabName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtCabRmk { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string rtShpCode { get; set; }
    }
}