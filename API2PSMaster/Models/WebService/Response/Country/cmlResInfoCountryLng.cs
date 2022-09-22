using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2PSMaster.Models.WebService.Response.Country
{
    public class cmlResInfoCountryLng
    {
        /// <summary>
        ///รหัสประเทศ
        /// </summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// </summary>
        public string rtCtyName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtCtyRmk { get; set; }
    }
}
