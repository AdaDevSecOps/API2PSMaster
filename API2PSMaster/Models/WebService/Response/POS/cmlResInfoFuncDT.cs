using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoFuncDT
    {
        /// <summary>
        ///รหัส Function Keyboard
        /// <summary>
        public string rtGhdCode { get; set; }

        /// <summary>
        ///รหัสคีย์บอร์ด : KB999
        /// <summary>
        public string rtSysCode { get; set; }

        /// <summary>
        ///แสดงใน หน้า กรณีเกิน 1 หน้า
        /// <summary>
        public Nullable<int> rnGdtPage { get; set; }

        /// <summary>
        ///ลำดับ ตามกลุ่ม-Default
        /// <summary>
        public Nullable<Int64> rnGdtDefSeq { get; set; }

        /// <summary>
        ///ลำดับ ตามกลุ่ม-User setup
        /// <summary>
        public Nullable<Int64> rnGdtUsrSeq { get; set; }

        /// <summary>
        ///ขนาดปุ่ม แกน X
        /// <summary>
        public Nullable<int> rnGdtBtnSizeX { get; set; }

        /// <summary>
        ///ขนาดปุ่ม แกน Y
        /// <summary>
        public Nullable<int> rnGdtBtnSizeY { get; set; }

        /// <summary>
        ///ฟังก์ชันที่อ้างอิงถึง
        /// <summary>
        public string rtGdtCallByName { get; set; }

        /// <summary>
        ///สถานะการใช้งาน ว่าง Null:ไม่ใช้งาน 1:ใช้งาน
        /// <summary>
        public string rtGdtStaUse { get; set; }

        /// <summary>
        ///ระดับที่อนุญาต ใช้งาน
        /// <summary>
        public Nullable<int> rnGdtFuncLevel { get; set; }

        /// <summary>
        ///สถานะระบบใช้งาน ว่าง Null:ไม่ใช้งาน 1:ใช้งาน
        /// <summary>
        public string rtGdtSysUse { get; set; }
    }
}