using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResCstTypeDwn
    {
        public List<cmlResInfoCstType> raCstType { get; set; }
        public List<cmlResInfoCstTypeLng> raCstTypeLng { get; set; }
    }
}