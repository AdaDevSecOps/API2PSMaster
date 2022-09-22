using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Warehouse;
using API2PSMaster.Models.WebService.Request.Warhouse;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using API2PSMaster.Class;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Warehouse Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Warehouse")]
    public class cWarehouseController : ControllerBase
    {
        /// <summary>
        ///     Download warehouse information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResWahDwn> GET_PDToDownloadWarehouse(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResWahDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResWahDwn oWahDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResWahDwn>();
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

                tKeyCache = "Warehouse" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResWahDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTWahCode AS rtWahCode, FTWahStaType AS rtWahStaType, FTWahRefCode AS rtWahRefCode,");
                //oSql.AppendLine("FTWahStaChkStk AS rtWahStaChkStk, FTWahStaPrcStk AS rtWahStaPrcStk,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMWaHouse with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResWahDwn();
                //oWahDwn = new cmlResWahDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oWahDwn.raWah = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoWah>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oWahDwn.raWah.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT distinct TCNMWaHouse_L.FTBchCode AS rtBchCode, TCNMWaHouse_L.FTWahCode AS rtWahCode, TCNMWaHouse_L.FNLngID AS rnLngID,"); //*Arm 63-03-27 distinct
                //            oSql.AppendLine("TCNMWaHouse_L.FTWahName AS rtWahName, TCNMWaHouse_L.FTWahRmk AS rtWahRmk");
                //            oSql.AppendLine("FROM TCNMWaHouse_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMWaHouse with(nolock) ON TCNMWaHouse_L.FTWahCode = TCNMWaHouse.FTWahCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMWaHouse.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oWahDwn.raWahLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoWahLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResWahDwn();
                oWahDwn = new cmlResWahDwn();

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTWahCode AS rtWahCode, FTWahStaType AS rtWahStaType, FTWahRefCode AS rtWahRefCode,");
                oSql.AppendLine("FTWahStaChkStk AS rtWahStaChkStk, FTWahStaPrcStk AS rtWahStaPrcStk,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMWaHouse with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oWahDwn.raWah = oDB.C_DATaSqlQuery<cmlResInfoWah>(oSql.ToString());
                if (oWahDwn.raWah.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT distinct TCNMWaHouse_L.FTBchCode AS rtBchCode, TCNMWaHouse_L.FTWahCode AS rtWahCode, TCNMWaHouse_L.FNLngID AS rnLngID,"); //*Arm 63-03-27 distinct
                    oSql.AppendLine("TCNMWaHouse_L.FTWahName AS rtWahName, TCNMWaHouse_L.FTWahRmk AS rtWahRmk");
                    oSql.AppendLine("FROM TCNMWaHouse_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMWaHouse with(nolock) ON TCNMWaHouse_L.FTWahCode = TCNMWaHouse.FTWahCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMWaHouse.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oWahDwn.raWahLng = oDB.C_DATaSqlQuery<cmlResInfoWahLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oWahDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResWahDwn>();
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

        ///// <summary>
        ///// Insert Warhouse
        ///// </summary>
        ///// <param name="poparam"></param>
        ///// <returns></returns>
        //[Route("Insert")]
        //[HttpPost]
        //public cmlResItem<cmlResWahDwn> POST_PDToInsWarhouse(cmlReqWarHouse poparam)
        //{
        //    cSP oFunc;
        //    cMS oMsg;
        //    cCS oCS;
        //    StringBuilder oSql;
        //    StringBuilder oSql_L;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResItem<cmlResWahDwn> aoResult;

        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache;

        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //        aoResult = new cmlResItem<cmlResWahDwn>();
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

        //            using (AdaAccEntities oAdaAcc = new AdaAccEntities())
        //            {

        //                oSql = new StringBuilder();
        //                oSql.Clear();
        //                oSql.AppendLine(" INSERT INTO TCNMWaHouse WITH(ROWLOCK)  (FTWahCode, FTWahStaType, FTWahRefCode,");
        //                oSql.AppendLine(" FDDateUpd, FTTimeUpd, FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns) ");
        //                oSql.AppendLine(" VALUES ('" + poparam.ptWahCode + "',");
        //                oSql.AppendLine(" '" + poparam.ptWahStaType + "',");
        //                oSql.AppendLine(" '" + poparam.ptWahRefCode + "',");
        //                oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
        //                oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108), ");
        //                oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
        //                oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
        //                oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
        //                oSql.AppendLine(" '" + poparam.ptWhoUpd + "')");

        //                oSql_L = new StringBuilder();
        //                oSql_L.Clear();
        //                oSql_L.AppendLine(" INSERT INTO TCNMWaHouse_L WITH(ROWLOCK) ");
        //                oSql_L.AppendLine(" (FTWahCode, FNLngID, FTWahName, FTWahRmk) ");
        //                oSql_L.AppendLine(" VALUES('" + poparam.ptWahCode + "',");
        //                oSql_L.AppendLine(" '" + poparam.pnLngID + "' ,");
        //                oSql_L.AppendLine(" '" + poparam.ptWahName + "',");
        //                oSql_L.AppendLine("'" + poparam.ptWahRmk + "') ");

        //                using (DbConnection oConn = oAdaAcc.Database.Connection)
        //                {
        //                    oConn.Open();
        //                    DbCommand oCmd = oConn.CreateCommand();
        //                    DbTransaction oTrans;
        //                    oTrans = oConn.BeginTransaction();
        //                    oCmd.Connection = oConn;
        //                    oCmd.Transaction = oTrans;
        //                    try
        //                    {
        //                        oCmd.CommandTimeout = nCmdTme;
        //                        oCmd.CommandType = CommandType.Text;
        //                        oCmd.CommandText = oSql.ToString();
        //                        oCmd.ExecuteNonQuery();

        //                        oCmd.CommandTimeout = nCmdTme;
        //                        oCmd.CommandType = CommandType.Text;
        //                        oCmd.CommandText = oSql_L.ToString();
        //                        oCmd.ExecuteNonQuery();
        //                        oTrans.Commit();

        //                    }
        //                    catch (SqlException oSqlExn)
        //                    {
        //                        oTrans.Rollback();
        //                        switch (oSqlExn.Number)
        //                        {
        //                            case 2627:
        //                                // Data is duplicate.
        //                                aoResult.rtCode = oMsg.tMS_RespCode801;
        //                                aoResult.rtDesc = oMsg.tMS_RespDesc801;
        //                                return aoResult;

        //                            default:
        //                                //Error statement or sql error
        //                                aoResult.rtCode = oMsg.tMS_RespCode999;
        //                                aoResult.rtDesc = oSqlExn.Message;
        //                                return aoResult;
        //                        }
        //                    }
        //                    catch (EntityException oEtyExn)
        //                    {
        //                        oTrans.Rollback();
        //                        switch (oEtyExn.HResult)
        //                        {
        //                            case -2146232060:
        //                                // Cannot connect database..
        //                                aoResult.rtCode = oMsg.tMS_RespCode905;
        //                                aoResult.rtDesc = oMsg.tMS_RespDesc905;
        //                                return aoResult;
        //                        }
        //                    }
        //                }
        //            }

        //        aoResult.rtCode = oMsg.tMS_RespCode001;
        //        aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //        return aoResult;

        //    }
        //    catch(Exception oEx)
        //    {
        //        // Return error.
        //        aoResult = new cmlResItem<cmlResWahDwn>();
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

        ///// <summary>
        ///// Delete Warhouse
        ///// </summary>
        ///// <param name="poparam">WarehouseCode</param>
        ///// <returns></returns>
        //[Route("Delete")]
        //[HttpPost]
        //public cmlResItem<cmlResWahDwn>POST_PDToDelWarhouse(cmlReqWarHouseDel poparam)
        //{
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    StringBuilder oSql;
        //    StringBuilder oSql_L;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResItem<cmlResWahDwn> aoResult;
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

        //        aoResult = new cmlResItem<cmlResWahDwn>();
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
        //            oSql.AppendLine(" FROM TCNMWaHouse  WITH(ROWLOCK) ");
        //            oSql.AppendLine(" WHERE (FTWahCode = '" + poparam.ptWhaCode + "')");

        //            oSql_L = new StringBuilder();
        //            oSql_L.Clear();
        //            oSql_L.AppendLine(" DELETE");
        //            oSql_L.AppendLine(" FROM TCNMWaHouse_L WITH(ROWLOCK)");
        //            oSql_L.AppendLine(" WHERE (FTWahCode = '" + poparam.ptWhaCode + "')");
        //            oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID +"')");


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
        //                    int nSuccess =  oCmd.ExecuteNonQuery();

        //                    if (nSuccess == 0)
        //                    {
        //                        aoResult = new cmlResItem<cmlResWahDwn>();
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
        //    catch(Exception oEx)
        //    {
        //        // Return error.
        //        aoResult = new cmlResItem<cmlResWahDwn>();
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

        ///// <summary>
        ///// Update WareHouse
        ///// </summary>
        ///// <param name="poparam"></param>
        ///// <returns></returns>

        //[Route("Update")]
        //[HttpPost]
        //public cmlResItem<cmlResWahDwn>POST_PDToUptWarhouse(cmlReqWarHouse poparam)
        //{
        //    DataTable oDbTblWa;
        //    DataTable oDbTblWa_L;
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    cDatabase oDatabase;
        //    StringBuilder oSql;
        //    StringBuilder oSQL_L;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResItem<cmlResWahDwn> aoResult;
        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache,tWahStaType, tWahRefCode;

        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //        aoResult = new cmlResItem<cmlResWahDwn>();
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
        //        oSql.Clear();
        //        oSql.AppendLine(" SELECT TOP 1 FTWahCode,FTWahStaType,FTWahRefCode  ");
        //        oSql.AppendLine(" FROM TCNMWaHouse WITH(NOLOCK)");
        //        oSql.AppendLine(" WHERE FTWahCode = '" + poparam.ptWahCode + "'");
        //        oDbTblWa = oDatabase.C_DAToSqlQuery(oSql.ToString());

        //        oSql.Clear();
        //        oSql.AppendLine(" SELECT FTWahCode, FNLngID, FTWahName, FTWahRmk ");
        //        oSql.AppendLine(" FROM TCNMWaHouse_L WITH(NOLOCK) ");
        //        oSql.AppendLine(" WHERE FTWahCode = '" + poparam.ptWahCode + "'");
        //        oSql.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");
        //        oDbTblWa_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

        //        //check table TCNMWaHouse
        //        if (oDbTblWa != null && oDbTblWa.Rows.Count > 0)
        //        {
        //            //check table  TCNMWaHouse_L
        //            if (oDbTblWa_L != null && oDbTblWa_L.Rows.Count > 0)
        //            {
        //                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
        //                {

        //                    //string tUpdStatype = "";
        //                    //string tUpdRefCode = "";
        //                    //string tWhaRmk = "";

        //                    //if (poparam.ptWahStaType != "")
        //                    //{
        //                    //    tUpdStatype += "FTWahStaType = '" + poparam.ptWahStaType + "',";
        //                    //}

        //                    //if (poparam.ptWahRefCode != "")
        //                    //{
        //                    //    tUpdRefCode += "FTWahRefCode = '" + poparam.ptWahRefCode + "',";
        //                    //}

                           


        //                    oSql = new StringBuilder();
        //                    oSql.Clear();
        //                    oSql.AppendLine(" UPDATE ");
        //                    oSql.AppendLine(" TCNMWaHouse  WITH(ROWLOCK) ");
        //                    oSql.AppendLine(" SET " + oFunc.SP_SETtValueUpt(poparam.ptWahStaType, "FTWahStaType") + "");
        //                    oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptWahRefCode, "FTWahRefCode") + "");
        //                    oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
        //                    oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
        //                    oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
        //                    oSql.AppendLine(" WHERE FTWahCode = '" + poparam.ptWahCode + "'");

        //                    //if(poparam.ptWahRmk != "")
        //                    //{
        //                    //    tWhaRmk += ",FTWahRmk = '" + poparam.ptWahRmk +"'";

        //                    //}

        //                    oSQL_L = new StringBuilder();
        //                    oSQL_L.Clear();
        //                    oSQL_L.AppendLine(" UPDATE ");
        //                    oSQL_L.AppendLine(" TCNMWaHouse_L  WITH(ROWLOCK) ");
        //                    oSQL_L.AppendLine(" SET FTWahName = '" + poparam.ptWahName + "'");
        //                    oSQL_L.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptWahRmk, "FTWahRmk") + "");
        //                    oSQL_L.AppendLine(" WHERE FTWahCode = '" + poparam.ptWahCode + "'");
        //                    oSQL_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "' ");

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
        //                            oCmd.CommandText = oSQL_L.ToString();
        //                            oCmd.ExecuteNonQuery();
                                
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
        //                aoResult = new cmlResItem<cmlResWahDwn>();
        //                //aoResult = new cmlResPdtItemDwn();
        //                aoResult.rtCode = "";
        //                aoResult.rtDesc = "";
        //                return aoResult;
        //            }
          
        //        }
        //        else
        //        {
        //            aoResult = new cmlResItem<cmlResWahDwn>();
        //            //aoResult = new cmlResPdtItemDwn();
        //            aoResult.rtCode = "";
        //            aoResult.rtDesc = "";
        //            return aoResult;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Return error.
        //        aoResult = new cmlResItem<cmlResWahDwn>();
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