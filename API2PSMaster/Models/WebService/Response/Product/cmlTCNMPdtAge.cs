using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    public class cmlTCNMPdtAge
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///อายุสินค้า
        /// <summary>
        public Nullable<decimal> rcPdtAge { get; set; }

        /// <summary>
        ///อุณหภูมิ สูงสุด
        /// <summary>
        public Nullable<decimal> rcPdtMaxDegree { get; set; }

        /// <summary>
        ///อุณหภูมิ ต่ำสุด
        /// <summary>
        public Nullable<decimal> rcPdtMinDegree { get; set; }

        /// <summary>
        ///ส่วนผสม
        /// <summary>
        public string rtPdtIngredients { get; set; }

        /// <summary>
        ///วิธีรับประทาน
        /// <summary>
        public string rtPdtHowToUse { get; set; }

        /// <summary>
        ///คำเตือน
        /// <summary>
        public string rtPdtWarning { get; set; }

        /// <summary>
        ///วันที่ผลิต
        /// <summary>
        public Nullable<DateTime> rdPdtMfg { get; set; }

        /// <summary>
        ///วันที่หมดอายุ
        /// <summary>
        public Nullable<DateTime> rdPdtExp { get; set; }

        /// <summary>
        ///ค่าพลังงาน จาก ปริมาตร  (70 กรัม)
        /// <summary>
        public string rtPdtPerVolumn { get; set; }

        /// <summary>
        ///ค่าพลังงาน  (80 KCal)
        /// <summary>
        public Nullable<decimal> rcPdtPerCalories { get; set; }

        /// <summary>
        ///เวลาในการปรุง
        /// <summary>
        public Nullable<decimal> rcPdtCookTime { get; set; }

        /// <summary>
        ///อุณหภูมิในการปรุง
        /// <summary>
        public Nullable<decimal> rcPdtCookHeat { get; set; }
    }
}