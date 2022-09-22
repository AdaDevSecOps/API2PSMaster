using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoAppConfig
    {
        /// <summary>
        /// รหัสแอปพลิเคชั่น เช่น VS , PS , FC
        /// </summary>
        public string rtAppCode { get; set;}

        /// <summary>
        /// รหัส Key ที่ใช้งาน
        /// </summary>
        public string rtCNKey { get; set; }

        /// <summary>
        /// ลำดับ Key
        /// </summary>
        public Nullable<int> rnCNSeq { get; set; }

        /// <summary>
        /// สถานะการใช้งานแบบ Online    1=Online  2=Offline
        /// </summary>
        public string rtCNStaOnline { get; set; }

        /// <summary>
        /// สถานะการอนุญาตใน 1=อนุญาต  2=ไม่อนุญาต
        /// </summary>
        public string rtCNStaValue { get; set; }

        /// <summary>
        /// สถานะการใช้งาน 1=ใช้งาน  2=ไม่ใช้งาน
        /// </summary>
        public string rtCNStaUse { get; set; }

    }
}