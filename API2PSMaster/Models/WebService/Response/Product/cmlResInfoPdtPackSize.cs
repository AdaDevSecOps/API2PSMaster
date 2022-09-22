using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    /// <summary>
    ///  Product package size information.
    /// </summary>
    //[Serializable]
    public class cmlResInfoPdtPackSize
    {
        public string rtPdtCode { get; set; }
        public string rtPunCode { get; set; }
        public Nullable<decimal> rcPdtUnitFact { get; set; }
        public Nullable<decimal> rcPdtPriceRET { get; set; }
        public Nullable<decimal> rcPdtPriceWHS { get; set; }
        public Nullable<decimal> rcPdtPriceNET { get; set; }
        public string rtPdtGrade { get; set; }
        public Nullable<decimal> rcPdtWeight { get; set; }
        public string rtClrCode { get; set; }
        public string rtPszCode { get; set; }
        public string rtPdtUnitDim { get; set; }
        public string rtPdtPkgDim { get; set; }
        public string rtPdtStaAlwPick { get; set; }
        public string rtPdtStaAlwPoHQ { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtCreateBy { get; set; }
    }
}