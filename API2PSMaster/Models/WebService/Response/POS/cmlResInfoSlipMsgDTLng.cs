﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResInfoSlipMsgDTLng
    {
        public string rtSmgCode { get; set; }
        public Int64 rnLngID { get; set; }
        public string rtSmgType { get; set; }
        public int rnSmgSeq { get; set; }
        public string rtSmgName { get; set; }
    }
}