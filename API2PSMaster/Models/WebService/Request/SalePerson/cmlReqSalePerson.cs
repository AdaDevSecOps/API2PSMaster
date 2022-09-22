using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models.WebService.Request.SalePerson;

namespace API2PSMaster.Models.WebService.Request.SalePerson
{
    public class cmlReqSalePerson
    {
       
        /// <summary>
        /// รหัสพนักงานขาย
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptSpnCode { get; set; }

        /// <summary>
        /// เบอร์โทรศัพท์
        /// </summary>
        public string ptSpnTel { get; set; }

        /// <summary>
        /// รวมยอดขายได้
        /// </summary>
        public decimal pnSpnSleAmt { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string ptSpnEmail { get; set; }

        /// <summary>
        /// ภาษา
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ชื่อหนักงานขาย
        /// </summary>
        public string ptSpnName { get; set; }

        /// <summary>
        /// หมายเหตุ
        /// </summary>
        public string ptSpnRmk { get; set; }

        public cmlReqSalePersonGrp poPersonGrp { get; set; }
        /// <summary>
        /// Who Update
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }
    }
}