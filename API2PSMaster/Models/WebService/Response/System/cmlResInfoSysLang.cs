using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysLang
    {
        /// <summary>
        ///รหัสภาษา
        /// </summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อภาษาท้องถิ่น (รูป อยู่ใน TTKMImgObj)
        /// </summary>
        public string rtLngName { get; set; }

        /// <summary>
        ///ชื่อภาษาอังกฤษ
        /// </summary>
        public string rtLngNameEng { get; set; }

        /// <summary>
        ///ชื่อย่อภาษาอังกฤษ
        /// </summary>
        public string rtLngShortName { get; set; }

        /// <summary>
        ///สถานะภาษาท้องถิ่น
        /// </summary>
        public string rtLngStaLocal { get; set; }

        /// <summary>
        ///1:ใช้งาน 2 : ไม่ใช้งาน
        /// </summary>
        public string rtLngStaUse { get; set; }
    }
}