using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.ProductFhn;
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
    ///     Manage Product DCS.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product/DCS")]
    public class cProductDCSController : ControllerBase
    {
        /// <summary>
        ///     Download product fashion information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtDCSDwn> GET_PDToDownloadPdtF0DCS(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtDCSDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtDCSDwn oPdtDCSDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtDCSDwn>();
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

                tKeyCache = "ProductDCS" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtDCSDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTDcsCode AS rtDcsCode, FTDepCode AS rtDepCode, FTClsCode AS rtClsCode, FTSclCode AS rtSclCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtF0DCS with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtDCSDwn();
                //oPdtDCSDwn = new cmlResPdtDCSDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtDCSDwn.raPdtDCS = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtDCS>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtDCSDwn.raPdtDCS.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF0DCS_L.FTDcsCode AS rtDcsCode, TCNMPdtF0DCS_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtF0DCS_L.FTDcsName AS rtDcsName, TCNMPdtF0DCS_L.FTDcsRmk AS rtDcsRmk");
                //            oSql.AppendLine("FROM TCNMPdtF0DCS_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtF0DCS with(nolock) ON TCNMPdtF0DCS_L.FTDcsCode = TCNMPdtF0DCS.FTDcsCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtF0DCS.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDCSDwn.raPdtDCSLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtDCSLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtDCSDwn();
                oPdtDCSDwn = new cmlResPdtDCSDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTDcsCode AS rtDcsCode, FTDepCode AS rtDepCode, FTClsCode AS rtClsCode, FTSclCode AS rtSclCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtF0DCS with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtDCSDwn.raPdtDCS = oDB.C_DATaSqlQuery<cmlResInfoPdtDCS>(oSql.ToString());
                if (oPdtDCSDwn.raPdtDCS.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF0DCS_L.FTDcsCode AS rtDcsCode, TCNMPdtF0DCS_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtF0DCS_L.FTDcsName AS rtDcsName, TCNMPdtF0DCS_L.FTDcsRmk AS rtDcsRmk");
                    oSql.AppendLine("FROM TCNMPdtF0DCS_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtF0DCS with(nolock) ON TCNMPdtF0DCS_L.FTDcsCode = TCNMPdtF0DCS.FTDcsCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtF0DCS.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDCSDwn.raPdtDCSLng = oDB.C_DATaSqlQuery<cmlResInfoPdtDCSLng>(oSql.ToString());

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }


                aoResult.roItem = oPdtDCSDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtDCSDwn>();
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