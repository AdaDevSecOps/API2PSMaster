using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.SalePerson
{
    public class cmlReqSalePersonGrp
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// สถานะร้านค้า 0:ไม่กำหนด 1:กำหนด
        /// </summary>
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptSpnStaShop { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        public string ptShpCode { get; set; }

        /// <summary>
        /// วันที่เริ่มดำเนินการ
        /// </summary>
        public Nullable<DateTime> pdSpnStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดดำเนินการ
        /// </summary>
        public Nullable<DateTime> pdSpnStop { get; set; }


    }
}