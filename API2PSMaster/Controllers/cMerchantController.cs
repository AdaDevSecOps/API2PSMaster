using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Merchant;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Merchant information
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Merchant")]
    public class cMerchantController : ControllerBase
    {
        /// <summary>
        ///     Download Merchant information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResMerchant> GET_MERoDownloadMerchant(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResMerchant> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResMerchant oMerchant;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResMerchant>();
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

                tKeyCache = "Merchant" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResMerchant>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTMerCode AS rtMerCode, FTPplCode AS rtPplCode, FTMerEmail AS rtMerEmail, FTMerTel AS rtMerTel,");      // *Arm 63-06-17  เพิม  FTPplCode
                oSql.AppendLine("FTMerFax AS rtMerFax, FTMerMo AS rtMerMo, FTMerStaActive AS rtMerStaActive, FTMerRefCode AS rtMerRefCode,");   // *Arm 63-06-17 เพิ่ม FTMerRefCode
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy,");
                oSql.AppendLine("FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMMerchant WITH(NOLOCK)");;
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResMerchant();
                oMerchant = new cmlResMerchant();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oMerchant.raTCNMMerchant = oConn.Query<cmlTCNMMerchant>(oSql.ToString(), nCmdTme).ToList();
                    if (oMerchant.raTCNMMerchant.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNMMerchant_L.FTMerCode AS rtMerCode, TCNMMerchant_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMMerchant_L.FTMerName AS rtMerName, TCNMMerchant_L.FTMerRmk AS rtMerRmk");
                        oSql.AppendLine("FROM TCNMMerchant_L WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMMerchant WITH(NOLOCK) ON TCNMMerchant_L.FTMerCode = TCNMMerchant.FTMerCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMMerchant.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oMerchant.raTCNMMerchant_L = oConn.Query<cmlTCNMMerchant_L>(oSql.ToString(), nCmdTme).ToList();

                        //Address
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
                        oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
                        oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
                        oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,"); //*Em 62-04-02  ปรับโครงสร้าง
                        oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
                        oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
                        oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMMerchant with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMMerchant.FTMerCode AND TCNMAddress_L.FTAddGrpType = '7'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMMerchant.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oMerchant.raAddrLng = oConn.Query<cmlResInfoAddrLng>(oSql.ToString(), nCmdTme).ToList();  //*Em 62-06-09
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oMerchant;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResMerchant>();
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