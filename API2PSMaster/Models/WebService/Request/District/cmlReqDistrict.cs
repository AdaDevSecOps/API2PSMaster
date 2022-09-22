using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.District
{
    public class cmlReqDistrict
    {
        /// <summary>
        /// District Code
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptDstCode { get; set; }

        /// <summary>
        /// District Post
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptDstPost { get; set; }

        /// <summary>
        /// Province Code
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptPvnCode { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public string ptPvnLatitude { get; set; }

        /// <summary>
        /// Longtitude
        /// </summary>
        public string ptPvnLongitude { get; set; }

        /// <summary>
        /// Who Update
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptWhoUpd { get; set; }

        /// <summary>
        /// ภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [Range(1, int.MaxValue, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public int pnLngID { get; set; }

        /// <summary>
        /// District Name
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptDstName { get; set; }

    }
}