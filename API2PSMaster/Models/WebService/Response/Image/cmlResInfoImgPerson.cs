using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Image
{
    //[Serializable]
    public class cmlResInfoImgPerson
    {
        private string tC_Pic; //*Em 62-06-08
        private cSP oC_SP;  //*Em 62-06-08


        /// <summary>
        ///(AUTONUMBER)รหัส
        /// </summary>
        public Nullable<Int64> rnImgID { get; set; }

        /// <summary>
        ///รหัสอ้างอิงข้อมูลหลัก
        /// </summary>
        public string rtImgRefID { get; set; }

        /// <summary>
        ///ลำดับรูป
        /// </summary>
        public Nullable<int> rnImgSeq { get; set; }

        /// <summary>
        ///ชื่อตาราง
        /// </summary>
        public string rtImgTable { get; set; }

        /// <summary>
        ///Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// </summary>
        public string rtImgKey { get; set; }

        /// <summary>
        ///เก็บรูปภาพเป็น Path ..\
        /// </summary>
        public string rtImgObj //{ get; set; }
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
        /// </summary>
        public Nullable<DateTime> rdLastUpdOn { get; set; }

        /// <summary>
        ///วันที่สร้างรายการ
        /// </summary>
        public Nullable<DateTime> rdCreateOn { get; set; }

        /// <summary>
        ///ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string rtLastUpdBy { get; set; }

        /// <summary>
        ///ผู้สร้างรายการ
        /// </summary>
        public string rtCreateBy { get; set; }
        
    }
}