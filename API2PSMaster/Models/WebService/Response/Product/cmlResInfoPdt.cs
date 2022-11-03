using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    /// <summary>
    ///     Product information.
    /// </summary>
    //[Serializable]
    public class cmlResInfoPdt
    {
        public string rtPdtCode { get; set; }
        //public string rtPdtStkCode { get; set; }
        public string rtPdtStkControl { get; set; }
        public string rtPdtGrpControl { get; set; }
        public string rtPdtForSystem { get; set; }
        public decimal rcPdtQtyOrdBuy { get; set; }
        public decimal rcPdtCostDef { get; set; }
        public decimal rcPdtCostOth { get; set; }
        public decimal rcPdtCostStd { get; set; }
        public decimal rcPdtMin { get; set; }
        public decimal rcPdtMax { get; set; }
        public string rtPdtPoint { get; set; }
        public decimal rcPdtPointTime { get; set; }
        public string rtPdtType { get; set; }
        public string rtPdtSaleType { get; set; }
        public string rtPdtSetOrSN { get; set; }
        public string rtPdtStaSetPri { get; set; }
        public string rtPdtStaSetShwDT { get; set; }
        public string rtPdtStaAlwDis { get; set; }
        public string rtPdtStaAlwReturn { get; set; }
        public string rtPdtStaVatBuy { get; set; }
        public string rtPdtStaVat { get; set; }
        public string rtPdtStaActive { get; set; }
        public string rtPdtStaAlwReCalOpt { get; set; }
        public string rtPdtStaCsm { get; set; }
        //public string rtShpCode { get; set; }
        //public string rtPdtRefShop { get; set; }
        public string rtTcgCode { get; set; }
        public string rtPgpChain { get; set; }
        public string rtPtyCode { get; set; }
        public string rtPbnCode { get; set; }
        public string rtPmoCode { get; set; }
        public string rtVatCode { get; set; }
        public string rtEvhCode { get; set; }
        public Nullable<DateTime> rdPdtSaleStart { get; set; }
        public Nullable<DateTime> rdPdtSaleStop { get; set; }
        //public string rtBchCode { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtCreateBy { get; set; }

        /// <summary>
        /// รหัสอ้างอิงสินค้า
        /// </summary>
        public string rtPdtRefID { get; set; }  //*Arm 65-08-11 -[CR-Oversea] 

        /// <summary>
        /// รหัสประเทศ
        /// </summary>
        public string rtCtyCode { get; set; }  //*Arm 65-08-11 -[CR-Oversea] 
    }
}