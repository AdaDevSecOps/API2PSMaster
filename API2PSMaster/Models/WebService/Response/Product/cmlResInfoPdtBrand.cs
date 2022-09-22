﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    /// <summary>
    ///     Product brand.
    /// </summary>
    //[Serializable]
    public class cmlResInfoPdtBrand
    {
        public string rtPbnCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}