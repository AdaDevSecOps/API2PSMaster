using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Branch;
using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Request.Branch;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Entity.Core;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage Branch Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Branch")]
    public class cBranchController : ControllerBase
    {
        /// <summary>
        ///     Download branch information.
        /// </summary>
        /// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResBchDwn> GET_PDToDownloadBranch(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResBchDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResBchDwn oBchDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResBchDwn>();
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

                tKeyCache = "Branch" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResBchDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTWahCode AS rtWahCode, FTBchType AS rtBchType, FTBchPriority AS rtBchPriority,");
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTPplCode AS rtPplCode, FTBchType AS rtBchType, FTBchPriority AS rtBchPriority,"); //*Arm 63-02-20 - เพิ่ม FTPplCode
                oSql.AppendLine("FTBchRegNo AS rtBchRegNo, FTBchRefID AS rtBchRefID, FDBchStart AS rdBchStart, FDBchStop AS rdBchStop,");
                oSql.AppendLine("FDBchSaleStart AS rdBchSaleStart, FDBchSaleStop AS rdBchSaleStop, FTBchStaActive AS rtBchStaActive,");
                oSql.AppendLine("FTWahCode AS rtWahCode, FNBchDefLang AS rnBchDefLang,"); //*Aarm 63-03-27
                oSql.AppendLine("FTBchUriSrvMQ AS rtBchUriSrvMQ, FTBchUriSrvSG AS rtBchUriSrvSG, FTBchStaHQ AS rtBchStaHQ,");
                oSql.AppendLine("FTMerCode AS rtMerCode, FTAgnCode AS rtAgnCode, "); //*Arm 63-06-23
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy,");
                oSql.AppendLine("FTCtyCode AS rtCtyCode");                  //*Nick 65-09-07
                oSql.AppendLine("FROM TCNMBranch with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResBchDwn();
                oBchDwn = new cmlResBchDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())   //*Em 62-06-09
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oBchDwn.raBch = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoBch>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oBchDwn.raBch = oConn.Query<cmlResInfoBch>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-06-09
                    if (oBchDwn.raBch.Count > 0)
                    {
                        //Languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMBranch_L.FTBchCode AS rtBchCode, TCNMBranch_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMBranch_L.FTBchName AS rtBchName, TCNMBranch_L.FTBchRmk AS rtBchRmk");
                        oSql.AppendLine("FROM TCNMBranch_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMBranch with(nolock) ON TCNMBranch_L.FTBchCode = TCNMBranch.FTBchCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMBranch.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oBchDwn.raBchLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoBchLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oBchDwn.raBchLng = oConn.Query<cmlResInfoBchLng>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-06-09

                        //Address
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
                        oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
                        oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
                        //oSql.AppendLine("TCNMAddress_L.FTAreCode AS rtAreCode, TCNMAddress_L.FTZneCode AS rtZneCode, TCNMAddress_L.FTAddVersion AS rtAddVersion,");
                        oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,"); //*Em 62-04-02  ปรับโครงสร้าง
                        oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
                        oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
                        oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMBranch with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMBranch.FTBchCode AND TCNMAddress_L.FTAddGrpType = '1'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMBranch.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oBchDwn.raAddrLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoAddrLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oBchDwn.raAddrLng = oConn.Query<cmlResInfoAddrLng>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-06-09

                        //Image
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMImgObj.FNImgID AS rnImgID, TCNMImgObj.FTImgRefID AS rtImgRefID, TCNMImgObj.FNImgSeq AS rnImgSeq,");
                        oSql.AppendLine("TCNMImgObj.FTImgTable AS rtImgTable, TCNMImgObj.FTImgKey AS rtImgKey, TCNMImgObj.FTImgObj AS rtImgObj,");
                        oSql.AppendLine("TCNMImgObj.FDLastUpdOn AS rdLastUpdOn, TCNMImgObj.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMImgObj.FTLastUpdBy AS rtLastUpdBy, TCNMImgObj.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMImgObj with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMBranch with(nolock) ON TCNMImgObj.FTImgRefID = TCNMBranch.FTBchCode AND TCNMImgObj.FTImgTable = 'TCNMBranch'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMBranch.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oBchDwn.raImage = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgObj>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oBchDwn.raImage = oConn.Query<cmlResInfoImgObj>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-06-09

                        //*Em 62-09-04
                        oSql.Clear();
                        //oSql.AppendLine("SELECT TCNTUrlObject.FNUrlID AS rnUrlID,TCNTUrlObject.FTUrlRefID AS rtUrlRefID,TCNTUrlObject.FNUrlSeq AS rnUrlSeq,");
                        //oSql.AppendLine("TCNTUrlObject.FTUrlTable AS rtUrlTable,TCNTUrlObject.FTUrlKey AS rtUrlKey,TCNTUrlObject.FTUrlAddress AS rtUrlAddress,");
                        //oSql.AppendLine("TCNTUrlObject.FTUrlPort AS rtUrlPort,TCNTUrlObject.FTUrlLogo AS rtUrlLogo,TCNTUrlObject.FDLastUpdOn AS rdLastUpdOn,");
                        //oSql.AppendLine("TCNTUrlObject.FTLastUpdBy AS rtLastUpdBy,TCNTUrlObject.FDCreateOn AS rdCreateOn,TCNTUrlObject.FTCreateBy AS rtCreateBy");

                        //*Em 62-09-13
                        oSql.AppendLine("SELECT TCNTUrlObject.FNUrlID AS rnUrlID,TCNTUrlObject.FTUrlRefID AS rtUrlRefID,TCNTUrlObject.FNUrlSeq AS rnUrlSeq,");
                        oSql.AppendLine("TCNTUrlObject.FNUrlType AS rnUrlType,TCNTUrlObject.FTUrlTable AS rtUrlTable,TCNTUrlObject.FTUrlKey AS rtUrlKey,");
                        oSql.AppendLine("TCNTUrlObject.FTUrlAddress AS rtUrlAddress,TCNTUrlObject.FTUrlPort AS rtUrlPort,TCNTUrlObject.FTUrlLogo AS rtUrlLogo,");
                        oSql.AppendLine("TCNTUrlObject.FDLastUpdOn AS rdLastUpdOn,TCNTUrlObject.FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("TCNTUrlObject.FDCreateOn AS rdCreateOn,TCNTUrlObject.FTCreateBy AS rtCreateBy");
                        //+++++++++++++
                        oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMBranch WITH(NOLOCK) ON TCNMBranch.FTBchCode = TCNTUrlObject.FTUrlRefID AND TCNTUrlObject.FTUrlTable = 'TCNMBranch'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMBranch.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //*Arm 63-08-17
                        oSql.AppendLine("UNION ALL");
                        oSql.AppendLine("SELECT FNUrlID AS rnUrlID,FTUrlRefID AS rtUrlRefID,FNUrlSeq AS rnUrlSeq,");
                        oSql.AppendLine("FNUrlType AS rnUrlType,FTUrlTable AS rtUrlTable,FTUrlKey AS rtUrlKey,");
                        oSql.AppendLine("FTUrlAddress AS rtUrlAddress,FTUrlPort AS rtUrlPort,FTUrlLogo AS rtUrlLogo,");
                        oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn,FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("FDCreateOn AS rdCreateOn,FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                        oSql.AppendLine("WHERE FTUrlRefID = 'CENTER' AND FTUrlTable = 'TCNMComp'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //+++++++++++++
                        oBchDwn.raTCNTUrlObject = oConn.Query<cmlResTCNTUrlObject>(oSql.ToString(), nCmdTme).ToList();

                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNTUrlObjectLogin.FTUrlRefID AS rtUrlRefID,TCNTUrlObjectLogin.FTUrlAddress AS rtUrlAddress,TCNTUrlObjectLogin.FTUolVhost AS rtUolVhost,");
                        oSql.AppendLine("TCNTUrlObjectLogin.FTUolUser AS rtUolUser,TCNTUrlObjectLogin.FTUolPassword AS rtUolPassword,TCNTUrlObjectLogin.FTUolKey AS rtUolKey,");
                        oSql.AppendLine("TCNTUrlObjectLogin.FTUolStaActive AS rtUolStaActive,TCNTUrlObjectLogin.FTUolgRmk AS rtUolgRmk");
                        oSql.AppendLine("FROM TCNTUrlObjectLogin WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNTUrlObject WITH(NOLOCK) ON TCNTUrlObject.FTUrlRefID = TCNTUrlObjectLogin.FTUrlRefID AND TCNTUrlObject.FTUrlAddress = TCNTUrlObjectLogin.FTUrlAddress");
                        oSql.AppendLine("INNER JOIN TCNMBranch WITH(NOLOCK) ON TCNMBranch.FTBchCode = TCNTUrlObject.FTUrlRefID AND TCNTUrlObject.FTUrlTable = 'TCNMBranch'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMBranch.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //*Net 63-10-02
                        oSql.AppendLine("UNION ALL");
                        oSql.AppendLine("SELECT DISTINCT TCNTUrlObjectLogin.FTUrlRefID AS rtUrlRefID,TCNTUrlObjectLogin.FTUrlAddress AS rtUrlAddress,TCNTUrlObjectLogin.FTUolVhost AS rtUolVhost,");
                        oSql.AppendLine("TCNTUrlObjectLogin.FTUolUser AS rtUolUser,TCNTUrlObjectLogin.FTUolPassword AS rtUolPassword,TCNTUrlObjectLogin.FTUolKey AS rtUolKey,");
                        oSql.AppendLine("TCNTUrlObjectLogin.FTUolStaActive AS rtUolStaActive,TCNTUrlObjectLogin.FTUolgRmk AS rtUolgRmk");
                        oSql.AppendLine("FROM TCNTUrlObjectLogin WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNTUrlObject WITH(NOLOCK) ON TCNTUrlObject.FTUrlRefID = TCNTUrlObjectLogin.FTUrlRefID AND TCNTUrlObject.FTUrlAddress = TCNTUrlObjectLogin.FTUrlAddress");
                        oSql.AppendLine("WHERE TCNTUrlObjectLogin.FTUrlRefID = 'CENTER' AND FTUrlTable = 'TCNMComp'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //+++++++++++++
                        oBchDwn.raTCNTUrlObjectLogin = oConn.Query<cmlResTCNTUrlObjectLogin>(oSql.ToString(), nCmdTme).ToList();
                        //++++++++++++++++++++
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oBchDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResBchDwn>();
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

        /// <summary>
        /// Insert Branch
        /// </summary>
        /// <param name="poparam"> data from API</param>
        /// <returns></returns>
        [Route("Insert")]
        [HttpPost]
        public cmlResList<cmlResBchDwn> POST_PDToInsBranch(cmlReqBranch poparam)
        {

            cSP oFunc;
            cCS oCS;
            cMS oMsg;

            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            cmlResList<cmlResBchDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResBchDwn>();
                oFunc = new cSP();
                oMsg = new cMS();
                oCS = new cCS();

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

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {

                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        DbTransaction oTrans;
                        oTrans = oConn.BeginTransaction();
                        oCmd.Connection = oConn;
                        oCmd.Transaction = oTrans;
                        try
                        {

                            oSql = new StringBuilder();
                            oSql.Clear();
                            oSql.AppendLine(" INSERT INTO TCNMBranch ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" ( FTBchCode, FTWahCode, FTBchType, FTBchPriority, FTBchRegNo,");
                            oSql.AppendLine("  FTBchRefID, FDBchStart, FDBchStop, FDBchSaleStart, FDBchSaleStop,");
                            oSql.AppendLine(" FTBchStaActive, FDDateUpd, FTTimeUpd,");
                            oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns ");
                            oSql.AppendLine(" )");
                            oSql.AppendLine(" VALUES( '" + poparam.ptBchCode + "',");
                            oSql.AppendLine(" '" + poparam.ptWahCode + "',");
                            oSql.AppendLine("'" + poparam.ptBchType + "',");
                            oSql.AppendLine("'" + poparam.ptBchPriority + "',");
                            oSql.AppendLine("'" + poparam.ptBchRegNo + "',");
                            oSql.AppendLine(" '" + poparam.ptBchRefID + "' ,");
                            oSql.AppendLine(" '" + string.Format("{0:yyyy-MM-dd}", poparam.pdBchStart) + "',");
                            oSql.AppendLine(" '" + string.Format("{0:yyyy-MM-dd}", poparam.pdBchStop) + "',");
                            oSql.AppendLine("'" + string.Format("{0:yyyy-MM-dd}", poparam.pdBchSaleStart) + "',");
                            oSql.AppendLine("'" + string.Format("{0:yyyy-MM-dd}", poparam.pdBchSaleStop) + "',");
                            oSql.AppendLine("'" + poparam.ptBchStaActive + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "' ");
                            oSql.AppendLine(")");


                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" INSERT INTO TCNMBranch_L");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" (FTBchCode, FNLngID, FTBchName, FTBchRmk) ");
                            oSql_L.AppendLine(" VALUES");
                            oSql_L.AppendLine("('" + poparam.ptBchCode + "', ");
                            oSql_L.AppendLine("'" + poparam.pnLngID + "', ");
                            oSql_L.AppendLine(" '" + poparam.ptBchName + "',");
                            oSql_L.AppendLine(" '" + poparam.ptBchRmk + "'");
                            oSql_L.AppendLine(")");

                            oSql_Img = new StringBuilder();
                            for (var nList = 0; nList < poparam.roImgUpl.Count; nList++)
                            {
                              if(poparam.roImgUpl[nList].ptImgobj != "")

                                {
                                    oFunc.SP_IMGbInsImgPath(poparam.ptBchCode, "Branch/", poparam.roImgUpl[nList].ptImgobj,
                                                              poparam.roImgUpl[nList].ptImgRefID, poparam.roImgUpl[nList].ptImgKey,
                                                              poparam.ptWhoUpd, "TCNMBranch");
                                }
                              
                            }
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            oCmd.ExecuteNonQuery();

                            oTrans.Commit();

                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    aoResult.rtCode = oMsg.tMS_RespCode801;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                    return aoResult;

                                default:
                                    //Error statement or sql error
                                    aoResult.rtCode = oMsg.tMS_RespCode999;
                                    aoResult.rtDesc = oSqlExn.Message;
                                    return aoResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    aoResult.rtCode = oMsg.tMS_RespCode905;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return aoResult;
                            }
                        }
                    }
                }
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;

            }
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResList<cmlResBchDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;

            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;
                oSql_Img = null;
                oSql_L = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();


            }

        }

        /// <summary>
        /// Update Branch
        /// </summary>
        /// <param name="poparam">data from API</param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public cmlResList<cmlResBchDwn> POST_PDToUptBranch(cmlReqBranch poparam)
        {

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_M;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResBchDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblBch;
            DataTable oDbTblBch_L;
            DataTable oDbTblImg;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResBchDwn>();
                oFunc = new cSP();
                oMsg = new cMS();
                oCS = new cCS();


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


                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTWahCode, FTBchType, FTBchPriority, FTBchRegNo,   ");
                oSql.AppendLine("FTBchRefID, FDBchStart, FDBchStop, FDBchSaleStart, FDBchSaleStop, FTBchStaActive");
                oSql.AppendLine("FROM TCNMBranch WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poparam.ptBchCode + "'");
                oDbTblBch = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1) FNLngID, FTBchName, FTBchRmk");
                oSql.AppendLine(" FROM TCNMBranch_L");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTBchCode = '" + poparam.ptBchCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblBch_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblBch != null && oDbTblBch.Rows.Count > 0)
                {
                    if (oDbTblBch_L != null && oDbTblBch_L.Rows.Count > 0)
                    {
                        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                        {

                            using (DbConnection oConn = oAdaAcc.Database.Connection)
                            {
                                oConn.Open();
                                DbCommand oCmd = oConn.CreateCommand();
                                DbTransaction oTrans;
                                oTrans = oConn.BeginTransaction();
                                oCmd.Connection = oConn;
                                oCmd.Transaction = oTrans;

                                try
                                {

                                    oSql_M = new StringBuilder();
                                    oSql_M.Clear();
                                    oSql_M.AppendLine(" UPDATE ");
                                    oSql_M.AppendLine(" TCNMBranch ");
                                    oSql_M.AppendLine(" WITH(ROWLOCK)");
                                    oSql_M.AppendLine(" SET ");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptWahCode, "FTWahCode") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptBchType, "FTBchType") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptBchPriority, "FTBchPriority") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(poparam.ptBchRegNo, "FTBchRegNo") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(poparam.ptBchRefID, "FTBchRefID") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdBchStart), "FDBchStart") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdBchStop), "FDBchStop") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdBchSaleStart), "FDBchSaleStart") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdBchSaleStop), "FDBchSaleStop") + "");
                                    oSql_M.AppendLine("" +  oFunc.SP_SETtValueUpt(poparam.ptBchStaActive, "FTBchStaActive") + "");
                                    oSql_M.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                                    oSql_M.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                                    oSql_M.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                                    oSql_M.AppendLine(" WHERE FTBchCode = '" + poparam.ptBchCode + "'");

                                    oSql_L = new StringBuilder();
                                    oSql_L.Clear();
                                    oSql_L.AppendLine(" UPDATE ");
                                    oSql_L.AppendLine(" TCNMBranch_L ");
                                    oSql_L.AppendLine(" WITH(ROWLOCK)");
                                    oSql_L.AppendLine(" SET");
                                    oSql_L.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptBchRmk, "FTBchRmk") + "");
                                    oSql_L.AppendLine(" FTBchName = '" + poparam.ptBchName + "'");
                                    oSql_L.AppendLine(" WHERE FTBchCode = '" + poparam.ptBchCode + "' ");
                                    oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");

                                    oSql_Img = new StringBuilder();

                                    for (var nList = 0; nList < poparam.roImgUpl.Count; nList++)
                                    {
                                        if(poparam.roImgUpl[nList].ptImgobj != "")
                                        {
                                            oFunc.SP_IMGbInsImgPath(poparam.ptBchCode, "Branch/", poparam.roImgUpl[nList].ptImgobj,
                                                              poparam.roImgUpl[nList].ptImgRefID, poparam.roImgUpl[nList].ptImgKey,
                                                              poparam.ptWhoUpd, "TCNMBranch");
                                        }
                                    }
                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_M.ToString();
                                    oCmd.ExecuteNonQuery();

                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_L.ToString();
                                    int nSuccess = oCmd.ExecuteNonQuery();
                                    if (nSuccess == 0)
                                    {
                                        aoResult = new cmlResList<cmlResBchDwn>();
                                        //aoResult = new cmlResPdtItemDwn();
                                        aoResult.rtCode = "";
                                        aoResult.rtDesc = "";
                                        return aoResult;
                                    }

                                    oTrans.Commit();
                                }
                                catch (SqlException oSqlExn)
                                {
                                    oTrans.Rollback();
                                    switch (oSqlExn.Number)
                                    {
                                        case 2627:
                                            // Data is duplicate.
                                            aoResult.rtCode = oMsg.tMS_RespCode801;
                                            aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                            return aoResult;

                                        default:
                                            //Error statement or sql error
                                            aoResult.rtCode = oMsg.tMS_RespCode999;
                                            aoResult.rtDesc = oSqlExn.Message;
                                            return aoResult;
                                    }
                                }
                                catch (EntityException oEtyExn)
                                {
                                    oTrans.Rollback();
                                    switch (oEtyExn.HResult)
                                    {
                                        case -2146232060:
                                            // Cannot connect database..
                                            aoResult.rtCode = oMsg.tMS_RespCode905;
                                            aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                            return aoResult;
                                    }
                                }
                            }
                        }

                        aoResult.rtCode = oMsg.tMS_RespCode001;
                        aoResult.rtDesc = oMsg.tMS_RespDesc001;
                        return aoResult;
                    }
                    else
                    {
                        aoResult = new cmlResList<cmlResBchDwn>();
                        //aoResult = new cmlResPdtItemDwn();
                        aoResult.rtCode = "";
                        aoResult.rtDesc = "";
                        return aoResult;
                    }

                }
                else
                {
                    aoResult = new cmlResList<cmlResBchDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }


            }
            catch (Exception oExn)
            {
                // Return error.
                aoResult = new cmlResList<cmlResBchDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;
                oSql_L = null;
                oSql_M = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        /// Delete Branch
        /// </summary>
        /// <param name="poparam">data from API</param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        public cmlResList<cmlResBchDwn> POST_PDToDelBranch(cmlReqBranchDel poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            StringBuilder oSql_Addr;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResBchDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                tFuncName = MethodBase.GetCurrentMethod().Name;

                aoResult = new cmlResList<cmlResBchDwn>();
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

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {

                    oSql = new StringBuilder();
                    oSql.Clear();
                    oSql.AppendLine(" DELETE ");
                    oSql.AppendLine(" FROM TCNMBranch  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTBchCode = '" + poparam.ptBchCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMBranch_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTBchCode = '" + poparam.ptBchCode + "') ");
                    oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");

                    oSql_Img = new StringBuilder();
                    oSql_Img.Clear();
                    oSql_Img.AppendLine(" DELETE ");
                    oSql_Img.AppendLine(" FROM TCNMImgObj WITH(ROWLOCK) ");
                    oSql_Img.AppendLine("  WHERE (FTImgRefID = '" + poparam.ptBchCode + "') ");

                    oSql_Addr = new StringBuilder();
                    oSql_Addr.Clear();
                    oSql_Addr.AppendLine(" DELETE ");
                    oSql_Addr.AppendLine(" FROM TCNMAddress_L WITH(ROWLOCK)");
                    oSql_Addr.AppendLine(" WHERE FTAddRefCode = '" + poparam.ptBchCode +"'");

                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        DbTransaction oTrans;
                        oTrans = oConn.BeginTransaction();
                        oCmd.Connection = oConn;
                        oCmd.Transaction = oTrans;
                        try
                        {
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Img.ToString();
                            oCmd.ExecuteNonQuery();


                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            int nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                aoResult = new cmlResList<cmlResBchDwn>();
                                //aoResult = new cmlResPdtItemDwn();
                                aoResult.rtCode = "";
                                aoResult.rtDesc = "";
                                return aoResult;
                            }

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Addr.ToString();
                            oCmd.ExecuteNonQuery();


                            oTrans.Commit();
                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    aoResult.rtCode = oMsg.tMS_RespCode801;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                    return aoResult;

                                default:
                                    //Error statement or sql error
                                    aoResult.rtCode = oMsg.tMS_RespCode999;
                                    aoResult.rtDesc = oSqlExn.Message;
                                    return aoResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    aoResult.rtCode = oMsg.tMS_RespCode905;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return aoResult;
                            }
                        }
                    }
                }
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;

            }
            catch(Exception oExn)
            {
                // Return error.
                aoResult = new cmlResList<cmlResBchDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();

            }
        }
        
        /// <summary>
        /// Insert / Update Address
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Address")]
        [HttpPost]
        public cmlResList<cmlResBchDwn> POST_PDToInsAddr(cmlReqAddress poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql_Ins;
            StringBuilder oSql_Upt;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResBchDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResBchDwn>();
                oFunc = new cSP();
                oMsg = new cMS();
                oCS = new cCS();

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

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {

                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        DbTransaction oTrans;
                        oTrans = oConn.BeginTransaction();
                        oCmd.Connection = oConn;
                        oCmd.Transaction = oTrans;

                        try
                        {
                            oSql_Upt = new StringBuilder();
                            oSql_Upt.Clear();
                            oSql_Upt.AppendLine(" UPDATE TCNMAddress_L ");
                            oSql_Upt.AppendLine(" SET ");
                            oSql_Upt.AppendLine(" FTAddGrpType = '" + poparam.ptAddGrpType + "', ");
                            oSql_Upt.AppendLine(" FTAddRefCode = '" + poparam.ptAddRefCode + "',");
                            oSql_Upt.AppendLine(" FTAddRefNo = '" + poparam.ptAddRefNo +"',");
                            oSql_Upt.AppendLine(" FTAddName = '" + poparam.ptAddName + "',");
                            oSql_Upt.AppendLine(" FTAddTaxNo = '" + poparam.ptAddTaxNo + "', ");
                            oSql_Upt.AppendLine(" FTAddRmk = '" + poparam.ptAddRmk +"',  ");
                            oSql_Upt.AppendLine(" FTAddCountry = '" + poparam.ptAddCountry + "', ");
                            oSql_Upt.AppendLine(" FTAreCode = '" + poparam.ptAreCode +"',");
                            oSql_Upt.AppendLine(" FTZneCode = '" + poparam.ptZneCode +"', ");
                            oSql_Upt.AppendLine(" FTAddVersion = '" + poparam.ptVersion + "',");
                            oSql_Upt.AppendLine(" FTAddV1No = '" + poparam.ptV1No + "',");
                            oSql_Upt.AppendLine(" FTAddV1Soi = '" + poparam.ptV1Soi +"',");
                            oSql_Upt.AppendLine(" FTAddV1Village = '" + poparam.ptV1Village + "',");
                            oSql_Upt.AppendLine(" FTAddV1Road = '" + poparam.ptV1Road +"',");
                            oSql_Upt.AppendLine(" FTAddV1SubDist = '" + poparam.ptV1SubDist + "',");
                            oSql_Upt.AppendLine(" FTAddV1DstCode = '" + poparam.ptV1DstCode +"',");
                            oSql_Upt.AppendLine(" FTAddV1PvnCode = '" + poparam.ptV1PvnCode + "',");
                            oSql_Upt.AppendLine(" FTAddV1PostCode = '" + poparam.ptV1PostCode +"',  ");
                            oSql_Upt.AppendLine(" FTAddV2Desc1 = '" + poparam.ptV2Desc1 + "',");
                            oSql_Upt.AppendLine(" FTAddV2Desc2 = '" + poparam.ptV2Desc2 +"', ");
                            oSql_Upt.AppendLine(" FTAddWebsite = '" + poparam.ptAddWebsite + "',");
                            oSql_Upt.AppendLine(" FTAddLongitude = '" + poparam.ptAddLongtitude +"',  ");
                            oSql_Upt.AppendLine(" FTAddLatitude = '" + poparam.ptAddLatitude + "',");
                            oSql_Upt.AppendLine(" FDDateUpd =  CONVERT(VARCHAR(10), GETDATE(), 121) ,");
                            oSql_Upt.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql_Upt.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd +"'");
                            oSql_Upt.AppendLine(" WHERE FNLngID = '" + poparam.pnLngID +"'");
                            oSql_Upt.AppendLine(" AND FTAddGrpType = '" + poparam.ptAddGrpType + "'");
                            oSql_Upt.AppendLine(" AND FTAddRefCode = '" + poparam.ptAddRefCode + "'");



                            oSql_Ins = new StringBuilder();
                            oSql_Ins.Clear();
                            oSql_Ins.AppendLine(" INSERT INTO TCNMAddress_L ");
                            oSql_Ins.AppendLine(" (FNLngID, FTAddGrpType, FTAddRefCode,  ");
                            oSql_Ins.AppendLine(" FTAddRefNo, FTAddName, FTAddTaxNo,");
                            oSql_Ins.AppendLine(" FTAddRmk, FTAddCountry, FTAreCode, FTZneCode, ");
                            oSql_Ins.AppendLine(" FTAddVersion, FTAddV1No, FTAddV1Soi, FTAddV1Village,");
                            oSql_Ins.AppendLine(" FTAddV1Road, FTAddV1SubDist, FTAddV1DstCode, ");
                            oSql_Ins.AppendLine(" FTAddV1PvnCode, FTAddV1PostCode, FTAddV2Desc1, ");
                            oSql_Ins.AppendLine(" FTAddV2Desc2, FTAddWebsite, FTAddLongitude, ");
                            oSql_Ins.AppendLine(" FTAddLatitude, FDDateUpd, FTTimeUpd, FTWhoUpd)");
                            oSql_Ins.AppendLine(" VALUES ('" + poparam.pnLngID + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddGrpType + "' ,");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddRefCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddRefNo + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddName + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddTaxNo + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddRmk + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddCountry + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAreCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptZneCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptVersion + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1No + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1Soi + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1Village + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1Road + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1SubDist + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1DstCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1PvnCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1PostCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV2Desc1 + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV2Desc2 + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddWebsite + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddLongtitude + "', ");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddLatitude + "', ");
                            oSql_Ins.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql_Ins.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql_Ins.AppendLine(" '" + poparam.ptWhoUpd + "'");
                            oSql_Ins.AppendLine(")");

                            int nSuccess;
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Upt.ToString();
                            nSuccess = oCmd.ExecuteNonQuery();
                            if(nSuccess == 0)
                            {
                                oCmd.CommandTimeout = nCmdTme;
                                oCmd.CommandType = CommandType.Text;
                                oCmd.CommandText = oSql_Ins.ToString();
                                oCmd.ExecuteNonQuery();
                            }

                      
                            oTrans.Commit();

                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    aoResult.rtCode = oMsg.tMS_RespCode801;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
                                    return aoResult;

                                default:
                                    //Error statement or sql error
                                    aoResult.rtCode = oMsg.tMS_RespCode999;
                                    aoResult.rtDesc = oSqlExn.Message;
                                    return aoResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    aoResult.rtCode = oMsg.tMS_RespCode905;
                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return aoResult;
                            }
                        }
                    }
                }
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;

            }
            catch(Exception oExn)
            {
                // Return error.
                aoResult = new cmlResList<cmlResBchDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql_Ins = null;
                oSql_Upt = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

    }
}
