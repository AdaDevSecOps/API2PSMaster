using API2PSMaster.Class;
using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Class.UserRole;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.UserRole;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.UserRole;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    /// Manage User Role.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/UserRol")]
    public class cUserRoleController : ControllerBase
    {
        /// <summary>
        ///     Insert User Department.
        /// </summary>
        /// 
        /// <param name="poPara">User Department information.</param>
        /// 
        /// <returns>
        ///     User Department varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Insert/Item")]
        [HttpPost]
        public cmlResItem<cmlResUsrRolInsItem> POST_PUNoInsUseDepItem([FromBody] cmlReqUsrRolInsItem poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cUserRole oUserRole;
            List<cmlTSysConfig> aoSysConfig;
            cmlResUsrRolInsItem oResUsrErr;
            cmlResItem<cmlResUsrRolInsItem> oResult;
            int nRowEff, nConTme, nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tErrCode, tErrDesc;
            bool bVerifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResUsrRolInsItem>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Check range time use function.
                    if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
                    {
                        tKeyApi = "";
                        // Check KeyApi.
                        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
                        {
                            // Varify parameter value.
                            oUserRole = new cUserRole();
                            bVerifyPara = oUserRole.C_DATbVerifyInsItemParameterValue(poPara, aoSysConfig, out tErrCode, out tErrDesc, out oResUsrErr);
                            if (bVerifyPara == true)
                            {
                                // Insert.
                                oSql = new StringBuilder();
                                oSql.AppendLine("INSERT INTO TCNMUsrRole WITH(ROWLOCK)");
                                oSql.AppendLine("(");
                                oSql.AppendLine("	FTRolCode,FNRolLevel,");
                                oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd,");
                                oSql.AppendLine("	FDDateIns, FTTimeIns, FTWhoIns");
                                oSql.AppendLine(")");
                                oSql.AppendLine("VALUES");
                                oSql.AppendLine("(");
                                oSql.AppendLine("	'" + poPara.ptRolCode + "'," + poPara.pnRolLevel+ ",");
                                oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), 'AdaLink',");
                                oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), 'AdaLink'");
                                oSql.AppendLine(")");

                                try
                                {
                                    // Confuguration database.
                                    nConTme = 0;
                                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "003");
                                    nCmdTme = 0;
                                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "004");

                                    oDatabase = new cDatabase(nConTme);
                                    nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                                }
                                catch (SqlException oSqlExn)
                                {
                                    switch (oSqlExn.Number)
                                    {
                                        case 2627:
                                            // Data is duplicate.
                                            oResult.roItem = new cmlResUsrRolInsItem();
                                            oResult.roItem.rtRolCode = poPara.ptRolCode;
                                            oResult.rtCode = oMsg.tMS_RespCode801;
                                            oResult.rtDesc = oMsg.tMS_RespDesc801 + " (User Role).";
                                            return oResult;
                                    }
                                }
                                catch (EntityException oEtyExn)
                                {
                                    switch (oEtyExn.HResult)
                                    {
                                        case -2146232060:
                                            // Cannot connect database..
                                            oResult.rtCode = oMsg.tMS_RespCode905;
                                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                                            return oResult;
                                    }
                                }
                                oResult.rtCode = oMsg.tMS_RespCode001;
                                oResult.rtDesc = oMsg.tMS_RespDesc001;
                                return oResult;
                            }
                            else
                            {
                                // Varify parameter value false.
                                oResult.roItem = oResUsrErr;
                                oResult.rtCode = tErrCode;
                                oResult.rtDesc = tErrDesc;
                                return oResult;
                            }

                        }
                        else
                        {
                            // Key not allowed to use method.
                            oResult.rtCode = oMsg.tMS_RespCode904;
                            oResult.rtDesc = oMsg.tMS_RespDesc904;
                            return oResult;
                        }
                    }
                    else
                    {
                        // This time not allowed to use method.
                        oResult.rtCode = oMsg.tMS_RespCode906;
                        oResult.rtDesc = oMsg.tMS_RespDesc906;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResUsrRolInsItem>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                aoSysConfig = null;
                oResUsrErr = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Update User.
        /// </summary>
        /// 
        /// <param name="poPara">User information.</param>
        /// 
        /// <returns>
        ///     Product varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     802 : formate data incorrect..<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Update/Item")]
        [HttpPost]
        public cmlResItem<cmlResUsrRolUpdItem> POST_PUNoUpdateUsrItem([FromBody] cmlReqUsrRolUpdItem poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cUserRole oUserRole;
            cDatabase oDatabase;
            StringBuilder oSql;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResUsrRolUpdItem> oResult;
            cmlResUsrRolUpdItem oResPgpErr;
            int nRowEff, nConTme, nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tErrCode, tErrDesc;
            bool bVarifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResUsrRolUpdItem>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Check range time use function.
                    if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
                    {
                        tKeyApi = "";
                        // Check KeyApi.
                        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
                        {
                            oUserRole = new cUserRole();
                            bVarifyPara = oUserRole.C_DATbVerifyUpdItemParameterValue(poPara, aoSysConfig, out tErrCode, out tErrDesc, out oResPgpErr);
                            if (bVarifyPara == true)
                            {
                                // Update.
                                oSql = new StringBuilder();
                                oSql.AppendLine("UPDATE TCNMUsrRole WITH(ROWLOCK) SET ");
                                oSql.AppendLine("FNRolLevel='" + poPara.pnRolLevel + "',");
                                oSql.AppendLine("FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),");
                                oSql.AppendLine("FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),114),");
                                oSql.AppendLine("FTWhoUpd = 'AdaLink'");
                                oSql.AppendLine("WHERE FTROlCode = '" + poPara.ptRolCode + "'");

                                try
                                {
                                    // Confuguration database.
                                    nConTme = 0;
                                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "003");
                                    nCmdTme = 0;
                                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "004");

                                    oDatabase = new cDatabase(nConTme);
                                    nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                                    if (nRowEff == 0)
                                    {
                                        // Data not  found.
                                        oResult.rtCode = oMsg.tMS_RespCode800;
                                        oResult.rtDesc = oMsg.tMS_RespDesc800 + "(User Role)";
                                        return oResult;
                                    }
                                }
                                catch (EntityException oEtyExn)
                                {
                                    switch (oEtyExn.HResult)
                                    {
                                        case -2146232060:
                                            // Cannot connect database..
                                            oResult.rtCode = oMsg.tMS_RespCode905;
                                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                                            return oResult;
                                    }
                                }

                                // Success.
                                oResult.rtCode = oMsg.tMS_RespCode001;
                                oResult.rtDesc = oMsg.tMS_RespDesc001;
                                return oResult;
                            }
                            else
                            {
                                // Varify parameter value false.
                                oResult.roItem = oResPgpErr;
                                oResult.rtCode = tErrCode;
                                oResult.rtDesc = tErrDesc;
                                return oResult;
                            }

                        }
                        else
                        {
                            // Key not allowed to use method.
                            oResult.rtCode = oMsg.tMS_RespCode904;
                            oResult.rtDesc = oMsg.tMS_RespDesc904;
                            return oResult;
                        }
                    }
                    else
                    {
                        // This time not allowed to use method.
                        oResult.rtCode = oMsg.tMS_RespCode906;
                        oResult.rtDesc = oMsg.tMS_RespDesc906;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResUsrRolUpdItem>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                aoSysConfig = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Download user role information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResUserRoleDwn> GET_PDToDownloadUserRole(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResUserRoleDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResUserRoleDwn oUserRoleDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResUserRoleDwn>();
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

                //tKeyCache = "UserRole" + string.Format("{0:yyyyMMdd}", pdDate);
                tKeyCache = "UsrActRole" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResUserRoleDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRolCode AS rtRolCode, ISNULL(FNRolLevel,1) AS rnRolLevel,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMUsrRole with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRolCode AS rtRolCode, FTUsrCode AS rtUsrCode, FDLastUpdOn AS rdLastUpdOn, ");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMUsrActRole WITH(NOLOCK)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                
                //aoResult.roItem = new cmlResUserRoleDwn();
                //oUserRoleDwn = new cmlResUserRoleDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            //oUserRoleDwn.raUserRole = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUserRole>(oDR).ToList();
                //            oUserRoleDwn.raUsrActRole = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUsrActRole>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oUserRoleDwn.raUsrActRole.Count > 0)
                //        {
                //            ////Languague
                //            //oSql = new StringBuilder();
                //            //oSql.AppendLine("SELECT TCNMUsrRole_L.FTRolCode AS rtRolCode, TCNMUsrRole_L.FNLngID AS rnLngID,");
                //            //oSql.AppendLine("TCNMUsrRole_L.FTRolName AS rtRolName, TCNMUsrRole_L.FTRolRmk AS rtRolRmk");
                //            //oSql.AppendLine("FROM TCNMUsrRole_L with(nolock)");
                //            //oSql.AppendLine("INNER JOIN TCNMUsrRole with(nolock) ON TCNMUsrRole_L.FTRolCode = TCNMUsrRole.FTRolCode");
                //            //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUsrRole.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            //oCmd.CommandText = oSql.ToString();
                //            //using (DbDataReader oDR = oCmd.ExecuteReader())
                //            //{
                //            //    oUserRoleDwn.raUserRoleLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUserRoleLng>(oDR).ToList();
                //            //    ((IDisposable)oDR).Dispose();
                //            //}
                //        }
                //        else
                //        {
                //            aoResult.rtCode = oMsg.tMS_RespCode800;
                //            aoResult.rtDesc = oMsg.tMS_RespDesc800;
                //            return aoResult;
                //        }
                //    }
                //}
                //*Net 64-10-19 ปรับไปใช้ Dapper
                oDB = new cDatabase();
                aoResult.roItem = new cmlResUserRoleDwn();
                oUserRoleDwn = new cmlResUserRoleDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRolCode AS rtRolCode, FTUsrCode AS rtUsrCode, FDLastUpdOn AS rdLastUpdOn, ");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMUsrActRole WITH(NOLOCK)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oUserRoleDwn.raUsrActRole = oDB.C_DATaSqlQuery<cmlResInfoUsrActRole>(oSql.ToString());
                if (oUserRoleDwn.raUsrActRole.Count > 0)
                {
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oUserRoleDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResUserRoleDwn>();
                //aoResult = new cmlResPdtItemDwn();
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