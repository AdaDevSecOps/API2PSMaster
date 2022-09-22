using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Rate
{
    //[Serializable]
    public class cmlResInfoRateLng
    {
        public string rtRteCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtRteName { get; set; }
        public string rtRteShtName { get; set; }
        public string rtRteNameText { get; set; }
        public string rtRteDecText { get; set; }
        public string rtAgnCode { get; set; }  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์
    }
}