using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Base
{
    public class cmlResult<Model>
    {
        public string tMsg { get; set; }
        public IEnumerable<Model> aItems { get; set; }
        public Model oItem { get; set; }
        public int nCount { get; set; }
    }
}