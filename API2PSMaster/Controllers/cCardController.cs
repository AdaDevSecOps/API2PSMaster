using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Card;
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
    ///     Card information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PAY/Card")]
    public class cCardController : ControllerBase
    {
        /// <summary>
        ///     Download card information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResCardDwn> GET_PDToDownloadCard(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResCardDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCardDwn oCardDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCardDwn>();
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

                tKeyCache = "PAYCard" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCardDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCrdCode AS rtCrdCode, FDCrdStartDate AS rdCrdStartDate, FDCrdExpireDate AS rdCrdExpireDate, FDCrdResetDate AS rdCrdResetDate,");
                oSql.AppendLine("FDCrdLastTopup AS rdCrdLastTopup, FTCtyCode AS rtCtyCode, FCCrdValue AS rcCrdValue, FTCrdHolderID AS rtCrdHolderID,");
                oSql.AppendLine("FTCrdRefID AS rtCrdRefID, FTCrdStaType AS rtCrdStaType, FTCrdStaLocate AS rtCrdStaLocate, FTCrdStaActive AS rtCrdStaActive,");
                oSql.AppendLine("FTDptCode AS rtDptCode, FCCrdDepositPdt AS rcCrdDepositPdt, FTCrdStaShift AS rtCrdStaShift, FNCrdTxnOffline AS rnCrdTxnOffline, FNCrdTxnPrcAdj AS rnCrdTxnPrcAdj,"); //*Arm 63-01-28
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TFNMCard with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResCardDwn();
                oCardDwn = new cmlResCardDwn();

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
                //            oCardDwn.raCard = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCard>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                oCardDwn.raCard = oDB.C_DATaSqlQuery<cmlResInfoCard>(oSql.ToString()); //*Arm 64-10-19
                if (oCardDwn.raCard.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMCard_L.FTCrdCode AS rtCrdCode, TFNMCard_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMCard_L.FTCrdName AS rtCrdName, TFNMCard_L.FTCrdRmk AS rtCrdRmk");
                    oSql.AppendLine("FROM TFNMCard_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMCard with(nolock) ON TFNMCard_L.FTCrdCode = TFNMCard.FTCrdCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMCard.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code           
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCardDwn.raCardLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCardLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCardDwn.raCardLng = oDB.C_DATaSqlQuery<cmlResInfoCardLng>(oSql.ToString()); //*Arm 64-10-19

                    //Card Type 
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMCardType.FTCtyCode AS rtCtyCode, TFNMCardType.FCCtyDeposit AS rcCtyDeposit, TFNMCardType.FCCtyTopupAuto AS rcCtyTopupAuto,");
                    oSql.AppendLine("TFNMCardType.FNCtyExpirePeriod AS rnCtyExpirePeriod, TFNMCardType.FNCtyExpiredType AS rnCtyExpiredType, TFNMCardType.FTCtyStaAlwRet AS rtCtyStaAlwRet,");
                    oSql.AppendLine("TFNMCardType.FTCtyStaPay AS rtCtyStaPay, TFNMCardType.FCCtyCreditLimit AS rcCtyCreditLimit,"); //*Arm 63-01-28
                    oSql.AppendLine("TFNMCardType.FDLastUpdOn AS rdLastUpdOn, TFNMCardType.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TFNMCardType.FTLastUpdBy AS rtLastUpdBy, TFNMCardType.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TFNMCardType with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMCard with(nolock) ON TFNMCardType.FTCtyCode = TFNMCard.FTCtyCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMCard.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code           
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCardDwn.raCardType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCardType>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCardDwn.raCardType = oDB.C_DATaSqlQuery<cmlResInfoCardType>(oSql.ToString()); //*Arm 64-10-19

                    //Card type Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMCardType_L.FTCtyCode AS rtCtyCode, TFNMCardType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMCardType_L.FTCtyName AS rtCtyName, TFNMCardType_L.FTCtyRmk AS rtCtyRmk");
                    oSql.AppendLine("FROM TFNMCardType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMCard with(nolock) ON TFNMCardType_L.FTCtyCode = TFNMCard.FTCtyCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMCard.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code           
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCardDwn.raCardTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCardTypeLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCardDwn.raCardTypeLng = oDB.C_DATaSqlQuery<cmlResInfoCardTypeLng>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                aoResult.roItem = oCardDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCardDwn>();
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