using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Rcv;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Type payment receive information.
    /// </summary>
    //[ApiController]
    [Route(cCS.tCS_APIVer + "/PAY/Rcv")]
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PAY")]
    public class cTypeReceiveController : ControllerBase
    {
        /// <summary>
        ///     Download Pos advertising message information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Rcv/Download")]
        [HttpGet]
        public cmlResItem<cmlResRcvDwn> GET_PDToDownloadRcv(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResRcvDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRcvDwn oRcvDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRcvDwn>();
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

                tKeyCache = "PAYRCV" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRcvDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRcvCode AS rtRcvCode, FTFmtCode AS rtFmtCode, FTRcvStaUse AS rtRcvStaUse,");
                //oSql.AppendLine("FTRcvStaShwInSlip AS rtRcvStaShwInSlip, FTRcv4Ret AS rtRcv4Ret, FTRcv4ChkOut AS rtRcv4ChkOut,");
                //oSql.AppendLine("FTAppStaAlwRet AS rtAppStaAlwRet,FTAppStaAlwCancel AS rtAppStaAlwCancel,FTAppStaPayLast AS rtAppStaPayLast,"); //*Arm 63-07-30 (เพิ่มฟิลด์)
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TFNMRcv with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResRcvDwn();
                //oRcvDwn = new cmlResRcvDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oRcvDwn.raRcv = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRcv>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oRcvDwn.raRcv.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMRcv_L.FTRcvCode AS rtRcvCode, TFNMRcv_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TFNMRcv_L.FTRcvName AS rtRcvName, TFNMRcv_L.FTRcvRmk AS rtRcvRmk");
                //            oSql.AppendLine("FROM TFNMRcv_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMRcv with(nolock) ON TFNMRcv_L.FTRcvCode = TFNMRcv.FTRcvCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRcv.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRcvDwn.raRcvLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRcvLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //RcvSpc
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMRcvSpc.FTRcvCode AS rtRcvCode,TFNMRcvSpc.FTAppCode AS rtAppCode,TFNMRcvSpc.FNRcvSeq AS rnRcvSeq,TFNMRcvSpc.FTBchCode AS rtBchCode,");
                //            oSql.AppendLine("TFNMRcvSpc.FTMerCode AS rtMerCode,TFNMRcvSpc.FTShpCode AS rtShpCode,TFNMRcvSpc.FTAggCode AS rtAggCode,TFNMRcvSpc.FTPdtRmk AS rtPdtRmk, ");
                //            oSql.AppendLine("TFNMRcvSpc.FTPosCode AS rtPosCode"); //*Arm 63-10-30
                //            //oSql.AppendLine("TFNMRcvSpc.FTAppStaAlwRet AS rtAppStaAlwRet,TFNMRcvSpc.FTAppStaAlwCancel AS rtAppStaAlwCancel,TFNMRcvSpc.FTAppStaPayLast AS rtAppStaPayLast "); //*Arm 63-07-30 Comment Code (ยกเลิกฟิลด์)
                //            oSql.AppendLine("FROM TFNMRcvSpc WITH(NOLOCK)");
                //            oSql.AppendLine("INNER JOIN TFNMRcv with(nolock) ON TFNMRcvSpc.FTRcvCode = TFNMRcv.FTRcvCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRcv.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRcvDwn.raRcvSpc = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRcvSpc>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //*Arm 63-10-30 RcvSpcConfig
                //            oSql.Clear();
                //            oSql.AppendLine("SELECT CFG.FTRcvCode AS rtRcvCode, CFG.FNRcvSeq AS rnRcvSeq, CFG.FNSysSeq AS rnSysSeq, CFG.FTSysKey AS rtSysKey,");
                //            oSql.AppendLine("CFG.FTSysStaUsrValue AS rtSysStaUsrValue, CFG.FTSysStaUsrRef AS rtSysStaUsrRef ");
                //            oSql.AppendLine("FROM TFNMRcvSpcConfig CFG with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMRcv RCV with(nolock) ON CFG.FTRcvCode = RCV.FTRcvCode ");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),RCV.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRcvDwn.raRcvSpcConfig = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRcvSpcConfig>(oDR).ToList();
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
                aoResult.roItem = new cmlResRcvDwn();
                oRcvDwn = new cmlResRcvDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRcvCode AS rtRcvCode, FTFmtCode AS rtFmtCode, FTRcvStaUse AS rtRcvStaUse,");
                oSql.AppendLine("FTRcvStaShwInSlip AS rtRcvStaShwInSlip, FTRcv4Ret AS rtRcv4Ret, FTRcv4ChkOut AS rtRcv4ChkOut,");
                oSql.AppendLine("FTAppStaAlwRet AS rtAppStaAlwRet,FTAppStaAlwCancel AS rtAppStaAlwCancel,FTAppStaPayLast AS rtAppStaPayLast,"); //*Arm 63-07-30 (เพิ่มฟิลด์)
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy,");
                oSql.AppendLine("FTRcvStaAllRte AS rtRcvStaAllRte,");      //*Nick 65-08-23
                oSql.AppendLine("FTRcvRefRate AS rtRcvRefRate");           //*Nick 65-08-26
                oSql.AppendLine("FROM TFNMRcv with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oRcvDwn.raRcv = oDB.C_DATaSqlQuery<cmlResInfoRcv>(oSql.ToString());
                if (oRcvDwn.raRcv.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMRcv_L.FTRcvCode AS rtRcvCode, TFNMRcv_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMRcv_L.FTRcvName AS rtRcvName, TFNMRcv_L.FTRcvRmk AS rtRcvRmk");
                    oSql.AppendLine("FROM TFNMRcv_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMRcv with(nolock) ON TFNMRcv_L.FTRcvCode = TFNMRcv.FTRcvCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRcv.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oRcvDwn.raRcvLng = oDB.C_DATaSqlQuery<cmlResInfoRcvLng>(oSql.ToString());

                    //RcvSpc
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMRcvSpc.FTRcvCode AS rtRcvCode,TFNMRcvSpc.FTAppCode AS rtAppCode,TFNMRcvSpc.FNRcvSeq AS rnRcvSeq,TFNMRcvSpc.FTBchCode AS rtBchCode,");
                    oSql.AppendLine("TFNMRcvSpc.FTMerCode AS rtMerCode,TFNMRcvSpc.FTShpCode AS rtShpCode,TFNMRcvSpc.FTAggCode AS rtAggCode,TFNMRcvSpc.FTPdtRmk AS rtPdtRmk, ");
                    oSql.AppendLine("TFNMRcvSpc.FTPosCode AS rtPosCode"); //*Arm 63-10-30
                                                                          //oSql.AppendLine("TFNMRcvSpc.FTAppStaAlwRet AS rtAppStaAlwRet,TFNMRcvSpc.FTAppStaAlwCancel AS rtAppStaAlwCancel,TFNMRcvSpc.FTAppStaPayLast AS rtAppStaPayLast "); //*Arm 63-07-30 Comment Code (ยกเลิกฟิลด์)
                    oSql.AppendLine("FROM TFNMRcvSpc WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TFNMRcv with(nolock) ON TFNMRcvSpc.FTRcvCode = TFNMRcv.FTRcvCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRcv.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oRcvDwn.raRcvSpc = oDB.C_DATaSqlQuery<cmlResInfoRcvSpc>(oSql.ToString());

                    //*Arm 63-10-30 RcvSpcConfig
                    oSql.Clear();
                    oSql.AppendLine("SELECT CFG.FTRcvCode AS rtRcvCode, CFG.FNRcvSeq AS rnRcvSeq, CFG.FNSysSeq AS rnSysSeq, CFG.FTSysKey AS rtSysKey,");
                    oSql.AppendLine("CFG.FTSysStaUsrValue AS rtSysStaUsrValue, CFG.FTSysStaUsrRef AS rtSysStaUsrRef ");
                    oSql.AppendLine("FROM TFNMRcvSpcConfig CFG with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMRcv RCV with(nolock) ON CFG.FTRcvCode = RCV.FTRcvCode ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),RCV.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oRcvDwn.raRcvSpcConfig = oDB.C_DATaSqlQuery<cmlResInfoRcvSpcConfig>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oRcvDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRcvDwn>();
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
        ///     Download TFNMRcvSpc
        /// </summary>
        /// <returns></returns>
        [Route("RcvSpc/Download")]
        [HttpGet]
        public cmlResItem<cmlResRcvSpcDwn> GET_PDToDownloadRcvSpc()
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResRcvSpcDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRcvSpcDwn oRcvSpcDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRcvSpcDwn>();
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

                tKeyCache = "RcvSpc" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRcvSpcDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRcvCode AS rtRcvCode,FTAppCode AS rtAppCode,FNRcvSeq AS rnRcvSeq,FTBchCode AS rtBchCode,");
                //oSql.AppendLine("FTMerCode AS rtMerCode,FTShpCode AS rtShpCode,FTAggCode AS rtAggCode,FTPdtRmk AS rtPdtRmk, ");
                //oSql.AppendLine("TFNMRcvSpc.FTPosCode AS rtPosCode"); //*Arm 63-10-30
                ////oSql.AppendLine("FTAppStaAlwRet AS rtAppStaAlwRet,FTAppStaAlwCancel AS rtAppStaAlwCancel,FTAppStaPayLast AS rtAppStaPayLast "); //*Arm 63-07-30 Comment Code (ยกเลิกฟิลด์)
                //oSql.AppendLine("FROM TFNMRcvSpc WITH(NOLOCK)");

                //aoResult.roItem = new cmlResRcvSpcDwn();
                //oRcvSpcDwn = new cmlResRcvSpcDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oRcvSpcDwn.raRcvSpc = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRcvSpc>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }
                //        if (oRcvSpcDwn.raRcvSpc.Count > 0)
                //        {
                            
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
                aoResult.roItem = new cmlResRcvSpcDwn();
                oRcvSpcDwn = new cmlResRcvSpcDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRcvCode AS rtRcvCode,FTAppCode AS rtAppCode,FNRcvSeq AS rnRcvSeq,FTBchCode AS rtBchCode,");
                oSql.AppendLine("FTMerCode AS rtMerCode,FTShpCode AS rtShpCode,FTAggCode AS rtAggCode,FTPdtRmk AS rtPdtRmk, ");
                oSql.AppendLine("TFNMRcvSpc.FTPosCode AS rtPosCode"); //*Arm 63-10-30
                oSql.AppendLine("FROM TFNMRcvSpc WITH(NOLOCK)");
                oRcvSpcDwn.raRcvSpc = oDB.C_DATaSqlQuery<cmlResInfoRcvSpc>(oSql.ToString());
                if (oRcvSpcDwn.raRcvSpc.Count > 0)
                {

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oRcvSpcDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRcvSpcDwn>();
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