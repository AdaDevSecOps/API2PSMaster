using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    public class cmlResPosRegister
    {
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// รหัสเครื่องจุดขาย
        /// </summary>
        public string rtPosCode { get; set; }

        /// <summary>
        /// Mac Address
        /// </summary>
        public string rtMacAddr { get; set; }

        /// <summary>
        /// Computer Name
        /// </summary>
        public string rtCompName { get; set; }

        /// <summary>
        /// สถานะ
        /// </summary>
        public string rtStaApv { get; set; }

        /// <summary>
        /// System process status.
        /// </summary>
        //public string rtCode;
        public string rtCode { get; set; } //*Arm 65-09-09

        /// <summary>
        /// System process description.
        /// </summary>
        //public string rtDesc;
        public string rtDesc { get; set; } //*Arm 65-09-09
    }
}