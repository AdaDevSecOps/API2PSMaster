using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    public class cmlResInfoSysRateLng
    {
        /// <summary>
        ///รหัส ISO Code (สกุลเงิน)
        /// </summary>
        public string rtRteIsoCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string rtFmtCode { get; set; }

        /// <summary>
        ///ชื่อ
        /// </summary>
        public string rtRteIsoName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtRteIsoRmk { get; set; }

        /// <summary>
        ///ชื่อหน่อยของสกุลเงิน
        /// </summary>
        public string rtRteUnitName { get; set; }

        /// <summary>
        ///ชื่อหน่อยย่อยของสกุลเงิน
        /// </summary>
        public string rtRteSubUnitName { get; set; }

    }
}
