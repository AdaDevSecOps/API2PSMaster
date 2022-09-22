using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.System;
using Dapper;
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
    ///     Manage system data.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/DataSystem")]
    public class cSystemController : ControllerBase
    {
        /// <summary>
        ///     Download system document type information.
        /// </summary>
        /// <returns></returns>
        [Route("SysDocType/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysDocType> GET_SYSoDownloadSysDocType()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysDocType> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysDocType>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysDocType" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysDocType>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSdtTblName AS rtSdtTblName, FTSdtFedTypeName AS rtSdtFedTypeName, FNSdtDocType AS rnSdtDocType,");
                oSql.AppendLine("FNSdtID AS rnSdtID, FTSdtDocGrp AS rtSdtDocGrp,"); //*Arm 63-0-24
                oSql.AppendLine("FTSdtDocName AS rtSdtDocName, FTSdtDocTypeRef AS rtSdtDocTypeRef");
                oSql.AppendLine("FROM TSysDocType with(nolock)");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysDocType>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    aoResult.raItems = oConn.Query<cmlResInfoSysDocType>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysDocType>();
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
        ///     Download system EDC information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <returns></returns>
        [Route("SysEdc/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysEdc> GET_SYSoDownloadSysEdc(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysEdc> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysEdc>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysEdc" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysEdc>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSedCode AS rtSedCode, FTSedModel AS rtSedModel, FNSedAck AS rnSedAck,");
                oSql.AppendLine("FTSedDllVer AS rtSedDllVer, FNSedTimeOut AS rnSedTimeOut,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TSysEdc with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysEdc>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    aoResult.raItems = oConn.Query<cmlResInfoSysEdc>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysEdc>();
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
        ///     Download system promotion information.
        /// </summary>
        /// <returns></returns>
        [Route("SysLang/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysLang> GET_SYSoDownloadSysLang()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysLang> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysLang>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysLang" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysLang>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNLngID AS rnLngID, FTLngName AS rtLngName, FTLngNameEng AS rtLngNameEng,");
                oSql.AppendLine("FTLngShortName AS rtLngShortName, FTLngStaLocal AS rtLngStaLocal, FTLngStaUse AS rtLngStaUse");
                oSql.AppendLine("FROM TSysLanguage with(nolock)");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysLang>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    aoResult.raItems = oConn.Query<cmlResInfoSysLang>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysLang>();
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
        ///     Download system EDC information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <returns></returns>
        [Route("SysPmt/Download")]
        [HttpGet]
        public cmlResItem<cmlResSysPmtDwn> GET_SYSoDownloadSysPmt(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSysPmtDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSysPmtDwn oSysPmtDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSysPmtDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysPmt" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSysPmtDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSpmCode AS rtSpmCode, FTSpmType AS rtSpmType, FTSpmStaGrpBuy AS rtSpmStaGrpBuy, FTSpmStaBuy AS rtSpmStaBuy,");
                oSql.AppendLine("FTSpmStaGrpRcv AS rtSpmStaGrpRcv, FTSpmStaRcv AS rtSpmStaRcv, FTSpmStaGrpBoth AS rtSpmStaGrpBoth, FTSpmStaGrpReject AS rtSpmStaGrpReject,");
                oSql.AppendLine("FTSpmStaAllPdt AS rtSpmStaAllPdt, FTSpmStaExceptPmt AS rtSpmStaExceptPmt, FTSpmStaGetNewPri AS rtSpmStaGetNewPri, FTSpmStaGetDisAmt AS rtSpmStaGetDisAmt,");
                oSql.AppendLine("FTSpmStaGetDisPer AS rtSpmStaGetDisPer, FTSpmStaGetPoint AS rtSpmStaGetPoint, FTSpmStaRcvFree AS rtSpmStaRcvFree, FTSpmStaAlwOffline AS rtSpmStaAlwOffline,");
                oSql.AppendLine("FTSpmStaChkLimitGet AS rtSpmStaChkLimitGet, FTSpmStaChkCst AS rtSpmStaChkCst, FTSpmStaChkCstDOB AS rtSpmStaChkCstDOB,");
                oSql.AppendLine("FTSpmStaUseRange AS rtSpmStaUseRange, FNSpmLimitGrpRcv AS rnSpmLimitGrpRcv,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TSysPmt with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResSysPmtDwn();
                oSysPmtDwn = new cmlResSysPmtDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using(DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oSysPmtDwn.raSysPmt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysPmt>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oSysPmtDwn.raSysPmt = oConn.Query<cmlResInfoSysPmt>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oSysPmtDwn.raSysPmt.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSysPmt_L.FTSpmCode AS rtSpmCode, TSysPmt_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TSysPmt_L.FTSpmName AS rtSpmName, TSysPmt_L.FTSpmRmk AS rtSpmRmk");
                        oSql.AppendLine("FROM TSysPmt_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysPmt with(nolock) ON TSysPmt_L.FTSpmCode = TSysPmt.FTSpmCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TSysPmt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oSysPmtDwn.raSysPmtLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysPmtLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSysPmtDwn.raSysPmtLng = oConn.Query<cmlResInfoSysPmtLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oSysPmtDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSysPmtDwn>();
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
        ///     Download system POS Hardware information.
        /// </summary>
        /// <returns></returns>
        [Route("SysPosHW/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysPosHW> GET_SYSoDownloadSysPosHW()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysPosHW> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysPosHW>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysPosHW" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysPosHW>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTShwCode AS rtShwCode, FTShwHWKey AS rtShwHWKey, FTShwName AS rtShwName,");
                oSql.AppendLine("FTShwNameEng AS rtShwNameEng, FTShwSystem AS rtShwSystem, FTShwStaAlwPrinter AS rtShwStaAlwPrinter,");
                oSql.AppendLine("FTShwStaAlwCom AS rtShwStaAlwCom, FTShwStaAlwTCP AS rtShwStaAlwTCP, FTShwStaAlwBT AS rtShwStaAlwBT");
                oSql.AppendLine("FROM TSysPosHW with(nolock)");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysPosHW>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    aoResult.raItems = oConn.Query<cmlResInfoSysPosHW>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysPosHW>();
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
        ///     Download system printer information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <returns></returns>
        
        //*Ton 64-05-22 UnComment by Tanachote
        [Route("SysPrinter/Download")]
        [HttpGet]
        public cmlResItem<cmlResSysPrnDwn> GET_SYSoDownloadSysPrinter(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSysPrnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSysPrnDwn oSysPrnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSysPrnDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysPrinter" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSysPrnDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSppCode AS rtSppCode, FTSppValue AS rtSppValue, FTSppRef AS rtSppRef,");
                oSql.AppendLine("FTSppType AS rtSppType, FTSppStaUse AS rtSppStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TSysPrinter with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResSysPrnDwn();
                oSysPrnDwn = new cmlResSysPrnDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oSysPrnDwn.raSysPrn = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysPrn>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oSysPrnDwn.raSysPrn = oConn.Query<cmlResInfoSysPrn>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oSysPrnDwn.raSysPrn.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSysPrinter_L.FTSppCode AS rtSppCode, TSysPrinter_L.FNLngID AS rnLngID, TSysPrinter_L.FTSppName AS rtSppName");
                        oSql.AppendLine("FROM TSysPrinter_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysPrinter with(nolock) ON TSysPrinter_L.FTSppCode = TSysPrinter.FTSppCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TSysPrinter.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oSysPrnDwn.raSysPrnLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysPrnLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSysPrnDwn.raSysPrnLng = oConn.Query<cmlResInfoSysPrnLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oSysPrnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSysPrnDwn>();
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
        ///     Download system config information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <param name="ptBchCode"></param>
        /// <returns></returns>
        [Route("SysConfig/Download")]
        [HttpGet]
        //public cmlResItem<cmlResSysConfigDwn> GET_SYSoDownloadSysConfig(DateTime pdDate)
        public cmlResItem<cmlResSysConfigDwn> GET_SYSoDownloadSysConfig(DateTime pdDate, string ptBchCode)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSysConfigDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSysConfigDwn oSysConfigDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSysConfigDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysConfig" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSysConfigDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSysCode AS rtSysCode, FTSysApp AS rtSysApp, FTSysKey AS rtSysKey, FTSysSeq AS rtSysSeq,");
                oSql.AppendLine("FTGmnCode AS rtGmnCode, FTSysStaAlwEdit AS rtSysStaAlwEdit, FTSysStaDataType AS rtSysStaDataType, ISNULL(FNSysMaxLength,1) AS rnSysMaxLength,");
                oSql.AppendLine("FTSysStaDefValue AS rtSysStaDefValue, FTSysStaDefRef AS rtSysStaDefRef, FTSysStaUsrValue AS rtSysStaUsrValue, FTSysStaUsrRef AS rtSysStaUsrRef,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TSysConfig with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResSysConfigDwn();
                oSysConfigDwn = new cmlResSysConfigDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using(DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oSysConfigDwn.raConfig = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysConfig>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oSysConfigDwn.raConfig = oConn.Query<cmlResInfoSysConfig>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oSysConfigDwn.raConfig.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSysConfig_L.FTSysCode AS rtSysCode, TSysConfig_L.FTSysApp AS rtSysApp, TSysConfig_L.FTSysKey AS rtSysKey,");
                        oSql.AppendLine("TSysConfig_L.FTSysSeq AS rtSysSeq, TSysConfig_L.FNLngID AS rnLngID, TSysConfig_L.FTSysName AS rtSysName,");
                        oSql.AppendLine("TSysConfig_L.FTSysDesc AS rtSysDesc, TSysConfig_L.FTSysRmk AS rtSysRmk");
                        oSql.AppendLine("FROM TSysConfig_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysConfig with(nolock) ON TSysConfig_L.FTSysCode = TSysConfig.FTSysCode AND TSysConfig_L.FTSysApp = TSysConfig.FTSysApp");
                        oSql.AppendLine("AND TSysConfig_L.FTSysKey = TSysConfig.FTSysKey AND TSysConfig_L.FTSysSeq = TSysConfig.FTSysSeq");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TSysConfig.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oSysConfigDwn.raConfigLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysConfigLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSysConfigDwn.raConfigLng = oConn.Query<cmlResInfoSysConfigLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //*Arm 64-01-11
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNTConfigSpc.FTSysCode AS rtSysCode, TCNTConfigSpc.FTSysApp AS rtSysApp, TCNTConfigSpc.FTSysKey AS rtSysKey, TCNTConfigSpc.FTSysSeq AS rtSysSeq, ");
                        oSql.AppendLine("TCNTConfigSpc.FTAgnCode AS rtAgnCode, TCNTConfigSpc.FTSysStaDataType AS rtSysStaDataType, TCNTConfigSpc.FTCfgStaUsrValue AS rtCfgStaUsrValue, TCNTConfigSpc.FTCfgStaUsrRef AS rtCfgStaUsrRef");
                        oSql.AppendLine("FROM TCNTConfigSpc with(nolock)");
                        //* Arm 64-01-19 Comment Code
                        //oSql.AppendLine("INNER JOIN TSysConfig with(nolock) ON TCNTConfigSpc.FTSysCode = TSysConfig.FTSysCode AND TCNTConfigSpc.FTSysApp = TSysConfig.FTSysApp");
                        //oSql.AppendLine("AND TCNTConfigSpc.FTSysKey = TSysConfig.FTSysKey AND TCNTConfigSpc.FTSysSeq = TSysConfig.FTSysSeq");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TSysConfig.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //++++++++++++++
                        oSql.AppendLine("INNER JOIN TCNMBranch ON TCNTConfigSpc.FTAgnCode = TCNMBranch.FTAgnCode AND TCNMBranch.FTBchCode = '" + ptBchCode + "'"); //* Arm 64-01-19 เอาที่อยู่ใน AD ไปทั้งหมด
                        oSysConfigDwn.raConfigSpc = oConn.Query<cmlResInfoConfigSpc>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                        //++++++++++++++

                        //*Arm 64-01-25
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNSAppConfig.FTAppCode AS rtAppCode, TCNSAppConfig.FTCNKey AS rtCNKey, TCNSAppConfig.FNCNSeq AS rnCNSeq, ");
                        oSql.AppendLine("TCNSAppConfig.FTCNStaOnline AS rtCNStaOnline, TCNSAppConfig.FTCNStaValue AS rtCNStaValue, TCNSAppConfig.FTCNStaUse AS rtCNStaUse");
                        oSql.AppendLine("FROM TCNSAppConfig with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysConfig with(nolock) ON TCNSAppConfig.FTCNKey = TSysConfig.FTSysCode AND TCNSAppConfig.FNCNSeq = TSysConfig.FTSysSeq");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TSysConfig.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oSysConfigDwn.raAppConfig = oConn.Query<cmlResInfoAppConfig>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oSysConfigDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSysConfigDwn>();
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
        ///     Download system receive format information.
        /// </summary>
        /// <returns></returns>
        [Route("SysRcvFmt/Download")]
        [HttpGet]
        public cmlResItem<cmlResSysRcvFmtDwn> GET_SYSoDownloadSysRcvFmt()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSysRcvFmtDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSysRcvFmtDwn oSysRcvFmtDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSysRcvFmtDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysRcvFmt" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSysRcvFmtDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTFmtCode AS rtFmtCode, FTFmtKbRef AS rtFmtKbRef, FTFmtRef AS rtFmtRef,");
                oSql.AppendLine("FTFmtStaUsed AS rtFmtStaUsed, FTFmtStaAlwKeySum AS rtFmtStaAlwKeySum");
                oSql.AppendLine("FROM TSysRcvFmt with(nolock)");

                aoResult.roItem = new cmlResSysRcvFmtDwn();
                oSysRcvFmtDwn = new cmlResSysRcvFmtDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oSysRcvFmtDwn.raSysRcvFmt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysRcvFmt>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oSysRcvFmtDwn.raSysRcvFmt = oConn.Query<cmlResInfoSysRcvFmt>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oSysRcvFmtDwn.raSysRcvFmt.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSysRcvFmt_L.FTFmtCode AS rtFmtCode, TSysRcvFmt_L.FNLngID AS rnLngID, TSysRcvFmt_L.FTFmtName AS rtFmtName,TSysRcvFmt_L.FTRcvRmk AS rtRcvRmk");
                        oSql.AppendLine("FROM TSysRcvFmt_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysRcvFmt with(nolock) ON TSysRcvFmt_L.FTFmtCode = TSysRcvFmt.FTFmtCode");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oSysRcvFmtDwn.raSysRcvFmtLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysRcvFmtLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSysRcvFmtDwn.raSysRcvFmtLng = oConn.Query<cmlResInfoSysRcvFmtLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oSysRcvFmtDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSysRcvFmtDwn>();
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
        ///     Download system scale information.
        /// </summary>
        /// <returns></returns>
        [Route("SysScale/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysScale> GET_SYSoDownloadSysScale()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysScale> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysScale>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysScale" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysScale>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSslCode AS rtSslCode, FTSslName AS rtSslName, FTSslComSetting AS rtSslComSetting,");
                oSql.AppendLine("FTSslStaCom AS rtSslStaCom, FTSslStaTCP AS rtSslStaTCP");
                oSql.AppendLine("FROM TSysScale with(nolock)");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysScale>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    aoResult.raItems = oConn.Query<cmlResInfoSysScale>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysScale>();
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
        ///     Download system scale mapping information.
        /// </summary>
        /// <returns></returns>
        [Route("SysScaleMap/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysScaleMap> GET_SYSoDownloadSysScaleMap()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysScaleMap> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysScaleMap>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysScaleMap" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysScaleMap>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSslCode AS rtSslCode, FNSsmSeq AS rnSsmSeq, FTSsmDesc AS rtSsmDesc,");
                oSql.AppendLine("FNSsmFixLen AS rnSsmFixLen, FTSsmChrSplit AS rtSsmChrSplit, FTSsmChrExtra AS rtSsmChrExtra,");
                oSql.AppendLine("FTsmKey AS rtsmKey");
                oSql.AppendLine("FROM TSysScaleMap with(nolock)");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using(DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSysScaleMap>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    aoResult.raItems = oConn.Query<cmlResInfoSysScaleMap>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysScaleMap>();
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
        ///     Download system sync data information.
        /// </summary>
        /// <returns></returns>
        [Route("SysSyncData/Download")]
        [HttpGet]
        public cmlResItem<cmlResSyncDataDwn> GET_SYSoDownloadSysSyncData()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSyncDataDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSyncDataDwn oSyncDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSyncDataDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysSyncData" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSyncDataDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNSynSeqNo AS rnSynSeqNo, FTSynGroup AS rtSynGroup, FTSynTable AS rtSynTable,");
                oSql.AppendLine("FTSynTable_L AS rtSynTable_L, FTSynType AS rtSynType, FDSynLast AS rdSynLast,");
                oSql.AppendLine("FNSynSchedule AS rnSynSchedule, FTSynStaUse AS rtSynStaUse,");
                oSql.AppendLine("FTSynUriDwn AS rtSynUriDwn, FTSynUriUld AS rtSynUriUld");
                oSql.AppendLine("FROM TSysSyncData with(nolock)");

                aoResult.roItem = new cmlResSyncDataDwn();
                oSyncDataDwn = new cmlResSyncDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oSyncDataDwn.raSyncData = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSyncData>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oSyncDataDwn.raSyncData = oConn.Query<cmlResInfoSyncData>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oSyncDataDwn.raSyncData.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSysSyncData_L.FNSynSeqNo AS rnSynSeqNo, TSysSyncData_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TSysSyncData_L.FTSynName AS rtSynName, TSysSyncData_L.FTSynRmk AS rtSynRmk");
                        oSql.AppendLine("FROM TSysSyncData_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysSyncData with(nolock) ON TSysSyncData_L.FNSynSeqNo = TSysSyncData.FNSynSeqNo");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oSyncDataDwn.raSyncDataLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSyncDataLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSyncDataDwn.raSyncDataLng = oConn.Query<cmlResInfoSyncDataLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Module  *Arm 63-07-08
                        oSql.Clear();
                        oSql.AppendLine("SELECT TSysSyncModule.FTAppCode AS rtAppCode, TSysSyncModule.FNSynSeqNo AS rnSynSeqNo ");
                        oSql.AppendLine("FROM TSysSyncModule with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysSyncData with(nolock) ON TSysSyncModule.FNSynSeqNo = TSysSyncData.FNSynSeqNo");
                        oSyncDataDwn.raSyncModule = oConn.Query<cmlResInfoSyncModule>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oSyncDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSyncDataDwn>();
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
        ///     Download system receive by application information.
        /// </summary>
        /// <returns></returns>
        //*Arm 63-01-10 [comment code] เลิกใช้ TSysRcvApp
        //-----------------------------------------------
        //[Route("SysRcvApp/Download")]
        //[HttpGet]
        //public cmlResList<cmlTSysRcvApp> GET_SYSoDownloadSysRcvApp()
        //{
        //    cSP oFunc;
        //    cCS oCS;
        //    cMS oMsg;
        //    StringBuilder oSql;
        //    cmlResList<cmlTSysRcvApp> aoResult;
        //    List<cmlTSysConfig> aoSysConfig;
        //    //cmlResSysDataDwn oSysDataDwn;
        //    cCacheFunc oCacheFunc;
        //    int nRowEff, nCmdTme, nConTme;
        //    string tFuncName, tModelErr, tKeyApi, tKeyCache;
        //    bool bHaveData;
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        //        aoResult = new cmlResList<cmlTSysRcvApp>();
        //        //aoResult = new cmlResPdtItemDwn();
        //        oFunc = new cSP();
        //        oCS = new cCS();
        //        oMsg = new cMS();
        //        oCacheFunc = new cCacheFunc(43200, 43200, false);
        //        bHaveData = false;

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

        //        tKeyCache = "SysRcvApp" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
        //        if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
        //        {
        //            // ถ้ามี key อยุ่ใน cache
        //            aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlTSysRcvApp>>(tKeyCache);
        //            //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
        //            aoResult.rtCode = oMsg.tMS_RespCode001;
        //            aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //            return aoResult;
        //        }

        //        // Get data
        //        oSql = new StringBuilder();
        //        oSql.AppendLine("SELECT FTAppCode AS rtAppCode, FNAppSeq AS rnAppSeq, FTFmtCode AS rtFmtCode,");
        //        oSql.AppendLine("FNLngID AS rnLngID, FTAppName AS rtAppName, FTAppStaAlwRet AS rtAppStaAlwRet,");
        //        oSql.AppendLine("FTAppStaAlwCancel AS rtAppStaAlwCancel, FTAppStaPayLast AS rtAppStaPayLast");
        //        oSql.AppendLine("FROM TSysRcvApp WITH(NOLOCK)");
        //        using (DbConnection oConn = new cDatabase().C_CONoDatabase())
        //        {
        //            aoResult.raItems = oConn.Query<cmlTSysRcvApp>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
        //            if (aoResult.raItems.Count > 0)
        //            {

        //            }
        //            else
        //            {
        //                aoResult.rtCode = oMsg.tMS_RespCode800;
        //                aoResult.rtDesc = oMsg.tMS_RespDesc800;
        //                return aoResult;
        //            }
        //        }
        //        //}

        //        //aoResult.roItem = oSysDataDwn;
        //        // เก็บ KeyApi ลง Cache
        //        oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

        //        aoResult.rtCode = oMsg.tMS_RespCode001;
        //        aoResult.rtDesc = oMsg.tMS_RespDesc001;
        //        return aoResult;
        //    }
        //    catch (Exception oExcept)
        //    {
        //        // Return error.
        //        aoResult = new cmlResList<cmlTSysRcvApp>();
        //        //aoResult = new cmlResPdtItemDwn();
        //        aoResult.rtCode = new cMS().tMS_RespCode900;
        //        aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();
        //        return aoResult;
        //    }
        //    finally
        //    {
        //        oFunc = null;
        //        oCS = null;
        //        oMsg = null;
        //        oSql = null;

        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        //GC.Collect();
        //    }
        //}

        /// <summary>
        ///     Download system receive config information.
        /// </summary>
        /// <returns></returns>
        [Route("SysRcvConfig/Download")]
        [HttpGet]
        public cmlResList<cmlTSysRcvConfig> GET_SYSoDownloadSysRcvConfig()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlTSysRcvConfig> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlTSysRcvConfig>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "SysRcvConfig" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlTSysRcvConfig>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTFmtCode AS rtFmtCode, FNSysSeq AS rnSysSeq, FTSysKey AS rtSysKey,");
                oSql.AppendLine("FTSysStaUsrValue AS rtSysStaUsrValue, FTSysStaUsrRef AS rtSysStaUsrRef, FTBchCode AS rtBchCode");
                oSql.AppendLine("FROM TSysRcvConfig WITH(NOLOCK)");
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlTSysRcvConfig>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
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
                //}

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlTSysRcvConfig>();
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
        ///     Download system Pos Model information.
        /// </summary>
        /// <returns></returns>
        [Route("SysPosModel/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysPosModel> GET_SYSoDownloadSysPosModel()
        {
            // *Arm 63-01-17  - Create Function GET_SYSoDownloadSysPosModel()
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysPosModel> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysPosModel>();
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

                tKeyCache = "SysPosModel" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysPosModel>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSpmCode AS rtSpmCode, FTSpmBrand AS rtSpmBrand, FTSpmName AS rtSpmName,");
                oSql.AppendLine("FTSpmNameEng AS rtSpmNameEng, FTSpmSystem AS rtSpmSystem, FTSpmRemark AS rtSpmRemark");
                oSql.AppendLine("FROM TSysPosModel with(nolock)");

                
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    
                    aoResult.raItems = oConn.Query<cmlResInfoSysPosModel>(oSql.ToString(), nCmdTme).ToList();
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
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysPosModel>();
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
        ///     Download system Document Approve information.
        /// </summary>
        /// <returns></returns>
        [Route("SysDocApv/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoSysDocApv> GET_SYSoDownloadSysDovApv()
        {
            // *Arm 63-01-17  - Create Function GET_SYSoDownloadSysDovApv()

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoSysDocApv> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoSysDocApv>();
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

                tKeyCache = "SysPosModel" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoSysDocApv>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNDapID AS rnDapID, FTDapTable AS rtDapTable, FTDapRefType AS rtDapRefType,");
                oSql.AppendLine("FNDapSeq AS rnDapSeq, FTDapName AS rtDapName, FTDapNameOth AS rtDapNameOth");
                oSql.AppendLine("FROM TSysDocApv with(nolock)");


                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {

                    aoResult.raItems = oConn.Query<cmlResInfoSysDocApv>(oSql.ToString(), nCmdTme).ToList();
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
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoSysDocApv>();
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
        /// Discount Policy
        /// </summary>
        /// <returns></returns>
        [Route("SysDisPolicy/Download")]
        [HttpGet]
        public cmlResItem<cmlResDisPolicyDwn> GET_SYSoDownloadDisPolicy()
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResDisPolicyDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResDisPolicyDwn oPolicyDwnDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResDisPolicyDwn>();
                //aoResult = new cmlResPdtItemDwn();
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

                tKeyCache = "DisPolicy" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResDisPolicyDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTDisCode AS rtDisCode, FTDisGroup AS rtDisGroup, FNDisLevel AS rnDisLevel, FTDisStaUse AS rtDisStaUse,");
                oSql.AppendLine("FTDisPosFunc AS rtDisPosFunc, FTDisStaPrice AS rtDisStaPrice, FTDisCodeRef AS rtDisCodeRef,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TSysDisPolicy WITH(NOLOCK) ");
                
                aoResult.roItem = new cmlResDisPolicyDwn();
                oPolicyDwnDwn = new cmlResDisPolicyDwn();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    
                    oPolicyDwnDwn.raDisPolicy = oConn.Query<cmlResInfoDisPolicy>(oSql.ToString(), nCmdTme).ToList();
                    if (oPolicyDwnDwn.raDisPolicy.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSysDisPolicy_L.FTDisCode AS rtDisCode, TSysDisPolicy_L.FNLngID AS rnLngID, TSysDisPolicy_L.FTDisName AS rtDisName, TSysDisPolicy_L.FTDisRemark AS rtDisRemark ");
                        oSql.AppendLine("FROM TSysDisPolicy_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysDisPolicy with(nolock) ON TSysDisPolicy_L.FTDisCode = TSysDisPolicy.FTDisCode");
                        oPolicyDwnDwn.raDisPolicyLng = oConn.Query<cmlResInfoDisPolicyLng>(oSql.ToString(), nCmdTme).ToList();

                        // TPSTDiscPolicy
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TPSTDiscPolicy.FTDpcDisCodeX AS rtDpcDisCodeX, TPSTDiscPolicy.FTDpcDisCodeY AS rtDpcDisCodeY, TPSTDiscPolicy.FTDpcStaAlw AS rtDpcStaAlw,");
                        oSql.AppendLine("TPSTDiscPolicy.FDLastUpdOn AS rdLastUpdOn, TPSTDiscPolicy.FTLastUpdBy AS rtLastUpdBy, TPSTDiscPolicy.FDCreateOn AS rdCreateOn, TPSTDiscPolicy.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TPSTDiscPolicy with(nolock)");
                        oSql.AppendLine("INNER JOIN TSysDisPolicy with(nolock) ON TPSTDiscPolicy.FTDpcDisCodeX = TSysDisPolicy.FTDisCode");
                        oPolicyDwnDwn.raTPSTDiscPolicy = oConn.Query<cmlResInfoTPSTDisPolicy>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }

                aoResult.roItem = oPolicyDwnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResDisPolicyDwn>();
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