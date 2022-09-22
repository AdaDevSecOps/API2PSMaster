using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Province;
using API2PSMaster.Models.WebService.Request.Provice;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data.Entity.Core;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage Province.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Province")]
    public class cProvinceController : ControllerBase
    {
        /// <summary>
        ///     Download Province information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResPvnDwn> GET_PDToDownloadProvince(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPvnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPvnDwn oPvnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPvnDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "Province" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPvnDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPvnCode AS rtPvnCode, FTZneCode AS rtZneCode, FTPvnLatitude AS rtPvnLatitude,");
                //oSql.AppendLine("FTPvnLongitude AS rtPvnLongitude,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMProvince with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPvnDwn();
                //oPvnDwn = new cmlResPvnDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPvnDwn.raPvn = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPvn>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPvnDwn.raPvn.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMProvince_L.FTPvnCode AS rtPvnCode, TCNMProvince_L.FNLngID AS rnLngID, TCNMProvince_L.FTPvnName AS rtPvnName");
                //            oSql.AppendLine("FROM TCNMProvince_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMProvince with(nolock) ON TCNMProvince_L.FTPvnCode = TCNMProvince.FTPvnCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMProvince.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPvnDwn.raPvnLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPvnLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPvnDwn();
                oPvnDwn = new cmlResPvnDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPvnCode AS rtPvnCode, FTZneCode AS rtZneCode, FTPvnLatitude AS rtPvnLatitude,");
                oSql.AppendLine("FTPvnLongitude AS rtPvnLongitude,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMProvince with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPvnDwn.raPvn = oDB.C_DATaSqlQuery<cmlResInfoPvn>(oSql.ToString());
                if (oPvnDwn.raPvn.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMProvince_L.FTPvnCode AS rtPvnCode, TCNMProvince_L.FNLngID AS rnLngID, TCNMProvince_L.FTPvnName AS rtPvnName");
                    oSql.AppendLine("FROM TCNMProvince_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMProvince with(nolock) ON TCNMProvince_L.FTPvnCode = TCNMProvince.FTPvnCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMProvince.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPvnDwn.raPvnLng = oDB.C_DATaSqlQuery<cmlResInfoPvnLng>(oSql.ToString());
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
                aoResult = new cmlResItem<cmlResPvnDwn>();
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

        /// <summary>
        /// Insert Province
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Insert")]
        [HttpPost]
        public cmlResItem<cmlResPvnDwn> POST_PDToInsProvice(cmlReqProvince poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            cmlResItem<cmlResPvnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPvnDwn oPvnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResPvnDwn>();
                oFunc = new cSP();
                oMsg = new cMS();
                oCS = new cCS();

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

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    oSql = new StringBuilder();
                    oSql.Clear();
                    oSql.AppendLine(" INSERT ");
                    oSql.AppendLine(" INTO TCNMProvince WITH(ROWLOCK) ");
                    oSql.AppendLine(" (FTPvnCode, FTZneCode, FTPvnLatitude, ");
                    oSql.AppendLine(" FTPvnLongitude, FDDateUpd, FTTimeUpd, ");
                    oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns) ");
                    oSql.AppendLine(" VALUES (");
                    oSql.AppendLine(" '" + poparam.ptPvnCode + "', '" + poparam.ptZneCode + "',");
                    oSql.AppendLine(" '" + poparam.ptPvnLatitude + "' , '" + poparam.ptPvnLongitude + "',");
                    oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121) ,");
                    oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                    oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
                    oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121), ");
                    oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108), ");
                    oSql.AppendLine(" '" + poparam.ptWhoUpd + "') ");


                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" INSERT ");
                    oSql_L.AppendLine(" INTO");
                    oSql_L.AppendLine(" TCNMProvince_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" (FTPvnCode, FNLngID, FTPvnName)");
                    oSql_L.AppendLine(" VALUES");
                    oSql_L.AppendLine(" ('" + poparam.ptPvnCode + "',");
                    oSql_L.AppendLine(" '" + poparam.pnLngID + "',");
                    oSql_L.AppendLine(" '" + poparam.ptPvmName + "')");
                      
                    
                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        DbTransaction oTrans;
                        oTrans = oConn.BeginTransaction();
                        oCmd.Connection = oConn;
                        oCmd.Transaction = oTrans;
                        try
                        {
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            oCmd.ExecuteNonQuery();

                            oTrans.Commit();
                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    aoResult.rtCode = oMsg.tMS_RespCode801;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                    return aoResult;

                                default:
                                    //Error statement or sql error
                                    aoResult.rtCode = oMsg.tMS_RespCode999;
                                    aoResult.rtDesc = oSqlExn.Message;
                                    return aoResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    aoResult.rtCode = oMsg.tMS_RespCode905;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return aoResult;
                            }
                        }
                    }
                }
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;


            }
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPvnDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;

            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();

            }
        }

        [Route("Update")]
        [HttpPost]
        public cmlResItem<cmlResPvnDwn> POST_PDToUptProvince(cmlReqProvince poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResPvnDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblPvn;
            DataTable oDbTblPvn_L;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResPvnDwn>();
                oFunc = new cSP();
                oMsg = new cMS();
                oCS = new cCS();


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


                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPvnCode,FTZneCode, FTPvnLatitude, FTPvnLongitude ");
                oSql.AppendLine("FROM TCNMProvince WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTPvnCode = '" + poparam.ptPvnCode + "'");
                oDbTblPvn = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1)   FTPvnCode, FNLngID, FTPvnName");
                oSql.AppendLine(" FROM TCNMProvince_L");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTPvnCode = '" + poparam.ptPvnCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblPvn_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblPvn != null && oDbTblPvn.Rows.Count > 0)
                {
                    if (oDbTblPvn_L != null && oDbTblPvn_L.Rows.Count > 0)
                    {
                        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                        {

                            oSql = new StringBuilder();
                            oSql.Clear();
                            oSql.AppendLine(" UPDATE ");
                            oSql.AppendLine(" TCNMProvince ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" SET ");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptZneCode, "FTZneCode") + "");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptPvnLatitude, "FTPvnLatitude") + "");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptPvnLongitude, "FTPvnLongitude") + "");
                            oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                            oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                            oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                            oSql.AppendLine(" WHERE FTPvnCode = '" + poparam.ptPvnCode + "'");

                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" UPDATE ");
                            oSql_L.AppendLine(" TCNMProvince_L ");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" SET FTPvnName = '" + poparam.ptPvmName + "'");
                            oSql_L.AppendLine(" WHERE FTPvnCode = '" + poparam.ptPvnCode + "' ");
                            oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");

                            using (DbConnection oConn = oAdaAcc.Database.Connection)
                            {
                                oConn.Open();
                                DbCommand oCmd = oConn.CreateCommand();
                                DbTransaction oTrans;
                                oTrans = oConn.BeginTransaction();
                                oCmd.Connection = oConn;
                                oCmd.Transaction = oTrans;
                                try
                                {
                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql.ToString();
                                    oCmd.ExecuteNonQuery();

                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_L.ToString();
                                    int nSuccess = oCmd.ExecuteNonQuery();
                                    if (nSuccess == 0)
                                    {
                                        aoResult = new cmlResItem<cmlResPvnDwn>();
                                        //aoResult = new cmlResPdtItemDwn();
                                        aoResult.rtCode = "";
                                        aoResult.rtDesc = "";
                                        return aoResult;
                                    }

                                    oTrans.Commit();
                                }
                                catch (SqlException oSqlExn)
                                {
                                    oTrans.Rollback();
                                    switch (oSqlExn.Number)
                                    {
                                        case 2627:
                                            // Data is duplicate.
                                            aoResult.rtCode = oMsg.tMS_RespCode801;
                                            aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                            return aoResult;

                                        default:
                                            //Error statement or sql error
                                            aoResult.rtCode = oMsg.tMS_RespCode999;
                                            aoResult.rtDesc = oSqlExn.Message;
                                            return aoResult;
                                    }
                                }
                                catch (EntityException oEtyExn)
                                {
                                    oTrans.Rollback();
                                    switch (oEtyExn.HResult)
                                    {
                                        case -2146232060:
                                            // Cannot connect database..
                                            aoResult.rtCode = oMsg.tMS_RespCode905;
                                            aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                            return aoResult;
                                    }
                                }
                            }
                        }

                        aoResult.rtCode = oMsg.tMS_RespCode001;
                        aoResult.rtDesc = oMsg.tMS_RespDesc001;
                        return aoResult;
                    }
                    else
                    {
                        aoResult = new cmlResItem<cmlResPvnDwn>();
                        //aoResult = new cmlResPdtItemDwn();
                        aoResult.rtCode = "";
                        aoResult.rtDesc = "";
                        return aoResult;
                    }

                    //aoResult.rtCode = oMsg.tMS_RespCode001;
                    //aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    //return aoResult;
                }
                else
                {
                    aoResult = new cmlResItem<cmlResPvnDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }
            }
            catch(Exception oEx)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPvnDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        [Route("Delete")]
        [HttpPost]
        public cmlResItem<cmlResPvnDwn> POST_PDToDelProvince(cmlReqProvinceDel poparam)
        {
            cSP oFunc;
            cMS oMsg;
            cCS oCS;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResPvnDwn> aoResult;

            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                tFuncName = MethodBase.GetCurrentMethod().Name;

                aoResult = new cmlResItem<cmlResPvnDwn>();
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

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {

                    oSql = new StringBuilder();
                    oSql.Clear();
                    oSql.AppendLine(" DELETE ");
                    oSql.AppendLine(" FROM TCNMProvince  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTPvnCode = '" + poparam.ptPvnCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMProvince_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTPvnCode = '" + poparam.ptPvnCode + "') ");
                    oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");



                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        DbTransaction oTrans;
                        oTrans = oConn.BeginTransaction();
                        oCmd.Connection = oConn;
                        oCmd.Transaction = oTrans;
                        try
                        {
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            int nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                aoResult = new cmlResItem<cmlResPvnDwn>();
                                //aoResult = new cmlResPdtItemDwn();
                                aoResult.rtCode = "";
                                aoResult.rtDesc = "";
                                return aoResult;
                            }

                            oTrans.Commit();
                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    aoResult.rtCode = oMsg.tMS_RespCode801;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                    return aoResult;

                                default:
                                    //Error statement or sql error
                                    aoResult.rtCode = oMsg.tMS_RespCode999;
                                    aoResult.rtDesc = oSqlExn.Message;
                                    return aoResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    aoResult.rtCode = oMsg.tMS_RespCode905;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return aoResult;
                            }
                        }
                    }
                }
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;


            }
            catch(Exception oEx)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPvnDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;

            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}