using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public class cmlReqCstDwn
    {
        /// <summary>
        /// Customer languageg ID.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<long> pnLngID { get; set; }

        /// <summary>
        /// Date time last download.<br/>
        /// <b>Format yyyy-MM-dd HH:mm:ss</b>
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Nullable<DateTime> pdLastDwn { get; set; }

        /// <summary>
        /// Page to get data.
        /// </summary>
        public int pnPage { get; set; }

        /// <summary>
        /// Row data return per page.
        /// </summary>
        public int pnRowPerPage { get; set; }
    }
}