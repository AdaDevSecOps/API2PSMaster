using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.SubDistrict;
using API2PSMaster.Models.WebService.Request.SubDistrict;
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
    ///     Sub District Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/SubDistrict")]
    public class cSubDistrictController : ControllerBase
    {
        /// <summary>
        ///     Download sub district information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResSubDistDwn> GET_PDToDownloadSubDistrict(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSubDistDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSubDistDwn oPvnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSubDistDwn>();
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

                tKeyCache = "SubDistrict" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSubDistDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTSudCode AS rtSudCode, FTDstCode AS rtDstCode,");
                //oSql.AppendLine("FTSudLatitude AS rtSudLatitude, FTSudLongitude AS rtSudLongitude,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMSubDistrict with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResSubDistDwn();
                //oPvnDwn = new cmlResSubDistDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPvnDwn.raSubDist = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSubDist>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPvnDwn.raSubDist.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMSubDistrict_L.FTSudCode AS rtSudCode, TCNMSubDistrict_L.FNLngID AS rnLngID, TCNMSubDistrict_L.FTSudName AS rtSudName");
                //            oSql.AppendLine("FROM TCNMSubDistrict_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSubDistrict with(nolock) ON TCNMSubDistrict_L.FTSudCode = TCNMSubDistrict.FTSudCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSubDistrict.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPvnDwn.raSubDistLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSubDistLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResSubDistDwn();
                oPvnDwn = new cmlResSubDistDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSudCode AS rtSudCode, FTDstCode AS rtDstCode,");
                oSql.AppendLine("FTSudLatitude AS rtSudLatitude, FTSudLongitude AS rtSudLongitude,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMSubDistrict with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPvnDwn.raSubDist = oDB.C_DATaSqlQuery<cmlResInfoSubDist>(oSql.ToString());
                if (oPvnDwn.raSubDist.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMSubDistrict_L.FTSudCode AS rtSudCode, TCNMSubDistrict_L.FNLngID AS rnLngID, TCNMSubDistrict_L.FTSudName AS rtSudName");
                    oSql.AppendLine("FROM TCNMSubDistrict_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSubDistrict with(nolock) ON TCNMSubDistrict_L.FTSudCode = TCNMSubDistrict.FTSudCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSubDistrict.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPvnDwn.raSubDistLng = oDB.C_DATaSqlQuery<cmlResInfoSubDistLng>(oSql.ToString());
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
                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
        public cmlResItem<cmlResSubDistDwn> POST_PDToInsSubDistrict(cmlReqSubDistrict poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResSubDistDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
                    oSql.AppendLine(" INTO TCNMSubDistrict WITH(ROWLOCK) ");
                    oSql.AppendLine(" (FTSudCode,FTDstCode, FTSudLatitude, ");
                    oSql.AppendLine(" FTSudLongitude, FDDateUpd, FTTimeUpd, ");
                    oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns) ");
                    oSql.AppendLine(" VALUES (");
                    oSql.AppendLine(" '" + poparam.ptSudCode + "', '" + poparam.ptDstCode + "',");
                    oSql.AppendLine(" '" + poparam.ptSudLatitude + "',");
                    oSql.AppendLine(" '" + poparam.ptSudLongitude + "' ,");
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
                    oSql_L.AppendLine(" TCNMSubDistrict_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" (FTSudCode, FNLngID, FTSudName)");
                    oSql_L.AppendLine(" VALUES");
                    oSql_L.AppendLine(" ('" + poparam.ptSudCode + "',");
                    oSql_L.AppendLine(" '" + poparam.pnLngID + "',");
                    oSql_L.AppendLine(" '" + poparam.ptSudName + "')");


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
                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
        public cmlResItem<cmlResSubDistDwn> POST_PDToUptSubDistrict(cmlReqSubDistrict poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResSubDistDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblSud;
            DataTable oDbTblSud_L;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
                oSql.AppendLine("SELECT  FTSudCode, FTDstCode, FTSudLatitude, FTSudLongitude ");
                oSql.AppendLine("FROM TCNMSubDistrict WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSudCode = '" + poparam.ptSudCode + "'");
                oDbTblSud = oDatabase.C_DAToSqlQuery(oSql.ToString());


                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1)   FTSudCode, FNLngID, FTSudName ");
                oSql.AppendLine(" FROM TCNMSubDistrict_L ");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTSudCode = '" + poparam.ptSudCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblSud_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblSud != null && oDbTblSud.Rows.Count > 0)
                {
                    if (oDbTblSud_L != null && oDbTblSud_L.Rows.Count > 0)
                    {
                        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                        {

                        
                            //string tLatitude = "";
                            //string tLongtitude = "";

                            //if (poparam.ptSudLatitude != "")
                            //{
                            //    tLatitude += "FTSudLatitude = '" + poparam.ptSudLatitude + "',";
                            //}

                            //if (poparam.ptSudLongitude != "")
                            //{
                            //    tLongtitude += "FTSudLongitude = '" + poparam.ptSudLongitude + "',";
                            //}

                            oSql = new StringBuilder();
                            oSql.Clear();
                            oSql.AppendLine(" UPDATE ");
                            oSql.AppendLine(" TCNMSubDistrict ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" SET ");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptSudLatitude, "FTSudLatitude") + "");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptSudLongitude, "FTSudLongitude") + "");
                            oSql.AppendLine(" FTDstCode = '" + poparam.ptDstCode + "',");
                            oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                            oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                            oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                            oSql.AppendLine(" WHERE FTSudCode = '" + poparam.ptSudCode + "'");

                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" UPDATE ");
                            oSql_L.AppendLine(" TCNMSubDistrict_L ");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" SET FTSudName = '" + poparam.ptSudName + "'");
                            oSql_L.AppendLine(" WHERE FTSudCode = '" + poparam.ptSudCode + "' ");
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
                                        aoResult = new cmlResItem<cmlResSubDistDwn>();
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
                        aoResult = new cmlResItem<cmlResSubDistDwn>();
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
                    aoResult = new cmlResItem<cmlResSubDistDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }



            }
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
        public cmlResItem<cmlResSubDistDwn> POST_PDToDelSubDistrict(cmlReqSubDistrictDel poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResSubDistDwn> aoResult;
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

                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
                    oSql.AppendLine(" FROM TCNMSubDistrict  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTSudCode = '" + poparam.ptSudCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMSubDistrict_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTSudCode = '" + poparam.ptSudCode + "') ");
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
                                aoResult = new cmlResItem<cmlResSubDistDwn>();
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
                aoResult = new cmlResItem<cmlResSubDistDwn>();
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