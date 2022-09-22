using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Agency;
using API2PSMaster.Models.WebService.Response.Base;
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
    ///     Agency infomation.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Agency")]
    public class cAgencyController : ControllerBase
    {
        /// <summary>
        /// Sync Download Data Agency
        /// </summary>
        /// <param name="pdDate">Date (yyyy-MM-dd)</param>
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResAgencyDwn> GET_DWNoDownloadAgency(DateTime pdDate, string ptBchCode)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB;
            cmlResItem<cmlResAgencyDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResAgencyDwn oAgnDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResAgencyDwn>();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(21600, 21600, false);

                #region Check Para
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
                #endregion

                #region Check APIKey
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

                //tKeyCache = "Channel" + string.Format("{0:yyyyMMdd}", pdDate);
                //if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                //{
                //    // ถ้ามี key อยุ่ใน cache
                //    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResAgencyDwn>>(tKeyCache);
                //    aoResult.rtCode = oMsg.tMS_RespCode001;
                //    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                //    return aoResult;
                //}
                #endregion

                #region Get Data

                aoResult.roItem = new cmlResAgencyDwn();
                oAgnDwn = new cmlResAgencyDwn();

                oSql = new StringBuilder();
                oDB = new cDatabase();
                oSql.AppendLine("SELECT AD.FTAgnCode AS rtAgnCode, AD.FTPplCode AS rtPplCode, AD.FTAgnKeyAPI AS rtAgnKeyAPI, AD.FTAgnPwd AS rtAgnPwd, AD.FTAgnEmail AS rtAgnEmail, ");
                oSql.AppendLine("AD.FTAgnTel AS rtAgnTel, AD.FTAgnFax AS rtAgnFax, AD.FTAgnMo AS rtAgnMo, AD.FTAgnStaApv AS rtAgnStaApv, AD.FTAgnStaActive AS rtAgnStaActive, ");
                oSql.AppendLine("AD.FTAtyCode AS rtAtyCode, AD.FTAggCode AS rtAggCode, AD.FTAgnRefCode AS rtAgnRefCode, AD.FTChnCode AS rtChnCode, ");
                oSql.AppendLine("AD.FTCtyCode AS rtCtyCode, ");  //*Arm 65-08-16 [CR-Oversea] เพิ่มรหัสประเทศ
                oSql.AppendLine("AD.FDLastUpdOn AS rdLastUpdOn, AD.FTLastUpdBy AS rtLastUpdBy, AD.FDCreateOn AS rdCreateOn, AD.FTCreateBy AS rtCreateBy ");
                oSql.AppendLine("FROM TCNMAgency AD WITH(NOLOCK) ");
                oSql.AppendLine("INNER JOIN TCNMBranch BCH WITH(NOLOCK) ON AD.FTAgnCode = BCH.FTAgnCode ");
                oSql.AppendLine("WHERE BCH.FTBchCode = '"+ ptBchCode +"' ");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), AD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oAgnDwn.raAgency = oDB.C_DATaSqlQuery<cmlResInfoAgency>(oSql.ToString());
                
                if (oAgnDwn.raAgency.Count > 0)
                {
                    //Agency Languague
                    oSql.Clear();
                    oSql.AppendLine("SELECT ADL.FTAgnCode AS rtAgnCode, ADL.FNLngID AS rnLngID,");
                    oSql.AppendLine("ADL.FTAgnName AS rtAgnName, ADL.FTAgnRmk AS rtAgnRmk");
                    oSql.AppendLine("FROM TCNMAgency_L ADL WITH(NOLOCK) ");
                    oSql.AppendLine("INNER JOIN TCNMAgency AD WITH(NOLOCK) ON ADL.FTAgnCode = AD.FTAgnCode ");
                    oSql.AppendLine("INNER JOIN TCNMBranch BCH WITH(NOLOCK) ON AD.FTAgnCode = BCH.FTAgnCode ");
                    oSql.AppendLine("WHERE BCH.FTBchCode = '" + ptBchCode + "' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), AD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    oAgnDwn.raAgencyLng = oDB.C_DATaSqlQuery<cmlResInfoAgencyLng>(oSql.ToString());
                    //+++++++++++++
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                    
                #endregion

                aoResult.roItem = oAgnDwn;
                // เก็บ KeyApi ลง Cache
                //oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResAgencyDwn>();
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