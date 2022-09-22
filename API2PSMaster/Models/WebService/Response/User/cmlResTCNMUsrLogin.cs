using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.User
{
    public class cmlResTCNMUsrLogin
    {
        /// <summary>
        ///รหัสผู้ใช้
        /// </summary>
        public string rtUsrCode { get; set; }

        /// <summary>
        ///ประเภทการเข้าใช้งาน 1:Password 2:Pin 3:RFID  4:QR
        /// </summary>
        public string rtUsrLogType { get; set; }

        /// <summary>
        ///วันที่เริมใช้งาน
        /// </summary>
        public Nullable<DateTime> rdUsrPwdStart { get; set; }

        /// <summary>
        ///วันที่สิ้นสุด  Start + 3 M
        /// </summary>
        public Nullable<DateTime> rdUsrPwdExpired { get; set; }

        /// <summary>
        ///Account ห้ามซ้ำ เช่น รหัสพนักงาน email เบอร์ โทร
        /// </summary>
        public string rtUsrLogin { get; set; }

        /// <summary>
        ///Type 2: 6 digit,3:รหัส RFID,4 QR {FTUsrCode|Passoword เข้ารหัส}/ถ้าไม่เข้า Format เช็ค LoginPwd)
        /// </summary>
        public string rtUsrLoginPwd { get; set; }

        /// <summary>
        ///Remark,Hint
        /// </summary>
        public string rtUsrRmk { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// </summary>
        public string rtUsrStaActive { get; set; }
    }
}