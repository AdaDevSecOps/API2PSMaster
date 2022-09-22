using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Document
{
    //[Serializable]
    public class cmlResPdtTnfDwn
    {
        public List<cmlResInfoPdtTnfHD> raHD { get; set; }
        public List<cmlResInfoPdtTnfHDRef> raHDRef { get; set; }
        public List<cmlResInfoPdtTnfDT> raDT { get; set; }
        //public List<cmlResInfoPdtTnfDTSrn> raDTSrn { get; set; }
    }
}