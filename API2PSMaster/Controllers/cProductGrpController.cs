using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Response.Product;
using Dapper;
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
    ///     Product group information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductGrpController : ControllerBase
    {
        /// <summary>
        ///     Download product unit information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Group/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtGrpDwn> GET_PDToDownloadPdtGrp(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtGrpDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtGrpDwn oPdtGrpDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtGrpDwn>();
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

                tKeyCache = "ProductGrp" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtGrpDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPgpCode AS rtPgpCode, FNPgpLevel AS rnPgpLevel, FTPgpParent AS rtPgpParent, FTPgpChain AS rtPgpChain,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtGrp with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResPdtGrpDwn();
                oPdtGrpDwn = new cmlResPdtGrpDwn();

                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oPdtGrpDwn.raPdtGrp = oConn.Query<cmlResInfoPdtGrp>(oSql.ToString(), nCmdTme).ToList();
                    //using (AdaAccEntities oDB = new AdaAccEntities())
                    //{
                    //    using (DbConnection oConn = oDB.Database.Connection)
                    //    {
                    //        oConn.Open();
                    //        DbCommand oCmd = oConn.CreateCommand();
                    //        oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oPdtGrpDwn.raPdtGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtGrp>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    if (oPdtGrpDwn.raPdtGrp.Count > 0)
                        {
                        //Product Languague
                        oSql = new StringBuilder();
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtGrp_L.FTPgpChain AS rtPgpChain, TCNMPdtGrp_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtGrp_L.FTPgpName AS rtPgpName, TCNMPdtGrp_L.FTPgpChainName AS rtPgpChainName, TCNMPdtGrp_L.FTPgpRmk AS rtPgpRmk");
                        oSql.AppendLine("FROM TCNMPdtGrp_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtGrp with(nolock) ON TCNMPdtGrp_L.FTPgpChain = TCNMPdtGrp.FTPgpChain");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oPdtGrpDwn.raPdtGrpLng = oConn.Query<cmlResInfoPdtGrpLng>(oSql.ToString(), nCmdTme).ToList();
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtGrpDwn.raPdtGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtGrpLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        //Product Image
                        oSql = new StringBuilder();
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMImgPdt.FNImgID AS rnImgID, TCNMImgPdt.FTImgRefID AS rtImgRefID, TCNMImgPdt.FNImgSeq AS rnImgSeq,");
                        oSql.AppendLine("TCNMImgPdt.FTImgTable AS rtImgTable, TCNMImgPdt.FTImgKey AS rtImgKey, TCNMImgPdt.FTImgObj AS rtImgObj,");
                        oSql.AppendLine("TCNMImgPdt.FDLastUpdOn AS rdLastUpdOn, TCNMImgPdt.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMImgPdt.FTLastUpdBy AS rtLastUpdBy, TCNMImgPdt.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMImgPdt with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtGrp with(nolock) ON TCNMImgPdt.FTImgRefID = TCNMPdtGrp.FTPgpCode AND TCNMImgPdt.FTImgTable = 'TCNMPdtGrp'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oPdtGrpDwn.raPdtGrpImg = oConn.Query<cmlResInfoImgPdt>(oSql.ToString(), nCmdTme).ToList();

                        }
                        else
                        {
                            aoResult.rtCode = oMsg.tMS_RespCode800;
                            aoResult.rtDesc = oMsg.tMS_RespDesc800;
                            return aoResult;
                        }
                }
                

                aoResult.roItem = oPdtGrpDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtGrpDwn>();
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