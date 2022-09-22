using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.TxnAPI
{
    public class cmlResTxnAPIDwn
    {
        public List<cmlResInfoTxnAPI> raTCNMTxnAPI { get; set; }
        public List<cmlResInfoTxnAPILng> raTCNMTxnAPILng { get; set; }
        public List<cmlResInfoTxnSpcAPI> raTCNMTxnSpcAPI { get; set; }
    }
}