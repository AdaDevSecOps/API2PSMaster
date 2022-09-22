using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Interface;
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
    ///     Download Data Interface.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/DataInterface")]
    public class cInterfaceController : ControllerBase
    {
        /// <summary>
        /// Interface WaHouse
        /// </summary>
        /// <param name="pdDate"></param>
        /// <returns></returns>
        [Route("WaHouse/Download")]
        [HttpGet]
        public cmlResList<cmlResLinkWahouseInfo> GET_DAToDownloadLinkWahouse(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResLinkWahouseInfo> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResLinkWahouseInfo>();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

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
                
                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTAgnCode AS rtAgnCode, FTBchCode AS rtBchCode, FTWahCode AS rtWahCode, FTWahRefNo AS rtWahRefNo, FTWahStaChannel AS rtWahStaChannel,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy, ");
                //oSql.AppendLine("FTWahRefRemark AS rtWahRefRemark,	FTWahRemark AS rtWahRemark");
                oSql.AppendLine("FTWahRemark AS rtWahRemark"); //*Arm 64-01-23
                oSql.AppendLine("FROM TLKMWaHouse WITH(NOLOCK) ");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.raItems = new List<cmlResLinkWahouseInfo>();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase()) 
                {
                    aoResult.raItems = oConn.Query<cmlResLinkWahouseInfo>(oSql.ToString(), nCmdTme).ToList();
                    if(aoResult.raItems.Count>0)
                    {
                        //
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResLinkWahouseInfo>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();
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