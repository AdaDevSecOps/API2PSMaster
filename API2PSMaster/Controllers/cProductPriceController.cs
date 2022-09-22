using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Product;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage system data.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PdtPrice")]
    public class cProductPriceController : ControllerBase
    {
        /// <summary>
        ///     Download system document type information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <param name="ptBchCode"></param>
        /// <returns></returns>
        [Route("Price4Pdt/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoPdtApv4Pdt> GET_SYSoDownloadPrice4Pdt(DateTime pdDate, string ptBchCode ="")
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql = new StringBuilder();
            cmlResList<cmlResInfoPdtApv4Pdt> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            cDatabase oDB;      //*Arm 62-10-18
            DataTable oDbTbl ;     //*Arm 62-10-18
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            bool bThisBch = false;  //*Arm 62-10-18

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoPdtApv4Pdt>();
                //aoResult = new cmlResPdtItemDwn();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);
                bHaveData = false;

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;


                //*Arm 62-10-18 - ปิดเงื่อนไขการเช็ค Validate parameter.
                // Validate parameter.
                //tModelErr = "";
                //if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                //{
                    // Validate parameter model false.
                    //aoResult.rtCode = oMsg.tMS_RespCode701;
                    //aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    //return aoResult;
                //}


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

                tKeyCache = "PdtPri4Pdt" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoPdtApv4Pdt>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                oDB = new cDatabase(); //*Arm 62-10-18
                oDbTbl = new DataTable(); //*Arm 62-10-18

                //*Arm 62-10-18
                //แก้ไข เช็คสาขาที่ส่งมาเป็นสาขาเดียวกันหรือไม่
                if (string.IsNullOrEmpty(ptBchCode))
                {
                    bThisBch = true;
                }
                else
                {
                    oSql.Clear();
                    oSql.AppendLine("select FTBchcode From TCNMComp WHERE FTBchcode = '" + ptBchCode + "' ");
                    oDbTbl = oDB.C_DAToSqlQuery(oSql.ToString());
                    if(oDbTbl == null)
                    {
                        bThisBch = false;
                    }
                    else
                    {
                        if(oDbTbl.Rows.Count >0)
                        {
                            bThisBch = true;
                        }
                        else
                        {
                            bThisBch = false;
                        }
                    }
                }

                if (bThisBch)
                {
                    // Get data
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPunCode AS rtPunCode, FDPghDStart AS rdPghDStart,");
                    oSql.AppendLine("FTPplCode AS rtPplCode, "); //*Arm 63-03-24
                    oSql.AppendLine("FTPghTStart AS rtPghTStart, FDPghDStop AS rdPghDStop, FTPghTStop AS rtPghTStop,");
                    oSql.AppendLine("FTPghDocNo AS rtPghDocNo, FTPghDocType AS rtPghDocType, FTPghStaAdj AS rtPghStaAdj,");
                    //oSql.AppendLine("FCPgdPriceRet AS rcPgdPriceRet, FCPgdPriceWhs AS rcPgdPriceWhs, FCPgdPriceNet AS rcPgdPriceNet,"); //*Arm 63-06-16
                    oSql.AppendLine("FCPgdPriceRet AS rcPgdPriceRet,"); //*Arm 63-06-16
                    oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNTPdtPrice4Pdt with(nolock)");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                }
                else
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPunCode AS rtPunCode, FDPghDStart AS rdPghDStart,");
                    oSql.AppendLine("FTPghTStart AS rtPghTStart, FDPghDStop AS rdPghDStop, FTPghTStop AS rtPghTStop,");
                    oSql.AppendLine("FTPghDocNo AS rtPghDocNo, FTPghDocType AS rtPghDocType, FTPghStaAdj AS rtPghStaAdj,");
                    oSql.AppendLine("FCPgdPriceRet AS rcPgdPriceRet, FCPgdPriceWhs AS rcPgdPriceWhs, FCPgdPriceNet AS rcPgdPriceNet,");
                    oSql.AppendLine("FCPgdPriceRet AS rcPgdPriceRet,"); //*Arm 63-06-16
                    oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNTPdtPrice4BCH with(nolock)");
                    oSql.AppendLine("WHERE (FTPghBchTo = '*' OR FTPghBchTo ='" + ptBchCode + "') ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), FDPghDStop, 121) >= '" + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + "' ");

                    oSql.AppendLine("UNION ");

                    oSql.AppendLine("SELECT  P4Z.FTPdtCode AS rtPdtCode,  P4Z.FTPunCode AS rtPunCode,  P4Z.FDPghDStart AS rdPghDStart,");
                    oSql.AppendLine("P4Z.FTPghTStart AS rtPghTStart,  P4Z.FDPghDStop AS rdPghDStop,  P4Z.FTPghTStop AS rtPghTStop,");
                    oSql.AppendLine("P4Z.FTPghDocNo AS rtPghDocNo, P4Z.FTPghDocType AS rtPghDocType, P4Z.FTPghStaAdj AS rtPghStaAdj,");
                    oSql.AppendLine("P4Z.FCPgdPriceRet AS rcPgdPriceRet, P4Z.FCPgdPriceWhs AS rcPgdPriceWhs, P4Z.FCPgdPriceNet AS rcPgdPriceNet,");
                    oSql.AppendLine("P4Z.FDLastUpdOn AS rdLastUpdOn, P4Z.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("P4Z.FTLastUpdBy AS rtLastUpdBy, P4Z.FTCreateBy AS rtCreateBy ");
                    oSql.AppendLine("FROM TCNTPdtPrice4ZNE P4Z WITH(NOLOCK) ");
                    oSql.AppendLine("INNER JOIN TCNMZone ZNE WITH(NOLOCK) ON ZNE.FTZneChain = P4Z.FTPghZneTo ");
                    oSql.AppendLine("INNER JOIN TCNMZoneObj ZOJ WITH(NOLOCK) ON ZOJ.FTZneChain = ZNE.FTZneChain AND ZOJ.FTZneTable = 'TCNMBranch' AND ZOJ.FTZneRefCode = '"+ ptBchCode + "' ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), P4Z.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), P4Z.FDPghDStop, 121) >= '" + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + "' ");
                }   // *Arm 62-10-17 

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                //{
                //oConn.Open();
                //DbCommand oCmd = oConn.CreateCommand();
                //oCmd.CommandText = oSql.ToString();
                //using (DbDataReader oDR = oCmd.ExecuteReader())
                //{
                //aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtApv4Pdt>(oDR).ToList();
                //((IDisposable)oDR).Dispose();
                //}

                //if (aoResult.raItems.Count > 0)
                //{

                //}
                //else
                //{
                //aoResult.rtCode = oMsg.tMS_RespCode800;
                //aoResult.rtDesc = oMsg.tMS_RespDesc800;
                //return aoResult;
                //}
                //}
                //}

                //*Arm 62-10-10  - แก้ไขใช้ dapper Query
                aoResult.raItems = new List<cmlResInfoPdtApv4Pdt>();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlResInfoPdtApv4Pdt>(oSql.ToString(), nCmdTme).ToList();
                }

                if (aoResult.raItems.Count > 0)
                {

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

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
                aoResult = new cmlResList<cmlResInfoPdtApv4Pdt>();
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
        ///     Download system document type information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <returns></returns>
        [Route("Price4Cst/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoPdtApv4Cst> GET_SYSoDownloadPrice4Cst(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoPdtApv4Cst> aoResult;
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

                aoResult = new cmlResList<cmlResInfoPdtApv4Cst>();
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

                tKeyCache = "PdtPri4Cst" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoPdtApv4Cst>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPplCode AS rtPplCode, FTPdtCode AS rtPdtCode, FTPunCode AS rtPunCode,");
                oSql.AppendLine("FDPghDStart AS rdPghDStart, FTPghTStart AS rtPghTStart, FDPghDStop AS rdPghDStop,");
                oSql.AppendLine("FTPghTStop AS rtPghTStop, FTPghDocNo AS rtPghDocNo, FTPghDocType AS rtPghDocType,");
                oSql.AppendLine("FTPghStaAdj AS rtPghStaAdj, FCPgdPriceRet AS rcPgdPriceRet, FCPgdPriceWhs AS rcPgdPriceWhs,");
                oSql.AppendLine("FCPgdPriceNet AS rcPgdPriceNet,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNTPdtPrice4CST with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                //sing (AdaAccEntities oDB = new AdaAccEntities())
                //{
                    //using (DbConnection oConn = oDB.Database.Connection)
                    //{
                        //oConn.Open();
                        //DbCommand oCmd = oConn.CreateCommand();
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                            //aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtApv4Cst>(oDR).ToList();
                            //((IDisposable)oDR).Dispose();
                        //}

                        //if (aoResult.raItems.Count > 0)
                        //{

                        //}
                        //else
                        //{
                            //aoResult.rtCode = oMsg.tMS_RespCode800;
                            //aoResult.rtDesc = oMsg.tMS_RespDesc800;
                            //return aoResult;
                        //}
                    //}
                //}

                //*Arm 62-10-10  - แก้ไขใช้ dapper Query
                aoResult.raItems = new List<cmlResInfoPdtApv4Cst>();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlResInfoPdtApv4Cst>(oSql.ToString(), nCmdTme).ToList();
                }

                if (aoResult.raItems.Count > 0)
                {

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                
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
                aoResult = new cmlResList<cmlResInfoPdtApv4Cst>();
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



        /*
        /// <summary>
        ///     Download system document type information.
        /// </summary>
        /// <param name="ptPghBchTo"></param>
        /// <param name="pdDate"></param>
        /// <returns></returns>
        [Route("Price4BCH/Download")]
        [HttpGet]
        //*Arm 62-10-10 -Created  Price4BCH/Download
        public cmlResList<cmlResInfoPdtApv4Pdt> GET_SYSoDownloadPrice4BCH(DateTime pdDate, string ptPghBchTo)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoPdtApv4Pdt> aoResult;
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

                aoResult = new cmlResList<cmlResInfoPdtApv4Pdt>();
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

                tKeyCache = "PdtPri4BCH" + ptPghBchTo + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoPdtApv4Pdt>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPunCode AS rtPunCode, FDPghDStart AS rdPghDStart,");
                oSql.AppendLine("FTPghTStart AS rtPghTStart, FDPghDStop AS rdPghDStop, FTPghTStop AS rtPghTStop,");
                oSql.AppendLine("FTPghDocNo AS rtPghDocNo, FTPghDocType AS rtPghDocType, FTPghStaAdj AS rtPghStaAdj,");
                oSql.AppendLine("FCPgdPriceRet AS rcPgdPriceRet, FCPgdPriceWhs AS rcPgdPriceWhs, FCPgdPriceNet AS rcPgdPriceNet,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNTPdtPrice4BCH with(nolock)");
                oSql.AppendLine("WHERE (FTPghBchTo = '*' OR FTPghBchTo ='" + ptPghBchTo + "') ");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDPghDStop, 121) >= '" + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + "' ");
                //aoResult.raItems = new cmlResList<cmlResInfoSysDocType>();
                //oSysDataDwn = new cmlResSysDataDwn();
                aoResult.raItems = new List<cmlResInfoPdtApv4Pdt>();

                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlResInfoPdtApv4Pdt>(oSql.ToString(), nCmdTme).ToList();
                }

                if (aoResult.raItems.Count > 0)
                {

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
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
                aoResult = new cmlResList<cmlResInfoPdtApv4Pdt>();
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
        */
    }
}