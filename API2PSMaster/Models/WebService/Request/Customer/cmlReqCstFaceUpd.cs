using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    public class cmlReqCstFaceUpd
    {
        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string ptCstCode { get; set; }
        /// <summary>
        /// ลําดับรูป 
        /// </summary>
        public string pnCstPicSeq { get; set; }
        /// <summary>
        ///  รูปภาพ base 64
        /// </summary>
        public string ptPicture { get; set; }
        /// <summary>
        /// User/client ทีเรียก 
        /// </summary>
        public string ptUsrCreate { get; set; }

    }
}
