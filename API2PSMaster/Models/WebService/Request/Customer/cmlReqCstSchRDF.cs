using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    public class cmlReqCstSchRDF
    {
        public string ptSearchCond { get; set; }

        public string ptSearchValue { get; set; }

        public string ptAggCode { get; set; }
    }
}