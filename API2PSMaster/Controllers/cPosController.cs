using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Pos;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.POS;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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
    ///     Point of sale infomation.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Pos")]
    public class cPosController : ControllerBase
    {
        /// <summary>
        ///     Download Pos information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResPosDwn> GET_PDToDownloadPos(DateTime pdDate, string ptBchCode)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPosDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPosDwn oPosDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPosDwn>();
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

                tKeyCache = "Pos" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPosDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTPosCode AS rtPosCode, FTPosType AS rtPosType, FTPosRegNo AS rtPosRegNo, FTSmgCode AS rtSmgCode,");    //*Arm 63-01-23
                oSql.AppendLine("FTPosStaRorW AS rtPosStaRorW, FTPosStaPrnEJ AS rtPosStaPrnEJ, FTPosStaVatSend AS rtPosStaVatSend, FTPosStaUse AS rtPosStaUse,");
                oSql.AppendLine("FTPosStaShift AS rtPosStaShift,FTPosStaSumScan AS rtPosStaSumScan, FTPosStaSumPrn AS rtPosStaSumPrn,"); //*Arm 63-05-05 เพิ่ม FTPosStaSumScan, FTPosStaSumPrn
                oSql.AppendLine("FTPosStaDate AS rtPosStaDate,FTPrgRegToken AS rtPrgRegToken,"); //*Arm 63-06-15 เพิ่ม FTPosStaDate ปรับตามโครงสร้าง DataBase SKC
                oSql.AppendLine("FTChnCode AS rtChnCode,"); //*Net 63-12-25 เพิ่ม FTChnCode
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPos with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '"+ ptBchCode +"'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResPosDwn();
                oPosDwn = new cmlResPosDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                //using (DbConnection oConn = oDB.Database.Connection)
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oPosDwn.raPos = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPos>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oPosDwn.raPos = oConn.Query<cmlResInfoPos>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17
                    if (oPosDwn.raPos.Count > 0)
                    {
                        //*Arm 63-04-08
                        //Pos Languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPos_L.FTBchCode AS rtBchCode, TCNMPos_L.FTPosCode AS rtPosCode, TCNMPos_L.FNLngID AS rnLngID, TCNMPos_L.FTPosName AS rtPosName ,TCNMPos_L.FTPosNameOth AS PosNameOth,TCNMPos_L.FTPosRmk AS rtPosRmk ");
                        oSql.AppendLine("FROM TCNMPos_L with(nolock) ");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPos_L.FTPosCode = TCNMPos.FTPosCode AND TCNMPos_L.FTBchCode = TCNMPos.FTBchCode ");
                        oSql.AppendLine("WHERE TCNMPos.FTBchCode = '" + ptBchCode + "'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oPosDwn.raPosLng = oConn.Query<cmlResInfoPosLng>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                        //Pos Hardware
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPosHW.FTBchCode AS rtBchCode, TCNMPosHW.FTPhwCode AS rtPhwCode, TCNMPosHW.FTPosCode AS rtPosCode, TCNMPosHW.FTShwCode AS rtShwCode,");     //*Arm 63-01-23
                        oSql.AppendLine("TCNMPosHW.FTPhwCodeRef AS rtPhwCodeRef, TCNMPosHW.FNPhwSeq AS rnPhwSeq, TCNMPosHW.FTPhwName AS rtPhwName,");
                        oSql.AppendLine("TCNMPosHW.FTPhwConnType AS rtPhwConnType, TCNMPosHW.FTPhwConnRef AS rtPhwConnRef, TCNMPosHW.FTPhwCustom AS rtPhwCustom,");
                        oSql.AppendLine("TCNMPosHW.FNPhwTimeOut AS rnPhwTimeOut");
                        oSql.AppendLine("FROM TCNMPosHW with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPos.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPosHW.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raPosHW = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPosHW>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raPosHW = oConn.Query<cmlResInfoPosHW>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17

                        //Pos LastNo
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPosLastNo.FTPosCode AS rtPosCode, TCNMPosLastNo.FNPosDocType AS rnPosDocType, TCNMPosLastNo.FTPosComName AS rtPosComName,");
                        oSql.AppendLine("TCNMPosLastNo.FNPosLastNo AS rnPosLastNo, TCNMPosLastNo.FDPosLastSale AS rdPosLastSale");
                        oSql.AppendLine("FROM TCNMPosLastNo with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosLastNo.FTPosCode = TCNMPos.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPos.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raPosLastNo = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPosLastNo>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raPosLastNo = oConn.Query<cmlResInfoPosLastNo>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17

                        //Edc
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TFNMEdc.FTEdcCode AS rtEdcCode, TFNMEdc.FTSedCode AS rtSedCode, TFNMEdc.FTBnkCode AS rtBnkCode,");
                        oSql.AppendLine("TFNMEdc.FTEdcShwFont AS rtEdcShwFont, TFNMEdc.FTEdcShwBkg AS rtEdcShwBkg, TFNMEdc.FTEdcOther AS rtEdcOther,");
                        oSql.AppendLine("TFNMEdc.FDLastUpdOn AS rdLastUpdOn, TFNMEdc.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TFNMEdc.FTLastUpdBy AS rtLastUpdBy, TFNMEdc.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TFNMEdc with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPosHW with(nolock) ON TFNMEdc.FTEdcCode = TCNMPosHW.FTPhwCodeRef");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPos.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPosHW.FTBchCode = '" + ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raEdc = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoEdc>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raEdc = oConn.Query<cmlResInfoEdc>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17

                        //Edc Languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TFNMEdc_L.FTEdcCode AS rtEdcCode, TFNMEdc_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TFNMEdc_L.FTEdcName AS rtEdcName, TFNMEdc_L.FTEdcRmk AS rtEdcRmk");
                        oSql.AppendLine("FROM TFNMEdc_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPosHW with(nolock) ON TFNMEdc_L.FTEdcCode = TCNMPosHW.FTPhwCodeRef");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPos.FTPosCode");       //[BOY][63-03-27]
                        //oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPosHW.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPosHW.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raEdcLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoEdcLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raEdcLng = oConn.Query<cmlResInfoEdcLng>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17

                        //System edc
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TSysEdc.FTSedCode AS rtSedCode, TSysEdc.FTSedModel AS rtSedModel, TSysEdc.FNSedAck AS rnSedAck,");
                        oSql.AppendLine("TSysEdc.FTSedDllVer AS rtSedDllVer, TSysEdc.FNSedTimeOut AS rnSedTimeOut,");
                        oSql.AppendLine("TSysEdc.FDLastUpdOn AS rdLastUpdOn, TSysEdc.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TSysEdc.FTLastUpdBy AS rtLastUpdBy, TSysEdc.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TSysEdc with(nolock)");
                        oSql.AppendLine("INNER JOIN TFNMEdc with(nolock) ON TSysEdc.FTSedCode = TFNMEdc.FTSedCode");
                        oSql.AppendLine("INNER JOIN TCNMPosHW with(nolock) ON TFNMEdc.FTEdcCode = TCNMPosHW.FTPhwCodeRef");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPos.FTPosCode");    //[BOY][63-03-27]
                        //oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPosHW.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPosHW.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raTSysEdc = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoTSysEdc>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raTSysEdc = oConn.Query<cmlResInfoTSysEdc>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17

                        //Printer
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPrinter.FTPrnCode AS rtPrnCode, TCNMPrinter.FTPrnSrcType AS rtPrnSrcType, TCNMPrinter.FTSppCode AS rtSppCode,");
                        oSql.AppendLine("TCNMPrinter.FTPrnDriver AS rtPrnDriver, TCNMPrinter.FTPrnType AS rtPrnType,");
                        oSql.AppendLine("TCNMPrinter.FDLastUpdOn AS rdLastUpdOn, TCNMPrinter.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPrinter.FTLastUpdBy AS rtLastUpdBy, TCNMPrinter.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPrinter with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPosHW with(nolock) ON TCNMPrinter.FTPrnCode = TCNMPosHW.FTPhwCodeRef");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPos.FTPosCode");      //[BOY][63-03-27]
                        //oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPosHW.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPosHW.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raPrinter = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPrinter>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raPrinter = oConn.Query<cmlResInfoPrinter>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17

                        //Printer Languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPrinter_L.FTPrnCode AS rtPrnCode, TCNMPrinter_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPrinter_L.FTPrnName AS rtPrnName, TCNMPrinter_L.FTPrnRmk AS rtPrnRmk");
                        oSql.AppendLine("FROM TCNMPrinter_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPosHW with(nolock) ON TCNMPrinter_L.FTPrnCode = TCNMPosHW.FTPhwCodeRef");
                        oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPos.FTPosCode");  //[BOY][63-03-27]
                        //oSql.AppendLine("INNER JOIN TCNMPos with(nolock) ON TCNMPosHW.FTPosCode = TCNMPosHW.FTPosCode");
                        oSql.AppendLine("WHERE TCNMPosHW.FTBchCode = '" + ptBchCode + "'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPos.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPosDwn.raPrinterLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPrinterLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPosDwn.raPrinterLng = oConn.Query<cmlResInfoPrinterLng>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-08-17
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oPosDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPosDwn>();
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
        ///     Download pos advertisement information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("PosAds")]
        [HttpGet]
        public cmlResList<cmlTCNMPosAds> GET_PDToDownloadPosAds(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlTCNMPosAds> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            //cmlResSysDataDwn oSysDataDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            bool bHaveData;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlTCNMPosAds>();
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

                tKeyCache = "PosAds" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlTCNMPosAds>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTShpCode AS rtShpCode, FTPosCode AS rtPosCode,");
                oSql.AppendLine("FNPsdSeq AS rnPsdSeq, FTPsdPosition AS rtPsdPosition, FTAdvCode AS rtAdvCode,");
                oSql.AppendLine("FNPsdWide AS rnPsdWide, FNPsdHigh AS rnPsdHigh, FDPsdStart AS rdPsdStart,");
                oSql.AppendLine("FDPsdStop AS rdPsdStop,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy,");
                oSql.AppendLine("FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPosAds WITH(NOLOCK)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-07-31 Download มาทั้งหมด(ยกมาจาก Moshi)


                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlTCNMPosAds>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (aoResult.raItems.Count > 0)
                    {

                    }
                    else
                    {
                        //*Arm 63-07-31 ถ้าไม่มีข้อมูลให้ส่ง success กลับไป เพื่อไปอัพเดตตารางที่ Front (ยกมาจาก Moshi)
                        //aoResult.rtCode = oMsg.tMS_RespCode800;
                        //aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        aoResult.rtCode = oMsg.tMS_RespCode001;
                        aoResult.rtDesc = oMsg.tMS_RespDesc001;
                        return aoResult;
                    }
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
                aoResult = new cmlResList<cmlTCNMPosAds>();
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
        /// ลงทะเบียนเครื่องจุดขาย
        /// </summary>
        /// <param name="poData"></param>
        /// <returns>
        ///&#8195;     1 : Success.<br/>
        ///&#8195;     701 : Validate parameter model false.<br/>
        ///&#8195;     709 : This Mac.Address is already registered.<br/>
        ///&#8195;     710 : This POS is already registered.<br/>
        ///&#8195;     900 : Service process false.<br/>
        ///&#8195;     904 : Key not allowed to use method.<br/>
        /// </returns>
        [Route("PosRegister")]
        [HttpPost]
        public cmlResPosRegister POST_CHKoCheckPosRegister(cmlReqPosRegister poData)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB;
            cmlResPosRegister oResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            DataTable odtTmp;
            int nRowEff, nCmdTme;
            string tFuncName, tModelErr, tKeyApi;
            bool bHaveData;
            int nStaChk = 0; // 0: Active , 1:Pending Approve , 2:Insert
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResPosRegister();
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
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }
                
                odtTmp = new DataTable();
                oSql = new StringBuilder();

                // 1.นำค่า Mac Addr ไปตรวจสอบว่ามีที่อนุญาตใช้งานอยู่ใหม ถ้ามี แจ้งกลับ ไม่สามารถลงทะเบียนได้ซ้ำอีก
                oSql.AppendLine("SELECT TOP 1 FTBchCode, FTPosCode, FTPrgMacAddr, FTPrgStaApv, FTPrgPosName");
                oSql.AppendLine("FROM TPSTPosReg WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTPrgMacAddr = '" + poData.ptMacAddress + "' AND FTPrgStaApv != '3' "); //Arm 63-08-05
                oSql.AppendLine("ORDER BY FDPrgDate DESC");
                
                oDB = new cDatabase();
                odtTmp = oDB.C_DAToSqlQuery(oSql.ToString());
                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    //1.1 กรณีเจอ Response ว่าลงทะเบียนด้วย BCH, POS
                    oResult.rtCode = oMsg.tMS_RespCode709;
                    oResult.rtDesc = oMsg.tMS_RespDesc709;
                    oResult.rtBchCode = odtTmp.Rows[0].Field<string>("FTBchCode");
                    oResult.rtPosCode = odtTmp.Rows[0].Field<string>("FTPosCode");
                    oResult.rtMacAddr = odtTmp.Rows[0].Field<string>("FTPrgMacAddr");
                    oResult.rtCompName = odtTmp.Rows[0].Field<string>("FTPrgPosName");
                    oResult.rtStaApv = odtTmp.Rows[0].Field<string>("FTPrgStaApv");
                }
                else
                {
                    // 2. กรณีไม่เจอ
                    // 2.1  ต้องนำค่า BCH, POS ไปหาว่ามี Active หรือไม่
                    odtTmp = new DataTable();
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 FTBchCode, FTPosCode, FTPrgMacAddr, FTPrgStaApv, FTPrgPosName");
                    oSql.AppendLine("FROM TPSTPosReg WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode = '" + poData.ptBchCode + "' AND FTPosCode = '"+ poData.ptPosCode + "' AND FTPrgStaApv != '3' ");
                    oSql.AppendLine("ORDER BY FDPrgDate DESC");
                    odtTmp = oDB.C_DAToSqlQuery(oSql.ToString());

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        // 2.1.1  กรณีเจอ Response กลับ BCH,POS,MacAddr , CompName ใช้งานอยู่
                        oResult.rtCode = oMsg.tMS_RespCode710;
                        oResult.rtDesc = oMsg.tMS_RespDesc710;
                        oResult.rtBchCode = odtTmp.Rows[0].Field<string>("FTBchCode");
                        oResult.rtPosCode = odtTmp.Rows[0].Field<string>("FTPosCode");
                        oResult.rtMacAddr = odtTmp.Rows[0].Field<string>("FTPrgMacAddr");
                        oResult.rtCompName = odtTmp.Rows[0].Field<string>("FTPrgPosName");
                        oResult.rtStaApv = odtTmp.Rows[0].Field<string>("FTPrgStaApv");
                    }
                    else
                    {
                        //ลงทะเบียนสำเร็จ รออนุมัติ
                        oSql.Clear();
                        oSql.AppendLine("INSERT INTO TPSTPosReg (");
                        oSql.AppendLine("FDPrgDate,FTPrgMacAddr,FTPosCode,FTBchCode,");
                        oSql.AppendLine("FDPrgExpire,FTPrgStaApv,FTPrgPosName,");
                        oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy ");
                        oSql.AppendLine(") VALUES (");
                        oSql.AppendLine("GETDATE(), '" + poData.ptMacAddress + "', '" + poData.ptPosCode + "', '" + poData.ptBchCode + "',");
                        oSql.AppendLine("GETDATE(), '2', '"+ poData .ptCompName + "',");
                        oSql.AppendLine("GETDATE(), 'API2PSMaster', GETDATE(), 'API2PSMaster' ");
                        oSql.AppendLine(")");
                        nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());

                        oResult.rtCode = oMsg.tMS_RespCode001;
                        oResult.rtDesc = oMsg.tMS_RespDesc001;
                    }
                }
                return oResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                oResult = new cmlResPosRegister();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCS = null;
                oMsg = null;
                oSql = null;
                oDB = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        /// ตรวจสอบรหัสเครื่องจุดขาย
        /// </summary>
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <param name="ptPosCode">รหัสเครื่องจุดขาย</param>
        /// <returns>
        ///&#8195;     001 : success.(สามารถใช้งานเครื่องจุดขายนี้ได้)<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     800 : data not found.(ไม่สามารถใช้งานเครื่องจุดขายนี้ได้)<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        /// </returns>
        [Route("CheckPosCode")]
        [HttpGet]
        public cmlResBase GET_CHKoCheckPosCode(string ptBchCode, string ptPosCode)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResBase oResult;
            List<cmlTSysConfig> aoSysConfig;
            int nCmdTme;
            string tModelErr, tKeyApi;
            
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oResult = new cmlResBase();

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }

                //
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPosCode FROM TCNMPos WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "' AND FTPosCode = '" + ptPosCode + "'");
                
                string tPosCode = new cDatabase().C_DAToSqlQuery<string>(oSql.ToString());

                if(!string.IsNullOrEmpty(tPosCode))
                {
                    oResult.rtCode = oMsg.tMS_RespCode001;
                    oResult.rtDesc = oMsg.tMS_RespDesc001;
                }
                else
                {
                    oResult.rtCode = oMsg.tMS_RespCode800;
                    oResult.rtDesc = oMsg.tMS_RespDesc800;
                }

                return oResult;
            }
            catch (Exception oEx)
            {
                oResult = new cmlResBase();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oEx.Message.ToString();
                return oResult;
            }
            finally
            {
                oResult = null;
                oSql = null;
            }
        }
    }
}