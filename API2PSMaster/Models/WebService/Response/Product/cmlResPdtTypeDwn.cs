using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    //[Serializable]
    public class cmlResPdtTypeDwn
    {
        public List<cmlResInfoPdtType> raPdtType { get; set; }
        public List<cmlResInfoPdtTypeLng> raPdtTypeLng { get; set; }
    }
}