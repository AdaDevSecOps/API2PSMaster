using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Voucher
{
    //[Serializable]
    public class cmlResVchDwn
    {
        public List<cmlResInfoVch> raVch { get; set; }
        public List<cmlResInfoVchLng> raVchLng { get; set; }
        public List<cmlResInfoVchType> raVchType { get; set; }
        public List<cmlResInfoVchTypeLng> raVchTypeLng { get; set; }
    }
}