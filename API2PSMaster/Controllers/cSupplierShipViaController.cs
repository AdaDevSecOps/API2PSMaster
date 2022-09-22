using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Supplier;
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
    ///     Supplier shipvia information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Supplier")]
    public class cSupplierShipViaController : ControllerBase
    {
        /// <summary>
        ///     Download supplier shipvia information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("ShipVia/Download")]
        [HttpGet]
        public cmlResItem<cmlResSplShipViaDwn> GET_PDToDownloadSplShipVia(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSplShipViaDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSplShipViaDwn oSplShipViaDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSplShipViaDwn>();
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

                tKeyCache = "SupplierShipVia" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSplShipViaDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTViaCode AS rtViaCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMShipVia with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResSplShipViaDwn();
                //oSplShipViaDwn = new cmlResSplShipViaDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oSplShipViaDwn.raSplShipVia = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSplShipVia>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oSplShipViaDwn.raSplShipVia.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMShipVia_L.FTViaCode AS rtViaCode, TCNMShipVia_L.FNLngID AS rnLngID, TCNMShipVia_L.FTViaName AS rtViaName");
                //            oSql.AppendLine("FROM TCNMShipVia_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMShipVia with(nolock) ON TCNMShipVia_L.FTViaCode = TCNMShipVia.FTViaCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMShipVia.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSplShipViaDwn.raSplShipViaLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSplShipViaLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResSplShipViaDwn();
                oSplShipViaDwn = new cmlResSplShipViaDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTViaCode AS rtViaCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMShipVia with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSplShipViaDwn.raSplShipVia = oDB.C_DATaSqlQuery<cmlResInfoSplShipVia>(oSql.ToString());
                if (oSplShipViaDwn.raSplShipVia.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMShipVia_L.FTViaCode AS rtViaCode, TCNMShipVia_L.FNLngID AS rnLngID, TCNMShipVia_L.FTViaName AS rtViaName");
                    oSql.AppendLine("FROM TCNMShipVia_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMShipVia with(nolock) ON TCNMShipVia_L.FTViaCode = TCNMShipVia.FTViaCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMShipVia.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSplShipViaDwn.raSplShipViaLng = oDB.C_DATaSqlQuery<cmlResInfoSplShipViaLng>(oSql.ToString());

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oSplShipViaDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSplShipViaDwn>();
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