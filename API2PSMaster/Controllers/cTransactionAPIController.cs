using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.TxnAPI;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///Transaction API information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/TxnAPI")]
    public class cTransactionAPIController : ControllerBase
    {
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResTxnAPIDwn> GET_TXNoDownloadTxnAPI(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResTxnAPIDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResTxnAPIDwn oTxnAPIDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResTxnAPIDwn>();
                //aoResult = new cmlResPdtItemDwn();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(21600, 21600, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    aoResult.rtCode = oMsg.tMS_RespCode701;
                    aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return aoResult;
                }
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

                tKeyCache = "TxnAPI" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResTxnAPIDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTApiCode AS rtApiCode, FTApiTxnType AS rtApiTxnType, FTApiPrcType AS rtApiPrcType, FTApiGrpPrc AS rtApiGrpPrc, FNApiGrpSeq AS rnApiGrpSeq, ");
                oSql.AppendLine("FTApiFmtCode AS rtApiFmtCode, FTApiURL AS rtApiURL, FTApiLoginUsr AS rtApiLoginUsr, FTApiLoginPwd AS rtApiLoginPwd, FTApiToken AS rtApiToken, ");
                oSql.AppendLine("FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy, FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy ");
                oSql.AppendLine("From TCNMTxnAPI WITH(NOLOCK) ");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //SELECT FTApiCode AS rtApiCode, FTApiTxnType AS rtApiTxnType, FTApiPrcType AS rtApiPrcType, FTApiGrpPrc AS rtApiGrpPrc, FNApiGrpSeq AS rnApiGrpSeq,
                //FTApiFmtCode AS rtApiFmtCode, FTApiURL AS rtApiURL, FTApiLoginUsr AS rtApiLoginUsr, FTApiLoginPwd AS rtApiLoginPwd, FTApiToken AS rtApiToken,
                //FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy, FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy
                //From TCNMTxnAPI WITH(NOLOCK)
                //WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= ''

                aoResult.roItem = new cmlResTxnAPIDwn();
                oTxnAPIDwn = new cmlResTxnAPIDwn();

                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oTxnAPIDwn.raTCNMTxnAPI = oConn.Query<cmlResInfoTxnAPI>(oSql.ToString(), nCmdTme).ToList();
                    if (oTxnAPIDwn.raTCNMTxnAPI.Count > 0)
                    {
                        //SELECT TCNMTxnAPI_L.FTApiCode AS rtApiCode, TCNMTxnAPI_L.FNLngID AS rnLngID, TCNMTxnAPI_L.FTApiName AS rtApiName, TCNMTxnAPI_L.FTApiRmk AS rtApiRmk
                        //FROM TCNMTxnAPI_L WITH(NOLOCK)
                        //INNER JOIN TCNMTxnAPI ON TCNMTxnAPI_L.FTApiCode = TCNMTxnAPI.FTApiCode
                        //WHERE CONVERT(VARCHAR(10),TCNMTxnAPI.FDLastUpdOn,121) >= '2020-06-15'  
                        
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMTxnAPI_L.FTApiCode AS rtApiCode, TCNMTxnAPI_L.FNLngID AS rnLngID, TCNMTxnAPI_L.FTApiName AS rtApiName, TCNMTxnAPI_L.FTApiRmk AS rtApiRmk ");
                        oSql.AppendLine("FROM TCNMTxnAPI_L WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TCNMTxnAPI WITH(NOLOCK) ON TCNMTxnAPI_L.FTApiCode = TCNMTxnAPI.FTApiCode ");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMTxnAPI.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                        oTxnAPIDwn.raTCNMTxnAPILng = oConn.Query<cmlResInfoTxnAPILng>(oSql.ToString(), nCmdTme).ToList();



                        //SELECT DISTINCT TCNMTxnSpcAPI.FTApiCode AS rtApiCode, TCNMTxnSpcAPI.FTCmpCode AS rtCmpCode, TCNMTxnSpcAPI.FTAgnCode AS rtAgnCode, TCNMTxnSpcAPI.FTBchCode AS rtBchCode, TCNMTxnSpcAPI.FTMerCode AS rtMerCode,
                        //TCNMTxnSpcAPI.FTShpCode AS rtShpCode, TCNMTxnSpcAPI.FTPosCode AS rtPosCode, TCNMTxnSpcAPI.FTApiFmtCode AS rtApiFmtCode, TCNMTxnSpcAPI.FTApiURL AS rtApiURL, TCNMTxnSpcAPI.FTSpaUsrCode AS rtSpaUsrCode,
                        //TCNMTxnSpcAPI.FTSpaUsrPwd AS rtSpaUsrPwd, TCNMTxnSpcAPI.FTSpaApiKey AS rtSpaApiKey, TCNMTxnSpcAPI.FTSpaRmk AS rtSpaRmk,
                        //TCNMTxnSpcAPI.FDCreateOn AS rdCreateOn, TCNMTxnSpcAPI.FTCreateBy AS rtCreateBy, TCNMTxnSpcAPI.FDLastUpdOn AS rdLastUpdOn, TCNMTxnSpcAPI.FTLastUpdBy AS rtLastUpdBy
                        //From TCNMTxnSpcAPI WITH(NOLOCK)
                        //INNER JOIN TCNMTxnAPI ON TCNMTxnSpcAPI.FTApiCode = TCNMTxnAPI.FTApiCode
                        //WHERE CONVERT(VARCHAR(10),TCNMTxnAPI.FDLastUpdOn,121) >= '2020-06-15'
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMTxnSpcAPI.FTApiCode AS rtApiCode, TCNMTxnSpcAPI.FTCmpCode AS rtCmpCode, TCNMTxnSpcAPI.FTAgnCode AS rtAgnCode, TCNMTxnSpcAPI.FTBchCode AS rtBchCode, TCNMTxnSpcAPI.FTMerCode AS rtMerCode, ");
                        oSql.AppendLine("TCNMTxnSpcAPI.FTShpCode AS rtShpCode, TCNMTxnSpcAPI.FTPosCode AS rtPosCode, TCNMTxnSpcAPI.FTApiFmtCode AS rtApiFmtCode, TCNMTxnSpcAPI.FTApiURL AS rtApiURL, TCNMTxnSpcAPI.FTSpaUsrCode AS rtSpaUsrCode, ");
                        oSql.AppendLine("TCNMTxnSpcAPI.FTSpaUsrPwd AS rtSpaUsrPwd, TCNMTxnSpcAPI.FTSpaApiKey AS rtSpaApiKey, TCNMTxnSpcAPI.FTSpaRmk AS rtSpaRmk, ");
                        oSql.AppendLine("TCNMTxnSpcAPI.FDCreateOn AS rdCreateOn, TCNMTxnSpcAPI.FTCreateBy AS rtCreateBy, TCNMTxnSpcAPI.FDLastUpdOn AS rdLastUpdOn, TCNMTxnSpcAPI.FTLastUpdBy AS rtLastUpdBy ");
                        oSql.AppendLine("From TCNMTxnSpcAPI WITH(NOLOCK) ");
                        oSql.AppendLine("INNER JOIN TCNMTxnAPI WITH(NOLOCK) ON TCNMTxnSpcAPI.FTApiCode = TCNMTxnAPI.FTApiCode ");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMTxnAPI.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                        oTxnAPIDwn.raTCNMTxnSpcAPI = oConn.Query<cmlResInfoTxnSpcAPI>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oTxnAPIDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            
            }
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResTxnAPIDwn>();
                //aoResult = new cmlResPdtItemDwn();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oEx.Message.ToString();
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oCS = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}