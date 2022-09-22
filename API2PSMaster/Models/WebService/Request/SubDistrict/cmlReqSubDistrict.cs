using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.SubDistrict
{

    public class cmlReqSubDistrict
    {

        /// <summary>
        /// รหัสตำบล
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptSudCode { get; set; }

        /// <summary>
        /// รหัสเขต/อำเภอ
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptDstCode { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวนอน
        /// </summary>
        public string ptSudLatitude { get; set; }

        /// <summary>
        /// ตำแหน่งบนแผนที่ แนวตั้ง
        /// </summary>
        public string ptSudLongitude { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ชื่อตำบล
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptSudName { get; set; }

    }
}