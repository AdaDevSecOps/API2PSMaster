using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Customer
{
    public class cmlResCstFaceLists
    {
        private string tC_Pic;
        private cSP oC_SP;  

        public string rtFTCstCode { get; set; }
        public int rnFNImgSeq { get; set; }
        public string rtFTImgObj
        {
            get { return tC_Pic; }
            set
            {
                oC_SP = new cSP();
                tC_Pic = oC_SP.SP_PRCtConvertImage2Base64(value);
            }
        }
    }
}