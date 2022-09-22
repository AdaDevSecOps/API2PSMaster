using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.SalePerson
{
    public class cmlReqSalePersonDel
    {
        /// <summary>
        /// รหัสพนักงานขาย
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptSpnCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// กลุ่มพนักงานขาย
        /// </summary>
        public cmlReqSalePersonGrpDel poDelGrp { get; set; }

        /// <summary>
        /// ที่อยู่
        /// </summary>
        public cmlSalePersonAdrDel poDelAddr { get; set; }
    }
}