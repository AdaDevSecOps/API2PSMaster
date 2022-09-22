using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API2PSMaster.Class.Standard;
using System.ComponentModel.DataAnnotations;

namespace API2PSMaster.Models.WebService.Request.Warhouse
{
    /// <summary>
    /// Request for Insert to TCNMWaHouse
    /// </summary>
    public class cmlReqWarHouse
    {
        //For TCNMWaHouse

        /// <summary>
        /// รหัสคลังสินค้า
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWahCode { get; set; }

        /// <summary>
        /// ประเภทคลัง 1:มาตรฐาน 2:คลังทั่วไป 3 :คลังสาขา 4 :คลังฝากขาย/ร้านค้า 5:คลังหน่วยรถ
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWahStaType { get; set; }

        /// <summary>
        /// ประเภทคลัง 3.คลังสาขา 4:Shop 5:Sale Man
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWahRefCode { get; set; }

        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        //For TCNMWaHouse_L
        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ชื่อคลังสินค้า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWahName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptWahRmk { get; set; }

    }
}