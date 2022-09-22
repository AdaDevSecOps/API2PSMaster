using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Sale;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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
    ///     Sale information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Sale")]
    public class cSaleController : ControllerBase
    {
        /// <summary>
        ///     Product hot sale.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <param name="ptShop"></param>
        /// <returns></returns>
        [Route("HotSale")]
        [HttpGet]
        public cmlResList<cmlResHotSale> GET_DAToDownloadHotSale(DateTime pdDate,String ptShop)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResHotSale> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            List<cmlResHotSale> aoHotSale;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResHotSale>();
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

                tKeyCache = "HotSale" + ptShop + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResHotSale>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode,FTShpCode AS rtShpCode,FTPosCode AS rtPosCode,");
                oSql.AppendLine("FTPdtCode AS rtPdtCode,FCXsdQty AS rcXsdQty,FDCreateOn AS rdCreateOn");
                oSql.AppendLine("FROM TVDTPdtHotSale with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDCreateOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                if (!string.IsNullOrEmpty(ptShop)) oSql.AppendLine("AND FTShpCode = '"+ ptShop +"'");
                aoResult.raItems = new List<cmlResHotSale>();

                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                    {
                        aoResult.raItems = oConn.Query<cmlResHotSale>(oSql.ToString(),nCmdTme).ToList();

                        if (aoResult.raItems.Count > 0)
                        {
                        }
                        else
                        {
                            aoResult.rtCode = oMsg.tMS_RespCode800;
                            aoResult.rtDesc = oMsg.tMS_RespDesc800;
                            return aoResult;
                        }
                    }

                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResList<cmlResHotSale>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oEx.Message.ToString();
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