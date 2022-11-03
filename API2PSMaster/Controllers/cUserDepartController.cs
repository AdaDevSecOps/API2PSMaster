using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.UserDepart;
using API2PSMaster.Models.WebService.Request.Depart;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Core;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     User department Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/UserDepart")]
    public class cUserDepartController : ControllerBase
    {
        /// <summary>
        ///     Download sub district information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResUsrDepDwn> GET_PDToDownloadUserDepart(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResUsrDepDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResUsrDepDwn oPvnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResUsrDepDwn>();
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

                tKeyCache = "UserDepart" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResUsrDepDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTDptCode AS rtDptCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMUsrDepart with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResUsrDepDwn();
                //oPvnDwn = new cmlResUsrDepDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPvnDwn.raUsrDep = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUsrDep>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPvnDwn.raUsrDep.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMUsrDepart_L.FTDptCode AS rtDptCode, TCNMUsrDepart_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMUsrDepart_L.FTDptName AS rtDptName, TCNMUsrDepart_L.FTDptRmk AS rtDptRmk");
                //            oSql.AppendLine("FROM TCNMUsrDepart_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMUsrDepart with(nolock) ON TCNMUsrDepart_L.FTDptCode = TCNMUsrDepart.FTDptCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUsrDepart.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPvnDwn.raUsrDepLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUsrDepLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
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
                aoResult.roItem = new cmlResUsrDepDwn();
                oPvnDwn = new cmlResUsrDepDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTDptCode AS rtDptCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMUsrDepart with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPvnDwn.raUsrDep = oDB.C_DATaSqlQuery<cmlResInfoUsrDep>(oSql.ToString());
                if (oPvnDwn.raUsrDep.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMUsrDepart_L.FTDptCode AS rtDptCode, TCNMUsrDepart_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMUsrDepart_L.FTDptName AS rtDptName, TCNMUsrDepart_L.FTDptRmk AS rtDptRmk");
                    oSql.AppendLine("FROM TCNMUsrDepart_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMUsrDepart with(nolock) ON TCNMUsrDepart_L.FTDptCode = TCNMUsrDepart.FTDptCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUsrDepart.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPvnDwn.raUsrDepLng = oDB.C_DATaSqlQuery<cmlResInfoUsrDepLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oPvnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResUsrDepDwn>();
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

        //[Route("Insert")]
        //[HttpPost]
        //public cmlResItem<cmlResUsrDepDwn>POST_PDToInsUserDepart(cmlReqDepart poparam)
        //{
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    StringBuilder oSql;
        //    StringBuilder oSql_L;
        //    cmlResItem<cmlResUsrDepDwn> aoResult;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cCacheFunc oCacheFunc;
        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache;

        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //        oFunc = new cSP();
        //        oMsg = new cMS();
        //        oCS = new cCS();

        //        // Get method name.
        //        tFuncName = MethodBase.GetCurrentMethod().Name;

        //        // Validate parameter.
        //        tModelErr = "";
        //        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
        //        {
        //            // Validate parameter model false.
        //            aoResult.rtCode = oMsg.tMS_RespCode701;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
        //            return aoResult;
        //        }

        //        // Load configuration.
        //        aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
        //        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

        //        tKeyApi = "";
        //        // Check KeyApi.
        //        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
        //        {
        //            // Key not allowed to use method.
        //            aoResult.rtCode = oMsg.tMS_RespCode904;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc904;
        //            return aoResult;
        //        }

        //        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
        //        {
        //            oSql = new StringBuilder();
        //            oSql.Clear();
        //            oSql.AppendLine(" INSERT ");
        //            oSql.AppendLine(" INTO TCNMUsrDepart WITH(ROWLOCK) ");
        //            oSql.AppendLine(" (FTDptCode, FDDateUpd, FTTimeUpd, ");
        //            oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns) ");
        //            oSql.AppendLine(" VALUES (");
        //            oSql.AppendLine(" '" + poparam.ptDptCode + "',");
        //            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121) ,");
        //            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
        //            oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
        //            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121), ");
        //            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108), ");
        //            oSql.AppendLine(" '" + poparam.ptWhoUpd + "') ");


        //            oSql_L = new StringBuilder();
        //            oSql_L.Clear();
        //            oSql_L.AppendLine(" INSERT ");
        //            oSql_L.AppendLine(" INTO");
        //            oSql_L.AppendLine(" TCNMUsrDepart_L WITH(ROWLOCK)");
        //            oSql_L.AppendLine(" (FTDptCode, FNLngID, FTDptName,FTDptRmk)");
        //            oSql_L.AppendLine(" VALUES");
        //            oSql_L.AppendLine(" ('" + poparam.ptDptCode + "',");
        //            oSql_L.AppendLine(" '" + poparam.pnLngID + "',");
        //            oSql_L.AppendLine(" '" + poparam.ptDptName + "',");
        //            oSql_L.AppendLine("'" + poparam.ptDptRmk + "')");


        //            using (DbConnection oConn = oAdaAcc.Database.Connection)
        //            {
        //                oConn.Open();
        //                DbCommand oCmd = oConn.CreateCommand();
        //                DbTransaction oTrans;
        //                oTrans = oConn.BeginTransaction();
        //                oCmd.Connection = oConn;
        //                oCmd.Transaction = oTrans;
        //                try
        //                {
        //                    oCmd.CommandTimeout = nCmdTme;
        //                    oCmd.CommandType = CommandType.Text;
        //                    oCmd.CommandText = oSql.ToString();
        //                    oCmd.ExecuteNonQuery();

        //                    oCmd.CommandTimeout = nCmdTme;
        //                    oCmd.CommandType = CommandType.Text;
        //                    oCmd.CommandText = oSql_L.ToString();
        //                    oCmd.ExecuteNonQuery();

        //                    oTrans.Commit();
        //                }
        //                catch (SqlException oSqlExn)
        //                {
        //                    oTrans.Rollback();
        //                    switch (oSqlExn.Number)
        //                    {
        //                        case 2627:
        //                            // Data is duplicate.
        //                            aoResult.rtCode = oMsg.tMS_RespCode801;
        //                            aoResult.rtDesc = oMsg.tMS_RespDesc801;
        //                            return aoResult;

        //                        default:
        //                            //Error statement or sql error
        //                            aoResult.rtCode = oMsg.tMS_RespCode999;
        //                            aoResult.rtDesc = oSqlExn.Message;
        //                            return aoResult;
        //                    }
        //                }
        //                catch (EntityException oEtyExn)
        //                {
        //                    oTrans.Rollback();
        //                    switch (oEtyExn.HResult)
        //                    {
        //                        case -2146232060:
        //                            // Cannot connect database..
        //                            aoResult.rtCode = oMsg.tMS_RespCode905;
        //                            aoResult.rtDesc = oMsg.tMS_RespDesc905;
        //                            return aoResult;
        //                    }
        //                }
        //            }
        //        }
        //        aoResult.rtCode = oMsg.tMS_RespCode001;
        //        aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //        return aoResult;

        //    }
        //    catch (Exception oExn)
        //    {
        //        // Return error.
        //        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //        aoResult.rtCode = new cMS().tMS_RespCode900;
        //        aoResult.rtDesc = new cMS().tMS_RespDesc900;
        //        return aoResult;
        //    }
        //    finally
        //    {
        //        oFunc = null;
        //        oMsg = null;
        //        oSql = null;

        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        //GC.Collect();
        //    }

        //}

        //[Route("Update")]
        //[HttpPost]
        //public cmlResItem<cmlResUsrDepDwn> POST_PDToUptUserDepart(cmlReqDepart poparam)
        //{
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    StringBuilder oSql;
        //    StringBuilder oSql_M;
        //    StringBuilder oSql_L;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResItem<cmlResUsrDepDwn> aoResult;
        //    cDatabase oDatabase;
        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache;
        //    DataTable oDbTblDpt;
        //    DataTable oDbTblDpt_L;

        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //        oFunc = new cSP();
        //        oMsg = new cMS();
        //        oCS = new cCS();


        //        // Get method name.
        //        tFuncName = MethodBase.GetCurrentMethod().Name;

        //        // Validate parameter.
        //        tModelErr = "";
        //        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
        //        {
        //            // Validate parameter model false.
        //            aoResult.rtCode = oMsg.tMS_RespCode701;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
        //            return aoResult;
        //        }

        //        // Load configuration.
        //        aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
        //        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

        //        tKeyApi = "";
        //        // Check KeyApi.
        //        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
        //        {
        //            // Key not allowed to use method.
        //            aoResult.rtCode = oMsg.tMS_RespCode904;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc904;
        //            return aoResult;
        //        }


        //        oDatabase = new cDatabase();
        //        oSql = new StringBuilder();
        //        oSql.AppendLine("SELECT FTDptCode");
        //        oSql.AppendLine("FROM TCNMUsrDepart WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTDptCode = '" + poparam.ptDptCode + "'");
        //        oDbTblDpt = oDatabase.C_DAToSqlQuery(oSql.ToString());

        //        oSql = new StringBuilder();
        //        oSql.AppendLine(" SELECT TOP(1)   FTDptCode, FNLngID, FTDptName, FTDptRmk");
        //        oSql.AppendLine(" FROM TCNMUsrDepart_L");
        //        oSql.AppendLine(" WHERE ");
        //        oSql.AppendLine(" FTDptCode = '" + poparam.ptDptCode + "' AND ");
        //        oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
        //        oDbTblDpt_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

        //        if (oDbTblDpt != null && oDbTblDpt.Rows.Count > 0)
        //        {
        //            if (oDbTblDpt_L != null && oDbTblDpt_L.Rows.Count > 0)
        //            {
        //                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
        //                {
        //                    oSql_M = new StringBuilder();
        //                    oSql_M.Clear();
        //                    oSql_M.AppendLine(" UPDATE");
        //                    oSql_M.AppendLine(" TCNMUsrDepart ");
        //                    oSql_M.AppendLine(" WITH(ROWLOCK)");
        //                    oSql_M.AppendLine(" SET FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121), ");
        //                    oSql_M.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108), ");
        //                    oSql_M.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd +"'");


        //                    oSql_L = new StringBuilder();
        //                    oSql_L.Clear();
        //                    oSql_L.AppendLine(" UPDATE ");
        //                    oSql_L.AppendLine(" TCNMUsrDepart_L ");
        //                    oSql_L.AppendLine(" SET FTDptName = '" + poparam.ptDptName + "'");
        //                    oSql_L.AppendLine(" WHERE FTDptCode = '" + poparam.ptDptCode + "' ");
        //                    oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");

        //                    using (DbConnection oConn = oAdaAcc.Database.Connection)
        //                    {
        //                        oConn.Open();
        //                        DbCommand oCmd = oConn.CreateCommand();
        //                        DbTransaction oTrans;
        //                        oTrans = oConn.BeginTransaction();
        //                        oCmd.Connection = oConn;
        //                        oCmd.Transaction = oTrans;
        //                        try
        //                        {
        //                            oCmd.CommandTimeout = nCmdTme;
        //                            oCmd.CommandType = CommandType.Text;
        //                            oCmd.CommandText = oSql.ToString();
        //                            oCmd.ExecuteNonQuery();

        //                            oCmd.CommandTimeout = nCmdTme;
        //                            oCmd.CommandType = CommandType.Text;
        //                            oCmd.CommandText = oSql_M.ToString();
        //                            oCmd.ExecuteNonQuery();

        //                            oCmd.CommandTimeout = nCmdTme;
        //                            oCmd.CommandType = CommandType.Text;
        //                            oCmd.CommandText = oSql_L.ToString();
        //                            int nSuccess = oCmd.ExecuteNonQuery();
        //                            if (nSuccess == 0)
        //                            {
        //                                aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //                                //aoResult = new cmlResPdtItemDwn();
        //                                aoResult.rtCode = "";
        //                                aoResult.rtDesc = "";
        //                                return aoResult;
        //                            }

        //                            oTrans.Commit();
        //                        }
        //                        catch (SqlException oSqlExn)
        //                        {
        //                            oTrans.Rollback();
        //                            switch (oSqlExn.Number)
        //                            {
        //                                case 2627:
        //                                    // Data is duplicate.
        //                                    aoResult.rtCode = oMsg.tMS_RespCode801;
        //                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
        //                                    return aoResult;

        //                                default:
        //                                    //Error statement or sql error
        //                                    aoResult.rtCode = oMsg.tMS_RespCode999;
        //                                    aoResult.rtDesc = oSqlExn.Message;
        //                                    return aoResult;
        //                            }
        //                        }
        //                        catch (EntityException oEtyExn)
        //                        {
        //                            oTrans.Rollback();
        //                            switch (oEtyExn.HResult)
        //                            {
        //                                case -2146232060:
        //                                    // Cannot connect database..
        //                                    aoResult.rtCode = oMsg.tMS_RespCode905;
        //                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
        //                                    return aoResult;
        //                            }
        //                        }
        //                    }
        //                }

        //                aoResult.rtCode = oMsg.tMS_RespCode001;
        //                aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //                return aoResult;
        //            }
        //            else
        //            {
        //                aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //                //aoResult = new cmlResPdtItemDwn();
        //                aoResult.rtCode = "";
        //                aoResult.rtDesc = "";
        //                return aoResult;
        //            }

        //        }
        //        else
        //        {
        //            aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //            //aoResult = new cmlResPdtItemDwn();
        //            aoResult.rtCode = "";
        //            aoResult.rtDesc = "";
        //            return aoResult;

        //        }

        //    }
        //    catch(Exception oExn)
        //    {
        //        // Return error.
        //        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //        aoResult.rtCode = new cMS().tMS_RespCode900;
        //        aoResult.rtDesc = new cMS().tMS_RespDesc900;
        //        return aoResult;
        //    }
        //    finally
        //    {
        //        oFunc = null;
        //        oMsg = null;
        //        oSql = null;

        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        //GC.Collect();
        //    }
        //}

        //[Route("Delete")]
        //[HttpPost]
        //public cmlResItem<cmlResUsrDepDwn> POST_PDToDelDepart(cmlReqDepartDel poparam)
        //{
        //    cSP oFunc;
        //    cMS oMsg;
        //    cCS oCS;
        //    StringBuilder oSql;
        //    StringBuilder oSql_L;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResItem<cmlResUsrDepDwn> aoResult;

        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache;

        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        //        oFunc = new cSP();
        //        oCS = new cCS();
        //        oMsg = new cMS();
        //        tFuncName = MethodBase.GetCurrentMethod().Name;

        //        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //        // Get method name.
        //        tFuncName = MethodBase.GetCurrentMethod().Name;

        //        // Validate parameter.
        //        tModelErr = "";
        //        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
        //        {
        //            // Validate parameter model false.
        //            aoResult.rtCode = oMsg.tMS_RespCode701;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
        //            return aoResult;
        //        }

        //        // Load configuration.
        //        aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
        //        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

        //        tKeyApi = "";
        //        // Check KeyApi.
        //        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
        //        {
        //            // Key not allowed to use method.
        //            aoResult.rtCode = oMsg.tMS_RespCode904;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc904;
        //            return aoResult;
        //        }

        //        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
        //        {

        //            oSql = new StringBuilder();
        //            oSql.Clear();
        //            oSql.AppendLine(" DELETE ");
        //            oSql.AppendLine(" FROM TCNMUsrDepart  WITH(ROWLOCK) ");
        //            oSql.AppendLine(" WHERE (FTDptCode = '" + poparam.ptDptCode + "')");

        //            oSql_L = new StringBuilder();
        //            oSql_L.Clear();
        //            oSql_L.AppendLine(" DELETE ");
        //            oSql_L.AppendLine(" FROM TCNMUsrDepart_L WITH(ROWLOCK)");
        //            oSql_L.AppendLine(" WHERE (FTDptCode = '" + poparam.ptDptCode + "') ");
        //            oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");



        //            using (DbConnection oConn = oAdaAcc.Database.Connection)
        //            {
        //                oConn.Open();
        //                DbCommand oCmd = oConn.CreateCommand();
        //                DbTransaction oTrans;
        //                oTrans = oConn.BeginTransaction();
        //                oCmd.Connection = oConn;
        //                oCmd.Transaction = oTrans;
        //                try
        //                {
        //                    oCmd.CommandTimeout = nCmdTme;
        //                    oCmd.CommandType = CommandType.Text;
        //                    oCmd.CommandText = oSql.ToString();
        //                    oCmd.ExecuteNonQuery();

        //                    oCmd.CommandTimeout = nCmdTme;
        //                    oCmd.CommandType = CommandType.Text;
        //                    oCmd.CommandText = oSql_L.ToString();
        //                    int nSuccess = oCmd.ExecuteNonQuery();
        //                    if (nSuccess == 0)
        //                    {
        //                        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //                        //aoResult = new cmlResPdtItemDwn();
        //                        aoResult.rtCode = "";
        //                        aoResult.rtDesc = "";
        //                        return aoResult;
        //                    }

        //                    oTrans.Commit();
        //                }
        //                catch (SqlException oSqlExn)
        //                {
        //                    oTrans.Rollback();
        //                    switch (oSqlExn.Number)
        //                    {
        //                        case 2627:
        //                            // Data is duplicate.
        //                            aoResult.rtCode = oMsg.tMS_RespCode801;
        //                            aoResult.rtDesc = oMsg.tMS_RespDesc801;
        //                            return aoResult;

        //                        default:
        //                            //Error statement or sql error
        //                            aoResult.rtCode = oMsg.tMS_RespCode999;
        //                            aoResult.rtDesc = oSqlExn.Message;
        //                            return aoResult;
        //                    }
        //                }
        //                catch (EntityException oEtyExn)
        //                {
        //                    oTrans.Rollback();
        //                    switch (oEtyExn.HResult)
        //                    {
        //                        case -2146232060:
        //                            // Cannot connect database..
        //                            aoResult.rtCode = oMsg.tMS_RespCode905;
        //                            aoResult.rtDesc = oMsg.tMS_RespDesc905;
        //                            return aoResult;
        //                    }
        //                }
        //            }
        //        }
        //        aoResult.rtCode = oMsg.tMS_RespCode001;
        //        aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //        return aoResult;
        //    }
        //    catch(Exception oExn)
        //    {
        //        // Return error.
        //        aoResult = new cmlResItem<cmlResUsrDepDwn>();
        //        aoResult.rtCode = new cMS().tMS_RespCode900;
        //        aoResult.rtDesc = new cMS().tMS_RespDesc900;
        //        return aoResult;
        //    }
        //    finally
        //    {
        //        oFunc = null;
        //        oMsg = null;
        //        oSql = null;

        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        //GC.Collect();
        //    }
        //}
    }
}