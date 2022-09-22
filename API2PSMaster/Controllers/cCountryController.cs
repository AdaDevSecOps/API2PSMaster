using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Country;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Download country infomation.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Country")]
    public class cCountryController : ControllerBase
    {
        /// <summary>
        ///     Download country information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResCountryDwn> GET_DWNoDownloadCountry(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB;
            cmlResItem<cmlResCountryDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCountryDwn oCountryDwn;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCountryDwn>();
                aoResult.roItem = new cmlResCountryDwn();
                oCountryDwn = new cmlResCountryDwn();

                oSql = new StringBuilder();
                oDB = new cDatabase();
                
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

                // Get data
                oSql.Clear();
                oSql.AppendLine("SELECT CTY.FTCtyCode AS rtCtyCode, CTY.FTVatCode AS rtVatCode, CTY.FNLngID AS rnLngID, CTY.FTCtyLongitude AS rtCtyLongitude, CTY.FTCtyLatitude AS rtCtyLatitude ");
                //oSql.AppendLine(", CTY.FTCtyStaUse AS rtCtyStaUse, CTY.FTCurCode AS rtCurCode, CTY.FTCtyStaCtrlRate AS rtCtyStaCtrlRate, CTY.FTCtyRefID AS rtCtyRefID ");
                oSql.AppendLine(", CTY.FTCtyStaUse AS rtCtyStaUse, CTY.FTRteIsoCode AS rtRteIsoCode, CTY.FTCtyStaCtrlRate AS rtCtyStaCtrlRate, CTY.FTCtyRefID AS rtCtyRefID "); //*Arm 65-08-18
                oSql.AppendLine(", CTY.FDLastUpdOn AS rdLastUpdOn, CTY.FTLastUpdBy AS rtLastUpdBy, CTY.FDCreateOn AS rdCreateOn, CTY.FTCreateBy AS rtCreateBy ");
                oSql.AppendLine("FROM TCNMCountry CTY WITH(NOLOCK) ");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), CTY.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                oCountryDwn.raCountry = oDB.C_DATaSqlQuery<cmlResInfoCountry>(oSql.ToString());
                
                if (oCountryDwn.raCountry != null && oCountryDwn.raCountry.Count > 0)
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT DISTINCT CTYL.FTCtyCode AS rtCtyCode, CTYL.FNLngID AS rnLngID, CTYL.FTCtyName AS rtCtyName, CTYL.FTCtyRmk AS rtCtyRmk ");
                    oSql.AppendLine("FROM TCNMCountry_L CTYL WITH(NOLOCK) ");
                    oSql.AppendLine("INNER JOIN TCNMCountry CTY WITH(NOLOCK) ON CTY.FTCtyCode = CTYL.FTCtyCode ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), CTY.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                    oCountryDwn.raCountryLng = oDB.C_DATaSqlQuery<cmlResInfoCountryLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                aoResult.roItem = oCountryDwn;
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCountryDwn>();
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
            }
        }
    }
}
