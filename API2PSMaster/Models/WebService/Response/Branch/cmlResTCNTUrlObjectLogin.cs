using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Branch
{
    public class cmlResTCNTUrlObjectLogin
    {
        /// <summary>
        ///รหัส URL
        /// <summary>
        public string rtUrlRefID { get; set; }

        /// <summary>
        ///URL Address
        /// <summary>
        public string rtUrlAddress { get; set; }

        /// <summary>
        ///Virtual Host
        /// <summary>
        public string rtUolVhost { get; set; }

        /// <summary>
        ///ชื่อเข้าใช้งาน
        /// <summary>
        public string rtUolUser { get; set; }

        /// <summary>
        ///รหัสผ่าน
        /// <summary>
        public string rtUolPassword { get; set; }

        /// <summary>
        ///ล็อกอินคีย์
        /// <summary>
        public string rtUolKey { get; set; }

        /// <summary>
        ///สถานะใช้งาน 1:Active 2:Disable
        /// <summary>
        public string rtUolStaActive { get; set; }

        /// <summary>
        ///หมายเหตุ
        /// <summary>
        public string rtUolgRmk { get; set; }
    }
}