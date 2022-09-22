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
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage Product brand.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductBrandController : ControllerBase
    {
        /// <summary>
        ///     Download product brand information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Brand/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtBrandDwn> GET_PDToDownloadPdtBrand(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtBrandDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtBrandDwn oPdtBrandDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtBrandDwn>();
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

                tKeyCache = "ProductBrand" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtBrandDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPbnCode AS rtPbnCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtBrand with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtBrandDwn();
                //oPdtBrandDwn = new cmlResPdtBrandDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtBrandDwn.raPdtBrand = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBrand>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtBrandDwn.raPdtBrand.Count > 0)
                //        {
                //            //Product Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtBrand_L.FTPbnCode AS rtPbnCode, TCNMPdtBrand_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtBrand_L.FTPbnName AS rtPbnName, TCNMPdtBrand_L.FTPbnRmk AS rtPbnRmk");
                //            oSql.AppendLine("FROM TCNMPdtBrand_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtBrand with(nolock) ON TCNMPdtBrand_L.FTPbnCode = TCNMPdtBrand.FTPbnCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtBrand.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtBrandDwn.raPdtBrandLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBrandLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtBrandDwn();
                oPdtBrandDwn = new cmlResPdtBrandDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPbnCode AS rtPbnCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtBrand with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtBrandDwn.raPdtBrand = oDB.C_DATaSqlQuery<cmlResInfoPdtBrand>(oSql.ToString());
                if (oPdtBrandDwn.raPdtBrand.Count > 0)
                {
                    //Product Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtBrand_L.FTPbnCode AS rtPbnCode, TCNMPdtBrand_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtBrand_L.FTPbnName AS rtPbnName, TCNMPdtBrand_L.FTPbnRmk AS rtPbnRmk");
                    oSql.AppendLine("FROM TCNMPdtBrand_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtBrand with(nolock) ON TCNMPdtBrand_L.FTPbnCode = TCNMPdtBrand.FTPbnCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtBrand.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtBrandDwn.raPdtBrandLng = oDB.C_DATaSqlQuery<cmlResInfoPdtBrandLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oPdtBrandDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtBrandDwn>();
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