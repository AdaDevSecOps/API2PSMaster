using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Merchant
{
    public class cmlTCNMMerchant_L
    {
        /// <summary>
        ///รหัสคู้ค้า
        /// <summary>
        public string rtMerCode { get; set; }

        /// <summary>
        ///รหัสภาษา
        /// <summary>
        public Nullable<Int64> rnLngID { get; set; }

        /// <summary>
        ///ชื่อ
        /// <summary>
        public string rtMerName { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtMerRmk { get; set; }
    }
}