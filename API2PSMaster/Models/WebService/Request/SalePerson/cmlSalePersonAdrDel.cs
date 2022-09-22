using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.SalePerson
{
    public class cmlSalePersonAdrDel
    {
        /// <summary>
        /// รหัสอ้างอิง Branch , User ,Saleman ,ร้านค้า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptRefNo { get; set; }
    }
}