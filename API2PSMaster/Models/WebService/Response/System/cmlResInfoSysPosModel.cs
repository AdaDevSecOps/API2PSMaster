using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysPosModel
    {
        /// <summary>
        /// รหัส
        /// </summary>
        public string rtSpmCode { get; set; }

        /// <summary>
        /// ยี่ห้อ ผู้ผลิต
        /// </summary>
        public string rtSpmBrand { get; set; }

        /// <summary>
        /// ชื่อ
        /// </summary>
        public string rtSpmName { get; set; }

        /// <summary>
        /// ชื่ออื่น
        /// </summary>
        public string rtSpmNameEng { get; set; }

        /// <summary>
        /// ระบบ
        /// </summary>
        public string rtSpmSystem { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string rtSpmRemark { get; set; }
    }
}