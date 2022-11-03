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
    ///     Product promotion group information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductPmtGroupController : ControllerBase
    {
        /// <summary>
        ///     Download product promotion group information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("PromotionGroup/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtPmtGrpDwn> GET_PDToDownloadPdtPmtGrp(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtPmtGrpDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtPmtGrpDwn oPdtPmtGrpDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtPmtGrpDwn>();
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

                tKeyCache = "ProductPmtGrp" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtPmtGrpDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPmgCode AS rtPmgCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtPmtGrp with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtPmtGrpDwn();
                //oPdtPmtGrpDwn = new cmlResPdtPmtGrpDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtPmtGrpDwn.raPdtPmtGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtGrp>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtPmtGrpDwn.raPdtPmtGrp.Count > 0)
                //        {
                //            //Product Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtPmtGrp_L.FTPmgCode AS rtPmgCode, TCNMPdtPmtGrp_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtPmtGrp_L.FTPmgName AS rtPmgName, TCNMPdtPmtGrp_L.FTPmgRmk AS rtPmgRmk");
                //            oSql.AppendLine("FROM TCNMPdtPmtGrp_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtPmtGrp with(nolock) ON TCNMPdtPmtGrp_L.FTPmgCode = TCNMPdtPmtGrp.FTPmgCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtPmtGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtGrpDwn.raPdtPmtGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtGrpLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtPmtGrpDwn();
                oPdtPmtGrpDwn = new cmlResPdtPmtGrpDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPmgCode AS rtPmgCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtPmtGrp with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtPmtGrpDwn.raPdtPmtGrp = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtGrp>(oSql.ToString());
                if (oPdtPmtGrpDwn.raPdtPmtGrp.Count > 0)
                {
                    //Product Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtPmtGrp_L.FTPmgCode AS rtPmgCode, TCNMPdtPmtGrp_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtPmtGrp_L.FTPmgName AS rtPmgName, TCNMPdtPmtGrp_L.FTPmgRmk AS rtPmgRmk");
                    oSql.AppendLine("FROM TCNMPdtPmtGrp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtPmtGrp with(nolock) ON TCNMPdtPmtGrp_L.FTPmgCode = TCNMPdtPmtGrp.FTPmgCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtPmtGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtPmtGrpDwn.raPdtPmtGrpLng = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtGrpLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }


                aoResult.roItem = oPdtPmtGrpDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtPmtGrpDwn>();
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