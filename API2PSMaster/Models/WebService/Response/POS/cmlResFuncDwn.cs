using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResFuncDwn
    {
        public List<cmlResInfoFuncHD> raFuncHD { get; set; }
        public List<cmlResInfoFuncDT> raFuncDT { get; set; }
        public List<cmlResInfoFuncDTLng> raFuncDTLng { get; set; }
        //public List<cmlResInfoFuncDTSpc> raFuncDTSpc { get; set; }  //*Arm 63-09-10 //*Net 63-10-02 ปิด Model
    }
}