using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Company
{
    [Serializable]
    public class cmlResInfoComp
    {
        public string rtCmpCode { get; set; }
        public string rtCmpTel { get; set; }
        public string rtCmpFax { get; set; }
        public string rtBchcode { get; set; }
        public string rtCmpWhsInOrEx { get; set; }
        public string rtCmpRetInOrEx { get; set; }
        public string rtCmpEmail { get; set; }
        public string rtRteCode { get; set; }
        public string rtVatCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public string rtCreateBy { get; set; }

        /// <summary>
        /// รหัสประเทศ
        /// </summary>
        public string rtCtyCode { get; set; }  //*Arm 65-08-16 [CR-Oversea] เพิ่มรหัสประเทศ
    }
}