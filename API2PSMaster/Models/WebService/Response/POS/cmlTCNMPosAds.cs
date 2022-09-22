using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    public class cmlTCNMPosAds
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัส Shop
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่อง Pos / รหัสตู้
        /// <summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///ลำดับ
        /// <summary>
        public Nullable<int> rnPsdSeq { get; set; }

        /// <summary>
        ///ตำแหน่งโฆษณา  TL,TM,TR,ML,MM,MR,BL,BM,BR,AL
        /// <summary>
        public string rtPsdPosition { get; set; }

        /// <summary>
        ///รหัสโฆษณา
        /// <summary>
        public string rtAdvCode { get; set; }

        /// <summary>
        ///ความกว้าง
        /// <summary>
        public Nullable<int> rnPsdWide { get; set; }

        /// <summary>
        ///ความสูง
        /// <summary>
        public Nullable<int> rnPsdHigh { get; set; }

        /// <summary>
        ///วันที่/เวลาเริ่ม
        /// <summary>
        public Nullable<DateTime> rdPsdStart { get; set; }

        /// <summary>
        ///วันที่/เวลาหมดอายุ
        /// <summary>
        public Nullable<DateTime> rdPsdStop { get; set; }

        /// <summary>
        ///วันที่ Update ล่าสุด
        /// <summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// <summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// <summary>
        public string rtCreateBy { get; set; }
    }
}