using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ข้อมูล ประเภทคูปอง
    /// </summary>
    public class cmlResCpnTypeDwn
    {
        //*Net 63-03-05
        //public List<cmlResInfoCpn> raCpn { get; set; }
        //public List<cmlResInfoCpnLng> raCpnLng { get; set; }

        /// <summary>
        /// ประเภทคูปอง
        /// </summary>
        public List<cmlResInfoCpnType> raCpnType { get; set; }
        /// <summary>
        /// ภาษา ประเภทคูปอง
        /// </summary>
        public List<cmlResInfoCpnType_L> raCpnTypeLng { get; set; }
    }
}