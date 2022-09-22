using System;

namespace API2PSMaster.Models.WebService.Response.App
{
    /// <summary>
    /// Class model TCNMAppModule
    /// </summary>
    public class cmlResAppModule
    {
        /// <summary>
        ///รหัส Application
        /// </summary>
        public string rtAppCode { get; set; }

        /// <summary>
        ///โมดูล 1:LogIn 2:Drop 3:Pick
        /// </summary>
        public string rtAppModule { get; set; }

        /// <summary>
        ///ลำดับรายการ
        /// </summary>
        public Nullable<int> rnAppSeqNo { get; set; }

        /// <summary>
        ///รูปแบบ LogIn 1:Password 2:Pincode 3:RFID 4:Finger 5:Face
        /// </summary>
        public string rtAppSignType { get; set; }


    }
}