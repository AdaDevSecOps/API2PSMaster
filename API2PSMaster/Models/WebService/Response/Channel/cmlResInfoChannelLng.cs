using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Channel
{
    /// <summary>
    /// ตาราง TCNMChannel_L
    /// </summary>
    public class cmlResInfoChannelLng
    {
        /// <summary>
        ///รหัสช่องทางการจำหน่าย
        /// </summary>
        public string rtChnCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Int64 rnLngID { get; set; }

        /// <summary>
        ///ชื่อช่องทางการจำหน่าย
        /// </summary>
        public string rtChnName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// </summary>
        public string rtChnRmk { get; set; }

        ///// <summary>
        /////รหัสสาขา
        ///// </summary>
        //public string rtBchCode { get; set; }


    }
}