using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.District;
using API2PSMaster.Models.WebService.Request.District;
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
using System.Data.SqlClient;
using System.Data.Entity.Core;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     District Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/District")]
    public class cDistrictController : ControllerBase
    {
        /// <summary>
        ///     Download District information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResDistrictDwn> GET_PDToDownloadDistrict(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResDistrictDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResDistrictDwn oPvnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResDistrictDwn>();
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

                tKeyCache = "District" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResDistrictDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTDstCode AS rtDstCode, FTDstPost AS rtDstPost, FTPvnCode AS rtPvnCode,");
                oSql.AppendLine("FTDstLatitude AS rtDstLatitude, FTDstLongitude AS rtDstLongitude,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMDistrict with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResDistrictDwn();
                oPvnDwn = new cmlResDistrictDwn();

                //*Arm 64-10-19 Comment Code
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPvnDwn.raDistrinct = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoDistrict>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                oPvnDwn.raDistrinct = oDB.C_DATaSqlQuery<cmlResInfoDistrict>(oSql.ToString()); //*Arm 64-10-19
                if (oPvnDwn.raDistrinct.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMDistrict_L.FTDstCode AS rtDstCode, TCNMDistrict_L.FNLngID AS rnLngID, TCNMDistrict_L.FTDstName AS rtDstName");
                    oSql.AppendLine("FROM TCNMDistrict_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMDistrict with(nolock) ON TCNMDistrict_L.FTDstCode = TCNMDistrict.FTDstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMDistrict.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code  
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oPvnDwn.raDistrinctLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoDistrictLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oPvnDwn.raDistrinctLng = oDB.C_DATaSqlQuery<cmlResInfoDistrictLng>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

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
                aoResult = new cmlResItem<cmlResDistrictDwn>();
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

        [Route("Insert")]
        [HttpPost]
        public cmlResList<cmlResDistrictDwn> POST_PDToInsDistrict(cmlReqDistrict poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResDistrictDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResDistrictDwn>();
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
                    oSql.AppendLine(" INTO TCNMDistrict WITH(ROWLOCK) ");
                    oSql.AppendLine(" (FTDstCode, FTDstPost,FTPvnCode, FTDstLatitude, ");
                    oSql.AppendLine(" FTDstLongitude, FDDateUpd, FTTimeUpd, ");
                    oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns) ");
                    oSql.AppendLine(" VALUES (");
                    oSql.AppendLine(" '" + poparam.ptDstCode + "', '" + poparam.ptDstPost + "',");
                    oSql.AppendLine(" '" + poparam.ptPvnCode + "',");
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
                    oSql_L.AppendLine(" TCNMDistrict_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" (FTDstCode, FNLngID, FTDstName)");
                    oSql_L.AppendLine(" VALUES");
                    oSql_L.AppendLine(" ('" + poparam.ptDstCode + "',");
                    oSql_L.AppendLine(" '" + poparam.pnLngID + "',");
                    oSql_L.AppendLine(" '" + poparam.ptDstName + "')");


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
            catch(Exception oEx)
            {
                // Return error.
                aoResult = new cmlResList<cmlResDistrictDwn>();
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

        //*Ton 64-05-22
        //[Route("Update")]
        [HttpPut("Update")]
        public cmlResList<cmlResDistrictDwn> POST_PDToUptDistrict(cmlReqDistrict poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResDistrictDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblDst;
            DataTable oDbTblDst_L;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResDistrictDwn>();
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
                oSql.AppendLine("SELECT FTDstCode, FTDstPost, FTPvnCode, FTDstLatitude,FTDstLongitude ");
                oSql.AppendLine("FROM TCNMDistrict WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTDstCode = '" + poparam.ptDstCode + "'");
                oDbTblDst = oDatabase.C_DAToSqlQuery(oSql.ToString());


                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1)   FTDstCode, FNLngID, FTDstName ");
                oSql.AppendLine(" FROM TCNMDistrict_L ");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTDstCode = '" + poparam.ptDstCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblDst_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblDst != null && oDbTblDst.Rows.Count > 0)
                {
                    if (oDbTblDst_L != null && oDbTblDst_L.Rows.Count > 0)
                    {
                        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                        {

                            oSql = new StringBuilder();
                            oSql.Clear();
                            oSql.AppendLine(" UPDATE ");
                            oSql.AppendLine(" TCNMDistrict ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" SET ");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptDstPost, "FTDstPost") + "");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptPvnLatitude, "FTDstLatitude") + "");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptPvnLongitude, "FTDstLongitude") + "");
                            oSql.AppendLine(" FTPvnCode = '" + poparam.ptPvnCode + "',");
                            oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                            oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                            oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                            oSql.AppendLine(" WHERE FTPvnCode = '" + poparam.ptPvnCode + "'");

                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" UPDATE ");
                            oSql_L.AppendLine(" TCNMDistrict_L ");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" SET FTDstName = '" + poparam.ptDstName + "'");
                            oSql_L.AppendLine(" WHERE FTDstCode = '" + poparam.ptDstCode + "' ");
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
                                        aoResult = new cmlResList<cmlResDistrictDwn>();
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
                        aoResult = new cmlResList<cmlResDistrictDwn>();
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
                    aoResult = new cmlResList<cmlResDistrictDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }

            }
            catch(Exception oEx)
            {
                // Return error.
                aoResult = new cmlResList<cmlResDistrictDwn>();
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

        //[Route("Delete")]
        //*Ton 64-05-22
        [HttpDelete("Delete")]
        public cmlResList<cmlResDistrictDwn> POST_PDToDelDistrict(cmlReqDistrictDel poparam)
        {

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResDistrictDwn> aoResult;
            cDatabase oDatabase;
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

                aoResult = new cmlResList<cmlResDistrictDwn>();
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
                    oSql.AppendLine(" FROM TCNMDistrict  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTDstCode = '" + poparam.ptDstCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMDistrict_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTDstCode = '" + poparam.ptDstCode + "') ");
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
                                aoResult = new cmlResList<cmlResDistrictDwn>();
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
                aoResult = new cmlResList<cmlResDistrictDwn>();
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