using API2PSMaster.Class;
using API2PSMaster.Class.Online;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Online;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    /// Information Class online
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/CheckOnline")]
    public class cOnlineController : ControllerBase
    {
        /// <summary>
        /// Is online ตรวจสอบ connection
        /// </summary>
        /// <returns>
        /// System process status.<br/>
        ///&#8195;     1   : success.<br/>    
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("IsOnline")]
        [HttpGet]
        public cmlResIsOnline POST_CHKoIsOnline()
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cIsOnline oIsOnline;
            List<cmlTSysConfig> aoSysConfig;
            cmlResIsOnline oResIsOnlineErr;
            cmlResIsOnline oResult;
            string tErrCode, tErrDesc;
            bool bVerifyPara;
            string tDBName = "";
            cDatabase oDatabase;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResIsOnline();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                // Varify parameter value.
                oIsOnline = new cIsOnline();
                bVerifyPara = oIsOnline.C_DATbVerifyIsOnline(aoSysConfig, out tErrCode, out tErrDesc, out oResIsOnlineErr);
                if (bVerifyPara == true)
                {
                    oDatabase = new cDatabase();
                    tDBName = new SqlConnection(oDatabase.C_CONoDatabase().ConnectionString).Database ;
                    oResult.rtResult = tDBName + " :Ready";
                    oResult.rtCode = oMsg.tMS_RespCode001;
                    oResult.rtDesc = oMsg.tMS_RespDesc001;
                    return oResult;
                }
                else
                {
                    // Varify parameter value false.
                    oResult.rtCode = tErrCode;
                    oResult.rtDesc = tErrDesc;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResIsOnline();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                aoSysConfig = null;
                oResIsOnlineErr = null;
                oResult = null;
                oResult = null;
                oDatabase = null;
                tDBName = null;
            }
        }
    }
}