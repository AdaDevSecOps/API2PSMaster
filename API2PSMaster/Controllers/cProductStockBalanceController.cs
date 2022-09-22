using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage Product Stock Balance.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductStockBalanceController : ControllerBase
    {
        /// <summary>
        ///     Download Product Stock Balance.
        /// </summary>
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <param name="ptWahCode">รหัสคลัง</param>
        /// <returns>
        ///&#8195;     001 : Success.<br/>
        ///&#8195;     701 : validate parameter model false.|<br/>
        ///&#8195;     800 : Data not found.<br/>
        ///&#8195;     900 : Service process false.<br/>
        ///&#8195;     904 : Key not allowed to use method.<br/>
        /// </returns>
        [Route("StockBalance/Download")]
        [HttpGet]
        public cmlResList<cmlResInfoPdtStkBal> GET_DWNoDownloadPdtStkBal(string ptBchCode, string ptWahCode="")
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResInfoPdtStkBal> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            int nCmdTme = 60;
            string tFuncName, tModelErr, tKeyApi;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResList<cmlResInfoPdtStkBal>();
                //aoResult = new cmlResPdtItemDwn();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();

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

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTWahCode AS rtWahCode, FTPdtCode AS rtPdtCode, FCStkQty AS rcStkQty,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNTPdtStkBal WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + ptBchCode + "'");

                if (!string.IsNullOrEmpty(ptWahCode))
                {
                    oSql.AppendLine("AND FTWahCode = '" + ptWahCode + "'");
                }

                aoResult.raItems = new cDatabase().C_DATaSqlQuery<cmlResInfoPdtStkBal>(oSql.ToString(),60);
                if(aoResult.raItems != null && aoResult.raItems.Count > 0)
                {
                    
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch(Exception oEx)
            {
                aoResult = new cmlResList<cmlResInfoPdtStkBal>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oEx.Message.ToString();
                return aoResult;
            }
            finally
            {
                oSql = null;
                aoResult = null;
                aoSysConfig = null;
                oFunc = null;
                oMsg = null;
                oCS = null;
            }
        }

    }
}