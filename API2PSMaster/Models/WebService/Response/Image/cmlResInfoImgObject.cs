using System;

namespace API2PSMaster.Models.WebService.Response.Image
{
    public class cmlResInfoImgObject
    {
        public Nullable<int> rnImgID { get; set; }
        public string rtImgRefID { get; set; }
        public Nullable<int> rnImgSeq { get; set; }
        public string rtImgTable { get; set; }
        public string rtImgKey { get; set; }
        public string rtImgObj { get; set; }
        public Nullable<DateTime> rdLastUpdOn { get; set; }
        public string rtLastUpdBy { get; set; }
        public Nullable<DateTime> rdCreateOn { get; set; }
        public string rtCreateBy { get; set; }

    }
}
