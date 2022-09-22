using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.PdtLayout;
using API2PSMaster.Models.WebService.Response.Vending;
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
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Vending Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Vending")]
    public class cVendingController : ControllerBase
    {
        /// <summary>
        ///     Download product layout information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <param name="ptShop">Shop code.</param>
        /// <returns>
        ///     Product layout information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("PdtLayout/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtLayoutDwn> GET_PDToDownloadPdtLayout(DateTime pdDate,string ptShop)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtLayoutDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtLayoutDwn oZoneDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtLayoutDwn>();
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

                tKeyCache = "PdtLayout" + ptShop + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtLayoutDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTMerCode AS rtMerCode, FTShpCode AS rtShpCode, FNLayRow AS rnLayRow, FNLayCol AS rnLayCol,");
                oSql.AppendLine("FTPdtCode AS rtPdtCode, FCLayColQtyMax AS rcLayColQtyMax, FCLayDim AS rcLayDim,");
                oSql.AppendLine("FCLayHigh AS rcLayHigh, FCLayWide AS rcLayWide, FTLayStaUse AS rtLayStaUse,");
                oSql.AppendLine("FNCabSeq AS rnCabSeq, FTLayStaCtrlXY AS rtLayStaCtrlXY, FTWahCode AS rtWahCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TVDMPdtLayout WITH(NOLOCK)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSql.AppendLine("AND FTShpCode = '" + ptShop + "' ");

                aoResult.roItem = new cmlResPdtLayoutDwn();
                oZoneDwn = new cmlResPdtLayoutDwn();
               
                    using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                    {
                        //*Em 62-06-08
                        oZoneDwn.raPdtLayout = oConn.Query<cmlResInfoPdtLayout>(oSql.ToString(), nCmdTme).ToList();
                        if (oZoneDwn.raPdtLayout.Count > 0)
                        {
                            //Languague
                            oSql = new StringBuilder();
                            oSql.AppendLine("SELECT distinct TVDMPdtLayout_L.FTBchCode AS rtBchCode, TVDMPdtLayout_L.FTShpCode AS rtShpCode, TVDMPdtLayout_L.FNLayRow AS rnLayRow,");
                            oSql.AppendLine("TVDMPdtLayout_L.FNLayCol AS rnLayCol, TVDMPdtLayout_L.FNLngID AS rnLngID,");
                            oSql.AppendLine("TVDMPdtLayout_L.FTLayName AS rtLayName, TVDMPdtLayout_L.FTLayRemark AS rtLayRemark");
                            oSql.AppendLine("FROM TVDMPdtLayout_L with(nolock)");
                            oSql.AppendLine("INNER JOIN TVDMPdtLayout with(nolock) ON TVDMPdtLayout_L.FTBchCode = TVDMPdtLayout.FTBchCode AND TVDMPdtLayout_L.FTShpCode = TVDMPdtLayout.FTShpCode");
                            oSql.AppendLine("AND TVDMPdtLayout_L.FNLayRow = TVDMPdtLayout.FNLayRow AND TVDMPdtLayout_L.FNLayCol = TVDMPdtLayout.FNLayCol");
                            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TVDMPdtLayout.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                            oSql.AppendLine("AND TVDMPdtLayout.FTShpCode = '" + ptShop + "' ");

                            oZoneDwn.raPdtLayoutLng = oConn.Query<cmlResInfoPdtLayoutLng>(oSql.ToString(), nCmdTme).ToList();   //*Em 62-06-08
                        }
                        else
                        {
                            aoResult.rtCode = oMsg.tMS_RespCode800;
                            aoResult.rtDesc = oMsg.tMS_RespDesc800;
                            return aoResult;
                        }
                    }
                

                aoResult.roItem = oZoneDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtLayoutDwn>();
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
        ///     Download Pos shop information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <param name="ptShop">Shop Code.</param>
        /// <returns>
        ///     Pos shop information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("PosShop/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoPosShop> GET_PDToDownloadPosShop(DateTime pdDate, string ptShop)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoPosShop> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoPosShop>();
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

                tKeyCache = "PosShop" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoPosShop>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTShpCode AS rtShpCode, FTPosCode AS rtPosCode,");
                oSql.AppendLine("FTPshPosSN AS rtPshPosSN, FTPshStaUse AS rtPshStaUse, FTShpSceLayout AS rtShpSceLayout,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TVDMPosShop with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSql.AppendLine("AND FTShpCode = '" + ptShop + "'");

                aoResult.raItems = new List<cmlResInfoPosShop>();
               
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())   //*Em 62-06-08
                {
                    aoResult.raItems = oConn.Query<cmlResInfoPosShop>(oSql.ToString(), nCmdTme).ToList();   //*Em 62-06-08
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
                aoResult = new cmlResList<cmlResInfoPosShop>();
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
        ///     Download shop type information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns>
        ///     Shop type information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ShopType/Download")]
        [HttpGet]
        public cmlResItem<cmlResShopTypeDwn> GET_PDToDownloadShopType(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResShopTypeDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResShopTypeDwn oShopTypeDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResShopTypeDwn>();
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

                tKeyCache = "ShopType" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResShopTypeDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTShtCode AS rtShtCode, FTShtType AS rtShtType,");
                oSql.AppendLine("FNShtValue AS rnShtValue, FNShtMin AS rnShtMin, FNShtMax AS rnShtMax,"); 
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TVDMShopType with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                
                aoResult.roItem = new cmlResShopTypeDwn();
                oShopTypeDwn = new cmlResShopTypeDwn();
                
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oShopTypeDwn.raShopType = oConn.Query<cmlResInfoShopType>(oSql.ToString(), nCmdTme).ToList();
                    if (oShopTypeDwn.raShopType.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TVDMShopType_L.FTShtCode AS rtShtCode, TVDMShopType_L.FNLngID AS rnLngID, TVDMShopType_L.FTShtName AS rtShtName, TVDMShopType_L.FTShtRemark AS rtShtRemark");
                        oSql.AppendLine("FROM TVDMShopType_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TVDMShopType with(nolock) ON TVDMShopType_L.FTShtCode = TVDMShopType.FTShtCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TVDMShopType.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        
                        oShopTypeDwn.raShopTypeLng = oConn.Query<cmlResInfoShopTypeLng>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }

                aoResult.roItem = oShopTypeDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResShopTypeDwn>();
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
        ///     Download Product stock balance information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns>
        ///     Product stock balance information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("PdtStkBal/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoPdtStkBalVD> GET_PDToDownloadPdtStkBal(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoPdtStkBalVD> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoPdtStkBalVD>();
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

                tKeyCache = "PdtStkBalVD" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoPdtStkBalVD>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTWahCode AS rtWahCode, FNLayRow AS rnLayRow,");
                oSql.AppendLine("FNLayCol AS rnLayCol, FTPdtCode AS rtPdtCode, FCStkQty AS rcStkQty,"); //*Em 62-06-18
                oSql.AppendLine("FNCabSeq AS rnCabSeq,"); //*Arm 63-1-01-16
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TVDTPdtStkBal with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.raItems = new List<cmlResInfoPdtStkBalVD>();
                
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                   
                    aoResult.raItems = oConn.Query<cmlResInfoPdtStkBalVD>(oSql.ToString(), nCmdTme).ToList();   //*Em 62-06-08
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
                aoResult = new cmlResList<cmlResInfoPdtStkBalVD>();
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
        ///     Download ShopCabinet information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <param name="ptShop">Shop Code.</param>
        /// <returns>
        ///     Shop layout information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ShopCabinet/Download")]
        [HttpGet]
        public cmlResItem<cmlResShopCabinetDwn> GET_PDToDownloadShopCabinet(DateTime pdDate, string ptShop)
        {
            // *Arm 63-01-16  Create Function

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResShopCabinetDwn> aoResult = new cmlResItem<cmlResShopCabinetDwn>();
            cmlResShopCabinetDwn oShopCabinetDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResShopCabinetDwn>();
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

                tKeyCache = "ShopCabinet" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResShopCabinetDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTShpCode AS rtShpCode, FNCabSeq AS rnCabSeq,");
                oSql.AppendLine("FNCabMaxRow AS rnCabMaxRow, FNCabMaxCol AS rnCabMaxCol, FNCabType AS rnCabType,");
                oSql.AppendLine("FTShtCode AS rtShtCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TVDMShopCabinet WITH(NOLOCK)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSql.AppendLine("AND FTShpCode = '"+ ptShop + "' ");
                
                aoResult.roItem = new cmlResShopCabinetDwn();
                oShopCabinetDwn = new cmlResShopCabinetDwn();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oShopCabinetDwn.raShopCabinet = oConn.Query<cmlResInfoShopCabinet>(oSql.ToString(), nCmdTme).ToList();
                    if (oShopCabinetDwn.raShopCabinet.Count > 0)
                    {
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TSC_L.FNCabSeq AS rnCabSeq, TSC_L.FNLngID AS rnLngID, TSC_L.FTCabName AS rtCabName,");
                        oSql.AppendLine("TSC_L.FTCabRmk AS rtCabRmk, TSC_L.FTShpCode AS rtShpCode");
                        oSql.AppendLine("FROM TVDMShopCabinet_L TSC_L WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TVDMShopCabinet TSC WITH(NOLOCK) ON TSC_L.FNCabSeq = TSC.FNCabSeq AND TSC_L.FTShpCode = TSC.FTShpCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TSC.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oSql.AppendLine("AND TSC.FTShpCode = '" + ptShop + "' ");
                        
                        oShopCabinetDwn.raShopCabinetLng = oConn.Query<cmlResInfoShopCabinetLng>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }

                aoResult.roItem = oShopCabinetDwn;

                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;

            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResShopCabinetDwn>();
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