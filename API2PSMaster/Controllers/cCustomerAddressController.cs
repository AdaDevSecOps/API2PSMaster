using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.Database;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Customer;
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
    /// Manage customer address.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Customer/Address")]
    public class cCustomerAddressController : ControllerBase
    {
        /// <summary>
        ///     Download Customer address information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResList<cmlResInfoCstAddrLng> GET_PDToDownloadCstAddr(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResList<cmlResInfoCstAddrLng> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoCstAddrLng>();
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

                tKeyCache = "CustomerAddress" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResInfoCstAddrLng>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCstCode AS rtCstCode, FNLngID AS rnLngID, FTAddGrpType AS rtAddGrpType, FNAddSeqNo AS rnAddSeqNo, ");
                oSql.AppendLine("FTAddRefNo AS rtAddRefNo, FTAddName AS rtAddName, FTAddRmk AS rtAddRmk, FTAddCountry AS rtAddCountry,");
                oSql.AppendLine("FTAreCode AS rtAreCode, FTZneCode AS rtZneCode, FTAddVersion AS rtAddVersion, FTAddV1No AS rtAddV1No,");
                oSql.AppendLine("FTAddV1Soi AS rtAddV1Soi, FTAddV1Village AS rtAddV1Village, FTAddV1Road AS rtAddV1Road, FTAddV1SubDist AS rtAddV1SubDist,");
                oSql.AppendLine("FTAddV1DstCode AS rtAddV1DstCode, FTAddV1PvnCode AS rtAddV1PvnCode, FTAddV1PostCode AS rtAddV1PostCode, FTAddV2Desc1 AS rtAddV2Desc1,");
                oSql.AppendLine("FTAddV2Desc2 AS rtAddV2Desc2, FTAddWebsite AS rtAddWebsite, FTAddLongitude AS rtAddLongitude, FTAddLatitude AS rtAddLatitude,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMCstAddress_L with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.raItems = new List<cmlResInfoCstAddrLng>();
                //*Arm 64-10-19 Comment Code 
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            aoResult.raItems = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstAddrLng>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }
                aoResult.raItems = oDB.C_DATaSqlQuery<cmlResInfoCstAddrLng>(oSql.ToString()); //*Arm 64-10-19
                if (aoResult.raItems.Count > 0)
                {

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResInfoCstAddrLng>();
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