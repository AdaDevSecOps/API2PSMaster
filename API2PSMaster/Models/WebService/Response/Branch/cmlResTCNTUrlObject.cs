using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Branch
{
    public class cmlResTCNTUrlObject
    {
        private string tC_Pic; //*Em 62-06-08
        private cSP oC_SP;  //*Em 62-06-08

        /// <summary>
        ///(AUTONUMBER)รหัส
        /// <summary>
        public Nullable<Int64> rnUrlID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// <summary>
        public string rtUrlRefID { get; set; }

        /// <summary>
        ///ลำดับ URL
        /// <summary>
        public Nullable<int> rnUrlSeq { get; set; }

        /// <summary>
        ///ประเภท URL
        /// <summary>
        public Nullable<int> rnUrlType { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// <summary>
        public string rtUrlTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// <summary>
        public string rtUrlKey { get; set; }

        /// <summary>
        ///Path URL
        /// <summary>
        public string rtUrlAddress { get; set; }

        /// <summary>
        ///Port สำหรับเชื่อต่อ
        /// <summary>
        public string rtUrlPort { get; set; }

        /// <summary>
        ///เก็บรูป icon ภาพเป็น Path ..\
        /// <summary>
        public string rtUrlLogo //{ get; set; }
        {
            get { return tC_Pic; }
            set
            {
                oC_SP = new cSP();
                tC_Pic = oC_SP.SP_PRCtConvertImage2Base64(value);
            }
        }

        /// <summary>
        ///วันที่ปรับปรุงรายการล่าสุด
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