using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Sale
{
    public class cmlResHotSale
    {
        /// <summary>
        ///รหัสสาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///รหัสเครื่องจุดขาย
        /// <summary>
        public string rtPosCode { get; set; }

        /// <summary>
        ///รหัสสินค้า
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///จำนวนขาย
        /// <summary>
        public Nullable<decimal> rcXsdQty { get; set; }

        /// <summary>
        ///วันที่ขาย
        /// <summary>
        public Nullable<DateTime> rdCreateOn { get; set; }
    }
}