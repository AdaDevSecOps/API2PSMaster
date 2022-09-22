using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.PdtLayout
{
    public class cmlResPdtLayoutDwn
    {
        public List<cmlResInfoPdtLayout> raPdtLayout { get; set; }
        public List<cmlResInfoPdtLayoutLng> raPdtLayoutLng { get; set; }
    }
}