using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.ProductFhn
{
    //[Serializable]
    public class cmlResInfoPdtDCS
    {
        public string rtDcsCode { get; set; }
        public string rtDepCode { get; set; }
        public string rtClsCode { get; set; }
        public string rtSclCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}