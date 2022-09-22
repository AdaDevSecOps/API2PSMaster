using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    public class cmlResInfoSysDocType
    {
        //public string rtSdtTblName { get; set; }
        //public string rtSdtFedTypeName { get; set; }
        //public int rnSdtDocType { get; set; }
        //public string rtSdtDocName { get; set; }
        //public string rtSdtDocTypeRef { get; set; }

        

        /// <summary>
        ///(AUTONUMBER)รหัสเอกสาร
        /// </summary>
        public Nullable<Int64> rnSdtID { get; set; }    //*Arm 63-0-24  

        /// <summary>
        ///ชื่อตาราง
        /// </summary>
        public string rtSdtTblName { get; set; }

        /// <summary>
        ///ชื่อฟิลด์ ประเภทเอกสาร FTXxxDocType
        /// </summary>
        public string rtSdtFedTypeName { get; set; }

        /// <summary>
        ///กลุ่มเอกสาร
        /// </summary>
        public string rtSdtDocGrp { get; set; }     //*Arm 63-0-24  

        /// <summary>
        ///ประเภทเอกสาร
        /// </summary>
        public Nullable<int> rnSdtDocType { get; set; }

        /// <summary>
        ///ชื่อเอกสาร
        /// </summary>
        public string rtSdtDocName { get; set; }

        /// <summary>
        ///ประเภทเอกสารอ้างอิงระบบอืน
        /// </summary>
        public string rtSdtDocTypeRef { get; set; }

    }
}