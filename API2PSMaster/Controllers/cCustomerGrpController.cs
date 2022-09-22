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
    /// Manage customer group.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Customer/Group")]
    public class cCustomerGrpController : ControllerBase
    {
        /// <summary>
        ///     Download Customer group information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResCstGrpDwn> GET_PDToDownloadCstGrp(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResCstGrpDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCstGrpDwn oCstGrpDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCstGrpDwn>();
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

                tKeyCache = "CustomerGrp" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCstGrpDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCgpCode AS rtCgpCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMCstGrp with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResCstGrpDwn();
                oCstGrpDwn = new cmlResCstGrpDwn();

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
                //            oCstGrpDwn.raCstGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstGrp>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                oCstGrpDwn.raCstGrp = oDB.C_DATaSqlQuery<cmlResInfoCstGrp>(oSql.ToString()); //*Arm 64-10-19
                if (oCstGrpDwn.raCstGrp.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstGrp_L.FTCgpCode AS rtCgpCode, TCNMCstGrp_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMCstGrp_L.FTCgpName AS rtCgpName, TCNMCstGrp_L.FTCgpRmk AS rtCgpRmk");
                    oSql.AppendLine("FROM TCNMCstGrp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCstGrp with(nolock) ON TCNMCstGrp_L.FTCgpCode = TCNMCstGrp.FTCgpCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCstGrp.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstGrpDwn.raCstGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstGrpLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstGrpDwn.raCstGrpLng = oDB.C_DATaSqlQuery<cmlResInfoCstGrpLng>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                aoResult.roItem = oCstGrpDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCstGrpDwn>();
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