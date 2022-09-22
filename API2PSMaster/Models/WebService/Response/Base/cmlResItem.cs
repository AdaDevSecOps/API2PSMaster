using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Base
{
    public class cmlResItem<T> : cmlResBase
    {
        public T roItem { get; set; }
    }
}