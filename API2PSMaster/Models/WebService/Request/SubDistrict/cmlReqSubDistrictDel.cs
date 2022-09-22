using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.SubDistrict
{
    public class cmlReqSubDistrictDel
    {
        /// <summary>
        /// รหัสตำบล
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptSudCode { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }
    }
}