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
    ///     Product type information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductTypeController : ControllerBase
    {
        /// <summary>
        ///     Download product type information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Type/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtTypeDwn> GET_PDToDownloadPdtType(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtTypeDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtTypeDwn oPdtTypeDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtTypeDwn>();
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

                tKeyCache = "ProductType" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtTypeDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPtyCode AS rtPtyCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtType with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtTypeDwn();
                //oPdtTypeDwn = new cmlResPdtTypeDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtTypeDwn.raPdtType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtType>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtTypeDwn.raPdtType.Count > 0)
                //        {
                //            //Product Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtType_L.FTPtyCode AS rtPtyCode, TCNMPdtType_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtType_L.FTPtyName AS rtPtyName, TCNMPdtType_L.FTPtyRmk AS rtPtyRmk");
                //            oSql.AppendLine("FROM TCNMPdtType_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtType with(nolock) ON TCNMPdtType_L.FTPtyCode = TCNMPdtType.FTPtyCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtType.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtTypeDwn.raPdtTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTypeLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtTypeDwn();
                oPdtTypeDwn = new cmlResPdtTypeDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPtyCode AS rtPtyCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtType with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtTypeDwn.raPdtType = oDB.C_DATaSqlQuery<cmlResInfoPdtType>(oSql.ToString());
                if (oPdtTypeDwn.raPdtType.Count > 0)
                {
                    //Product Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtType_L.FTPtyCode AS rtPtyCode, TCNMPdtType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtType_L.FTPtyName AS rtPtyName, TCNMPdtType_L.FTPtyRmk AS rtPtyRmk");
                    oSql.AppendLine("FROM TCNMPdtType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtType with(nolock) ON TCNMPdtType_L.FTPtyCode = TCNMPdtType.FTPtyCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtType.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtTypeDwn.raPdtTypeLng = oDB.C_DATaSqlQuery<cmlResInfoPdtTypeLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }


                aoResult.roItem = oPdtTypeDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtTypeDwn>();
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