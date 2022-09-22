using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Image
{
    public class cmlReqImgList
    {
        /// <summary>
        /// รหัสอ้างอิงข้อมูลหลัก
        /// </summary>
        public string ptImgRefID { get; set; }

        /// <summary>
        /// Key filter ระบุข้อมูล กรณีมีหลาย Seq
        /// </summary>
        public string ptImgKey { get; set; }

        /// <summary>
        /// เก็บรูปภาพเป็น Path ..\
        /// </summary>
        public string ptImgobj { get; set; }

    }
}