using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Role;
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
    ///     Role Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Role")]
    public class cRoleController : ControllerBase
    {
        /// <summary>
        ///     Download role information.
        /// </summary>
        /// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResRoleDwn> GET_ROLoDownloadRole(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResRoleDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRoleDwn oRoleDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRoleDwn>();
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

                tKeyCache = "Role" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRoleDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRolCode AS rtRolCode, FTUfrType AS rtUfrType, FTUfrGrpRef AS rtUfrGrpRef, FTUfrRef AS rtUfrRef,");
                //oSql.AppendLine("FTUfrStaAlw AS rtUfrStaAlw, FTUfrStaFavorite AS rtUfrStaFavorite,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNTUsrFuncRpt with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResRoleDwn();
                //oRoleDwn = new cmlResRoleDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oRoleDwn.raFuncRpt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoFuncRpt>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oRoleDwn.raFuncRpt.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT DISTINCT TCNTUsrMenu.FTRolCode AS rtRolCode, TCNTUsrMenu.FTGmnCode AS rtGmnCode, TCNTUsrMenu.FTMnuParent AS rtMnuParent,");
                //            oSql.AppendLine("TCNTUsrMenu.FTMnuCode AS rtMnuCode, TCNTUsrMenu.FTAutStaFull AS rtAutStaFull, TCNTUsrMenu.FTAutStaRead AS rtAutStaRead,");
                //            oSql.AppendLine("TCNTUsrMenu.FTAutStaAdd AS rtAutStaAdd, TCNTUsrMenu.FTAutStaEdit AS rtAutStaEdit, TCNTUsrMenu.FTAutStaDelete AS rtAutStaDelete,");
                //            oSql.AppendLine("TCNTUsrMenu.FTAutStaCancel AS rtAutStaCancel, TCNTUsrMenu.FTAutStaAppv AS rtAutStaAppv, TCNTUsrMenu.FTAutStaPrint AS rtAutStaPrint,");
                //            oSql.AppendLine("TCNTUsrMenu.FTAutStaPrintMore AS rtAutStaPrintMore, TCNTUsrMenu.FTAutStaFavorite AS rtAutStaFavorite,");
                //            oSql.AppendLine("TCNTUsrMenu.FDLastUpdOn AS rdLastUpdOn, TCNTUsrMenu.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNTUsrMenu.FTLastUpdBy AS rtLastUpdBy, TCNTUsrMenu.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNTUsrMenu with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTUsrFuncRpt with(nolock) ON TCNTUsrMenu.FTRolCode = TCNTUsrFuncRpt.FTRolCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNTUsrFuncRpt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRoleDwn.raUsrMenu = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUsrMenu>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
                //        }
                //        else
                //        {
                //            aoResult.rtCode = oMsg.tMS_RespCode800;
                //            aoResult.rtDesc = oMsg.tMS_RespDesc800;
                //            return aoResult;
                //        }
                //    }
                //}
                //*Net 64-10-19 ปรับไปใช้ Dapper
                oDB = new cDatabase();
                aoResult.roItem = new cmlResRoleDwn();
                oRoleDwn = new cmlResRoleDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRolCode AS rtRolCode, FTUfrType AS rtUfrType, FTUfrGrpRef AS rtUfrGrpRef, FTUfrRef AS rtUfrRef,");
                oSql.AppendLine("FTUfrStaAlw AS rtUfrStaAlw, FTUfrStaFavorite AS rtUfrStaFavorite,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNTUsrFuncRpt with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oRoleDwn.raFuncRpt = oDB.C_DATaSqlQuery<cmlResInfoFuncRpt>(oSql.ToString());
                if (oRoleDwn.raFuncRpt.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT TCNTUsrMenu.FTRolCode AS rtRolCode, TCNTUsrMenu.FTGmnCode AS rtGmnCode, TCNTUsrMenu.FTMnuParent AS rtMnuParent,");
                    oSql.AppendLine("TCNTUsrMenu.FTMnuCode AS rtMnuCode, TCNTUsrMenu.FTAutStaFull AS rtAutStaFull, TCNTUsrMenu.FTAutStaRead AS rtAutStaRead,");
                    oSql.AppendLine("TCNTUsrMenu.FTAutStaAdd AS rtAutStaAdd, TCNTUsrMenu.FTAutStaEdit AS rtAutStaEdit, TCNTUsrMenu.FTAutStaDelete AS rtAutStaDelete,");
                    oSql.AppendLine("TCNTUsrMenu.FTAutStaCancel AS rtAutStaCancel, TCNTUsrMenu.FTAutStaAppv AS rtAutStaAppv, TCNTUsrMenu.FTAutStaPrint AS rtAutStaPrint,");
                    oSql.AppendLine("TCNTUsrMenu.FTAutStaPrintMore AS rtAutStaPrintMore, TCNTUsrMenu.FTAutStaFavorite AS rtAutStaFavorite,");
                    oSql.AppendLine("TCNTUsrMenu.FDLastUpdOn AS rdLastUpdOn, TCNTUsrMenu.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNTUsrMenu.FTLastUpdBy AS rtLastUpdBy, TCNTUsrMenu.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNTUsrMenu with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTUsrFuncRpt with(nolock) ON TCNTUsrMenu.FTRolCode = TCNTUsrFuncRpt.FTRolCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNTUsrFuncRpt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oRoleDwn.raUsrMenu = oDB.C_DATaSqlQuery<cmlResInfoUsrMenu>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oRoleDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRoleDwn>();
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