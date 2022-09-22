using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.TxnAPI
{
    public class cmlResInfoTxnAPI
    {
        /// <summary>
        ///รหัสเส้น API ที่ Interface
        /// </summary>
        public string rtApiCode { get; set; }

        /// <summary>
        ///ประเภทของเส้น Interface 1=นำเข้า 2=ส่งออก  3= API (STD)
        /// </summary>
        public string rtApiTxnType { get; set; }

        /// <summary>
        ///ประเภทของการทำงาน 1 =Batch  2=Real time
        /// </summary>
        public string rtApiPrcType { get; set; }

        /// <summary>
        ///กลุ่มของการ Interface Ex. กลุ่มสินค้า  , กลุ่มลูกค้า เพื่อใช้จัดกลุ่มเส้น Interface
        /// </summary>
        public string rtApiGrpPrc { get; set; }

        /// <summary>
        ///ลำดับของกลุ่ม Interface
        /// </summary>
        public Nullable<int> rnApiGrpSeq { get; set; }

        /// <summary>
        ///รหัสรูปแบบของ API
        /// </summary>
        public string rtApiFmtCode { get; set; }

        /// <summary>
        ///เก็บ URL ของเส้น Interface กรณีที่ Interface นั้นต้องใช้ Webservice
        /// </summary>
        public string rtApiURL { get; set; }

        /// <summary>
        ///เก็บรหัส User ที่ใช้สำหรับ Login เข้า API
        /// </summary>
        public string rtApiLoginUsr { get; set; }

        /// <summary>
        ///เก็บรหัสผ่าน เข้ารหัสตาม Format ADA
        /// </summary>
        public string rtApiLoginPwd { get; set; }

        /// <summary>
        ///เก็บ Token ที่ใช้ในการ Interface กับ 3Party
        /// </summary>
        public string rtApiToken { get; set; }

        /// <summary>
        ///วันที่สร้าง
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้าง
        /// </summary>
        public string rtCreateBy { get; set; }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }
    }
}