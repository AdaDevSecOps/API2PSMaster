using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    //[Serializable]
    public class cmlResCstDwn
    {
        public List<cmlResInfoCst> raCst { get; set; }
        public List<cmlResInfoCstLng> raCstLng { get; set; }
        public List<cmlResInfoCstAddrLng> raCstAddrLng { get; set; }
        public List<cmlResInfoCstCard> raCstCard { get; set; }
        public List<cmlResInfoCstContactLng> raCstContactLng { get; set; }
        public List<cmlResInfoCstCredit> raCstCredit { get; set; }
        public List<cmlResInfoCstGrp> raCstGrp { get; set; }
        public List<cmlResInfoCstGrpLng> raCstGrpLng { get; set; }
        public List<cmlResInfoCstLev> raCstLev { get; set; }
        public List<cmlResInfoCstLevLng> raCstLevLng { get; set; }
        public List<cmlResInfoCstOcp> raCstOcp { get; set; }
        public List<cmlResInfoCstOcpLng> raCstOcpLng { get; set; }
        public List<cmlResInfoCstRFIDLng> raCstRFIDLng { get; set; }
        public List<cmlResInfoCstType> raCstType { get; set; }
        public List<cmlResInfoCstTypeLng> raCstTypeLng { get; set; }
        public List<cmlResInfoCstPoint> raCstPoint { get; set; }
    }
}