using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    //[Serializable]
    public class cmlResPosDwn
    {
        public List<cmlResInfoPos> raPos { get; set; }
        public List<cmlResInfoPosLng> raPosLng { get; set; }   //*Arm 63-04-08
        public List<cmlResInfoPosHW> raPosHW { get; set; }
        public List<cmlResInfoPosLastNo> raPosLastNo { get; set; }
        public List<cmlResInfoEdc> raEdc { get; set; }
        public List<cmlResInfoEdcLng> raEdcLng { get; set; }
        public List<cmlResInfoTSysEdc> raTSysEdc { get; set; }
        public List<cmlResInfoPrinter> raPrinter { get; set; }
        public List<cmlResInfoPrinterLng> raPrinterLng { get; set; }
    }
}