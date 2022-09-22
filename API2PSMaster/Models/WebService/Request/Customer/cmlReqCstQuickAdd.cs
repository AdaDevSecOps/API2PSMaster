using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    public class cmlReqCstQuickAdd
    {
        /// <summary>
        /// เบอร์โทรลูกค้า
        /// </summary>
        public string ptCstTel { get; set; }
        
        /// <summary>
        /// PIN สำหรับเข้าใช้งาน(ยังไม่ใช้ตอนนี้)
        /// </summary>
        public string ptCstPin { get; set; }
        
        /// <summary>
        /// 0:ไม่ส่ง 1:Retail 2:Wholesale 3: online(default 1)
        /// </summary>
        public int pnPriGrpType { get; set; }
        
        /// <summary>
        /// กลุ่มราคา
        /// </summary>
        public string ptPriGrp { get; set; }

        /// <summary>
        /// client ที่เรียก
        /// </summary>
        public string ptUsrCreate { get; set; }
    }
}