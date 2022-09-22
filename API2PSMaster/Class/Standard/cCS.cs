using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Class.Standard
{
    public class cCS
    {
        // Version Main API.
        public const string tCS_APIVer = "V4";

        public const string tCS_AESKey = "R7Yox/-iuH4aG=df"; // Key of more+
        public const string tCS_AESIV = "!kQm*fF3pXe1Kbm%";  // IV of more+
        public const string tCS_SHA1Key1 = "AdaSoft";
        public const string tCS_SHA1Key2 = "SOFTXada";

        // Paht AdaImage.
        public const string tCS_PathImg = @"C:\Program Files (x86)\AdaSoft\AdaPos4.0HpmFhn\AdaImage\";

        // Message attribute model.
        public const string tCS_MsgAtrRequired = "The {0} field is required.";
        public const string tCS_MsgAtrMaxLength = "{0} cannot be greater than {1}.";
        public const string tCS_MsgAtrArrayMaxLength = "{0} size cannot be greater than {1}.";

        // Default configuration database.
        public const int nCS_ConTme = 30;
        public const int nCS_CmdTme = 30;
        public const int nCS_BcpTme = 60;
    }
}