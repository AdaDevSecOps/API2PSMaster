using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Document
{
    //[Serializable]
    public class cmlResInfoPdtTnfDTSrn
    {
        public string rtBchCode { get; set; }
        public string rtXthDocNo { get; set; }
        public Int64 rnXtdSeqNo { get; set; }
        public string rtPdtCode { get; set; }
        public string rtSrnCode { get; set; }
        public Nullable<DateTime> rdXtdSDate { get; set; }
        public Nullable<decimal> rcPtsCost { get; set; }
    }
}