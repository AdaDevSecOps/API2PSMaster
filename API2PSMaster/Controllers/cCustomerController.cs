using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Customer;
using API2PSMaster.Models.WebService.Request.User;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Customer;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    /// Manage customer information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Customer")]
    public class cCustomerController : ControllerBase
    {
        /*
        /// <summary>
        /// Insert customer information.
        /// </summary>
        /// <param name="poPara">Customer information.</param>
        /// 
        /// <returns>
        /// System process status.<br/>
        /// &#8195; 001 : success.<br/>
        /// &#8195; 701 : validate parameter model false.<br/>
        /// &#8195; 801 : data is duplicate.<br/>
        /// &#8195; 802 : generate code false.<br/>
        /// &#8195; 803 : format code not found.<br/>
        /// &#8195; 804 : code maximum running number.<br/>
        /// &#8195; 900 : function process false.<br/>
        /// &#8195; 904 : key not allowed to use method.<br/>
        /// &#8195; 905 : cannot connect database.<br/>
        /// 
        /// </returns>
        [HttpPost]
        [Route("Insert")]
        public cmlResStatus POST_CSToInsertInformation([FromBody] cmlReqCstIns poPara)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResStatus oResult;
            List<cmlTSysConfig> aoSysConfig;
            string tModelErr, tKeyApi, tCode, tSqlMaster, tSqlLanguage;
            int nConTme, nCmdTme, nRowEff;
            bool bGenCode;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oMsg = new cMS();
                oResult = new cmlResStatus();

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Check KeyApi.
                    if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
                    {
                        if (string.IsNullOrEmpty(poPara.poCstInf.ptCstCode))
                        {
                            bGenCode = oFunc.SP_GENbAutoFmtCode("TCNMCst", "FTCstCode", "0", poPara.ptBchCode, aoSysConfig, out tCode);
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
                            poPara.poCstInf.ptCstCode = tCode;

                        }
                        poPara.poCstLng.ptCstCode = poPara.poCstInf.ptCstCode;

                        tSqlMaster = oFunc.SP_GENtSqlCmdInsert<cmlReqCstInfIns>(poPara.poCstInf, "TCNMCst", poPara.ptWhoUpd, true);
                        tSqlLanguage = oFunc.SP_GENtSqlCmdInsert<cmlReqCstLngIns>(poPara.poCstLng, "TCNMCst_L", "", false, true);


                        oSql = new StringBuilder();
                        oSql.AppendLine("BEGIN TRANSACTION");
                        oSql.AppendLine("BEGIN TRY");
                        oSql.AppendLine("   " + tSqlMaster + tSqlLanguage);
                        oSql.AppendLine("   COMMIT TRANSACTION");
                        oSql.AppendLine("END TRY");
                        oSql.AppendLine("BEGIN CATCH");
                        oSql.AppendLine("   ROLLBACK TRANSACTION");
                        oSql.AppendLine("END CATCH");

                        try
                        {
                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "1");
                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                            oDatabase = new cDatabase(nConTme, nCmdTme);
                            nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString());

                            oResult.rtCode = oMsg.tMS_RespCode001;
                            oResult.rtDesc = oMsg.tMS_RespDesc001;
                            return oResult;
                        }
                        catch (SqlException oSqlExn)
                        {
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    oResult.rtCode = oMsg.tMS_RespCode801;
                                    oResult.rtDesc = oMsg.tMS_RespDesc801;
                                    break;
                            }

                            return oResult;
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146233087:
                                    // Data is dupplicate.
                                    oResult.rtCode = oMsg.tMS_RespCode801;
                                    oResult.rtDesc = oMsg.tMS_RespDesc801;
                                    break;

                                case -2146232060:
                                    // Cannot connect database..
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    break;
                            }

                            return oResult;
                        }
                        catch (Exception oExn)
                        {
                            throw oExn;
                        }
                    }
                    else
                    {
                        // Key not allowed to use method.
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtDesc = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                oResult = new cmlResStatus();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;

                return oResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                oResult = null;
                aoSysConfig = null;
                oResult = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        /// Update customer information.
        /// </summary>
        /// 
        /// <param name="poPara">Customer information.</param>
        /// 
        /// <returns>
        /// System process status.<br/>
        /// &#8195; 001 : success.<br/>
        /// &#8195; 701 : validate parameter model false.<br/>
        /// &#8195; 800 : data not found.<br/>
        /// &#8195; 900 : function process false.<br/>
        /// &#8195; 904 : key not allowed to use method.<br/>
        /// &#8195; 905 : cannot connect database.<br/>
        /// 
        /// </returns>
        [HttpPost]
        [Route("Update")]
        public cmlResStatus POST_CSToUpdateInformation([FromBody] cmlReqCstUpd poPara)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResStatus oResult;
            List<cmlTSysConfig> aoSysConfig;
            string tModelErr, tKeyApi, tSqlMaster, tSqlLanguage;
            int nConTme, nCmdTme, nRowEff;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oMsg = new cMS();
                oResult = new cmlResStatus();

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Check KeyApi.
                    if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
                    {
                        tSqlMaster = oFunc.SP_GENtSqlCmdUpdateMaster<cmlReqCstInfUpd>(
                            poPara.poCstInf, "TCNMCst", "FTCstCode", poPara.ptCstCode, poPara.ptWhoUpd);
                        tSqlLanguage = oFunc.SP_GENtSqlCmdUpdateLanguage<cmlReqCstLngUpd>(
                            poPara.poCstLng, "TCNMCst_L", "FTCstCode", poPara.ptCstCode, poPara.ptWhoUpd, poPara.pnLngID, false);

                        oSql = new StringBuilder();
                        oSql.AppendLine("BEGIN TRANSACTION");
                        oSql.AppendLine("BEGIN TRY");
                        oSql.AppendLine("   " + string.Format(tSqlMaster, tSqlLanguage));
                        oSql.AppendLine("   COMMIT TRANSACTION");
                        oSql.AppendLine("END TRY");
                        oSql.AppendLine("BEGIN CATCH");
                        oSql.AppendLine("   ROLLBACK TRANSACTION");
                        oSql.AppendLine("END CATCH");

                        try
                        {
                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "1");
                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                            oDatabase = new cDatabase(nConTme, nCmdTme);
                            nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString());

                            if (nRowEff > 0)
                            {
                                // Success.
                                oResult.rtCode = oMsg.tMS_RespCode001;
                                oResult.rtDesc = oMsg.tMS_RespDesc001;
                            }
                            else
                            {
                                // Data not found.
                                oResult.rtCode = oMsg.tMS_RespCode800;
                                oResult.rtDesc = oMsg.tMS_RespDesc800;
                            }

                            return oResult;
                        }
                        catch (SqlException oSqlExn)
                        {
                            throw oSqlExn;
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    break;
                            }

                            return oResult;
                        }
                        catch (Exception oExn)
                        {
                            throw oExn;
                        }
                    }
                    else
                    {
                        // Key not allowed to use method.
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtDesc = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                oResult = new cmlResStatus();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;

                return oResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                oResult = null;
                aoSysConfig = null;
                oResult = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
        */


        /// <summary>
        /// ลบรายการลูกค้าที่เพิ่มโดย QuickAdd แล้ว แต่ลงทะเบียนรูปไม่ผ่าน
        /// </summary>
        /// 
        /// <param name="poPara">Customer information.</param>
        /// 
        /// <returns>
        /// System process status.<br/>
        /// &#8195; 001 : success.<br/>
        /// &#8195; 701 : validate parameter model false.<br/>
        /// &#8195; 800 : data not found.<br/>
        /// &#8195; 900 : function process false.<br/>
        /// &#8195; 904 : key not allowed to use method.<br/>
        /// &#8195; 905 : cannot connect database.<br/>
        /// 
        /// </returns>
        [HttpPost]
        [Route("CstDelete")]
        //[Route("Delete")]
        public cmlResStatus POST_CSToDeleteInformation([FromBody] cmlReqCstDel poPara)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResStatus oResult;
            List<cmlTSysConfig> aoSysConfig;
            string tFuncName, tModelErr, tKeyApi;
            int nConTme, nCmdTme, nRowEff;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oMsg = new cMS();
                oResult = new cmlResStatus();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Check KeyApi.
                    if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
                    {
                        oSql = new StringBuilder();
                        oSql.AppendLine("BEGIN TRANSACTION");
                        oSql.AppendLine("BEGIN TRY");
                        oSql.AppendLine("   DELETE");
                        oSql.AppendLine("   FROM TCNMCst WITH(ROWLOCK)");
                        oSql.AppendLine("   WHERE FTCstCode = '" + poPara.ptCstCode + "'");
                        oSql.AppendLine("   ;");
                        oSql.AppendLine("   DELETE");
                        oSql.AppendLine("   FROM TCNMCst_L WITH(ROWLOCK)");
                        oSql.AppendLine("   WHERE FTCstCode = '" + poPara.ptCstCode + "'");
                        oSql.AppendLine("   COMMIT TRANSACTION");
                        oSql.AppendLine("END TRY");
                        oSql.AppendLine("BEGIN CATCH");
                        oSql.AppendLine("   ROLLBACK TRANSACTION");
                        oSql.AppendLine("END CATCH");

                        try
                        {
                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "1");
                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                            oDatabase = new cDatabase(nConTme, nCmdTme);
                            nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString());

                            if (nRowEff > 0)
                            {
                                // Success.
                                oResult.rtCode = oMsg.tMS_RespCode001;
                                oResult.rtDesc = oMsg.tMS_RespDesc001;
                            }
                            else
                            {
                                // Data not found.
                                oResult.rtCode = oMsg.tMS_RespCode800;
                                oResult.rtDesc = oMsg.tMS_RespDesc800;
                            }

                            return oResult;
                        }
                        catch (SqlException oSqlExn)
                        {
                            throw oSqlExn;
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    break;
                            }

                            return oResult;
                        }
                        catch (Exception oExn)
                        {
                            throw oExn;
                        }
                    }
                    else
                    {
                        // Key not allowed to use method.
                        oResult.rtCode = oMsg.tMS_RespCode904;
                        oResult.rtDesc = oMsg.tMS_RespDesc904;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                oResult = new cmlResStatus();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;

                return oResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                oResult = null;
                aoSysConfig = null;
                oResult = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        /// <summary>
        ///     Download product color information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResCstDwn> GET_PDToDownloadCst(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResCstDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCstDwn oCstDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCstDwn>();
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

                tKeyCache = "ProductColor" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCstDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCstCode AS rtCstCode, FTCstCardID AS rtCstCardID, FTCstTaxNo AS rtCstTaxNo, FTCstTel AS rtCstTel,");
                oSql.AppendLine("FTCstFax AS rtCstFax, FTCstEmail AS rtCstEmail, FTCstSex AS rtCstSex, FDCstDob AS rdCstDob,");
                oSql.AppendLine("FTCgpCode AS rtCgpCode, FTCtyCode AS rtCtyCode, FTClvCode AS rtClvCode, FTPplCodeRet AS rtPplCodeRet,");
                oSql.AppendLine("FTPplCodeWhs AS rtPplCodeWhs, FTPplCodenNet AS rtPplCodenNet, FTPmgCode AS rtPmgCode, FTOcpCode AS rtOcpCode,");
                oSql.AppendLine("FTSpnCode AS rtSpnCode, FTUsrCode AS rtUsrCode, FTCstDiscWhs AS rtCstDiscWhs, FTCstDiscRet AS rtCstDiscRet,");
                oSql.AppendLine("FTCstBusiness AS rtCstBusiness, FTCstBchHQ AS rtCstBchHQ, FTCstBchCode AS rtCstBchCode, FTCstStaActive AS rtCstStaActive,");
                oSql.AppendLine("FTCstStaAlwPosCalSo AS rtCstStaAlwPosCalSo,"); //*Arm 63-02-20
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMCst with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResCstDwn();
                oCstDwn = new cmlResCstDwn();

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
                //            oCstDwn.raCst = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCst>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                oCstDwn.raCst = oDB.C_DATaSqlQuery<cmlResInfoCst>(oSql.ToString()); //*Arm 64-10-19
                if (oCstDwn.raCst.Count > 0)
                {
                    //Customer Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCst_L.FTCstCode AS rtCstCode, TCNMCst_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMCst_L.FTCstName AS rtCstName, TCNMCst_L.FTCstNameOth AS rtCstNameOth,");
                    oSql.AppendLine("TCNMCst_L.FTCstRmk AS rtCstRmk");
                    oSql.AppendLine("FROM TCNMCst_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCst_L.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstLng = oDB.C_DATaSqlQuery<cmlResInfoCstLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Card
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstCard.FTCstCode AS rtCstCode, TCNMCstCard.FDCstApply AS rdCstApply, TCNMCstCard.FTCstCrdNo AS rtCstCrdNo,");
                    oSql.AppendLine("TCNMCstCard.FTBchCode AS rtBchCode, TCNMCstCard.FDCstCrdIssue AS rdCstCrdIssue, TCNMCstCard.FDCstCrdExpire AS rdCstCrdExpire,");
                    oSql.AppendLine("TCNMCstCard.FTCstStaAge AS rtCstStaAge");
                    oSql.AppendLine("FROM TCNMCstCard with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstCard.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstCard = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstCard>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstCard = oDB.C_DATaSqlQuery<cmlResInfoCstCard>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Credit
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstCredit.FTCstCode AS rtCstCode, TCNMCstCredit.FNCstCrTerm AS rnCstCrTerm, TCNMCstCredit.FCCstCrLimit AS rcCstCrLimit,");
                    oSql.AppendLine("TCNMCstCredit.FTCstStaAlwOrdSun AS rtCstStaAlwOrdSun, TCNMCstCredit.FTCstStaAlwOrdMon AS rtCstStaAlwOrdMon, TCNMCstCredit.FTCstStaAlwOrdTue AS rtCstStaAlwOrdTue,");
                    oSql.AppendLine("TCNMCstCredit.FTCstStaAlwOrdWed AS rtCstStaAlwOrdWed, TCNMCstCredit.FTCstStaAlwOrdThu AS rtCstStaAlwOrdThu, TCNMCstCredit.FTCstStaAlwOrdFri AS rtCstStaAlwOrdFri,");
                    oSql.AppendLine("TCNMCstCredit.FTCstStaAlwOrdSat AS rtCstStaAlwOrdSat, TCNMCstCredit.FDCstLastCta AS rdCstLastCta, TCNMCstCredit.FDCstLastPay AS rdCstLastPay,");
                    oSql.AppendLine("TCNMCstCredit.FTCstPayRmk AS rtCstPayRmk, TCNMCstCredit.FTCstBillRmk AS rtCstBillRmk, TCNMCstCredit.FNCstViaTime AS rnCstViaTime,");
                    oSql.AppendLine("TCNMCstCredit.FTCstViaRmk AS rtCstViaRmk, TCNMCstCredit.FTViaCode AS rtViaCode, TCNMCstCredit.FTCstTspPaid AS rtCstTspPaid,");
                    oSql.AppendLine("TCNMCstCredit.FTCstStaApv AS rtCstStaApv, TCNMCstCredit.FTCstStaAlwPosCalSo AS rtCstStaAlwPosCalSo"); //*Arm 63-02-20 - เพิ่ม FTCstStaAlwPosCalSo
                    oSql.AppendLine("FROM TCNMCstCredit with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstCredit.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstCredit = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstCredit>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstCredit = oDB.C_DATaSqlQuery<cmlResInfoCstCredit>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Address
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstAddress_L.FTCstCode AS rtCstCode, TCNMCstAddress_L.FNLngID AS rnLngID, TCNMCstAddress_L.FTAddGrpType AS rtAddGrpType,");
                    oSql.AppendLine("TCNMCstAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMCstAddress_L.FTAddRefNo AS rtAddRefNo, TCNMCstAddress_L.FTAddName AS rtAddName,");
                    oSql.AppendLine("TCNMCstAddress_L.FTAddRmk AS rtAddRmk, TCNMCstAddress_L.FTAddCountry AS rtAddCountry, TCNMCstAddress_L.FTAreCode AS rtAreCode,");
                    oSql.AppendLine("TCNMCstAddress_L.FTZneCode AS rtZneCode, TCNMCstAddress_L.FTAddVersion AS rtAddVersion, TCNMCstAddress_L.FTAddV1No AS rtAddV1No,");
                    oSql.AppendLine("TCNMCstAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMCstAddress_L.FTAddV1Village AS rtAddV1Village, TCNMCstAddress_L.FTAddV1Road AS rtAddV1Road,");
                    oSql.AppendLine("TCNMCstAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMCstAddress_L.FTAddV1DstCode AS rtAddV1DstCode, TCNMCstAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode,");
                    oSql.AppendLine("TCNMCstAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMCstAddress_L.FTAddV2Desc1 AS rtAddV2Desc1, TCNMCstAddress_L.FTAddV2Desc2 AS rtAddV2Desc2,");
                    oSql.AppendLine("TCNMCstAddress_L.FTAddWebsite AS rtAddWebsite, TCNMCstAddress_L.FTAddLongitude AS rtAddLongitude, TCNMCstAddress_L.FTAddLatitude AS rtAddLatitude,");
                    oSql.AppendLine("TCNMCstAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMCstAddress_L.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMCstAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMCstAddress_L.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMCstAddress_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstAddress_L.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstAddrLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstAddrLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstAddrLng = oDB.C_DATaSqlQuery<cmlResInfoCstAddrLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Contact
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstContact_L.FTCstCode AS rtCstCode, TCNMCstContact_L.FNLngID AS rnLngID, TCNMCstContact_L.FNCtrSeq AS rnCtrSeq,");
                    oSql.AppendLine("TCNMCstContact_L.FTCtrName AS rtCtrName, TCNMCstContact_L.FTCtrFax AS rtCtrFax, TCNMCstContact_L.FTCtrTel AS rtCtrTel,");
                    oSql.AppendLine("TCNMCstContact_L.FTCtrEmail AS rtCtrEmail, TCNMCstContact_L.FTCtrRmk AS rtCtrRmk,");
                    oSql.AppendLine("TCNMCstContact_L.FDLastUpdOn AS rdLastUpdOn, TCNMCstContact_L.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMCstContact_L.FTLastUpdBy AS rtLastUpdBy, TCNMCstContact_L.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMCstContact_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstContact_L.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstContactLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstContactLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstContactLng = oDB.C_DATaSqlQuery<cmlResInfoCstContactLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Group
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstGrp.FTCgpCode AS rtCgpCode,");
                    oSql.AppendLine("TCNMCstGrp.FDLastUpdOn AS rdLastUpdOn, TCNMCstGrp.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMCstGrp.FTLastUpdBy AS rtLastUpdBy, TCNMCstGrp.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMCstGrp with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstGrp.FTCgpCode = TCNMCst.FTCgpCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstGrp>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstGrp = oDB.C_DATaSqlQuery<cmlResInfoCstGrp>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Group Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstGrp_L.FTCgpCode AS rtCgpCode, TCNMCstGrp_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMCstGrp_L.FTCgpName AS rtCgpName, TCNMCstGrp_L.FTCgpRmk AS rtCgpRmk");
                    oSql.AppendLine("FROM TCNMCstGrp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstGrp_L.FTCgpCode = TCNMCst.FTCgpCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstGrpLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstGrpLng = oDB.C_DATaSqlQuery<cmlResInfoCstGrpLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Level
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstLev.FTClvCode AS rtClvCode,");
                    oSql.AppendLine("TCNMCstLev.FDLastUpdOn AS rdLastUpdOn, TCNMCstLev.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMCstLev.FTLastUpdBy AS rtLastUpdBy, TCNMCstLev.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMCstLev with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstLev.FTClvCode = TCNMCst.FTClvCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstLev = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstLev>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstLev = oDB.C_DATaSqlQuery<cmlResInfoCstLev>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Level Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstLev_L.FTClvCode AS rtClvCode, TCNMCstLev_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMCstLev_L.FTClvName AS rtClvName, TCNMCstLev_L.FTClvRmk AS rtClvRmk");
                    oSql.AppendLine("FROM TCNMCstLev_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstLev_L.FTClvCode = TCNMCst.FTClvCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstLevLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstLevLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstLevLng = oDB.C_DATaSqlQuery<cmlResInfoCstLevLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer occupation
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstOcp.FTOcpCode AS rtOcpCode,");
                    oSql.AppendLine("TCNMCstOcp.FDLastUpdOn AS rdLastUpdOn, TCNMCstOcp.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMCstOcp.FTLastUpdBy AS rtLastUpdBy, TCNMCstOcp.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMCstOcp with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstOcp.FTOcpCode = TCNMCst.FTOcpCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstOcp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstOcp>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstOcp = oDB.C_DATaSqlQuery<cmlResInfoCstOcp>(oSql.ToString()); //*Arm 64-10-19

                    //Customer occupation Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstOcp_L.FTOcpCode AS rtOcpCode, TCNMCstOcp_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMCstOcp_L.FTOcpName AS rtOcpName, TCNMCstOcp_L.FTOcpRmk AS rtOcpRmk");
                    oSql.AppendLine("FROM TCNMCstOcp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstOcp_L.FTOcpCode = TCNMCst.FTOcpCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstOcpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstOcpLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstOcpLng = oDB.C_DATaSqlQuery<cmlResInfoCstOcpLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Type
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstType.FTCtyCode AS rtCtyCode,");
                    oSql.AppendLine("TCNMCstType.FDLastUpdOn AS rdLastUpdOn, TCNMCstType.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMCstType.FTLastUpdBy AS rtLastUpdBy, TCNMCstType.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMCstType with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstType.FTCtyCode = TCNMCst.FTCtyCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstType>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstType = oDB.C_DATaSqlQuery<cmlResInfoCstType>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Type Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstType_L.FTCtyCode AS rtCtyCode, TCNMCstType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMCstType_L.FTCtyName AS rtCtyName, TCNMCstType_L.FTCtyRmk AS rtCtyRmk");
                    oSql.AppendLine("FROM TCNMCstType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstType_L.FTCtyCode = TCNMCst.FTCtyCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstTypeLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstTypeLng = oDB.C_DATaSqlQuery<cmlResInfoCstTypeLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer RFID Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMCstRFID_L.FTCstCode AS rtCstCode, TCNMCstRFID_L.FTCstID AS rtCstID,");
                    oSql.AppendLine("TCNMCstRFID_L.FNLngID AS rnLngID, TCNMCstRFID_L.FTCrfName AS rtCrfName");
                    oSql.AppendLine("FROM TCNMCstRFID_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNMCstRFID_L.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstRFIDLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstRFIDLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstRFIDLng = oDB.C_DATaSqlQuery<cmlResInfoCstRFIDLng>(oSql.ToString()); //*Arm 64-10-19

                    //Customer Point
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNTCstPoint.FTBchCode AS rtBchCode, TCNTCstPoint.FTCstCode AS rtCstCode, TCNTCstPoint.FTPntRefDoc AS rtPntRefDoc,");
                    oSql.AppendLine("TCNTCstPoint.FTSplCode AS rtSplCode, TCNTCstPoint.FDPntDate AS rdPntDate, TCNTCstPoint.FCPntOptBuyAmt AS rcPntOptBuyAmt,");
                    oSql.AppendLine("TCNTCstPoint.FCPntOptGetAmt AS rcPntOptGetAmt, TCNTCstPoint.FNPntB4Bill AS rnPntB4Bill, TCNTCstPoint.FCPntBillAmt AS rcPntBillAmt,");
                    oSql.AppendLine("TCNTCstPoint.FNPntBillQty AS rnPntBillQty, TCNTCstPoint.FTPntExpired AS rtPntExpired, TCNTCstPoint.FTPntStaPrcDoc AS rtPntStaPrcDoc,");
                    oSql.AppendLine("TCNTCstPoint.FTPntCardType AS rtPntCardType, TCNTCstPoint.FTCptJDate AS rtCptJDate, TCNTCstPoint.FTCptTime AS rtCptTime,");
                    oSql.AppendLine("TCNTCstPoint.FDPntSplStart AS rdPntSplStart, TCNTCstPoint.FDPntSplExpired AS rdPntSplExpired, TCNTCstPoint.FTPntStaExpired AS rtPntStaExpired,");
                    oSql.AppendLine("TCNTCstPoint.FDLastUpdOn AS rdLastUpdOn, TCNTCstPoint.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNTCstPoint.FTLastUpdBy AS rtLastUpdBy, TCNTCstPoint.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNTCstPoint with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMCst with(nolock) ON TCNTCstPoint.FTCstCode = TCNMCst.FTCstCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMCst.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCstDwn.raCstPoint = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCstPoint>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCstDwn.raCstPoint = oDB.C_DATaSqlQuery<cmlResInfoCstPoint>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                aoResult.roItem = oCstDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCstDwn>();
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
        /// Search Customer
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        [Route("CstSearchRDF")]
        [HttpPost]
        public cmlResList<cmlResCstSchRDF> POST_CSToCstSearch([FromBody] cmlReqCstSchRDF poPara)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResCstSchRDF> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResCstSchRDF>();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (poPara == null)
                {
                    aoResult.rtCode = oMsg.tMS_RespCode700;
                    aoResult.rtDesc = oMsg.tMS_RespDesc700;
                    return aoResult;
                }
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

                tKeyCache = "CstSearch" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResCstSchRDF>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }
                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT CST.FTCstCode,CSTL.FTCstName,CST.FTCstCardID,");
                oSql.AppendLine("CST.FTCstTel,CST.FTCstEmail,CST.FTCstSex,");
                oSql.AppendLine("CST.FDCstDob,CST.FTPplCodeRet,CST.FTCstDiscRet,");
                oSql.AppendLine("VCRD.FTCrdCode");
                oSql.AppendLine("FROM TCNMCst CST WITH(NOLOCK) ");
                oSql.AppendLine("LEFT JOIN TCNMCst_L CSTL WITH(NOLOCK) ON CST.FTCstCode=CSTL.FTCstCode AND CSTL.FNLngID= 1");
                oSql.AppendLine("LEFT JOIN VFN_DebitCardByCst VCRD WITH(NOLOCK) ON CST.FTCstCode = VCRD.FTCrdRefCode ");
                //oSql.AppendLine("WHERE CSTL.FNLngID='1' ");
                oSql.AppendLine("WHERE 1=1");
                switch (poPara.ptSearchCond)
                {
                    case "1":
                        oSql.AppendLine("AND CST.FTCstCode = '" + poPara.ptSearchValue + "' ");

                        break;
                    case "2":
                        oSql.AppendLine("AND CST.FTCstTel = '" + poPara.ptSearchValue + "' ");
                        break;
                    case "3":
                        oSql.AppendLine("AND CST.FTCstCardID = '" + poPara.ptSearchValue + "' ");
                        break;
                    default:
                        aoResult.rtCode = oMsg.tMS_RespCode701;
                        aoResult.rtDesc = oMsg.tMS_RespDesc701;
                        return aoResult;
                }

                aoResult.raItems = new List<cmlResCstSchRDF>();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlResCstSchRDF>(oSql.ToString(), nCmdTme).ToList();
                }

                if (aoResult.raItems.Count > 0)
                {

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResCstSchRDF>();
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
        /// Customer Quick Add
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        [Route("CstQuickAdd")]
        [HttpPost]
        public cmlResItem<cmlResCstQuickAdd> POST_CSToCstQuickAdd([FromBody] cmlReqCstQuickAdd poPara)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResCstQuickAdd> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTbl;
            bool bStaPrc;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResCstQuickAdd>();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (poPara == null)
                {
                    aoResult.rtCode = oMsg.tMS_RespCode700;
                    aoResult.rtDesc = oMsg.tMS_RespDesc700;
                    return aoResult;
                }
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

                tKeyCache = "CstQuickAdd" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCstQuickAdd>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }
                

                if (poPara.pnPriGrpType < 0 || poPara.pnPriGrpType > 3) // pnPriGrpType: 0:ไม่ส่ง 1:Retail 2:Wholesale 3: online  ค่าอื่น default 1
                {
                    poPara.pnPriGrpType = 1;
                }

                string tConStr = new AdaAccEntities().Database.Connection.ConnectionString.ToString();
                SqlParameter[] oPara = new SqlParameter[] {
                //new SqlParameter ("@FTCstCode",SqlDbType.VarChar,20){ Value = tCstCode},
                new SqlParameter ("@ptCstTel",SqlDbType.VarChar,50){ Value = poPara.ptCstTel},
                new SqlParameter ("@ptPriGrp",SqlDbType.VarChar,20){ Value = poPara.ptPriGrp},
                new SqlParameter ("@ptCreateBy",SqlDbType.VarChar,20){ Value = poPara.ptUsrCreate },
                new SqlParameter ("@pnPriGrpType",SqlDbType.Int){ Value = poPara.pnPriGrpType }
                //new SqlParameter ("@FNResult",SqlDbType.Int){ Direction = ParameterDirection.Output}
                };
                
                oDbTbl = new DataTable();
                cDatabase oDB = new cDatabase();
                bStaPrc = oDB.C_DATbExecuteQueryStoreProcedure(tConStr,"STP_CSTxCstQuickAdd", ref oPara, ref oDbTbl);

                if(bStaPrc == true)
                {
                    aoResult.roItem = new cmlResCstQuickAdd();
                    if (oDbTbl.Rows.Count > 0 && oDbTbl != null)
                    {
                        foreach(var oItem in oDbTbl.Rows)
                        {
                            aoResult.roItem.rtCstCode = oDbTbl.Rows[0][0].ToString();
                        }
                        
                    }
                    
                    // เก็บ KeyApi ลง Cache
                    oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;

                }

                return aoResult;

            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCstQuickAdd>();
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
        /// Update Customer Face
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        [Route("CstFaceUpd")]
        [HttpPost]
        public cmlResStatus POST_CSToCstFaceUpd([FromBody] List<cmlReqCstFaceUpd> paImgObj)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResStatus oResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResStatus();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (paImgObj == null)
                {
                    oResult.rtCode = oMsg.tMS_RespCode700;
                    oResult.rtDesc = oMsg.tMS_RespDesc700;
                    return oResult;
                }
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

                //tKeyCache = "POST_CSToCstFaceUpd" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                
                int nRow = 0;
                oSql = new StringBuilder();
                cDatabase oDB = new cDatabase();
                foreach (var oItem  in paImgObj)
                {
                    string tPathImag = "";
                    string tImagName = "";
                    if (!string.IsNullOrEmpty(paImgObj[nRow].ptCstCode))
                    {
                        if (!string.IsNullOrEmpty(paImgObj[nRow].pnCstPicSeq))
                        {
                            if (!string.IsNullOrEmpty(paImgObj[nRow].ptPicture))
                            {

                                byte[] abytes = Convert.FromBase64String(paImgObj[nRow].ptPicture.ToString());
                                //tPathImag = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString() + @"\AdaImage\Customer";
                                tPathImag = ConfigurationManager.AppSettings["PathImage"];
                                tImagName = paImgObj[nRow].ptCstCode + "Face" + paImgObj[nRow].pnCstPicSeq;

                                if(!Directory.Exists(tPathImag))
                                {
                                    tPathImag = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString() + @"\AdaImage\Customer";
                                    if (!Directory.Exists(tPathImag))
                                    {
                                        Directory.CreateDirectory(tPathImag);
                                    }
                                }
                                else
                                {
                                    tPathImag += @"\Customer";
                                    if (!Directory.Exists(tPathImag))
                                    {
                                        Directory.CreateDirectory(tPathImag);
                                    }
                                }

                                tPathImag += @"\" + tImagName + ".jpg";
                                if (!System.IO.File.Exists(tPathImag)) System.IO.File.Delete(tPathImag);
                                using (FileStream oImageFile = new FileStream(tPathImag, FileMode.Create))
                                {
                                    oImageFile.Write(abytes, 0, abytes.Length);
                                    oImageFile.Flush();
                                }

                                oSql.Clear();
                                oSql.AppendLine(" UPDATE TCNMImgObj WITH(ROWLOCK) SET");
                                oSql.AppendLine(" FTImgObj = '" + tPathImag + "', ");
                                oSql.AppendLine(" FDLastUpdOn = GETDATE(), ");
                                oSql.AppendLine(" FTLastUpdBy = '" + paImgObj[nRow].ptUsrCreate + "' ");

                                oSql.AppendLine(" WHERE FTImgRefID = '" + paImgObj[nRow].ptCstCode + "' ");
                                oSql.AppendLine(" AND FNImgSeq = '" + paImgObj[nRow].pnCstPicSeq + "' ");
                                oSql.AppendLine(" AND FTImgTable = 'TCNMCst' ");
                                oSql.AppendLine(" AND FTImgKey = 'Face' ");

                                nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());

                                if (nRowEff == 0)
                                {
                                    oSql.Clear();
                                    oSql.AppendLine(" INSERT INTO TCNMImgObj WITH(ROWLOCK) ");
                                    oSql.AppendLine(" (FTImgRefID, FNImgSeq, FTImgTable, ");
                                    oSql.AppendLine(" FTImgKey, FTImgObj, FDLastUpdOn, FDCreateOn, FTLastUpdBy, FTCreateBy)");
                                    oSql.AppendLine(" VALUES ");
                                    oSql.AppendLine(" (");
                                    oSql.AppendLine(" '" + paImgObj[nRow].ptCstCode + "', ");
                                    oSql.AppendLine(" '" + paImgObj[nRow].pnCstPicSeq + "',");
                                    oSql.AppendLine(" 'TCNMCst',");
                                    oSql.AppendLine(" 'Face',");
                                    oSql.AppendLine(" '" + tPathImag + "',");
                                    oSql.AppendLine(" GETDATE(), GETDATE(),");
                                    oSql.AppendLine(" '" + paImgObj[nRow].ptUsrCreate + "', '" + paImgObj[nRow].ptUsrCreate + "'");
                                    oSql.AppendLine(")");

                                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                                }
                            }
                            else
                            {
                                oResult.rtCode = oMsg.tMS_RespCode701;
                                oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                                return oResult;
                            }
                        }
                        else
                        {
                            oResult.rtCode = oMsg.tMS_RespCode701;
                            oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                            return oResult;
                        }
                    }
                    else
                    {
                        oResult.rtCode = oMsg.tMS_RespCode701;
                        oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                        return oResult;
                    }

                    nRow = +1;
                }
                
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001; 
                return oResult;
            }
            catch (Exception oExcept)
            {
                oResult = new cmlResStatus();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();

                return oResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                //oDatabase = null;
                oSql = null;
                oResult = null;
                aoSysConfig = null;
                oResult = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

        }


        /// <summary>
        /// ดึงข้อมูลรูปใบหน้าทีลงทะเบียนแล้ว ตามจํานวน 
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        [Route("CstFaceLists")]
        [HttpPost]
        public cmlResList<cmlResCstFaceLists> POST_CSToCstFaceLists([FromBody] cmlReqCstFaceLists poPara)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResCstFaceLists> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResCstFaceLists>();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (poPara == null)
                {
                    aoResult.rtCode = oMsg.tMS_RespCode700;
                    aoResult.rtDesc = oMsg.tMS_RespDesc700;
                    return aoResult;
                }
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

                tKeyCache = "CstFaceLists" + string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResList<cmlResCstFaceLists>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP "+ poPara.pnCstPicQty + " FTImgRefID AS rtFTCstCode, FNImgSeq AS rnFNImgSeq, FTImgObj AS rtFTImgObj");
                oSql.AppendLine(" FROM TCNMImgObj  WITH(NOLOCK)");
                oSql.AppendLine(" WHERE FTImgRefID ='" + poPara.ptCstCode + "'");
                oSql.AppendLine(" AND FTImgTable ='TCNMCst'");
                oSql.AppendLine(" ORDER BY FNImgSeq ASC");

                aoResult.raItems = new List<cmlResCstFaceLists>();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlResCstFaceLists>(oSql.ToString(), nCmdTme).ToList();
                }

                if (aoResult.raItems.Count > 0)
                {
                    
                }

                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                //aoResult.roItem = oSysDataDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;


            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResCstFaceLists>();
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
        /// Search Customer
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        [Route("CstSearch")]
        [HttpPost]
        public cmlResList<cmlResCstSch> POST_CSToCstSearch([FromBody] cmlReqCstSch poPara)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResList<cmlResCstSch> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            string tCgpCode = ""; //*Arm 63-05-02

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResCstSch>();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (poPara == null)
                {
                    aoResult.rtCode = oMsg.tMS_RespCode700;
                    aoResult.rtDesc = oMsg.tMS_RespDesc700;
                    return aoResult;
                }
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
                //*Arm 63-05-02
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT ISNULL(FTSysStaUsrValue,'') AS FTSysStaUsrValue FROM TsysConfig with(nolock) WHERE FTSysCode = 'AMQMember' AND FTSysSeq = '1'");
                tCgpCode = new cDatabase().C_DAToSqlQuery<string>(oSql.ToString());
                //+++++++++++++

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT CST.FTCstCode AS rtCstCode,CSTL.FTCstName AS rtCstName,CST.FTCstCardID AS rtCstCardID,");
                oSql.AppendLine("CST.FTCstTel AS rtCstTel, CST.FTCstEmail AS rtCstEmail, CST.FTCstSex AS rtCstSex,");
                oSql.AppendLine("CST.FDCstDob AS rdCstDob, CST.FTPplCodeRet AS rtPplCodeRet, CST.FTCstDiscRet AS rtCstDiscRet,");
                oSql.AppendLine("CST.FTCstTaxNo AS rtCstTaxNo, CST.FTCstStaAlwPosCalSo AS rtCstStaAlwPosCalSo,");
                oSql.AppendLine("CRD.FTCstCrdNo AS rtCstCrdNo, CRD.FDCstCrdExpire AS rdCstCrdExpire,");
                oSql.AppendLine("ISNULL(PNT.FCTxnPntQty, 0.00) AS rtTxnPntQty,");
                oSql.AppendLine("ISNULL(AMT.FCTxnBuyTotal, 0.00) AS rtTxnBuyTotal");
                oSql.AppendLine("FROM TCNMCst CST WITH(NOLOCK)");
                oSql.AppendLine("LEFT JOIN TCNMCst_L CSTL WITH(NOLOCK) ON CST.FTCstCode = CSTL.FTCstCode AND CSTL.FNLngID = 1");
                oSql.AppendLine("LEFT JOIN TCNMCstGrp_L CGL with(nolock) ON CGL.FTCgpCode = CST.FTCgpCode");
                oSql.AppendLine("LEFT JOIN TCNMCstCard CRD WITH(NOLOCK) ON CST.FTCstCode = CRD.FTCstCode");
                oSql.AppendLine("LEFT JOIN TCNTMemPntActive PNT WITH(NOLOCK) ON CST.FTCstCode = PNT.FTMemCode AND PNT.FTCgpCode = '" + tCgpCode + "'"); //*Arm 63-05-02
                oSql.AppendLine("LEFT JOIN TCNTMemAmtActive AMT WITH(NOLOCK) ON CST.FTCstCode = AMT.FTMemCode AND AMT.FTCgpCode = '" + tCgpCode + "'"); //*Arm 63-05-02

                string tCond = "WHERE";

                for(int ni = 0; ni<=5; ni++)
                {
                   switch(ni)
                    {
                        case 0:
                            if (!string.IsNullOrEmpty(poPara.ptCstCode))
                            {
                                oSql.AppendLine(tCond + " ISNULL(CST.FTCstCode,'') ='" + poPara.ptCstCode + "' ");
                                tCond = "OR";
                            }
                            break;
                        case 1:
                            if (!string.IsNullOrEmpty(poPara.ptCstName))
                            {
                                oSql.AppendLine(tCond + "  ISNULL(CSTL.FTCstName,'') LIKE '" + poPara.ptCstName + "%" + "' ");
                                tCond = "OR";
                            }
                            break;
                        case 2:
                            if (!string.IsNullOrEmpty(poPara.ptCstTel))
                            {
                                oSql.AppendLine(tCond + "  ISNULL(CST.FTCstTel,'') ='" + poPara.ptCstTel + "' ");
                                tCond = "OR";
                            }
                            break;
                        case 3:
                            if (!string.IsNullOrEmpty(poPara.ptCstCardID))
                            {
                                oSql.AppendLine(tCond + "  ISNULL(CST.FTCstCardID,'') ='" + poPara.ptCstCardID + "' ");
                                tCond = "OR";
                            }
                            break;
                        case 4:
                            if (!string.IsNullOrEmpty(poPara.ptCstCrdNo))
                            {
                                oSql.AppendLine(tCond + "  ISNULL(CRD.FTCstCrdNo,'') ='" + poPara.ptCstCrdNo + "' ");
                                tCond = "OR";
                            }
                            break;
                        case 5:
                            if (!string.IsNullOrEmpty(poPara.ptCstTaxNo))
                            {
                                oSql.AppendLine(tCond + "  ISNULL(CST.FTCstTaxNo,'') ='" + poPara.ptCstTaxNo + "' ");
                                
                            }
                            break;

                    }
                    
                }
                
                
                aoResult.raItems = new List<cmlResCstSch>();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    aoResult.raItems = oConn.Query<cmlResCstSch>(oSql.ToString(), nCmdTme).ToList();
                }

                if (aoResult.raItems.Count > 0)
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
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResList<cmlResCstSch>();
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