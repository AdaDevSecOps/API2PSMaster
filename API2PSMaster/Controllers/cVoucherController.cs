using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Voucher;
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
    ///     Manage Voucher.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PAY/Voucher")]
    public class cVoucherController : ControllerBase
    {
        /// <summary>
        ///     Download voucher information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResVchDwn> GET_PDToDownloadVoucher(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResVchDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResVchDwn oVoucherDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResVchDwn>();
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

                tKeyCache = "PAYVoucher" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResVchDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTVocCode AS rtVocCode, FTVocBarCode AS rtVocBarCode, FDVocExpired AS rdVocExpired, FTVotCode AS rtVotCode,");
                //oSql.AppendLine("ISNULL(FCVocValue,0) AS rcVocValue, ISNULL(FCVocSalePri,0) AS rcVocSalePri, ISNULL(FCVocBalance,0) AS rcVocBalance, FTVocComBook AS rtVocComBook,");
                //oSql.AppendLine("FTVocStaBook AS rtVocStaBook, FTVocStaSale AS rtVocStaSale, FTVocStaUse AS rtVocStaUse,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TFNMVoucher with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResVchDwn();
                //oVoucherDwn = new cmlResVchDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oVoucherDwn.raVch = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoVch>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oVoucherDwn.raVch.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMVoucher_L.FTVocCode AS rtVocCode, TFNMVoucher_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TFNMVoucher_L.FTVocName AS rtVocName, TFNMVoucher_L.FTVocRemark AS rtVocRemark");
                //            oSql.AppendLine("FROM TFNMVoucher_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMVoucher with(nolock) ON TFNMVoucher_L.FTVocCode = TFNMVoucher.FTVocCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMVoucher.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oVoucherDwn.raVchLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoVchLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Type
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMVoucherType.FTVotCode AS rtVotCode, TFNMVoucherType.FTVotStaUse AS rtVotStaUse,");
                //            oSql.AppendLine("TFNMVoucherType.FDLastUpdOn AS rdLastUpdOn, TFNMVoucherType.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TFNMVoucherType.FTLastUpdBy AS rtLastUpdBy, TFNMVoucherType.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TFNMVoucherType with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMVoucher with(nolock) ON TFNMVoucherType.FTVotCode = TFNMVoucher.FTVotCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMVoucher.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oVoucherDwn.raVchType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoVchType>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Type voucher Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TFNMVoucherType_L.FTVotCode AS rtVotCode, TFNMVoucherType_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TFNMVoucherType_L.FTVotName AS rtVotName, TFNMVoucherType_L.FTVotRemark AS rtVotRemark");
                //            oSql.AppendLine("FROM TFNMVoucherType_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TFNMVoucher with(nolock) ON TFNMVoucherType_L.FTVotCode = TFNMVoucher.FTVotCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMVoucher.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oVoucherDwn.raVchTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoVchTypeLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResVchDwn();
                oVoucherDwn = new cmlResVchDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTVocCode AS rtVocCode, FTVocBarCode AS rtVocBarCode, FDVocExpired AS rdVocExpired, FTVotCode AS rtVotCode,");
                oSql.AppendLine("ISNULL(FCVocValue,0) AS rcVocValue, ISNULL(FCVocSalePri,0) AS rcVocSalePri, ISNULL(FCVocBalance,0) AS rcVocBalance, FTVocComBook AS rtVocComBook,");
                oSql.AppendLine("FTVocStaBook AS rtVocStaBook, FTVocStaSale AS rtVocStaSale, FTVocStaUse AS rtVocStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TFNMVoucher with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oVoucherDwn.raVch = oDB.C_DATaSqlQuery<cmlResInfoVch>(oSql.ToString());
                if (oVoucherDwn.raVch.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMVoucher_L.FTVocCode AS rtVocCode, TFNMVoucher_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMVoucher_L.FTVocName AS rtVocName, TFNMVoucher_L.FTVocRemark AS rtVocRemark");
                    oSql.AppendLine("FROM TFNMVoucher_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMVoucher with(nolock) ON TFNMVoucher_L.FTVocCode = TFNMVoucher.FTVocCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMVoucher.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oVoucherDwn.raVchLng = oDB.C_DATaSqlQuery<cmlResInfoVchLng>(oSql.ToString());

                    //Type
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMVoucherType.FTVotCode AS rtVotCode, TFNMVoucherType.FTVotStaUse AS rtVotStaUse,");
                    oSql.AppendLine("TFNMVoucherType.FDLastUpdOn AS rdLastUpdOn, TFNMVoucherType.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TFNMVoucherType.FTLastUpdBy AS rtLastUpdBy, TFNMVoucherType.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TFNMVoucherType with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMVoucher with(nolock) ON TFNMVoucherType.FTVotCode = TFNMVoucher.FTVotCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMVoucher.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oVoucherDwn.raVchType = oDB.C_DATaSqlQuery<cmlResInfoVchType>(oSql.ToString());
                    //Type voucher Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMVoucherType_L.FTVotCode AS rtVotCode, TFNMVoucherType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMVoucherType_L.FTVotName AS rtVotName, TFNMVoucherType_L.FTVotRemark AS rtVotRemark");
                    oSql.AppendLine("FROM TFNMVoucherType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMVoucher with(nolock) ON TFNMVoucherType_L.FTVotCode = TFNMVoucher.FTVotCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMVoucher.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oVoucherDwn.raVchTypeLng = oDB.C_DATaSqlQuery<cmlResInfoVchTypeLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }


                aoResult.roItem = oVoucherDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResVchDwn>();
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