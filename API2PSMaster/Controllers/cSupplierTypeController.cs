using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Class.Supplier;
using API2PSMaster.Models.WebService.Request.Supplier;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Supplier;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.ComponentModel;
using API2PSMaster.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Manage Supplier Type.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Supplier")]
    public class cSupplierTypeController : ControllerBase
    {
        /// <summary>
        ///     Insert Supplier Type.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier group information.</param>
        /// 
        /// <returns>
        ///     Supplier varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Type/Insert")]
        [HttpPost]
        public cmlResItem<cmlResSplTypeIns> POST_SPLoInsSplTypeItem([FromBody] cmlReqSplTypeIns poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSqlM,oSqlL;
            cmlResSplTypeIns oResErr;
            cmlResItem<cmlResSplTypeIns> oResult;
            int nRowEff, nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tErrCode, tErrDesc, tCode;
            bool bVarifyPara, bGenCode;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplTypeIns>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }

                if (string.IsNullOrEmpty(poPara.ptStyCode))
                {
                    bGenCode = oFunc.SP_GENbAutoFmtCode("TCNMSplType", "FTStyCode", "0", poPara.ptBchCode, aoSysConfig, out tCode);
                    if (bGenCode == false)
                    {
                        // Generate code false.
                        switch (tCode)
                        {
                            case "-1":
                                // Generate code false.
                                oResult.rtCode = oMsg.tMS_RespCode802;
                                oResult.rtDesc = oMsg.tMS_RespDesc802;
                                break;
                            case "-2":
                                // Format code not found.
                                oResult.rtCode = oMsg.tMS_RespCode803;
                                oResult.rtDesc = oMsg.tMS_RespDesc803;
                                break;
                            case "-3":
                                // Code maximum running.
                                oResult.rtCode = oMsg.tMS_RespCode804;
                                oResult.rtDesc = oMsg.tMS_RespDesc804;
                                break;
                        }

                        return oResult;
                    }
                    poPara.ptStyCode = tCode;
                }

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    // Insert Master.
                    oSqlM = new StringBuilder();
                    oSqlM.AppendLine("INSERT INTO TCNMSplType WITH(ROWLOCK)");
                    oSqlM.AppendLine("(");
                    oSqlM.AppendLine("	FTStyCode, ");
                    oSqlM.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd,");
                    oSqlM.AppendLine("	FDDateIns, FTTimeIns, FTWhoIns");
                    oSqlM.AppendLine(")");
                    oSqlM.AppendLine("VALUES");
                    oSqlM.AppendLine("(");
                    oSqlM.AppendLine("	'" + poPara.ptStyCode + "',");
                    oSqlM.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "',");
                    oSqlM.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "'");
                    oSqlM.AppendLine(")");

                    //Insert Languague
                    oSqlL = new StringBuilder();
                    oSqlL.AppendLine("INSERT INTO TCNMSplType_L WITH(ROWLOCK)");
                    oSqlL.AppendLine("(");
                    oSqlL.AppendLine("   FTStyCode, FNLngID, FTStyName, FTStyRmk");
                    oSqlL.AppendLine(")");
                    oSqlL.AppendLine("VALUES");
                    oSqlL.AppendLine("(");
                    oSqlL.AppendLine("   '" + poPara.ptStyCode + "'," + poPara.pnLngID + ",'" + poPara.ptStyName + "','" + poPara.ptStyRmk + "'");
                    oSqlL.AppendLine(")");
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
                            oCmd.CommandText = oSqlM.ToString();
                            oCmd.ExecuteNonQuery();
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSqlL.ToString();
                            oCmd.ExecuteNonQuery();
                            oTrans.Commit();
                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    oResult.rtCode = oMsg.tMS_RespCode801;
                                    oResult.rtDesc = oMsg.tMS_RespDesc801;
                                    return oResult;

                                default:
                                    //Error statement or sql error
                                    oResult.rtCode = oMsg.tMS_RespCode999;
                                    oResult.rtDesc = oSqlExn.Message;
                                    return oResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return oResult;
                            }
                        }
                    }
                }
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;

                //// Varify parameter value.
                //oSpl = new cSupplier();
                //bVarifyPara = oSpl.C_DATbVerifyInsSplTypeParamValue(poPara, out tErrCode, out tErrDesc, out oResErr);
                //if (bVarifyPara == true)
                //{
                //    // Insert.
                //    oSql = new StringBuilder();
                //    oSql.AppendLine("INSERT INTO TCNMSplType WITH(ROWLOCK)");
                //    oSql.AppendLine("(");
                //    oSql.AppendLine("	FTStyCode, ");
                //    oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd,");
                //    oSql.AppendLine("	FDDateIns, FTTimeIns, FTWhoIns");
                //    oSql.AppendLine(")");
                //    oSql.AppendLine("VALUES");
                //    oSql.AppendLine("(");
                //    oSql.AppendLine("	'" + poPara.ptStyCode + "',");
                //    oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '"+ poPara.ptWhoUpd +"',");
                //    oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '"+ poPara.ptWhoUpd +"'");
                //    oSql.AppendLine(")");

                //    try
                //    {
                //        oDB = new cDatabase();
                //        nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                //    }
                //    catch (SqlException oSqlExn)
                //    {
                //        switch (oSqlExn.Number)
                //        {
                //            case 2627:
                //                // Data is duplicate.
                //                oResult.roItem.rtStyCode = poPara.ptStyCode;
                //                oResult.rtCode = oMsg.tMS_RespCode801;
                //                oResult.rtDesc = oMsg.tMS_RespDesc801;
                //                return oResult;

                //            default:
                //                //Error statement or sql error
                //                oResult.rtCode = oMsg.tMS_RespCode999;
                //                oResult.rtDesc = oSqlExn.Message;
                //                return oResult;
                //        }
                //    }
                //    catch (EntityException oEtyExn)
                //    {
                //        switch (oEtyExn.HResult)
                //        {
                //            case -2146232060:
                //                // Cannot connect database..
                //                oResult.rtCode = oMsg.tMS_RespCode905;
                //                oResult.rtDesc = oMsg.tMS_RespDesc905;
                //                return oResult;
                //        }
                //    }

                //    //supplier group Languague
                //    if (oSpl.C_DATbVerifySplTypeLanguagueInsValue(poPara))
                //    {
                //        //Insert
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("INSERT INTO TCNMSplType_L WITH(ROWLOCK)");
                //        oSql.AppendLine("(");
                //        oSql.AppendLine("   FTStyCode, FNLngID, FTStyName, FTStyRmk");
                //        oSql.AppendLine(")");
                //        oSql.AppendLine("VALUES");
                //        oSql.AppendLine("(");
                //        oSql.AppendLine("   '" + poPara.ptStyCode + "'," + poPara.pnLngID + ",'" + poPara.ptStyName + "','" + poPara.ptStyRmk + "'");
                //        oSql.AppendLine(")");
                //    }
                //    else
                //    {
                //        //update
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("UPDATE TCNMSplType_L WITH(ROWLOCK)");
                //        oSql.AppendLine("SET FTStyName = '" + poPara.ptStyName + "'");
                //        oSql.AppendLine("   ,FTStyRmk = '" + poPara.ptStyRmk + "'");
                //        oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "' AND FNLngID = " + poPara.pnLngID);
                //    }

                //    try
                //    {
                //        oDB = new cDatabase();
                //        nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                //    }
                //    catch (SqlException oSqlExn)
                //    {
                //        //Error statement or sql error
                //        oResult.rtCode = oMsg.tMS_RespCode999;
                //        oResult.rtDesc = oSqlExn.Message;
                //        return oResult;
                //    }
                //    catch (EntityException oEtyExn)
                //    {
                //        switch (oEtyExn.HResult)
                //        {
                //            case -2146232060:
                //                // Cannot connect database..
                //                oResult.rtCode = oMsg.tMS_RespCode905;
                //                oResult.rtDesc = oMsg.tMS_RespDesc905;
                //                return oResult;
                //        }
                //    }
                //    oResult.rtCode = oMsg.tMS_RespCode001;
                //    oResult.rtDesc = oMsg.tMS_RespDesc001;
                //    return oResult;
                //}
                //else
                //{
                //    // Varify parameter value false.
                //    oResult.roItem = oResErr;
                //    oResult.rtCode = tErrCode;
                //    oResult.rtDesc = tErrDesc;
                //    return oResult;
                //}
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplTypeIns>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDB = null;
                oSqlM = null;
                oSqlL = null;
                oResErr = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Update Supplier type.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier type information.</param>
        /// 
        /// <returns>
        ///     Supplier varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Type/Update")]
        [HttpPost]
        public cmlResItem<cmlResSplTypeIns> POST_SPLoUpdSplTypeItem([FromBody] cmlReqSplTypeUpd poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResItem<cmlResSplTypeIns> oResult;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplTypeIns>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }
                // Varify parameter value.
                oSpl = new cSupplier();

                // Update.
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TCNMSplType WITH(ROWLOCK)");
                oSql.AppendLine("SET");
                oSql.AppendLine("	FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121) , ");
                oSql.AppendLine("	FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) ,");
                oSql.AppendLine("   FTWhoUpd = '"+ poPara.ptWhoUpd +"'");
                oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "'");

                try
                {
                    oDB = new cDatabase();
                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                    if (nRowEff == 0)
                    {
                        oResult.rtCode = oMsg.tMS_RespCode800;
                        oResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResult;
                    }
                }
                catch (SqlException oSqlExn)
                {
                    switch (oSqlExn.Number)
                    {
                        case 2627:
                            // Data is duplicate.
                            oResult.roItem.rtStyCode = poPara.ptStyCode;
                            oResult.rtCode = oMsg.tMS_RespCode801;
                            oResult.rtDesc = oMsg.tMS_RespDesc801;
                            return oResult;

                        default:
                            //Error statement or sql error
                            oResult.rtCode = oMsg.tMS_RespCode999;
                            oResult.rtDesc = oSqlExn.Message;
                            return oResult;
                    }
                }
                catch (EntityException oEtyExn)
                {
                    switch (oEtyExn.HResult)
                    {
                        case -2146232060:
                            // Cannot connect database..
                            oResult.rtCode = oMsg.tMS_RespCode905;
                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                            return oResult;
                    }
                }

                //supplier type Languague
                if (oSpl.C_DATbVerifySplTypeLanguagueInsValue(poPara.ptStyCode,poPara.pnLngID))
                {
                    //Insert
                    oSql = new StringBuilder();
                    oSql.AppendLine("INSERT INTO TCNMSplType_L WITH(ROWLOCK)");
                    oSql.AppendLine("(");
                    oSql.AppendLine("   FTStyCode, FNLngID, FTStyName, FTStyRmk");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES");
                    oSql.AppendLine("(");
                    oSql.AppendLine("   '" + poPara.ptStyCode + "'," + poPara.pnLngID + ",'" + poPara.ptStyName + "','" + poPara.ptStyRmk + "'");
                    oSql.AppendLine(")");
                }
                else
                {
                    //update
                    oSql = new StringBuilder();
                    oSql.AppendLine("UPDATE TCNMSplType_L WITH(ROWLOCK)");
                    oSql.AppendLine("SET FTStyName = '" + poPara.ptStyName + "'");
                    oSql.AppendLine("   ,FTStyRmk = '" + poPara.ptStyRmk + "'");
                    oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "' AND FNLngID = " + poPara.pnLngID);
                }

                try
                {
                    oDB = new cDatabase();
                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                }
                catch (SqlException oSqlExn)
                {
                    //Error statement or sql error
                    oResult.rtCode = oMsg.tMS_RespCode999;
                    oResult.rtDesc = oSqlExn.Message;
                    return oResult;
                }
                catch (EntityException oEtyExn)
                {
                    switch (oEtyExn.HResult)
                    {
                        case -2146232060:
                            // Cannot connect database..
                            oResult.rtCode = oMsg.tMS_RespCode905;
                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                            return oResult;
                    }
                }
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplTypeIns>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDB = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Delete Supplier Type.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier type information.</param>
        /// 
        /// <returns>
        ///     Supplier varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Type/Delete")]
        [HttpPost]
        public cmlResItem<cmlResSplTypeDel> POST_SPLoDelSplTypeItem([FromBody] cmlReqSplTypeDel poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResItem<cmlResSplTypeDel> oResult;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplTypeDel>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }
                // Varify parameter value.
                oSpl = new cSupplier();
                if (oSpl.C_DATbVerifyDelSplTypeParamValue(poPara) == false)
                {
                    oResult.rtCode = oMsg.tMS_RespCode813;
                    oResult.rtDesc = oMsg.tMS_RespDesc813;
                    return oResult;
                }
                // Delete.
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE TCNMSplType WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "'");

                try
                {
                    oDB = new cDatabase();
                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                    if (nRowEff == 0)
                    {
                        oResult.rtCode = oMsg.tMS_RespCode800;
                        oResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResult;
                    }
                }
                catch (SqlException oSqlExn)
                {
                    switch (oSqlExn.Number)
                    {
                        case 2627:
                            // Data is duplicate.
                            oResult.roItem.rtStyCode = poPara.ptStyCode;
                            oResult.rtCode = oMsg.tMS_RespCode801;
                            oResult.rtDesc = oMsg.tMS_RespDesc801;
                            return oResult;

                        default:
                            //Error statement or sql error
                            oResult.rtCode = oMsg.tMS_RespCode999;
                            oResult.rtDesc = oSqlExn.Message;
                            return oResult;
                    }
                }
                catch (EntityException oEtyExn)
                {
                    switch (oEtyExn.HResult)
                    {
                        case -2146232060:
                            // Cannot connect database..
                            oResult.rtCode = oMsg.tMS_RespCode905;
                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                            return oResult;
                    }
                }

                oSql = new StringBuilder();
                oSql.AppendLine("DELETE TCNMSplType_L WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "'");

                try
                {
                    oDB = new cDatabase();
                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                }
                catch (SqlException oSqlExn)
                {
                    //Error statement or sql error
                    oResult.rtCode = oMsg.tMS_RespCode999;
                    oResult.rtDesc = oSqlExn.Message;
                    return oResult;
                }
                catch (EntityException oEtyExn)
                {
                    switch (oEtyExn.HResult)
                    {
                        case -2146232060:
                            // Cannot connect database..
                            oResult.rtCode = oMsg.tMS_RespCode905;
                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                            return oResult;
                    }
                }
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplTypeDel>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDB = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Download supplier type.
        /// </summary>
        /// <param name="pdDate">Date update (format : yyyy-MM-dd).</param>
        /// <returns>
        ///     Supplier type information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Type/Download")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResItem<cmlResSplTypeDwn> GET_SPLoDownloadSplItem(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResItem<cmlResSplTypeDwn> aoResult;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSplTypeDwn oResSplTypeDwn;
            cCacheFunc oCacheFunc;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSplTypeDwn>();
                oFunc = new cSP();
                oCons = new cCS();
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
                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    aoResult.rtCode = oMsg.tMS_RespCode904;
                    aoResult.rtDesc = oMsg.tMS_RespDesc904;
                    return aoResult;
                }

                tKeyCache = "SupplierType" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSplTypeDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTStyCode AS rtStyCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMSplType with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //aoResult.roItem = new cmlResSplTypeDwn();
                //oResSplTypeDwn = new cmlResSplTypeDwn();
                //using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oAdaAcc.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oResSplTypeDwn.raSplType = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResInfoSplType>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }
                //        if (oResSplTypeDwn.raSplType.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMSplType_L.FTStyCode AS rtStyCode, TCNMSplType_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMSplType_L.FTStyName AS rtStyName, TCNMSplType_L.FTStyRmk AS rtStyRmk");
                //            oSql.AppendLine("FROM TCNMSplType_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSplType with(nolock) ON TCNMSplType_L.FTStyCode = TCNMSplType.FTStyCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSplType.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oResSplTypeDwn.raSplTypeLng = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResInfoSplTypeLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResSplTypeDwn();
                oResSplTypeDwn = new cmlResSplTypeDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTStyCode AS rtStyCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMSplType with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oResSplTypeDwn.raSplType = oDB.C_DATaSqlQuery<cmlResInfoSplType>(oSql.ToString());
                if (oResSplTypeDwn.raSplType.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMSplType_L.FTStyCode AS rtStyCode, TCNMSplType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMSplType_L.FTStyName AS rtStyName, TCNMSplType_L.FTStyRmk AS rtStyRmk");
                    oSql.AppendLine("FROM TCNMSplType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSplType with(nolock) ON TCNMSplType_L.FTStyCode = TCNMSplType.FTStyCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSplType.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oResSplTypeDwn.raSplTypeLng = oDB.C_DATaSqlQuery<cmlResInfoSplTypeLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oResSplTypeDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;

            }
            catch (Exception oExcep)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSplTypeDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                //oResult.rtDesc = new cMS().tMS_RespDesc900;
                aoResult.rtDesc = oExcep.Message;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Get supplier type information by code.
        /// </summary>
        /// <param name="ptCode">Supplier type code.</param>
        /// <returns>
        ///     Supplier information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Type/SearchByID")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplTypeDwn> GET_SPLoSearchCodeSplType(String ptCode)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResList<cmlResSplTypeDwn> oResult;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplTypeDwn>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }
                oSpl = new cSupplier();
                // Delete
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT STYM.FTStyCode AS rtStyCode, STYL.FNLngID AS rnLngID, STYL.FTStyName AS rtStyName, STYL.FTStyRmk AS rtStyRmk,");
                oSql.AppendLine("STYM.FDDateUpd AS rdDateUpd, STYM.FTTimeUpd AS rtTimeUpd, STYM.FTWhoUpd AS rtWhoUpd	");
                oSql.AppendLine("FROM TCNMSplType STYM with(nolock)");
                oSql.AppendLine("LEFT JOIN TCNMSplType_L STYL with(nolock) ON STYM.FTStyCode = STYL.FTStyCode ");
                oSql.AppendLine("WHERE STYM.FTStyCode = '" + ptCode + "'");
                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplTypeDwn>(oDR).ToList();
                            ((IDisposable)oDR).Dispose();
                        }

                    }


                    if (oResult.raItems.Count > 0)
                    {
                        int nCount = oResult.raItems.Count;
                        //oResult.rnAllPage = (int)Math.Ceiling((double)nCount / oCons.nCS_CNRowPerPagePic);
                        //oResult.rnCurrentPage = pnPage;
                        oResult.rtCode = oMsg.tMS_RespCode001;
                        oResult.rtDesc = oMsg.tMS_RespDesc001;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode800;
                        oResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResult;
                    }
                }

                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;

            }
            catch (Exception oExcep)
            {
                // Return error.
                oResult = new cmlResList<cmlResSplTypeDwn>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                //oResult.rtDesc = new cMS().tMS_RespDesc900;
                oResult.rtDesc = oExcep.Message;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Get supplier type information.
        /// </summary>
        /// <param name="poPara">Information for search.</param>
        /// <returns>
        ///     Supplier information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Type/SearchList")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplTypeDwn> GET_SPLoSearchListSplType([FromBody] cmlReqSplTypeSch poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResList<cmlResSplTypeDwn> oResult;
            string tFuncName, tModelErr, tKeyApi;
            DefaultValueAttribute oDefValueAttr;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplTypeDwn>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }
                oSpl = new cSupplier();
                // Delete
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT STYM.FTStyCode AS rtStyCode, STYL.FNLngID AS rnLngID, STYL.FTStyName AS rtStyName, STYL.FTStyRmk AS rtStyRmk,");
                oSql.AppendLine("STYM.FDDateUpd AS rdDateUpd, STYM.FTTimeUpd AS rtTimeUpd, STYM.FTWhoUpd AS rtWhoUpd	");
                oSql.AppendLine("FROM TCNMSplType STYM with(nolock)");
                oSql.AppendLine("LEFT JOIN TCNMSplType_L STYL with(nolock) ON STYM.FTStyCode = STYL.FTStyCode ");
                Boolean bFirst;
                bFirst = true;
                if (poPara.ptSearchType == "1")
                {
                    foreach (PropertyDescriptor  oProperty in TypeDescriptor.GetProperties (poPara))
                    {
                        if (oProperty.Name != "ptSearchType")
                        {
                            if (oProperty.GetValue(poPara) != null && oProperty.GetValue(poPara) != "")
                            {
                                oDefValueAttr = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                                switch (oProperty.Name.Substring(0, 2).ToLower())
                                {
                                    case ("pt"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "')");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "')");
                                            }
                                        break;

                                    case ("pd"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "')");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "')");
                                            }

                                        break;

                                    case ("pc"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ")");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ")");
                                            }

                                        break;

                                    case ("pn"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ")");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ")");
                                            }

                                        break;
                                }
                            }
                                
                        }
                    }

                }
                else
                {
                    foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poPara))
                    {
                        if (oProperty.Name != "ptSearchType")
                        {
                            if (oProperty.GetValue(poPara) != null && oProperty.GetValue(poPara) != "")
                            {
                                oDefValueAttr = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                                switch (oProperty.Name.Substring(0, 2).ToLower())
                                {
                                    case ("pt"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE STY" + oDefValueAttr.Value + ".FT" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND STY" + oDefValueAttr.Value + ".FT" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                            }

                                        break;

                                    case ("pd"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FD" + oProperty.Name.Substring(2) + " LIKE '%" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "%')");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FD" + oProperty.Name.Substring(2) + " LIKE '%" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "%')");
                                            }

                                        break;

                                    case ("pc"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FC" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%')");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FC" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%')");
                                            }

                                        break;

                                    case ("pn"):
                                            if (bFirst == true)
                                            {
                                                oSql.AppendLine("WHERE (STY" + oDefValueAttr.Value + ".FN" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%')");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oSql.AppendLine("AND (STY" + oDefValueAttr.Value + ".FN" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%')");
                                            }

                                        break;
                                }
                            } 
                        }
                    }
                }
                
                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplTypeDwn>(oDR).ToList();
                            ((IDisposable)oDR).Dispose();
                        }

                    }


                    if (oResult.raItems.Count > 0)
                    {
                        int nCount = oResult.raItems.Count;
                        //oResult.rnAllPage = (int)Math.Ceiling((double)nCount / oCons.nCS_CNRowPerPagePic);
                        //oResult.rnCurrentPage = pnPage;
                        oResult.rtCode = oMsg.tMS_RespCode001;
                        oResult.rtDesc = oMsg.tMS_RespDesc001;
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode800;
                        oResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResult;
                    }
                }

                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;

            }
            catch (Exception oExcep)
            {
                // Return error.
                oResult = new cmlResList<cmlResSplTypeDwn>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                //oResult.rtDesc = new cMS().tMS_RespDesc900;
                oResult.rtDesc = oExcep.Message;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oSql = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}