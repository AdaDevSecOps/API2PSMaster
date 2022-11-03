using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.VatRate;
using API2PSMaster.Models.WebService.Request.VatRate;
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
    ///     Vat rate Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/VatRate")]
    public class cVatRateController : ControllerBase
    {
        /// <summary>
        ///     Download vat rate information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResList<cmlResInfoVatRate> GET_PDToDownloadVateRate(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoVatRate> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResCompDwn oCompDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoVatRate>();
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

                tKeyCache = "VatRate" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoVatRate>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTVatCode AS rtVatCode, FDVatStart AS rdVatStart, FCVatRate AS rcVatRate,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMVatRate with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.raItems = new List<cmlResInfoVatRate>();
                ////oCompDwn = new cmlResCompDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoVatRate>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (aoResult.raItems.Count > 0)
                //        {
                //            //
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
                aoResult.raItems = new List<cmlResInfoVatRate>();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTVatCode AS rtVatCode, FDVatStart AS rdVatStart, FCVatRate AS rcVatRate,");
                oSql.AppendLine("FTCtyCode AS rtCtyCode, "); //*Arm 65-09-20 [CR-Oversea]
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMVatRate with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                aoResult.raItems = oDB.C_DATaSqlQuery<cmlResInfoVatRate>(oSql.ToString());
                if (aoResult.raItems.Count > 0)
                {
                    //
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                //aoResult.raItems.Add(oCompDwn);
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoVatRate>();
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
        ///// VatRate Insert
        ///// </summary>
        ///// <param name="poparam"></param>
        ///// <returns></returns>
        //[Route("Insert")]
        //[HttpPost]
        //public cmlResList<cmlResInfoVatRate> POST_SALoInsVatRate([FromBody]cmlReqVatRate poparam)
        //{
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    cCacheFunc oCacheFunc;
        //    StringBuilder oSql;
        //    cmlResList<cmlResInfoVatRate> aoResult;
        //    List<cmlTSysConfig> aoSysConfig;

        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache;


        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //        aoResult = new cmlResList<cmlResInfoVatRate>();
        //        oFunc = new cSP();
        //        oCS = new cCS();
        //        oMsg = new cMS();
        //        oCacheFunc = new cCacheFunc(43200, 43200, false);

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
        //            oSql.AppendLine(" INSERT INTO TCNMVatRate  WITH(ROWLOCK)  ( FTVatCode, FDVatStart, FCVatRate,");
        //            oSql.AppendLine(" FDDateUpd, FTTimeUpd, FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns ) ");
        //            oSql.AppendLine(" VALUES ");
        //            oSql.AppendLine(" ('" + poparam.ptVatCode + "',CONVERT(VARCHAR(10), GETDATE(), 121),");
        //            oSql.AppendLine(" '" + poparam.pcVatRate + "', CONVERT(VARCHAR(10), GETDATE(), 121),");
        //            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108), '" + poparam.ptWhoUpdate + "',");
        //            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121) , CONVERT(VARCHAR(8),GETDATE(),108) , ");
        //            oSql.AppendLine(" '" + poparam.ptWhoUpdate + "')");

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

        //        }
        //    catch(Exception oEx)
        //    {
        //        // Return error.
        //        aoResult = new cmlResList<cmlResInfoVatRate>();
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
        ///// DeleteVatRate
        ///// </summary>
        //[Route("Delete")]
        //[HttpPost]
        //public cmlResList<cmlResInfoVatRate> POST_SALoDelVatRate([FromBody]cmlReqVatRateDelete poparam)
        //{
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    StringBuilder oSql;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResList<cmlResInfoVatRate> aoResult;
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

        //        aoResult = new cmlResList<cmlResInfoVatRate>();
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
        //            oSql.AppendLine(" FROM TCNMVatRate  WITH(ROWLOCK) ");
        //            oSql.AppendLine(" WHERE (FTVatCode = '" + poparam.ptVatCode +"')");
        //            oSql.AppendLine(" AND (CONVERT(VARCHAR(10), FDVatStart, 121) = '" + string.Format("{0:yyyy-MM-dd}", poparam.pdVatStart) + "' ) ");

                    

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
        //    catch (Exception oEx)
        //    {
        //        // Return error.
        //        aoResult = new cmlResList<cmlResInfoVatRate>();
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
        //public cmlResList<cmlResInfoVatRate> POST_SALoUpdateVatRate([FromBody]cmlReqVatRateUpdate poparam)
        //{
        //    DataTable oDbTblTmp;
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    cDatabase oDatabase;
        //    StringBuilder oSql;
        //    List<cmlTSysConfig> aoSysConfig;
        //    cmlResList<cmlResInfoVatRate> aoResult;
        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache, tWahStaType, tWahRefCod;

        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //        aoResult = new cmlResList<cmlResInfoVatRate>();
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

        //        string tDate = "";
        //        tDate = Convert.ToDateTime(poparam.pdVatStart).ToString("yyyy-MM-dd", new System.Globalization.CultureInfo("en-US"));

        //        oSql.AppendLine("SELECT TOP 1 FTVatCode, FDVatStart, FCVatRate  ");
        //        oSql.AppendLine("FROM TCNMVatRate WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTVatCode = '" + poparam.ptVatCode + "' AND (CONVERT(varchar(10), FDVatStart, 121) = '" + tDate + "') ");
        //        oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
        //        if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
        //        {
                    
        //            using (AdaAccEntities oAdaAcc = new AdaAccEntities())
        //            {
        //                //string tUpdateVatRate = "";

        //                //if (poparam.ptVatrate != 0)
        //                //{
        //                //    tUpdateVatRate += "FCVatRate = '" + poparam.ptVatrate + "',";
        //                //}
                      


        //                oSql = new StringBuilder();
        //                oSql.Clear();
        //                oSql.AppendLine(" UPDATE ");
        //                oSql.AppendLine(" TCNMVatRate  WITH(ROWLOCK) ");
        //                oSql.AppendLine(" SET " + oFunc.SP_SETtValueUpt(Convert.ToString(poparam.ptVatrate), "FCVatRate") + "");
        //                oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
        //                oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
        //                oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
        //                oSql.AppendLine(" WHERE FTVatCode = '" + poparam.ptVatCode + "'");

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
        //            aoResult.rtCode = oMsg.tMS_RespCode001;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //            return aoResult;


        //        }
        //        else
        //        {
        //            aoResult = new cmlResList<cmlResInfoVatRate>();
        //            //aoResult = new cmlResPdtItemDwn();
        //            aoResult.rtCode = "";
        //            aoResult.rtDesc = "";
        //            return aoResult;

        //        }




        //    }
        //    catch (Exception oEx)
        //    {
        //        // Return error.
        //        aoResult = new cmlResList<cmlResInfoVatRate>();
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