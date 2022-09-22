using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Image;
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
    ///     Product.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductTouchGrpController : ControllerBase
    {
        /// <summary>
        ///     Download product touch group information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("TouchGrp/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtTouchGrpDwn> GET_PDToDownloadPdtTouchGrp(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtTouchGrpDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtTouchGrpDwn oPdtTouchGrpDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtTouchGrpDwn>();
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

                tKeyCache = "ProductTouchGrp" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtTouchGrpDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTTcgCode AS rtTcgCode, FTTcgStaUse AS rtTcgStaUse,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtTouchGrp with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtTouchGrpDwn();
                //oPdtTouchGrpDwn = new cmlResPdtTouchGrpDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtTouchGrpDwn.raPdtTouchGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTouchGrp>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtTouchGrpDwn.raPdtTouchGrp.Count > 0)
                //        {
                //            //Product Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtTouchGrp_L.FTTcgCode AS rtTcgCode, TCNMPdtTouchGrp_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtTouchGrp_L.FTTcgName AS rtTcgName, TCNMPdtTouchGrp_L.FTTcgRmk AS rtTcgRmk");
                //            oSql.AppendLine("FROM TCNMPdtTouchGrp_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtTouchGrp with(nolock) ON TCNMPdtTouchGrp_L.FTTcgCode = TCNMPdtTouchGrp.FTTcgCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtTouchGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtTouchGrpDwn.raPdtTouchGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTouchGrpLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Image Product Touch Group
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT DISTINCT TCNMImgPdt.FNImgID AS rnImgID, TCNMImgPdt.FTImgRefID AS rtImgRefID, TCNMImgPdt.FNImgSeq AS rnImgSeq,");
                //            oSql.AppendLine("TCNMImgPdt.FTImgTable AS rtImgTable, TCNMImgPdt.FTImgKey AS rtImgKey, TCNMImgPdt.FTImgObj AS rtImgObj,");
                //            oSql.AppendLine("TCNMImgPdt.FDLastUpdOn AS rdLastUpdOn, TCNMImgPdt.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMImgPdt.FTLastUpdBy AS rtLastUpdBy, TCNMImgPdt.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMImgPdt with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtTouchGrp with(nolock) ON TCNMImgPdt.FTImgRefID = TCNMPdtTouchGrp.FTTcgCode AND TCNMImgPdt.FTImgTable = 'TCNMPdtTouchGrp'");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtTouchGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();

                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtTouchGrpDwn.raPdtTouchGrpImg = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgPdt>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtTouchGrpDwn();
                oPdtTouchGrpDwn = new cmlResPdtTouchGrpDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTTcgCode AS rtTcgCode, FTTcgStaUse AS rtTcgStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtTouchGrp with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtTouchGrpDwn.raPdtTouchGrp = oDB.C_DATaSqlQuery<cmlResInfoPdtTouchGrp>(oSql.ToString());
                if (oPdtTouchGrpDwn.raPdtTouchGrp.Count > 0)
                {
                    //Product Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtTouchGrp_L.FTTcgCode AS rtTcgCode, TCNMPdtTouchGrp_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtTouchGrp_L.FTTcgName AS rtTcgName, TCNMPdtTouchGrp_L.FTTcgRmk AS rtTcgRmk");
                    oSql.AppendLine("FROM TCNMPdtTouchGrp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtTouchGrp with(nolock) ON TCNMPdtTouchGrp_L.FTTcgCode = TCNMPdtTouchGrp.FTTcgCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtTouchGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtTouchGrpDwn.raPdtTouchGrpLng = oDB.C_DATaSqlQuery<cmlResInfoPdtTouchGrpLng>(oSql.ToString());

                    //Image Product Touch Group
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT TCNMImgPdt.FNImgID AS rnImgID, TCNMImgPdt.FTImgRefID AS rtImgRefID, TCNMImgPdt.FNImgSeq AS rnImgSeq,");
                    oSql.AppendLine("TCNMImgPdt.FTImgTable AS rtImgTable, TCNMImgPdt.FTImgKey AS rtImgKey, TCNMImgPdt.FTImgObj AS rtImgObj,");
                    oSql.AppendLine("TCNMImgPdt.FDLastUpdOn AS rdLastUpdOn, TCNMImgPdt.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMImgPdt.FTLastUpdBy AS rtLastUpdBy, TCNMImgPdt.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMImgPdt with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtTouchGrp with(nolock) ON TCNMImgPdt.FTImgRefID = TCNMPdtTouchGrp.FTTcgCode AND TCNMImgPdt.FTImgTable = 'TCNMPdtTouchGrp'");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtTouchGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtTouchGrpDwn.raPdtTouchGrpImg = oDB.C_DATaSqlQuery<cmlResInfoImgPdt>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }


                aoResult.roItem = oPdtTouchGrpDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtTouchGrpDwn>();
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