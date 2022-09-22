using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Agency
{
    public class cmlResInfoAgencyLng
    {
        /// <summary>
        ///รหัสคู้ค้า
        /// </summary>
        public string rtAgnCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// </summary>
        public string rtAgnName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtAgnRmk { get; set; }
    }
}