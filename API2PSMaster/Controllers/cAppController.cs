using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.App;
using API2PSMaster.Models.WebService.Response.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///  Controller Application
    /// </summary> 
    [ApiController]
    [Route(cCS.tCS_APIVer + "/AppModule")]
    public class cAppController : ControllerBase
    {
        /// <summary>
        ///  Controller Application module
        /// </summary>
        /// <param name="pdDate"></param>
        /// <returns>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        /// </returns>
        [Route("Item/Download")]
        [HttpGet]
        public cmlResItem<cmlResAppitemDwn> GET_DAToDwdAppModuleItem(DateTime pdDate)
        {
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResAppitemDwn> aoResult;
            cmlResAppitemDwn oAppItemDwn;
            cSP oFunc;
            cMS oMsg;
            cCacheFunc oCacheFunc;
            StringBuilder oSql;

            int nCmdTme;
            string tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResAppitemDwn>();
                oAppItemDwn = new cmlResAppitemDwn();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                oFunc = new cSP();
                oMsg = new cMS();

                #region CheckPara
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    aoResult.rtCode = oMsg.tMS_RespCode701;
                    aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return aoResult;
                }
                #endregion

                #region Check API Key
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    aoResult.rtCode = oMsg.tMS_RespCode904;
                    aoResult.rtDesc = oMsg.tMS_RespDesc904;
                    return aoResult;
                }
                #endregion

                #region Check Cache
                tKeyCache = "AppModule" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResAppitemDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }
                #endregion

                aoResult.roItem = new cmlResAppitemDwn();
                oAppItemDwn = new cmlResAppitemDwn();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oSql = new StringBuilder();
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTAppCode AS rtAppCode, FTAppModule AS rtAppModule, ");
                    oSql.AppendLine("FNAppSeqNo AS rnAppSeqNo, FTAppSignType AS rtAppSignType");
                    oSql.AppendLine("FROM TCNMAppModule WITH(NOLOCK)");
                    oAppItemDwn.raTCNMAppModule = oConn.Query<cmlResAppModule>(oSql.ToString(), nCmdTme).ToList();

                    oSql.Clear();
                    oSql.AppendLine("SELECT FTAppCode AS rtAppCode, FTAppVersion AS rtAppVersion, FDLastUpdOn AS rdLastUpdOn, ");
                    oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TSysApp WITH(NOLOCK)");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oAppItemDwn.raTSysApp = oConn.Query<cmlResSysApp>(oSql.ToString(), nCmdTme).ToList();

                    oSql.Clear();
                    oSql.AppendLine("SELECT TSysApp_L.FTAppCode AS rtAppCode, FNLngID AS rnLngID, FTAppName AS rtAppName,  ");
                    oSql.AppendLine("FTAppRmk AS rtAppRmk");
                    oSql.AppendLine("FROM TSysApp_L WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TSysApp WITH(NOLOCK) ON TSysApp_L.FTAppCode=TSysApp.FTAppCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oAppItemDwn.raTSysApp_L = oConn.Query<cmlResSysApp_L>(oSql.ToString(), nCmdTme).ToList();
                }
                aoResult.roItem = oAppItemDwn;

                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                aoResult = new cmlResItem<cmlResAppitemDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();
                return aoResult;
            }
            finally
            {
                oFunc = null;
                aoResult = null;
                oMsg = null;
                oAppItemDwn = null;
            }
        }
    }
}