using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Product
{
    public class cmlResTCNMPdtSpcBch
    {
        /// <summary>
        ///รหัสสินค้า/รหัสควบคุมสต๊อก
        /// <summary>
        public string rtPdtCode { get; set; }

        /// <summary>
        ///สาขา
        /// <summary>
        public string rtBchCode { get; set; }

        /// <summary>
        ///รหัสตัวแทน/เจ้าของดำเนินการ
        /// <summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสกลุ่ม ตาม Merchant
        /// <summary>
        public string rtMgpCode { get; set; }

        /// <summary>
        ///จำนวนคงเหลือต่ำสุด
        /// <summary>
        public Nullable<decimal> rcPdtMin { get; set; }

        /// <summary>
        ///รหัสร้านค้า
        /// <summary>
        public string rtShpCode { get; set; }

        /// <summary>
        ///หมายเหตุ (Refer external)
        /// <summary>
        public string rtPdtRmk { get; set; }
    }
}