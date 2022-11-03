using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Reason;
using API2PSMaster.Models.WebService.Request.Reason.Insert;
using API2PSMaster.Models.WebService.Request.Reason.Update;
using API2PSMaster.Models.WebService.Request.Reason.Delete;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data.Entity.Core;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Reason Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Reason")]
    public class cReasonController : ControllerBase
    {
        /// <summary>
        ///     Download reason information.
        /// </summary>
        /// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResRsnDwn> GET_PDToDownloadReason(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResRsnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRsnDwn oRsnDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRsnDwn>();
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

                tKeyCache = "Reason" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRsnDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTRsnCode AS rtRsnCode, FTRsgCode AS rtRsgCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMRsn with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResRsnDwn();
                //oRsnDwn = new cmlResRsnDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oRsnDwn.raRsn = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRsn>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oRsnDwn.raRsn.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT DISTINCT TCNMRsn_L.FTRsnCode AS rtRsnCode, TCNMRsn_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMRsn_L.FTRsnName AS rtRsnName, TCNMRsn_L.FTRsnRmk AS rtRsnRmk");
                //            oSql.AppendLine("FROM TCNMRsn_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMRsn with(nolock) ON TCNMRsn_L.FTRsnCode = TCNMRsn.FTRsnCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMRsn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRsnDwn.raRsnLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRsnLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Group
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT DISTINCT TSysRsnGrp_L.FTRsgCode AS rtRsgCode, TSysRsnGrp_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TSysRsnGrp_L.FTRsgName AS rtRsgName, TSysRsnGrp_L.FTRsgRmk AS rtRsgRmk");
                //            oSql.AppendLine("FROM TSysRsnGrp_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMRsn with(nolock) ON TSysRsnGrp_L.FTRsgCode = TCNMRsn.FTRsgCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMRsn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRsnDwn.raRsnGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRsnGrpLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResRsnDwn();
                oRsnDwn = new cmlResRsnDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRsnCode AS rtRsnCode, FTRsgCode AS rtRsgCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMRsn with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oRsnDwn.raRsn = oDB.C_DATaSqlQuery<cmlResInfoRsn>(oSql.ToString());
                if (oRsnDwn.raRsn.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT TCNMRsn_L.FTRsnCode AS rtRsnCode, TCNMRsn_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMRsn_L.FTRsnName AS rtRsnName, TCNMRsn_L.FTRsnRmk AS rtRsnRmk");
                    oSql.AppendLine("FROM TCNMRsn_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMRsn with(nolock) ON TCNMRsn_L.FTRsnCode = TCNMRsn.FTRsnCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMRsn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oRsnDwn.raRsnLng = oDB.C_DATaSqlQuery<cmlResInfoRsnLng>(oSql.ToString());

                    //Group
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DISTINCT TSysRsnGrp_L.FTRsgCode AS rtRsgCode, TSysRsnGrp_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TSysRsnGrp_L.FTRsgName AS rtRsgName, TSysRsnGrp_L.FTRsgRmk AS rtRsgRmk");
                    oSql.AppendLine("FROM TSysRsnGrp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMRsn with(nolock) ON TSysRsnGrp_L.FTRsgCode = TCNMRsn.FTRsgCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMRsn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oRsnDwn.raRsnGrpLng = oDB.C_DATaSqlQuery<cmlResInfoRsnGrpLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oRsnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRsnDwn>();
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
        /// Insert Reason
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Insert")]
        [HttpPost]
        public cmlResItem<cmlResRsnDwn>POST_PDToInsReason(cmlReqReason poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Grp;
            cmlResItem<cmlResRsnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResRsnDwn>();
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

                    oSql = new StringBuilder();
                    oSql.Clear();
                    oSql.AppendLine(" INSERT ");
                    oSql.AppendLine(" INTO TCNMRsn WITH(ROWLOCK) ");
                    oSql.AppendLine(" (FTRsnCode, FTRsgCode, ");
                    oSql.AppendLine(" FDDateUpd, FTTimeUpd, ");
                    oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns) ");
                    oSql.AppendLine(" VALUES (");
                    oSql.AppendLine(" '" + poparam.ptRsnCode + "', '" + poparam.ptRsgCode + "',");
                    oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121) ,");
                    oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                    oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
                    oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121), ");
                    oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108), ");
                    oSql.AppendLine(" '" + poparam.ptWhoUpd + "') ");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" INSERT ");
                    oSql_L.AppendLine(" INTO TCNMRsn_L WITH(ROWLOCK) ");
                    oSql_L.AppendLine(" ( FTRsnCode, FNLngID, FTRsnName, FTRsnRmk) ");
                    oSql_L.AppendLine(" VALUES (");
                    oSql_L.AppendLine(" '" + poparam.ptRsnCode + "', '" + poparam.pnLngID + "',");
                    oSql_L.AppendLine(" '" + poparam.ptRsnName + "', '" + poparam.ptRsnRmk + "')");



                    oSql_Grp = new StringBuilder();
                    oSql_Grp.Clear();
                    oSql_Grp.AppendLine(" INSERT ");
                    oSql_Grp.AppendLine(" INTO");
                    oSql_Grp.AppendLine(" TSysRsnGrp_L WITH(ROWLOCK)");
                    oSql_Grp.AppendLine(" (FTRsgCode, FNLngID, FTRsgName,FTRsgRmk)");
                    oSql_Grp.AppendLine(" VALUES");
                    oSql_Grp.AppendLine(" ('" + poparam.poReasonGrp.ptRsgCode + "',");
                    oSql_Grp.AppendLine(" '" + poparam.poReasonGrp.pnLngID + "',");
                    oSql_Grp.AppendLine(" '" + poparam.poReasonGrp.ptRsgName + "',");
                    oSql_Grp.AppendLine(" '" + poparam.poReasonGrp.ptRsgRmk + "')");


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
                            oCmd.CommandText = oSql_L.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Grp.ToString();
                            oCmd.ExecuteNonQuery();

                            oTrans.Commit();
                        }
                        catch (System.Data.SqlClient.SqlException oSqlExn)
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
            catch (Exception oExn)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRsnDwn>();
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
        /// Update Reason
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public cmlResItem<cmlResRsnDwn> POST_PDToUptReason(cmlReqReasonUpt poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Grp;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResRsnDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblRmk;
            DataTable oDbTblRmk_L;
            DataTable oDbTblRmk_Grp;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResRsnDwn>();
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
                oSql.AppendLine("SELECT FTRsnCode, FTRsgCode ");
                oSql.AppendLine("FROM TCNMRsn WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTRsnCode = '" + poparam.ptRsnCode + "'");
                oDbTblRmk = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1)  FTRsnCode, FNLngID, FTRsnName, FTRsnRmk");
                oSql.AppendLine(" FROM TCNMRsn_L");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTRsnCode = '" + poparam.ptRsnCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblRmk_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblRmk != null && oDbTblRmk.Rows.Count > 0)
                {
                    if (oDbTblRmk_L != null && oDbTblRmk_L.Rows.Count > 0)
                    {
                        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                        {

                            oSql = new StringBuilder();
                            oSql.Clear();
                            oSql.AppendLine(" UPDATE ");
                            oSql.AppendLine(" TCNMRsn ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" SET ");
                            oSql.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptRsgCode, "FTRsgCode") + "");
                            oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                            oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                            oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                            oSql.AppendLine(" WHERE FTRsnCode = '" + poparam.ptRsnCode + "'");

                            string ptRsnName = poparam.ptRsnName;
                            if(poparam.ptRsnName == "")
                            {
                                ptRsnName = oDbTblRmk_L.Rows[0]["FTRsnName"].ToString();
                            }

                            oSql_L = new StringBuilder();
                            if (poparam.ptRsnName != "" || poparam.ptRsnRmk != "")
                            {
                                oSql_L.Clear();
                                oSql_L.AppendLine(" UPDATE ");
                                oSql_L.AppendLine(" TCNMRsn_L ");
                                oSql_L.AppendLine(" WITH(ROWLOCK)");
                                oSql_L.AppendLine(" SET ");
                                oSql_L.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.poReasonGrp.ptRsgRmk, "FTRsnRmk") + "");
                                oSql_L.AppendLine(" FTRsnName = '" + ptRsnName + "'");
                                oSql_L.AppendLine(" WHERE FTRsnCode = '" + poparam.ptRsnCode + "' ");
                                oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");
                            }



                            if(poparam.poReasonGrp.ptRsgName != "" || poparam.poReasonGrp.ptRsgRmk != "")
                            {
                                oSql_Grp = new StringBuilder();
                                oSql_Grp.Clear();
                                oSql_Grp.AppendLine(" UPDATE ");
                                oSql_Grp.AppendLine(" TSysRsnGrp_L");
                                oSql_Grp.AppendLine(" WITH(ROWLOCK)");
                                oSql_Grp.AppendLine(" SET");
                                oSql_Grp.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.poReasonGrp.ptRsgRmk, "FTRsgRmk") + "");
                                oSql_Grp.AppendLine(" FTRsgName  = '" + poparam.poReasonGrp.ptRsgName + "'");
                                oSql_Grp.AppendLine(" WHERE (FTRsgCode = '" + poparam.ptRsgCode +"' ) AND ");
                                oSql_Grp.AppendLine(" (FNLngID = '" + poparam.pnLngID +"')");

                            }
                           
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
                                    oCmd.CommandText = oSql_L.ToString();
                                    int nSuccess = oCmd.ExecuteNonQuery();
                                    if (nSuccess == 0)
                                    {
                                        aoResult = new cmlResItem<cmlResRsnDwn>();
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
                        aoResult = new cmlResItem<cmlResRsnDwn>();
                        //aoResult = new cmlResPdtItemDwn();
                        aoResult.rtCode = "";
                        aoResult.rtDesc = "";
                        return aoResult;
                    }
                }
                else
                {
                    aoResult = new cmlResItem<cmlResRsnDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }

            }
            catch(Exception oExn)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRsnDwn>();
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
        /// Delete Reason
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        public cmlResItem<cmlResRsnDwn> POST_PDToDelReason(cmlReqReasonDel poparam)
        {
            cSP oFunc;
            cMS oMsg;
            cCS oCS;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Grp;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResRsnDwn> aoResult;

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

                aoResult = new cmlResItem<cmlResRsnDwn>();
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
                    oSql.AppendLine(" FROM TCNMRsn  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTRsnCode = '" + poparam.ptRsnCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMRsn_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTRsnCode = '" + poparam.ptRsnCode + "') ");
                    oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");

                    oSql_Grp = new StringBuilder();
                    oSql_Grp.Clear();
                    oSql_Grp.AppendLine(" DELETE ");
                    oSql_Grp.AppendLine(" FROM TSysRsnGrp_L WITH(ROWLOCK) ");
                    oSql_Grp.AppendLine(" WHERE (FTRsgCode = '" + poparam.ptRsgCode + "') ");
                    oSql_Grp.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "') ");


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
                            int nSuccess;
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                aoResult = new cmlResItem<cmlResRsnDwn>();
                                //aoResult = new cmlResPdtItemDwn();
                                aoResult.rtCode = "";
                                aoResult.rtDesc = "";
                                return aoResult;
                            }

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Grp.ToString();
                            nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                aoResult = new cmlResItem<cmlResRsnDwn>();
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
            catch(Exception oExn)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRsnDwn>();
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

    }
}