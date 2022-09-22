using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Role;
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
    /// Manage Document Approve Role.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/DocApvRole")]
    public class cDocApvRoleController : ControllerBase
    {
        /// <summary>
        ///     Download Document Approve Role information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResList<cmlResInfoDocApvRole> GET_DOCoDownloadDocApvRole(DateTime pdDate)
        {
            // *Arm 63-01-17  - Create Function GET_DOCoDownloadDocApvRole()

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoDocApvRole> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoDocApvRole>();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);
                bHaveData = false;

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

                tKeyCache = "DocApvRole" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoDocApvRole>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }
                
                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNDarID AS rnDarID, FTDarTable AS rtDarTable, ");
                oSql.AppendLine("FTDarRefType AS rtDarRefType, FNDarApvSeq AS rnDarApvSeq, ");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, ");
                oSql.AppendLine("FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMDocApvRole with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate ) + "'");
                
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {

                    aoResult.raItems = oConn.Query<cmlResInfoDocApvRole>(oSql.ToString(), nCmdTme).ToList();
                    if (aoResult.raItems.Count > 0)
                    {

                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }

                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch(Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoDocApvRole>();
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