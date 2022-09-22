using API2PSMaster.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.System
{
    //[Serializable]
    public class cmlResSyncDataDwn
    {
        public List<cmlResInfoSyncData> raSyncData { get; set; }
        public List<cmlResInfoSyncDataLng> raSyncDataLng { get; set; }
        public List<cmlResInfoSyncModule> raSyncModule { get; set; } //*Arm 63-07-08
    }
}