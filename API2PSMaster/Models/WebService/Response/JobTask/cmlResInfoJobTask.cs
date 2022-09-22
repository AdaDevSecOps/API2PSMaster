using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2PSMaster.Models.WebService.Response.JobTask
{
    public class cmlResInfoJobTask
    {
        /// <summary>
        ///รหัสคู้ค้า/ตัวแทนขาย
        /// </summary>
        public string rtAgnCode { get; set; }

        /// <summary>
        ///ชื่อตารางอ้างอิง Job
        /// </summary>
        public string rtJobRefTbl { get; set; }

        /// <summary>
        ///วันที่ยืนยัน Job
        /// </summary>
        public Nullable<DateTime> rdJobDateCfm { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1 : ใช้งาน , 2 : ไม่ใช้งาน
        /// </summary>
        public string rtJobStaUse { get; set; }

        public Nullable<DateTime> rdLastUpdOn { get; set; }  //*Arm 65-09-14
        public string rtLastUpdBy { get; set; }  //*Arm 65-09-14
        public Nullable<DateTime> rdCreateOn { get; set; }  //*Arm 65-09-14
        public string rtCreateBy { get; set; }  //*Arm 65-09-14
    }
}
