using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Role
{
    //[Serializable]
    public class cmlResInfoUsrMenu
    {
        public string rtRolCode { get; set; }
        public string rtGmnCode { get; set; }
        public string rtMnuParent { get; set; }
        public string rtMnuCode { get; set; }
        public string rtAutStaFull { get; set; }
        public string rtAutStaRead { get; set; }
        public string rtAutStaAdd { get; set; }
        public string rtAutStaEdit { get; set; }
        public string rtAutStaDelete { get; set; }
        public string rtAutStaCancel { get; set; }
        public string rtAutStaAppv { get; set; }
        public string rtAutStaPrint { get; set; }
        public string rtAutStaPrintMore { get; set; }
        public string rtAutStaFavorite { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }
    }
}