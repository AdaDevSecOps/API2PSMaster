using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Event;
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
    ///     Event Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Event")]
    public class cEventController : ControllerBase
    {
        /// <summary>
        ///     Download Event information.
        /// </summary>
        /// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResEventDwn> GET_PDToDownloadEvent(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResEventDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResEventDwn oEventDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResEventDwn>();
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

                tKeyCache = "Event" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResEventDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data Header
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTEvhCode AS rtEvhCode, FTEvhStaActive AS rtEvhStaActive,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMEventHD with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResEventDwn();
                oEventDwn = new cmlResEventDwn();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())   //*Em 62-06-18
                {
                    oEventDwn.raEvnHD = oConn.Query<cmlResInfoEventHD>(oSql.ToString(), nCmdTme).ToList(); 
                    if (oEventDwn.raEvnHD.Count > 0)
                    {
                        //Header Lang
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMEventHD_L.FTEvhCode AS rtEvhCode, TCNMEventHD_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMEventHD_L.FTEvhName AS rtEvhName, TCNMEventHD_L.FTEvhRmk AS rtEvhRmk");
                        oSql.AppendLine("FROM TCNMEventHD_L WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMEventHD with(nolock) ON TCNMEventHD_L.FTEvhCode = TCNMEventHD.FTEvhCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMEventHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oEventDwn.raEvnHDLng = oConn.Query<cmlResInfoEventHDLng>(oSql.ToString(), nCmdTme).ToList();

                        //Detail
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNMEventDT.FTEvhCode AS rtEvhCode, TCNMEventDT.FNEvdSeqNo AS rnEvdSeqNo, TCNMEventDT.FTEvdType AS rtEvdType,");
                        oSql.AppendLine("TCNMEventDT.FTEvdTStart AS rtEvdTStart, TCNMEventDT.FDEvdDStart AS rdEvdDStart, TCNMEventDT.FTEvdTFinish AS rtEvdTFinish,");
                        oSql.AppendLine("TCNMEventDT.FDEvdDFinish AS rdEvdDFinish, TCNMEventDT.FTEvdStaUse AS rtEvdStaUse,");
                        oSql.AppendLine("TCNMEventDT.FDLastUpdOn AS rdLastUpdOn, TCNMEventDT.FTLastUpdBy AS rtLastUpdBy");
                        oSql.AppendLine("FROM TCNMEventDT with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMEventHD with(nolock) ON TCNMEventDT.FTEvhCode = TCNMEventHD.FTEvhCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMEventHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oEventDwn.raEvnDT = oConn.Query<cmlResInfoEventDT>(oSql.ToString(), nCmdTme).ToList(); 

                        //Detail Lang
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNMEventDT_L.FTEvhCode AS rtEvhCode, TCNMEventDT_L.FNEvdSeqNo AS rnEvdSeqNo,");
                        oSql.AppendLine("TCNMEventDT_L.FNLngID AS rnLngID, TCNMEventDT_L.FTEvdName AS rtEvdName");
                        oSql.AppendLine("FROM TCNMEventDT_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMEventHD with(nolock) ON TCNMEventDT_L.FTEvhCode = TCNMEventHD.FTEvhCode");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMEventHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oEventDwn.raEvnDTLng = oConn.Query<cmlResInfoEventDTLng>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oEventDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResEventDwn>();
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