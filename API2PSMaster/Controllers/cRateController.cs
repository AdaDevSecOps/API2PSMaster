using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Rate;
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
using API2PSMaster.Models.WebService.Response.Image;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Rate information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PAY/Rate")]
    public class cRateController : ControllerBase
    {
        /// <summary>
        ///     Download rate information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        //public cmlResItem<cmlResRateDwn> GET_PDToDownloadRate(DateTime pdDate)
        public cmlResItem<cmlResRateDwn> GET_PDToDownloadRate(DateTime pdDate, string ptAgnCode = "") //*Arm 65-09-03 -[CR-Oversea]
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResRateDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRateDwn oRateDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRateDwn>();
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

                tKeyCache = "PAYRate" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRateDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRteCode AS rtRteCode, ISNULL(FCRteRate,0) AS rcRteRate, ISNULL(FCRteFraction,0) AS rcRteFraction, FTRteType AS rtRteType,");
                //oSql.AppendLine("FTRteTypeChg AS rtRteTypeChg, FTRteSign AS rtRteSign, FTRteStaLocal AS rtRteStaLocal, FTRteStaUse AS rtRteStaUse,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TFNMRate with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResRateDwn();
                //oRateDwn = new cmlResRateDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oRateDwn.raRate = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRate>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oRateDwn.raRate.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMRate_L.FTRteCode AS rtRteCode, TFNMRate_L.FNLngID AS rnLngID, TFNMRate_L.FTRteName AS rtRteName,");
                //            oSql.AppendLine("TFNMRate_L.FTRteShtName AS rtRteShtName, TFNMRate_L.FTRteNameText AS rtRteNameText, TFNMRate_L.FTRteDecText AS rtRteDecText");
                //            oSql.AppendLine("FROM TFNMRate_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRate_L.FTRteCode = TFNMRate.FTRteCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRate.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRateDwn.raRateLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRateLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Rate Unit
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMRateUnit.FTRteCode AS rtRteCode, TFNMRateUnit.FNRtuSeq AS rnRtuSeq, TFNMRateUnit.FCRtuFac AS rcRtuFac");
                //            oSql.AppendLine("FROM TFNMRateUnit with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRateUnit.FTRteCode = TFNMRate.FTRteCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRate.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRateDwn.raRateUnit = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRateUnit>(oDR).ToList();
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
                aoResult.roItem = new cmlResRateDwn();
                oRateDwn = new cmlResRateDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRteCode AS rtRteCode, ISNULL(FCRteRate,0) AS rcRteRate, ISNULL(FCRteFraction,0) AS rcRteFraction, FTRteType AS rtRteType,");
                oSql.AppendLine("FTRteTypeChg AS rtRteTypeChg, FTRteSign AS rtRteSign, FTRteStaLocal AS rtRteStaLocal, FTRteStaUse AS rtRteStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy,");
                oSql.AppendLine("FTAgnCode AS rtAgnCode, FCRteLastRate AS rcRteLastRate, FTRteIsoCode AS rtRteIsoCode, FTRteStaAlwChange AS rtRteStaAlwChange, ");  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์ //*Arm 65-09-03 -เพิ่มฟิลด์ FTRteStaAlwChange
                oSql.AppendLine("FCRteMaxUnit AS rcRteMaxUnit, FDRteLastUpdOn AS rdRteLastUpdOn "); //*Arm 65-09-05 -[CR-Oversea] เพิ่มฟิลด์ 
                oSql.AppendLine("FROM TFNMRate with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSql.AppendLine("AND FTAgnCode = '" + ptAgnCode + "'"); //*Arm 65-09-03 -[CR-Oversea] 
                oRateDwn.raRate = oDB.C_DATaSqlQuery<cmlResInfoRate>(oSql.ToString());
                if (oRateDwn.raRate.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT TFNMRate_L.FTRteCode AS rtRteCode, TFNMRate_L.FNLngID AS rnLngID, TFNMRate_L.FTRteName AS rtRteName,"); //*Arm 65-09-03 -ใส่ DISTINCT
                    oSql.AppendLine("TFNMRate_L.FTRteShtName AS rtRteShtName, TFNMRate_L.FTRteNameText AS rtRteNameText, TFNMRate_L.FTRteDecText AS rtRteDecText,");
                    oSql.AppendLine("TFNMRate_L.FTAgnCode AS rtAgnCode");  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์
                    oSql.AppendLine("FROM TFNMRate_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRate_L.FTRteCode = TFNMRate.FTRteCode AND TFNMRate.FTAgnCode = TFNMRate_L.FTAgnCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRate.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSql.AppendLine("AND TFNMRate.FTAgnCode = '" + ptAgnCode + "'"); //*Arm 65-09-03 -[CR-Oversea]
                    oRateDwn.raRateLng = oDB.C_DATaSqlQuery<cmlResInfoRateLng>(oSql.ToString());

                    //Rate Unit
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT TFNMRateUnit.FTRteCode AS rtRteCode, TFNMRateUnit.FNRtuSeq AS rnRtuSeq, TFNMRateUnit.FCRtuFac AS rcRtuFac,"); //*Arm 65-09-03 -ใส่ DISTINCT
                    oSql.AppendLine("TFNMRateUnit.FTAgnCode AS rtAgnCode");  //*Arm 65-08-10 -[CR-Oversea] เพิ่มฟิลด์
                    oSql.AppendLine("FROM TFNMRateUnit with(nolock)");
                    //oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRateUnit.FTRteCode = TFNMRate.FTRteCode ");
                    oSql.AppendLine("INNER JOIN TFNMRate with(nolock) ON TFNMRateUnit.FTRteCode = TFNMRate.FTRteCode AND TFNMRateUnit.FTAgnCode = TFNMRate.FTAgnCode"); //*Arm 65-09-03 -[CR-Oversea]
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMRate.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSql.AppendLine("AND TFNMRate.FTAgnCode = '" + ptAgnCode + "'"); //*Arm 65-09-03 -[CR-Oversea]
                    oRateDwn.raRateUnit = oDB.C_DATaSqlQuery<cmlResInfoRateUnit>(oSql.ToString());

                    //Img Object    //*Nick 65-08-24
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT IMG.FNImgID AS rnImgID, IMG.FTImgRefID AS rtImgRefID, IMG.FNImgSeq AS rnImgSeq ");
                    oSql.AppendLine(", IMG.FTImgTable AS rtImgTable, IMG.FTImgKey AS rtImgKey, IMG.FTImgObj AS rtImgObj ");
                    oSql.AppendLine(", IMG.FDLastUpdOn AS rdLastUpdOn,IMG.FTLastUpdBy AS rtLastUpdBy,IMG.FDCreateOn AS rdCreateOn,IMG.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMImgObj IMG WITH(NOLOCK)");
                    //oSql.AppendLine("INNER JOIN TFNMRate RTE WITH(NOLOCK) ON IMG.FTImgRefID = RTE.FTRteCode AND IMG.FTImgTable = 'TFNMRate'");
                    oSql.AppendLine("INNER JOIN TFNMRate RTE WITH(NOLOCK) ON IMG.FTImgRefID = RTE.FTAgnCode + RTE.FTRteCode AND IMG.FTImgTable = 'TFNMRate' "); //*Arm 65-09-11
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),RTE.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                    oRateDwn.raImgObj = oDB.C_DATaSqlQuery<cmlResInfoImgObj>(oSql.ToString());

                    //*Arm 65-09-16
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT SRTE.FTRteIsoCode AS rtRteIsoCode, SRTE.FNLngID AS rnLngID, SRTE.FTFmtCode AS rtFmtCode, SRTE.FTRteIsoName AS rtRteIsoName ");
                    oSql.AppendLine(", SRTE.FTRteIsoRmk AS rtRteIsoRmk, SRTE.FTRteUnitName AS rtRteUnitName, SRTE.FTRteSubUnitName AS rtRteSubUnitName ");
                    oSql.AppendLine("FROM TCNSRate_L SRTE WITH(NOLOCK) ");
                    oSql.AppendLine("INNER JOIN TFNMRate RTE WITH(NOLOCK) ON SRTE.FTRteIsoCode = RTE.FTRteIsoCode ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),RTE.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSql.AppendLine("AND RTE.FTAgnCode = '" + ptAgnCode + "'");
                    oRateDwn.raSysRateLng = oDB.C_DATaSqlQuery<cmlResInfoSysRateLng>(oSql.ToString());
                    //++++++++++++++
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oRateDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRateDwn>();
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