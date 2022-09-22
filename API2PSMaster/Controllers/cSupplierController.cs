using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Class.Supplier;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Supplier;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Supplier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Supplier.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Supplier")]
    public class cSupplierController : ControllerBase
    {
        /// <summary>
        ///     Insert Supplier.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier information.</param>
        /// 
        /// <returns>
        ///     Product varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     802 : generate code false.<br/>
        ///     803 : format code not found.<br/>
        ///     804 : code maximum running number.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Item/Insert")]
        [HttpPost]
        public cmlResItem<cmlResSplInsItem> POST_SPLoInsSplItem([FromBody] cmlReqSplInsItem poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSqlM,oSqlL,oSqlC,oSqlD;
            cmlResItem<cmlResSplInsItem> oResult;
            List<cmlTSysConfig> aoSysConfig;
            int nRowEff, nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tCode;
            bool bVarifyPara, bGenCode;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplInsItem>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState)==false)
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

                if (string.IsNullOrEmpty(poPara.ptSplCode))
                {
                    bGenCode = oFunc.SP_GENbAutoFmtCode("TCNMSpl", "FTSplCode", "0", poPara.ptBchCode, aoSysConfig, out tCode);
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
                    poPara.ptSplCode = tCode;
                }

                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    // Insert Master.
                    oSqlM = new StringBuilder();
                    oSqlM.AppendLine("INSERT INTO TCNMSpl WITH(ROWLOCK)");
                    oSqlM.AppendLine("(");
                    oSqlM.AppendLine("	FTSplCode, FTSplTel, FTSplFax, FTSplEmail, FTSplSex, ");
                    oSqlM.AppendLine("	FDSplDob, FTSgpCode, FTStyCode, FTSlvCode, FTVatCode, ");
                    oSqlM.AppendLine("	FTSplStaVATInOrEx, FTSplDiscBillRet, FTSplDiscBillWhs, FTSplDiscBillNet,");
                    oSqlM.AppendLine("	FTSplBusiness, FTSplStaBchOrHQ, FTSplBchCode, FTSplStaActive, FTUsrCode,");
                    oSqlM.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd,");
                    oSqlM.AppendLine("	FDDateIns, FTTimeIns, FTWhoIns");
                    oSqlM.AppendLine(")");
                    oSqlM.AppendLine("VALUES");
                    oSqlM.AppendLine("(");
                    oSqlM.AppendLine("	'" + poPara.ptSplCode + "', '" + poPara.ptSplTel + "', '" + poPara.ptSplFax + "', '" + poPara.ptSplEmail + "', '" + poPara.ptSplSex + "',");
                    oSqlM.AppendLine("	'" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplDob) + "', '" + poPara.ptSgpCode + "', '" + poPara.ptStyCode + "', '" + poPara.ptSlvCode + "', '" + poPara.ptVatCode + "',");
                    oSqlM.AppendLine("	'" + poPara.ptSplStaVATInOrEx + "', '" + poPara.ptSplDiscBillRet + "', '" + poPara.ptSplDiscBillWhs + "', '" + poPara.ptSplDiscBillNet + "',");
                    oSqlM.AppendLine("	'" + poPara.ptSplBusiness + "', '" + poPara.ptSplStaBchOrHQ + "', '" + poPara.ptSplBchCode + "', '" + poPara.ptSplStaActive + "', '" + poPara.ptUsrCode + "',");
                    oSqlM.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "',");
                    oSqlM.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "'");
                    oSqlM.AppendLine(")");

                    //Insert Languague
                    oSqlL = new StringBuilder();
                    oSqlL.AppendLine("INSERT INTO TCNMSpl_L WITH(ROWLOCK)");
                    oSqlL.AppendLine("(");
                    oSqlL.AppendLine("   FTSplCode, FNLngID, FTSplName, FTSplNameOth, FTSplPayRmk,");
                    oSqlL.AppendLine("   FTSplBillRmk, FTSplViaRmk, FTSplRmk");
                    oSqlL.AppendLine(")");
                    oSqlL.AppendLine("VALUES");
                    oSqlL.AppendLine("(");
                    oSqlL.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + ",'" + poPara.ptSplName + "','" + poPara.ptSplNameOth + "','" + poPara.ptSplPayRmk + "',");
                    oSqlL.AppendLine("   '" + poPara.ptSplBillRmk + "','" + poPara.ptSplViaRmk + "','" + poPara.ptSplRmk + "'");
                    oSqlL.AppendLine(")");

                    // Supplier Card.
                    oSqlC = new StringBuilder();
                    oSqlC.AppendLine("INSERT INTO TCNMSplCard WITH(ROWLOCK)");
                    oSqlC.AppendLine("(");
                    oSqlC.AppendLine("	FTSplCode, FDSplApply, FTSplRefExCrdNo, FDSplCrdIssue, FDSplCrdExpire, ");
                    oSqlC.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                    oSqlC.AppendLine(")");
                    oSqlC.AppendLine("VALUES");
                    oSqlC.AppendLine("(");
                    oSqlC.AppendLine("	'" + poPara.ptSplCode + "', '" + poPara.pdSplApply + "', '" + poPara.ptSplRefExCrdNo + "', '" + poPara.pdSplCrdIssue + "','" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplCrdExpire) + "',");
                    oSqlC.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "'");
                    oSqlC.AppendLine(")");

                    // Supplier credit.
                    oSqlD = new StringBuilder();
                    oSqlD.AppendLine("INSERT INTO TCNMSplCredit WITH(ROWLOCK)");
                    oSqlD.AppendLine("(");
                    oSqlD.AppendLine("	FTSplCode, FNSplCrTerm, FCSplCrLimit, FTSplDayCta, FDSplLastCta,");
                    oSqlD.AppendLine("	FDSplLastPay, FNSplLimitRow, FCSplLeadTime, FTViaCode, FTSplViaRmk,");
                    oSqlD.AppendLine("	FTSplTspPaid, ");
                    oSqlD.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                    oSqlD.AppendLine(")");
                    oSqlD.AppendLine("VALUES");
                    oSqlD.AppendLine("(");
                    oSqlD.AppendLine("	'" + poPara.ptSplCode + "'," + poPara.pnSplCrTerm + ", " + poPara.pcSplCrLimit + ", '" + poPara.ptSplDayCta + "', '" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplLastCta) + "',");
                    oSqlD.AppendLine("	'" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplLastPay) + "'," + poPara.pnSplLimitRow + "," + poPara.pcSplLeadTime + ",'" + poPara.ptViaCode + "','" + poPara.ptSplViaRmk + "',");
                    oSqlD.AppendLine("	'" + poPara.ptSplTspPaid + "',");
                    oSqlD.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "'");
                    oSqlD.AppendLine(")");
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
                            if (!string.IsNullOrEmpty(poPara.ptSplRefExCrdNo))
                            {
                                oCmd.CommandType = CommandType.Text;
                                oCmd.CommandText = oSqlC.ToString();
                                oCmd.ExecuteNonQuery();
                            }  
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSqlD.ToString();
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
                //bVarifyPara = oSpl.C_DATbVerifyInsItemParamValue(poPara, out tErrCode, out tErrDesc, out oResUnitErr);
                //if (bVarifyPara == true)
                //{
                //    // Insert.
                //    oSql = new StringBuilder();
                //    oSql.AppendLine("INSERT INTO TCNMSpl WITH(ROWLOCK)");
                //    oSql.AppendLine("(");
                //    oSql.AppendLine("	FTSplCode, FTSplTel, FTSplFax, FTSplEmail, FTSplSex, ");
                //    oSql.AppendLine("	FDSplDob, FTSgpCode, FTStyCode, FTSlvCode, FTVatCode, ");
                //    oSql.AppendLine("	FTSplStaVATInOrEx, FTSplDiscBillRet, FTSplDiscBillWhs, FTSplDiscBillNet,");
                //    oSql.AppendLine("	FTSplBusiness, FTSplStaBchOrHQ, FTSplBchCode, FTSplStaActive, FTUsrCode,");
                //    oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd,");
                //    oSql.AppendLine("	FDDateIns, FTTimeIns, FTWhoIns");
                //    oSql.AppendLine(")");
                //    oSql.AppendLine("VALUES");
                //    oSql.AppendLine("(");
                //    oSql.AppendLine("	'" + poPara.ptSplCode + "', '" + poPara.ptSplTel + "', '" + poPara.ptSplFax + "', '" + poPara.ptSplEmail + "', '" + poPara.ptSplSex + "',");
                //    oSql.AppendLine("	'" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplDob) + "', '" + poPara.ptSgpCode + "', '" + poPara.ptStyCode + "', '" + poPara.ptSlvCode + "', '" + poPara.ptVatCode + "',");
                //    oSql.AppendLine("	'" + poPara.ptSplStaVATInOrEx + "', '" + poPara.ptSplDiscBillRet + "', '" + poPara.ptSplDiscBillWhs + "', '" + poPara.ptSplDiscBillNet + "',");
                //    oSql.AppendLine("	'" + poPara.ptSplBusiness + "', '" + poPara.ptSplStaBchOrHQ + "', '" + poPara.ptSplBchCode + "', '" + poPara.ptSplStaActive + "', '" + poPara.ptUsrCode + "',");
                //    oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '"+ poPara.ptWhoIns + "',");
                //    oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '"+ poPara.ptWhoIns + "'");
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
                //                oResult.roItem.rtSplCode = poPara.ptSplCode;
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

                //    //supplier Languague
                //    if (oSpl.C_DATbVerifySplLanguagueInsValue(poPara))
                //    {
                //        //Insert
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("INSERT INTO TCNMSpl_L WITH(ROWLOCK)");
                //        oSql.AppendLine("(");
                //        oSql.AppendLine("   FTSplCode, FNLngID, FTSplName, FTSplNameOth, FTSplPayRmk,");
                //        oSql.AppendLine("   FTSplBillRmk, FTSplViaRmk, FTSplRmk");
                //        oSql.AppendLine(")");
                //        oSql.AppendLine("VALUES");
                //        oSql.AppendLine("(");
                //        oSql.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + ",'" + poPara .ptSplName + "','" + poPara.ptSplNameOth + "','" + poPara.ptSplPayRmk + "',");
                //        oSql.AppendLine("   '" + poPara.ptSplBillRmk + "','" + poPara.ptSplViaRmk + "','" + poPara.ptSplRmk + "'");
                //        oSql.AppendLine(")");
                //    }
                //    else
                //    {
                //        //update
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("UPDATE TCNMSpl_L WITH(ROWLOCK)");
                //        oSql.AppendLine("SET FTSplName = '" + poPara.ptSplName + "'");
                //        oSql.AppendLine("   ,FTSplNameOth = '"+ poPara.ptSplNameOth  + "'");
                //        oSql.AppendLine("   ,FTSplPayRmk = '" + poPara.ptSplPayRmk + "'");
                //        oSql.AppendLine("   ,FTSplBillRmk = '" + poPara.ptSplBillRmk + "'");
                //        oSql.AppendLine("   ,FTSplViaRmk = '"+ poPara.ptSplViaRmk +"'");
                //        oSql.AppendLine("   ,FTSplRmk = '"+ poPara.ptSplRmk  +"'");
                //        oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID);
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

                //    if (!string.IsNullOrEmpty (poPara.ptSplRefExCrdNo))
                //    {
                //        // Supplier Card.
                //        oSql = new StringBuilder();
                //        oSql.AppendLine("INSERT INTO TCNMSplCard WITH(ROWLOCK)");
                //        oSql.AppendLine("(");
                //        oSql.AppendLine("	FTSplCode, FDSplApply, FTSplRefExCrdNo, FDSplCrdIssue, FDSplCrdExpire, ");
                //        oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                //        oSql.AppendLine(")");
                //        oSql.AppendLine("VALUES");
                //        oSql.AppendLine("(");
                //        oSql.AppendLine("	'" + poPara.ptSplCode + "', '" + poPara.pdSplApply + "', '" + poPara.ptSplRefExCrdNo + "', '" + poPara.pdSplCrdIssue + "','" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplCrdExpire) + "',");
                //        oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoIns + "'");
                //        oSql.AppendLine(")");

                //        try
                //        {
                //            oDB = new cDatabase();
                //            nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                //        }
                //        catch (SqlException oSqlExn)
                //        {
                //            switch (oSqlExn.Number)
                //            {
                //                case 2627:
                //                    // Data is duplicate.
                //                    oResult.roItem.rtSplCode = poPara.ptSplCode;
                //                    oResult.rtCode = oMsg.tMS_RespCode801;
                //                    oResult.rtDesc = oMsg.tMS_RespDesc801;
                //                    return oResult;

                //                default:
                //                    //Error statement or sql error
                //                    oResult.rtCode = oMsg.tMS_RespCode999;
                //                    oResult.rtDesc = oSqlExn.Message;
                //                    return oResult;
                //            }
                //        }
                //        catch (EntityException oEtyExn)
                //        {
                //            switch (oEtyExn.HResult)
                //            {
                //                case -2146232060:
                //                    // Cannot connect database..
                //                    oResult.rtCode = oMsg.tMS_RespCode905;
                //                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                //                    return oResult;
                //            }
                //        }
                //    }

                //    // Supplier credit.
                //    oSql = new StringBuilder();
                //    oSql.AppendLine("INSERT INTO TCNMSplCredit WITH(ROWLOCK)");
                //    oSql.AppendLine("(");
                //    oSql.AppendLine("	FTSplCode, FNSplCrTerm, FCSplCrLimit, FTSplDayCta, FDSplLastCta,");
                //    oSql.AppendLine("	FDSplLastPay, FNSplLimitRow, FCSplLeadTime, FTViaCode, FTSplViaRmk,");
                //    oSql.AppendLine("	FTSplTspPaid, ");
                //    oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                //    oSql.AppendLine(")");
                //    oSql.AppendLine("VALUES");
                //    oSql.AppendLine("(");
                //    oSql.AppendLine("	'" + poPara.ptSplCode + "',"  + poPara.pnSplCrTerm + ", " + poPara.pcSplCrLimit + ", '" + poPara.ptSplDayCta + "', '" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplLastCta) + "',");
                //    oSql.AppendLine("	'" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplLastPay) + "'," + poPara.pnSplLimitRow + "," + poPara.pcSplLeadTime + ",'" + poPara.ptViaCode + "','" + poPara.ptSplViaRmk + "',");
                //    oSql.AppendLine("	'" + poPara.ptSplTspPaid + "',");
                //    oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '"+ poPara.ptWhoIns +"'");
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
                //                oResult.roItem.rtSplCode = poPara.ptSplCode;
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
                //    oResult.rtCode = oMsg.tMS_RespCode001;
                //    oResult.rtDesc = oMsg.tMS_RespDesc001;
                //    return oResult;
                //}
                //else
                //{
                //    // Varify parameter value false.
                //    oResult.roItem = oResUnitErr;
                //    oResult.rtCode = tErrCode;
                //    oResult.rtDesc = tErrDesc;
                //    return oResult;
                //}
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplInsItem>();
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
                oSqlC = null;
                oSqlD = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Update Supplier.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier information.</param>
        /// 
        /// <returns>
        ///     Product varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Item/Update")]
        [HttpPost]
        public cmlResItem<cmlResSplInsItem> POST_SPLoUpdateSplItem([FromBody] cmlReqSplItemUpd poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql,oSqlU;
            cSupplier oSpl;
            cmlResItem<cmlResSplInsItem> oResult;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            DefaultValueAttribute oDefVal;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplInsItem>();
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
                // Update
                oSqlU = new StringBuilder();
                foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poPara))
                {
                    oDefVal  = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                    if (oProperty.GetValue(poPara) != null)
                    {
                        if (oDefVal != null)
                        {
                            if (oDefVal.Value.ToString().IndexOf("M") >= 0)
                            {
                                switch (oProperty.Name.Substring(0, 2).ToUpper())
                                {
                                    case "PT":
                                        if (oProperty.GetValue(poPara).ToString() != "")
                                        {
                                            oSqlU.AppendLine("FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "',");
                                        }
                                        break;
                                    case "PD":
                                        oSqlU.AppendLine("FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "',");
                                        break;
                                    case "PN":
                                        oSqlU.AppendLine("FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                        break;
                                    case "PC":
                                        oSqlU.AppendLine("FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                        break;
                                }
                            }
                        }
                    }
                }
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TCNMSpl WITH(ROWLOCK)");
                oSql.AppendLine("SET");
                oSql.AppendLine(oSqlU.ToString());
                oSql.AppendLine("	FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121), ");
                oSql.AppendLine("	FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108), ");
                oSql.AppendLine("	FTWhoUpd = '"+ poPara.ptWhoUpd + "' ");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");

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

                //supplier Languague
                oSql = new StringBuilder();
                if (oSpl.C_DATbVerifySplLanguagueInsValue(poPara.ptSplCode,poPara.pnLngID))
                {
                    //Insert
                    oSql.AppendLine("INSERT INTO TCNMSpl_L WITH(ROWLOCK)");
                    oSql.AppendLine("(");
                    oSql.AppendLine("   FTSplCode, FNLngID, FTSplName, FTSplNameOth, FTSplPayRmk,");
                    oSql.AppendLine("   FTSplBillRmk, FTSplViaRmk, FTSplRmk");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES");
                    oSql.AppendLine("(");
                    oSql.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + ",'" + poPara.ptSplName + "','" + poPara.ptSplNameOth + "','" + poPara.ptSplPayRmk + "',");
                    oSql.AppendLine("   '" + poPara.ptSplBillRmk + "','" + poPara.ptSplViaRmk + "','" + poPara.ptSplRmk + "'");
                    oSql.AppendLine(")");
                }
                else
                {
                    //update
                    oSqlU = new StringBuilder();
                    foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poPara))
                    {
                        oDefVal = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                        if (oProperty.GetValue(poPara) != null)
                        {
                            if (oDefVal != null)
                            {
                                if (oDefVal.Value.ToString().IndexOf("L") >= 0)
                                {
                                    switch (oProperty.Name.Substring(0, 2).ToUpper())
                                    {
                                        case "PT":
                                            if (oProperty.GetValue(poPara).ToString() != "")
                                            {
                                                oSqlU.AppendLine("FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "',");
                                            }
                                            break;
                                        case "PD":
                                            oSqlU.AppendLine("FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "',");
                                            break;
                                        case "PN":
                                            oSqlU.AppendLine("FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                            break;
                                        case "PC":
                                            oSqlU.AppendLine("FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    
                    if (oSqlU.Length != 0)
                    {
                        oSqlU.Remove(oSqlU.Length-3,1);
                        oSql.AppendLine("UPDATE TCNMSpl_L WITH(ROWLOCK)");
                        oSql.AppendLine("SET");
                        oSql.AppendLine(oSqlU.ToString());
                        oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID);
                    }
                    
                }
                if (oSql.Length != 0)
                {
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
                }


                // Supplier Card.
                oSql = new StringBuilder();
                if (!string.IsNullOrEmpty(poPara.ptSplRefExCrdNo))
                {
                    if (oSpl.C_DATbVerifySplCardInsValue(poPara.ptSplCode))
                    {
                        
                        oSql.AppendLine("INSERT INTO TCNMSplCard WITH(ROWLOCK)");
                        oSql.AppendLine("(");
                        oSql.AppendLine("	FTSplCode, FDSplApply, FTSplRefExCrdNo, FDSplCrdIssue, FDSplCrdExpire, ");
                        oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                        oSql.AppendLine(")");
                        oSql.AppendLine("VALUES");
                        oSql.AppendLine("(");
                        oSql.AppendLine("	'" + poPara.ptSplCode + "', '" + poPara.pdSplApply + "', '" + poPara.ptSplRefExCrdNo + "', '" + poPara.pdSplCrdIssue + "','" + string.Format("{0:yyyy-MM-dd}", poPara.pdSplCrdExpire) + "',");
                        oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poPara.ptWhoUpd + "'");
                        oSql.AppendLine(")");
                    }
                    else
                    {
                        oSqlU = new StringBuilder();
                        foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poPara))
                        {
                            oDefVal = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                            if (oProperty.GetValue(poPara) != null)
                            {
                                if (oDefVal != null)
                                {
                                    if (oDefVal.Value.ToString().IndexOf("C") >= 0)
                                    {
                                        switch (oProperty.Name.Substring(0, 2).ToUpper())
                                        {
                                            case "PT":
                                                if (oProperty.GetValue(poPara).ToString() != "")
                                                {
                                                    oSqlU.AppendLine("FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "',");
                                                }
                                                break;
                                            case "PD":
                                                oSqlU.AppendLine("FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "',");
                                                break;
                                            case "PN":
                                                oSqlU.AppendLine("FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                                break;
                                            case "PC":
                                                oSqlU.AppendLine("FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        if (oSqlU.Length != 0)
                        {
                            oSql.AppendLine("UPDATE TCNMSplCard WITH(ROWLOCK)");
                            oSql.AppendLine("SET");
                            oSql.AppendLine(oSqlU.ToString());
                            oSql.AppendLine("	FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121), ");
                            oSql.AppendLine("	FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108), ");
                            oSql.AppendLine("	FTWhoUpd = '" + poPara.ptWhoUpd + "' ");
                            oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");
                        }
                    }
                    if (oSql.Length != 0)
                    {
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
                    }
                }


                // Supplier Credit.
                oSqlU = new StringBuilder();
                foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poPara))
                {
                    oDefVal = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                    if (oProperty.GetValue(poPara) != null)
                    {
                        if (oDefVal != null)
                        {
                            if (oDefVal.Value.ToString().IndexOf("D") >= 0)
                            {
                                switch (oProperty.Name.Substring(0, 2).ToUpper())
                                {
                                    case "PT":
                                        if (oProperty.GetValue(poPara).ToString() != "")
                                        {
                                            oSqlU.AppendLine("FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "',");
                                        }
                                        break;
                                    case "PD":
                                        oSqlU.AppendLine("FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "',");
                                        break;
                                    case "PN":
                                        oSqlU.AppendLine("FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                        break;
                                    case "PC":
                                        oSqlU.AppendLine("FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara) + ",");
                                        break;
                                }
                            }
                        }
                    }
                }
                oSql = new StringBuilder();
                if (oSqlU.Length != 0)
                {
                    oSql.AppendLine("UPDATE TCNMSplCredit WITH(ROWLOCK)");
                    oSql.AppendLine("SET");
                    oSql.AppendLine(oSqlU.ToString());
                    oSql.AppendLine("	FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121), ");
                    oSql.AppendLine("	FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108), ");
                    oSql.AppendLine("	FTWhoUpd = '" + poPara.ptWhoUpd + "' ");
                    oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");
                }

                if (oSql.Length != 0)
                {
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
                }
                    
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;

            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplInsItem>();
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
        ///     Delete Supplier.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier information.</param>
        /// 
        /// <returns>
        ///     Product varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Item/Delete")]
        [HttpPost]
        public cmlResItem<cmlResSplDelItem> POST_SPLoDeleteSplItem([FromBody] cmlReqSplDelItem poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResItem<cmlResSplDelItem> oResult;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplDelItem>();
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
                oSql.AppendLine("DELETE TCNMSpl WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");

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

                //supplier Languague
                oSql.AppendLine("DELETE TCNMSpl_L WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");

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

                //supplier card
                oSql.AppendLine("DELETE TCNMSplCard WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");

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

                //supplier Credit
                oSql.AppendLine("DELETE TCNMSplCredit WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' ");

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
                oResult = new cmlResItem<cmlResSplDelItem>();
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
        ///     Download supplier.
        /// </summary>
        /// <param name="pdDate">Date last update.</param>
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
        [Route("Item/Download")]
        [HttpGet]
        ////[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResItem<cmlResSplItemDwn> GET_PDToDownloadSplShipVia(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSplItemDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSplItemDwn oSplItemDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSplItemDwn>();
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

                tKeyCache = "SupplierShipVia" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSplItemDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FTSplTel AS rtSplTel, FTSplFax AS rtSplFax, FTSplEmail AS rtSplEmail,");
                //oSql.AppendLine("FTSplSex AS rtSplSex, FDSplDob AS rdSplDob, FTSgpCode AS rtSgpCode, FTStyCode AS rtStyCode,");
                //oSql.AppendLine("FTSlvCode AS rtSlvCode, FTVatCode AS rtVatCode, FTSplStaVATInOrEx AS rtSplStaVATInOrEx, FTSplDiscBillRet AS rtSplDiscBillRet,");
                //oSql.AppendLine("FTSplDiscBillWhs AS rtSplDiscBillWhs, FTSplDiscBillNet AS rtSplDiscBillNet, FTSplBusiness AS rtSplBusiness, FTSplStaBchOrHQ AS rtSplStaBchOrHQ,");
                //oSql.AppendLine("FTSplBchCode AS rtSplBchCode, FTSplStaActive AS rtSplStaActive, FTUsrCode AS rtUsrCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMSpl with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResSplItemDwn();
                //oSplItemDwn = new cmlResSplItemDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oSplItemDwn.raSpl = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSpl>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oSplItemDwn.raSpl.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMSpl_L.FTSplCode AS rtSplCode, TCNMSpl_L.FNLngID AS rnLngID, TCNMSpl_L.FTSplName AS rtSplName,");
                //            oSql.AppendLine("TCNMSpl_L.FTSplNameOth AS rtSplNameOth, TCNMSpl_L.FTSplPayRmk AS rtSplPayRmk, TCNMSpl_L.FTSplBillRmk AS rtSplBillRmk,");
                //            oSql.AppendLine("TCNMSpl_L.FTSplViaRmk AS rtSplViaRmk, TCNMSpl_L.FTSplRmk AS rtSplRmk");
                //            oSql.AppendLine("FROM TCNMSpl_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSpl with(nolock) ON TCNMSpl_L.FTSplCode = TCNMSpl.FTSplCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSpl.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSplItemDwn.raSplLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSplLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Card
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMSplCard.FTSplCode AS rtSplCode, TCNMSplCard.FDSplApply AS rdSplApply, TCNMSplCard.FTSplRefExCrdNo AS rtSplRefExCrdNo,");
                //            oSql.AppendLine("TCNMSplCard.FDSplCrdIssue AS rdSplCrdIssue, TCNMSplCard.FDSplCrdExpire AS rdSplCrdExpire,");
                //            oSql.AppendLine("TCNMSplCard.FDLastUpdOn AS rdLastUpdOn, TCNMSplCard.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMSplCard.FTLastUpdBy AS rtLastUpdBy, TCNMSplCard.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMSplCard with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSpl with(nolock) ON TCNMSplCard.FTSplCode = TCNMSpl.FTSplCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSpl.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSplItemDwn.raSplCard = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSplCard>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Credit
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMSplCredit.FTSplCode AS rtSplCode, TCNMSplCredit.FNSplCrTerm AS rnSplCrTerm, TCNMSplCredit.FCSplCrLimit AS rcSplCrLimit,");
                //            oSql.AppendLine("TCNMSplCredit.FTSplDayCta AS rtSplDayCta, TCNMSplCredit.FDSplLastCta AS rdSplLastCta, TCNMSplCredit.FDSplLastPay AS rdSplLastPay,");
                //            oSql.AppendLine("TCNMSplCredit.FNSplLimitRow AS rnSplLimitRow, TCNMSplCredit.FCSplLeadTime AS rcSplLeadTime, TCNMSplCredit.FTSplViaRmk AS rtSplViaRmk,");
                //            oSql.AppendLine("TCNMSplCredit.FTViaCode AS rtViaCode, TCNMSplCredit.FTSplTspPaid AS rtSplTspPaid,");
                //            oSql.AppendLine("TCNMSplCredit.FDLastUpdOn AS rdLastUpdOn, TCNMSplCredit.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMSplCredit.FTLastUpdBy AS rtLastUpdBy, TCNMSplCredit.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMSplCredit with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSpl with(nolock) ON TCNMSplCredit.FTSplCode = TCNMSpl.FTSplCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSpl.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSplItemDwn.raSplCredit = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSplCredit>(oDR).ToList();
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
                aoResult.roItem = new cmlResSplItemDwn();
                oSplItemDwn = new cmlResSplItemDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FTSplTel AS rtSplTel, FTSplFax AS rtSplFax, FTSplEmail AS rtSplEmail,");
                oSql.AppendLine("FTSplSex AS rtSplSex, FDSplDob AS rdSplDob, FTSgpCode AS rtSgpCode, FTStyCode AS rtStyCode,");
                oSql.AppendLine("FTSlvCode AS rtSlvCode, FTVatCode AS rtVatCode, FTSplStaVATInOrEx AS rtSplStaVATInOrEx, FTSplDiscBillRet AS rtSplDiscBillRet,");
                oSql.AppendLine("FTSplDiscBillWhs AS rtSplDiscBillWhs, FTSplDiscBillNet AS rtSplDiscBillNet, FTSplBusiness AS rtSplBusiness, FTSplStaBchOrHQ AS rtSplStaBchOrHQ,");
                oSql.AppendLine("FTSplBchCode AS rtSplBchCode, FTSplStaActive AS rtSplStaActive, FTUsrCode AS rtUsrCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMSpl with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSplItemDwn.raSpl = oDB.C_DATaSqlQuery<cmlResInfoSpl>(oSql.ToString());
                if (oSplItemDwn.raSpl.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMSpl_L.FTSplCode AS rtSplCode, TCNMSpl_L.FNLngID AS rnLngID, TCNMSpl_L.FTSplName AS rtSplName,");
                    oSql.AppendLine("TCNMSpl_L.FTSplNameOth AS rtSplNameOth, TCNMSpl_L.FTSplPayRmk AS rtSplPayRmk, TCNMSpl_L.FTSplBillRmk AS rtSplBillRmk,");
                    oSql.AppendLine("TCNMSpl_L.FTSplViaRmk AS rtSplViaRmk, TCNMSpl_L.FTSplRmk AS rtSplRmk");
                    oSql.AppendLine("FROM TCNMSpl_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSpl with(nolock) ON TCNMSpl_L.FTSplCode = TCNMSpl.FTSplCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSpl.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSplItemDwn.raSplLng = oDB.C_DATaSqlQuery<cmlResInfoSplLng>(oSql.ToString());

                    //Card
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMSplCard.FTSplCode AS rtSplCode, TCNMSplCard.FDSplApply AS rdSplApply, TCNMSplCard.FTSplRefExCrdNo AS rtSplRefExCrdNo,");
                    oSql.AppendLine("TCNMSplCard.FDSplCrdIssue AS rdSplCrdIssue, TCNMSplCard.FDSplCrdExpire AS rdSplCrdExpire,");
                    oSql.AppendLine("TCNMSplCard.FDLastUpdOn AS rdLastUpdOn, TCNMSplCard.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMSplCard.FTLastUpdBy AS rtLastUpdBy, TCNMSplCard.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMSplCard with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSpl with(nolock) ON TCNMSplCard.FTSplCode = TCNMSpl.FTSplCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSpl.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSplItemDwn.raSplCard = oDB.C_DATaSqlQuery<cmlResInfoSplCard>(oSql.ToString());

                    //Credit
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMSplCredit.FTSplCode AS rtSplCode, TCNMSplCredit.FNSplCrTerm AS rnSplCrTerm, TCNMSplCredit.FCSplCrLimit AS rcSplCrLimit,");
                    oSql.AppendLine("TCNMSplCredit.FTSplDayCta AS rtSplDayCta, TCNMSplCredit.FDSplLastCta AS rdSplLastCta, TCNMSplCredit.FDSplLastPay AS rdSplLastPay,");
                    oSql.AppendLine("TCNMSplCredit.FNSplLimitRow AS rnSplLimitRow, TCNMSplCredit.FCSplLeadTime AS rcSplLeadTime, TCNMSplCredit.FTSplViaRmk AS rtSplViaRmk,");
                    oSql.AppendLine("TCNMSplCredit.FTViaCode AS rtViaCode, TCNMSplCredit.FTSplTspPaid AS rtSplTspPaid,");
                    oSql.AppendLine("TCNMSplCredit.FDLastUpdOn AS rdLastUpdOn, TCNMSplCredit.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMSplCredit.FTLastUpdBy AS rtLastUpdBy, TCNMSplCredit.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMSplCredit with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSpl with(nolock) ON TCNMSplCredit.FTSplCode = TCNMSpl.FTSplCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMSpl.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSplItemDwn.raSplCredit = oDB.C_DATaSqlQuery<cmlResInfoSplCredit>(oSql.ToString());

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oSplItemDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSplItemDwn>();
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
        ///     Get supplier information by Code.
        /// </summary>
        /// <param name="ptCode">Supplier code.</param>
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
        [Route("Item/SearchByID")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplItemDwn> GET_SPLoSearchCodeSplItem(String ptCode)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResSplItemDwn> oResult;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplItemDwn>();
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
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT SPLM.FTSplCode AS rtSplCode, SPLM.FTSplTel AS rtSplTel, SPLM.FTSplFax AS rtSplFax, SPLM.FTSplEmail AS rtSplEmail, ");
                oSql.AppendLine("SPLM.FTSplSex AS rtSplSex,SPLM.FDSplDob AS rdSplDob, SPLM.FTSgpCode AS rtSgpCode, SPLM.FTStyCode AS rtStyCode, ");
                oSql.AppendLine("SPLM.FTSlvCode AS rtSlvCode, SPLM.FTVatCode AS rtVatCode, SCRT.FTViaCode AS rtViaCode, SCRT.FCSplLeadTime AS rcSplLeadTime,");
                oSql.AppendLine("SPLM.FTSplStaVATInOrEx AS rtSplStaVATInOrEx, SCRT.FNSplCrTerm AS rnSplCrTerm, SCRT.FCSplCrLimit AS rcSplCrLimit, ");
                oSql.AppendLine("SPLM.FTSplDiscBillRet AS rtSplDiscBillRet, SPLM.FTSplDiscBillWhs AS rtSplDiscBillWhs, SPLM.FTSplDiscBillNet AS rtSplDiscBillNet, SCRT.FTSplDayCta AS rtSplDayCta,");
                oSql.AppendLine("SCRT.FDSplLastCta AS rdSplLastCta, SCRT.FDSplLastPay AS rdSplLastPay, SCRT.FTSplTspPaid AS rtSplTspPaid, SCRT.FNSplLimitRow AS rnSplLimitRow, ");
                oSql.AppendLine("SPLM.FTSplBusiness AS rtSplBusiness, SPLM.FTSplStaBchOrHQ AS rtSplStaBchOrHQ, SPLM.FTSplBchCode AS rtSplBchCode, SCRD.FDSplApply AS rdSplApply, ");
                oSql.AppendLine("SCRD.FTSplRefExCrdNo AS rtSplRefExCrdNo, SCRD.FDSplCrdIssue AS rdSplCrdIssue, SCRD.FDSplCrdExpire AS rdSplCrdExpire, SPLM.FTSplStaActive AS rtSplStaActive, ");
                oSql.AppendLine("SPLM.FTUsrCode AS rtUsrCode, SPLL.FNLngID AS rnLngID, SPLL.FTSplName AS rtSplName, SPLL.FTSplNameOth AS rtSplNameOth, ");
                oSql.AppendLine("SPLL.FTSplPayRmk AS rtSplPayRmk, SPLL.FTSplBillRmk AS rtSplBillRmk, SCRT.FTSplViaRmk AS rtSplViaRmk, SPLL.FTSplRmk AS rtSplRmk");
                oSql.AppendLine("FROM TCNMSpl SPLM");
                oSql.AppendLine("LEFT JOIN TCNMSpl_L SPLL ON SPLM.FTSplCode = SPLL.FTSplCode ");
                oSql.AppendLine("LEFT JOIN TCNMSplCard SCRD ON SPLM.FTSplCode = SCRD.FTSplCode ");
                oSql.AppendLine("LEFT JOIN TCNMSplCredit SCRT ON SPLM.FTSplCode = SCRT.FTSplCode ");
                oSql.AppendLine("WHERE SPLM.FTSplCode = '" + ptCode + "'");
                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplItemDwn>(oDR).ToList();
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
                oResult = new cmlResList<cmlResSplItemDwn>();
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
        ///     Get supplier information by search.
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
        [Route("Item/SearchList")]
        [HttpPost]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplItemDwn> GET_SPLoSearchListSplItem([FromBody] cmlReqSplItemSch poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql,oWhe;
            cmlResList<cmlResSplItemDwn> oResult;
            string tFuncName, tModelErr, tKeyApi, tTbl;
            List<cmlTSysConfig> aoSysConfig;
            DefaultValueAttribute oDefVal;
            Boolean bFirst;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplItemDwn>();
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
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT SPLM.FTSplCode AS rtSplCode, SPLM.FTSplTel AS rtSplTel, SPLM.FTSplFax AS rtSplFax, SPLM.FTSplEmail AS rtSplEmail, ");
                oSql.AppendLine("SPLM.FTSplSex AS rtSplSex,SPLM.FDSplDob AS rdSplDob, SPLM.FTSgpCode AS rtSgpCode, SPLM.FTStyCode AS rtStyCode, ");
                oSql.AppendLine("SPLM.FTSlvCode AS rtSlvCode, SPLM.FTVatCode AS rtVatCode, SCRT.FTViaCode AS rtViaCode, SCRT.FCSplLeadTime AS rcSplLeadTime,");
                oSql.AppendLine("SPLM.FTSplStaVATInOrEx AS rtSplStaVATInOrEx, SCRT.FNSplCrTerm AS rnSplCrTerm, SCRT.FCSplCrLimit AS rcSplCrLimit, ");
                oSql.AppendLine("SPLM.FTSplDiscBillRet AS rtSplDiscBillRet, SPLM.FTSplDiscBillWhs AS rtSplDiscBillWhs, SPLM.FTSplDiscBillNet AS rtSplDiscBillNet, SCRT.FTSplDayCta AS rtSplDayCta,");
                oSql.AppendLine("SCRT.FDSplLastCta AS rdSplLastCta, SCRT.FDSplLastPay AS rdSplLastPay, SCRT.FTSplTspPaid AS rtSplTspPaid, SCRT.FNSplLimitRow AS rnSplLimitRow, ");
                oSql.AppendLine("SPLM.FTSplBusiness AS rtSplBusiness, SPLM.FTSplStaBchOrHQ AS rtSplStaBchOrHQ, SPLM.FTSplBchCode AS rtSplBchCode, SCRD.FDSplApply AS rdSplApply, ");
                oSql.AppendLine("SCRD.FTSplRefExCrdNo AS rtSplRefExCrdNo, SCRD.FDSplCrdIssue AS rdSplCrdIssue, SCRD.FDSplCrdExpire AS rdSplCrdExpire, SPLM.FTSplStaActive AS rtSplStaActive, ");
                oSql.AppendLine("SPLM.FTUsrCode AS rtUsrCode, SPLL.FNLngID AS rnLngID, SPLL.FTSplName AS rtSplName, SPLL.FTSplNameOth AS rtSplNameOth, ");
                oSql.AppendLine("SPLL.FTSplPayRmk AS rtSplPayRmk, SPLL.FTSplBillRmk AS rtSplBillRmk, SCRT.FTSplViaRmk AS rtSplViaRmk, SPLL.FTSplRmk AS rtSplRmk");
                oSql.AppendLine("FROM TCNMSpl SPLM");
                oSql.AppendLine("LEFT JOIN TCNMSpl_L SPLL ON SPLM.FTSplCode = SPLL.FTSplCode AND SPLL.FNLngID = " + poPara.pnLngID);
                oSql.AppendLine("LEFT JOIN TCNMSplCard SCRD ON SPLM.FTSplCode = SCRD.FTSplCode ");
                oSql.AppendLine("LEFT JOIN TCNMSplCredit SCRT ON SPLM.FTSplCode = SCRT.FTSplCode ");

                oWhe = new StringBuilder();
                bFirst = true;
                if (poPara.ptSearchType == "1")
                {
                    foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poPara))
                    {
                        oDefVal = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                        if (oProperty.GetValue(poPara) != null)
                        {
                            if (oDefVal != null)
                            {
                                tTbl = "SPL" + oDefVal.Value.ToString();
                                switch (oProperty.Name.Substring(0, 2).ToUpper())
                                {
                                    case "PT":
                                        if (oProperty.GetValue(poPara).ToString() != "")
                                        {
                                            if (bFirst)
                                            {
                                                oWhe.AppendLine("WHERE " + tTbl + ".FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "'");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oWhe.AppendLine("AND " + tTbl + ".FT" + oProperty.Name.Substring(2) + " = '" + oProperty.GetValue(poPara) + "'");
                                            }
                                        }
                                        break;
                                    case "PD":
                                        if (bFirst)
                                        {
                                            oWhe.AppendLine("WHERE " + tTbl + ".FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "'");
                                            bFirst = false;
                                        }
                                        else
                                        {
                                            oWhe.AppendLine("AND " + tTbl + ".FD" + oProperty.Name.Substring(2) + " = '" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "'");
                                        }
                                        break;
                                    case "PN":
                                        if (bFirst)
                                        {
                                            oWhe.AppendLine("WHERE " + tTbl + ".FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara));
                                            bFirst = false;
                                        }
                                        else
                                        {
                                            oWhe.AppendLine("AND " + tTbl + ".FN" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara));
                                        }
                                        break;
                                    case "PC":
                                        if (bFirst)
                                        {
                                            oWhe.AppendLine("WHERE " + tTbl + ".FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara));
                                            bFirst = false;
                                        }
                                        else
                                        {
                                            oWhe.AppendLine("AND " + tTbl + ".FC" + oProperty.Name.Substring(2) + " = " + oProperty.GetValue(poPara));
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
                        oDefVal = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                        if (oProperty.GetValue(poPara) != null)
                        {
                            if (oDefVal != null)
                            {
                                tTbl = "SPL" + oDefVal.Value.ToString();
                                switch (oProperty.Name.Substring(0, 2).ToUpper())
                                {
                                    case "PT":
                                        if (oProperty.GetValue(poPara).ToString() != "")
                                        {
                                            if (bFirst)
                                            {
                                                oWhe.AppendLine("WHERE " + tTbl + ".FT" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                                bFirst = false;
                                            }
                                            else
                                            {
                                                oWhe.AppendLine("AND " + tTbl + ".FT" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                            }
                                        }
                                        break;
                                    case "PD":
                                        if (bFirst)
                                        {
                                            oWhe.AppendLine("WHERE " + tTbl + ".FD" + oProperty.Name.Substring(2) + " LIKE '%" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "%'");
                                            bFirst = false;
                                        }
                                        else
                                        {
                                            oWhe.AppendLine("AND " + tTbl + ".FD" + oProperty.Name.Substring(2) + " LIKE '%" + string.Format("{0:yyyy-MM-dd}", oProperty.GetValue(poPara)) + "%'");
                                        }
                                        break;
                                    case "PN":
                                        if (bFirst)
                                        {
                                            oWhe.AppendLine("WHERE " + tTbl + ".FN" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                            bFirst = false;
                                        }
                                        else
                                        {
                                            oWhe.AppendLine("AND " + tTbl + ".FN" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                        }
                                        break;
                                    case "PC":
                                        if (bFirst)
                                        {
                                            oWhe.AppendLine("WHERE " + tTbl + ".FC" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                            bFirst = false;
                                        }
                                        else
                                        {
                                            oWhe.AppendLine("AND " + tTbl + ".FC" + oProperty.Name.Substring(2) + " LIKE '%" + oProperty.GetValue(poPara) + "%'");
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                
                if (oWhe.Length > 0)
                {
                    oSql.AppendLine(oWhe.ToString());
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
                            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplItemDwn>(oDR).ToList();
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
                oResult = new cmlResList<cmlResSplItemDwn>();
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