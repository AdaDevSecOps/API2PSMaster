using API2PSMaster.Class;
using API2PSMaster.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.POS
{
    public class resTCNMMediaObj
    {
        private string tC_MediaPath;
        public Int64 rnMedID { get; set; }
        public string rtMedRefID { get; set; }
        public int rnMedSeq { get; set; }
        public int rnMedType { get; set; }
        public string rtMedFileType { get; set; }
        public string rtMedTable { get; set; }
        public string rtMedKey { get; set; }
        public string rtMedPath
        {
            get { return tC_MediaPath; }
            set
            {
                //*Ton 64-05-22 ยกเลิกการอัพเดท fullpath ใน property
                //cPosAdvMsgController oPosAdvMsg = new cPosAdvMsgController();
                //tC_MediaPath = oPosAdvMsg.C_PRCtPrepareFile(value);
                tC_MediaPath = value;
            }
        }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtCreateBy { get; set; }
    }
}