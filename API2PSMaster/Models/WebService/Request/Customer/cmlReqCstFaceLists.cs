using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    public class cmlReqCstFaceLists
    {
        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string ptCstCode { get; set;}

        /// <summary>
        /// จํานวนรูปทีต้องการ
        /// </summary>
        public int pnCstPicQty { get; set; }

    }
}