using API2PSMaster.Class;
using API2PSMaster.Class.Product;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Product;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Response.Product;
using Dapper;
using System;
using System.Collections.Generic;
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
    ///     Product.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductController : ControllerBase
    {
        /// <summary>
        ///     Insert product.
        /// </summary>
        /// <param name="poPara">information for insert product.</param>
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
        public cmlResItem<cmlResPdtItemIns> POST_PDToInsPdtItem([FromBody] cmlReqPdtItemIns poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            cProduct oPdt;
            StringBuilder oSql;
            cmlResItem<cmlResPdtItemIns> oResult;
            List<cmlTSysConfig> aoSysConfig;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tCode;
            string tSqlPdt, tSqlPdtPackSize, tSqlPdtBar, tSqlPdtPri;
            bool bVarifyPara, bGenCode;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResPdtItemIns>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                oPdt = new cProduct();

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

                if (string.IsNullOrEmpty(poPara.poPdtInf.ptPdtCode))
                {
                    bGenCode = oFunc.SP_GENbAutoFmtCode("TCNMPdt", "FTPdtCode", "0", poPara.ptBchCode, aoSysConfig, out tCode);
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
                    poPara.poPdtInf.ptPdtCode = tCode;
                    poPara.poPdtPackSizeInf.ptPdtCode = tCode;
                    poPara.poPdtBar.ptPdtCode = tCode;
                }

                tSqlPdt = oFunc.SP_GENtSqlCmdInsert<cmlReqPdtInfIns>(poPara.poPdtInf, "TCNMPdt", poPara.ptWhoUpd, true, true);
                tSqlPdtPackSize = oFunc.SP_GENtSqlCmdInsert<cmlReqPdtPackSizeInf>(poPara.poPdtPackSizeInf, "TCNMPdtPackSize", poPara.ptWhoUpd, false, true);
                tSqlPdtBar = oFunc.SP_GENtSqlCmdInsert<cmlReqPdtBarInf>(poPara.poPdtBar, "TCNMPdtBar", poPara.ptWhoUpd, false, true);
                tSqlPdtPri = oPdt.C_GENtStatementPdtPri(poPara,aoSysConfig);

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("   " + tSqlPdt + tSqlPdtPackSize + tSqlPdtBar + tSqlPdtPri);
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   ROLLBACK TRANSACTION");
                oSql.AppendLine("END CATCH");

                try
                {
                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "1");
                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                    oDB = new cDatabase(nConTme, nCmdTme);
                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());

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
                };
            }
            catch(Exception oExcept)
            {
                // Return error.
                oResult = new cmlResItem<cmlResPdtItemIns>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();
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
        /// Download product information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <param name="ptWahCode">รหัสคลัง</param>
        /// <returns></returns>
        [Route("Item/Download")]
        [HttpGet]
        //public cmlResItem<cmlResPdtItemDwn> GET_PDToDownloadPdtItem(DateTime pdDate,string ptShpCode = null)
        public cmlResItem<cmlResPdtItemDwn> GET_PDToDownloadPdtItem(DateTime pdDate, string ptBchCode, string ptWahCode = "")
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtItemDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtItemDwn oPdtItemDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            string tPathImage;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtItemDwn>();
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
                tPathImage ="";

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    aoResult.rtCode = oMsg.tMS_RespCode904;
                    aoResult.rtDesc = oMsg.tMS_RespDesc904;
                    return aoResult;
                }

                tKeyCache = "Product" + string.Format("{0:yyyyMMdd}" ,pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtItemDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                nCmdTme = oFunc.SP_GETnSQLCmdTimeout(); //*Net 63-10-06 เอา nCmdTime มาจาก Web Config
                //nCmdTme = 300; //Arm 63-04-02 เพิ่มเวลาในการ Query

                // Get data
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPdtStkCode AS rtPdtStkCode, FTPdtStkControl AS rtPdtStkControl, FTPdtGrpControl AS rtPdtGrpControl, FTPdtForSystem AS rtPdtForSystem,");
                oSql.AppendLine("SELECT PDT.FTPdtCode AS rtPdtCode, PDT.FTPdtStkControl AS rtPdtStkControl, PDT.FTPdtGrpControl AS rtPdtGrpControl, PDT.FTPdtForSystem AS rtPdtForSystem,");    //*Em 62-06-18
                oSql.AppendLine("PDT.FCPdtQtyOrdBuy AS rcPdtQtyOrdBuy, PDT.FCPdtCostDef AS rcPdtCostDef, PDT.FCPdtCostOth AS rcPdtCostOth, PDT.FCPdtCostStd AS rcPdtCostStd, PDT.FCPdtMin AS rcPdtMin,");
                oSql.AppendLine("PDT.FCPdtMax AS rcPdtMax, PDT.FTPdtPoint AS rtPdtPoint, PDT.FCPdtPointTime AS rcPdtPointTime, PDT.FTPdtType AS rtPdtType, PDT.FTPdtSaleType AS rtPdtSaleType,");
                oSql.AppendLine("PDT.FTPdtSetOrSN AS rtPdtSetOrSN, PDT.FTPdtStaSetPri AS rtPdtStaSetPri, PDT.FTPdtStaSetShwDT AS rtPdtStaSetShwDT, PDT.FTPdtStaAlwDis AS rtPdtStaAlwDis, PDT.FTPdtStaAlwReturn AS rtPdtStaAlwReturn,");
                oSql.AppendLine("PDT.FTPdtStaVatBuy AS rtPdtStaVatBuy, PDT.FTPdtStaVat AS rtPdtStaVat, PDT.FTPdtStaActive AS rtPdtStaActive, PDT.FTPdtStaAlwReCalOpt AS rtPdtStaAlwReCalOpt, PDT.FTPdtStaCsm AS rtPdtStaCsm,");
                oSql.AppendLine("PDT.FTTcgCode AS rtTcgCode, PDT.FTPgpChain AS rtPgpChain, PDT.FTPtyCode AS rtPtyCode,");
                oSql.AppendLine("PDT.FTPbnCode AS rtPbnCode, PDT.FTPmoCode AS rtPmoCode, PDT.FTVatCode AS rtVatCode, PDT.FTEvhCode AS rtEvhCode, PDT.FDPdtSaleStart AS rdPdtSaleStart,");
                oSql.AppendLine("PDT.FDPdtSaleStop AS rdPdtSaleStop,");
                oSql.AppendLine("PDT.FTPdtRefID AS rtPdtRefID, PDT.FTCtyCode AS rtCtyCode,"); //*Arm 65-08-11 -[CR-Oversea] -เพิ่มฟิลด์รหัสอ้างอิงสินค้า และรหัสประเทศ
                oSql.AppendLine("PDT.FDLastUpdOn AS rdLastUpdOn, PDT.FTLastUpdBy AS rtLastUpdBy,");
                oSql.AppendLine("PDT.FDCreateOn AS rdCreateOn, PDT.FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdt PDT with(nolock)");
                oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode+ "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode+"')"); //*Arm 63-09-19 Comment Code
                //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}",pdDate) +"'"); //*Arm 63-09-19 Comment Code
                ////if (ptShpCode != null) oSql.AppendLine("AND FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03

                //*Arm 63-09-19
                if (string.IsNullOrEmpty(ptWahCode)) 
                {
                    //ไม่ได้ส่งคลังมาด้วย
                    oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                }
                else
                {
                    //ส่งคลังมาด้วย
                    //oSql.AppendLine("INNER JOIN TCNTPdtStkBal PSB ON PDT.FTPdtCode = PSB.FTPdtCode AND PSB.FTBchCode = '" + ptBchCode + "' AND PSB.FTWahCode = '" + ptWahCode +"' ");
                    oSql.AppendLine("LEFT JOIN TCNTPdtStkBal PSB ON PDT.FTPdtCode = PSB.FTPdtCode AND PSB.FTBchCode = '" + ptBchCode + "' AND PSB.FTWahCode = '" + ptWahCode + "' "); //*Arm 63-10-14
                    oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), PSB.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSql.AppendLine("OR PDT.FTPdtStkControl = '2'"); //*Arm 63-10-14
                }
                //++++++++++++++

                aoResult.roItem = new cmlResPdtItemDwn();
                oPdtItemDwn = new cmlResPdtItemDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())   //*Em 62-06-09
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oPdtItemDwn.raPdt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdt>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oPdtItemDwn.raPdt   = oConn.Query<cmlResInfoPdt>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oPdtItemDwn.raPdt.Count > 0)
                    {

                        //*Arm 63-09-19
                        string tCondition = "";
                        oSql.Clear();
                        if (string.IsNullOrEmpty(ptWahCode))
                        {
                            //ไม่ได้ส่งคลังมาด้วย
                            oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')");
                            oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        }
                        else
                        {
                            //ส่งคลังมาด้วย
                            //oSql.AppendLine("INNER JOIN TCNTPdtStkBal PSB ON PDT.FTPdtCode = PSB.FTPdtCode  AND PSB.FTBchCode = '"+ ptBchCode + "' AND PSB.FTWahCode = '" + ptWahCode + "' ");
                            oSql.AppendLine("LEFT JOIN TCNTPdtStkBal PSB ON PDT.FTPdtCode = PSB.FTPdtCode  AND PSB.FTBchCode = '" + ptBchCode + "' AND PSB.FTWahCode = '" + ptWahCode + "' "); //*Arm 63-10-14
                            oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')");
                            oSql.AppendLine("AND CONVERT(VARCHAR(10), PSB.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                            oSql.AppendLine("OR PDT.FTPdtStkControl = '2'"); //*Arm 63-10-14
                        }
                        tCondition = oSql.ToString();
                        //Product Languague
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT DISTINCT TCNMPdt_L.FTPdtCode AS rtPdtCode, TCNMPdt_L.FNLngID AS rnLngID, TCNMPdt_L.FTPdtName AS rtPdtName,");
                        //oSql.AppendLine("TCNMPdt_L.FTPdtNameOth AS rtPdtNameOth, TCNMPdt_L.FTPdtNameABB AS rtPdtNameABB, TCNMPdt_L.FTPdtRmk AS rtPdtRmk");
                        //oSql.AppendLine("FROM TCNMPdt_L with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdt_L.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdt_L.FTPdtCode AS rtPdtCode, TCNMPdt_L.FNLngID AS rnLngID, TCNMPdt_L.FTPdtName AS rtPdtName,");
                        oSql.AppendLine("TCNMPdt_L.FTPdtNameOth AS rtPdtNameOth, TCNMPdt_L.FTPdtNameABB AS rtPdtNameABB, TCNMPdt_L.FTPdtRmk AS rtPdtRmk");
                        oSql.AppendLine("FROM TCNMPdt_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdt_L.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        oSql.AppendLine(tCondition); //*Arm 63-09-19
                        oPdtItemDwn.raPdtLng = oConn.Query<cmlResInfoPdtLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product packsize
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtPackSize.FTPdtCode AS rtPdtCode, TCNMPdtPackSize.FTPunCode AS rtPunCode, TCNMPdtPackSize.FCPdtUnitFact AS rcPdtUnitFact,");
                        oSql.AppendLine("TCNMPdtPackSize.FCPdtPriceRET AS rcPdtPriceRET, TCNMPdtPackSize.FCPdtPriceWHS AS rcPdtPriceWHS, TCNMPdtPackSize.FCPdtPriceNET AS rcPdtPriceNET,");
                        oSql.AppendLine("TCNMPdtPackSize.FTPdtGrade AS rtPdtGrade, TCNMPdtPackSize.FCPdtWeight AS rcPdtWeight, TCNMPdtPackSize.FTClrCode AS rtClrCode,");
                        oSql.AppendLine("TCNMPdtPackSize.FTPszCode AS rtPszCode, TCNMPdtPackSize.FTPdtUnitDim AS rtPdtUnitDim, TCNMPdtPackSize.FTPdtPkgDim AS rtPdtPkgDim,");
                        oSql.AppendLine("TCNMPdtPackSize.FTPdtStaAlwPick AS rtPdtStaAlwPick, TCNMPdtPackSize.FTPdtStaAlwPoHQ AS rtPdtStaAlwPoHQ,");
                        oSql.AppendLine("TCNMPdtPackSize.FDLastUpdOn AS rdLastUpdOn, TCNMPdtPackSize.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtPackSize.FTLastUpdBy AS rtLastUpdBy, TCNMPdtPackSize.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtPackSize with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtPackSize.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        
                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtPackSize.FTPdtCode = PDT.FTPdtCode"); 
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode "); 
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtPackSize = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPackSize>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtPackSize = oConn.Query<cmlResInfoPdtPackSize>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product barcode
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtBar.FTPdtCode AS rtPdtCode, TCNMPdtBar.FTBarCode AS rtBarCode, TCNMPdtBar.FTPunCode AS rtPunCode,");
                        oSql.AppendLine("TCNMPdtBar.FTBarStaUse AS rtBarStaUse, TCNMPdtBar.FTBarStaAlwSale AS rtBarStaAlwSale, TCNMPdtBar.FTBarStaByGen AS rtBarStaByGen,");
                        oSql.AppendLine("TCNMPdtBar.FTPlcCode AS rtPlcCode, ISNULL(TCNMPdtBar.FNPldSeq,0) AS rnPldSeq,");
                        oSql.AppendLine("TCNMPdtBar.FDLastUpdOn AS rdLastUpdOn, TCNMPdtBar.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtBar.FTLastUpdBy AS rtLastUpdBy, TCNMPdtBar.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtBar with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtBar.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtBar.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19


                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtBar = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBar>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtBar = oConn.Query<cmlResInfoPdtBar>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product brand
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtBrand.FTPbnCode AS rtPbnCode,");
                        oSql.AppendLine("TCNMPdtBrand.FDLastUpdOn AS rdLastUpdOn, TCNMPdtBrand.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtBrand.FTLastUpdBy AS rtLastUpdBy, TCNMPdtBrand.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtBrand with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtBrand.FTPbnCode = TCNMPdt.FTPbnCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtBrand.FTPbnCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtBrand = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBrand>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtBrand = oConn.Query<cmlResInfoPdtBrand>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product brand languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtBrand_L.FTPbnCode AS rtPbnCode, TCNMPdtBrand_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtBrand_L.FTPbnName AS rtPbnName, TCNMPdtBrand_L.FTPbnRmk AS rtPbnRmk");
                        oSql.AppendLine("FROM TCNMPdtBrand_L with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtBrand_L.FTPbnCode = TCNMPdt.FTPbnCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtBrand_L.FTPbnCode  = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'')= '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtBrandLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBrandLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtBrandLng = oConn.Query<cmlResInfoPdtBrandLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product group
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtGrp.FTPgpCode AS rtPgpCode, TCNMPdtGrp.FNPgpLevel AS rnPgpLevel, TCNMPdtGrp.FTPgpParent AS rtPgpParent,");
                        oSql.AppendLine("TCNMPdtGrp.FTPgpChain AS rtPgpChain,");
                        oSql.AppendLine("TCNMPdtGrp.FDLastUpdOn AS rdLastUpdOn, TCNMPdtGrp.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtGrp.FTLastUpdBy AS rtLastUpdBy, TCNMPdtGrp.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtGrp with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpCode = TCNMPdt.FTPgpCode");

                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpChain = TCNMPdt.FTPgpChain");   //*Em 61-11-06
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtGrp.FTPgpChain  = PDT.FTPgpChain");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtGrp>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtGrp = oConn.Query<cmlResInfoPdtGrp>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product group languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtGrp_L.FTPgpChain AS rtPgpChain, TCNMPdtGrp_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtGrp_L.FTPgpName AS rtPgpName, TCNMPdtGrp_L.FTPgpChainName AS rtPgpChainName, TCNMPdtGrp_L.FTPgpRmk AS rtPgpRmk");
                        oSql.AppendLine("FROM TCNMPdtGrp_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtGrp with(nolock) ON TCNMPdtGrp_L.FTPgpChain = TCNMPdtGrp.FTPgpChain");
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpCode = TCNMPdt.FTPgpCode");

                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpChain = TCNMPdt.FTPgpChain");   //*Em 61-11-06
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtGrp.FTPgpChain  = PDT.FTPgpChain");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtGrpLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtGrpLng = oConn.Query<cmlResInfoPdtGrpLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product model
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtModel.FTPmoCode AS rtPmoCode,");
                        oSql.AppendLine("TCNMPdtModel.FDLastUpdOn AS rdLastUpdOn, TCNMPdtModel.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtModel.FTLastUpdBy AS rtLastUpdBy, TCNMPdtModel.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtModel with(nolock)");
                        //*Arm 63-06-16 Comment Code 
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtModel.FTPmoCode = TCNMPdt.FTPmoCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtModel.FTPmoCode = PDT.FTPmoCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtModel = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtModel>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtModel = oConn.Query<cmlResInfoPdtModel>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product model languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtModel_L.FTPmoCode AS rtPmoCode, TCNMPdtModel_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtModel_L.FTPmoName AS rtPmoName, TCNMPdtModel_L.FTPmoRmk AS rtPmoRmk");
                        oSql.AppendLine("FROM TCNMPdtModel_L with(nolock)");

                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtModel_L.FTPmoCode = TCNMPdt.FTPmoCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtModel_L.FTPmoCode = PDT.FTPmoCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19


                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtModelLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtModelLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtModelLng = oConn.Query<cmlResInfoPdtModelLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product price header
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT DISTINCT TCNTPdtAdjPriHD.FTBchCode AS rtBchCode, TCNTPdtAdjPriHD.FTXphDocNo AS rtXphDocNo, TCNTPdtAdjPriHD.FTXphDocType AS rtXphDocType,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphStaAdj AS rtXphStaAdj, TCNTPdtAdjPriHD.FDXphDocDate AS rdXphDocDate, TCNTPdtAdjPriHD.FTXphDocTime AS rtXphDocTime,");
                        ////oSql.AppendLine("TCNTPdtAdjPriHD.FTXphName AS rtXphName, TCNTPdtAdjPriHD.FTPplCode AS rtPplCode, TCNTPdtAdjPriHD.FTAggCode AS rtAggCode,");//*Arm 63-06-15 Comment Code
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphName AS rtXphName, TCNTPdtAdjPriHD.FTPplCode AS rtPplCode, ");//*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FDXphDStart AS rdXphDStart, TCNTPdtAdjPriHD.FTXphTStart AS rtXphTStart, TCNTPdtAdjPriHD.FDXphDStop AS rdXphDStop,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphTStop AS rtXphTStop, TCNTPdtAdjPriHD.FTXphPriType AS rtXphPriType, TCNTPdtAdjPriHD.FTXphStaDoc AS rtXphStaDoc,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphStaPrcDoc AS rtXphStaPrcDoc, TCNTPdtAdjPriHD.FNXphStaDocAct AS rnXphStaDocAct, TCNTPdtAdjPriHD.FTUsrCode AS rtUsrCode,");
                        ////oSql.AppendLine("TCNTPdtAdjPriHD.FTXphUsrApv AS rtXphUsrApv, TCNTPdtAdjPriHD.FTXphZneTo AS rtXphZneTo, TCNTPdtAdjPriHD.FTXphBchTo AS rtXphBchTo,"); //*Arm 63-06-15 Comment Code
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphUsrApv AS rtXphUsrApv,"); //*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphRmk AS rtXphRmk,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FDLastUpdOn AS rdLastUpdOn, TCNTPdtAdjPriHD.FDCreateOn AS rdCreateOn,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTLastUpdBy AS rtLastUpdBy, TCNTPdtAdjPriHD.FTCreateBy AS rtCreateBy");
                        //oSql.AppendLine("FROM TCNTPdtAdjPriHD with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNTPdtAdjPriDT with(nolock) ON TCNTPdtAdjPriHD.FTBchCode = TCNTPdtAdjPriDT.FTBchCode AND TCNTPdtAdjPriHD.FTXphDocNo = TCNTPdtAdjPriDT.FTXphDocNo");
                        ////*Arm 63-06-16  Comment Code
                        ////oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = TCNMPdt.FTPdtCode");
                        ////oSql.AppendLine("WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'");
                        ////oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        ////*Arm 63-06-16 
                        //oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = PDT.FTPdtCode");
                        //oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        //oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        ////oSql.AppendLine("WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("AND (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine(tCondition); //*Arm 63-09-19
                        //oSql.AppendLine("AND TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19
                        ////++++++++++++++

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtPriHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPriHD>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        //oPdtItemDwn.raPdtPriHD = oConn.Query<cmlResInfoPdtPriHD>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        ////Product price detail
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT DISTINCT TCNTPdtAdjPriDT.FTBchCode AS rtBchCode, TCNTPdtAdjPriDT.FTXphDocNo AS rtXphDocNo, TCNTPdtAdjPriDT.FNXpdSeq AS rnXpdSeq,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FTPdtCode AS rtPdtCode, TCNTPdtAdjPriDT.FTPunCode AS rtPunCode,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FCXpdPriceRet AS rcXpdPriceRet, TCNTPdtAdjPriDT.FCXpdPriceWhs AS rcXpdPriceWhs, TCNTPdtAdjPriDT.FCXpdPriceNet AS rcXpdPriceNet,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FDLastUpdOn AS rdLastUpdOn, TCNTPdtAdjPriDT.FDCreateOn AS rdCreateOn,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FTLastUpdBy AS rtLastUpdBy, TCNTPdtAdjPriDT.FTCreateBy AS rtCreateBy");
                        //oSql.AppendLine("FROM TCNTPdtAdjPriDT with(nolock)");
                        //oSql.AppendLine("INNER JOIN");
                        //oSql.AppendLine("    (SELECT DISTINCT TCNTPdtAdjPriHD.FTBchCode, TCNTPdtAdjPriHD.FTXphDocNo FROM TCNTPdtAdjPriHD with(nolock)");
                        //oSql.AppendLine("    INNER JOIN TCNTPdtAdjPriDT with(nolock) ON TCNTPdtAdjPriHD.FTXphDocNo = TCNTPdtAdjPriDT.FTXphDocNo");
                        ////*Arm 63-06-16  Comment Code
                        ////oSql.AppendLine("    INNER JOIN TCNMPdt with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = TCNMPdt.FTPdtCode");
                        ////oSql.AppendLine("    WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'");
                        //////if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        ////oSql.AppendLine("    AND CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "') HD ON TCNTPdtAdjPriDT.FTBchCode = HD.FTBchCode AND TCNTPdtAdjPriDT.FTXphDocNo = HD.FTXphDocNo");

                        ////*Arm 63-06-16 
                        //oSql.AppendLine("   INNER JOIN TCNMPdt PDT with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = PDT.FTPdtCode");
                        //oSql.AppendLine("   LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        //oSql.AppendLine("   LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        ////oSql.AppendLine("   WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("   AND (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("   AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("   "+ tCondition); //*Arm 63-09-19
                        //oSql.AppendLine("   AND TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19
                        //oSql.AppendLine(") HD ON TCNTPdtAdjPriDT.FTBchCode = HD.FTBchCode AND TCNTPdtAdjPriDT.FTXphDocNo = HD.FTXphDocNo");
                        ////++++++++++++++

                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtPriDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPriDT>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        //oPdtItemDwn.raPdtPriDT = oConn.Query<cmlResInfoPdtPriDT>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product touch group
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtTouchGrp.FTTcgCode AS rtTcgCode, TCNMPdtTouchGrp.FTTcgStaUse AS rtTcgStaUse, ");
                        oSql.AppendLine("TCNMPdtTouchGrp.FDLastUpdOn AS rdLastUpdOn, TCNMPdtTouchGrp.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtTouchGrp.FTLastUpdBy AS rtLastUpdBy, TCNMPdtTouchGrp.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtTouchGrp with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtTouchGrp.FTTcgCode = TCNMPdt.FTTcgCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtTouchGrp.FTTcgCode = PDT.FTTcgCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtTouchGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTouchGrp>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtTouchGrp = oConn.Query<cmlResInfoPdtTouchGrp>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product touch group languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtTouchGrp_L.FTTcgCode AS rtTcgCode, TCNMPdtTouchGrp_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtTouchGrp_L.FTTcgName AS rtTcgName, TCNMPdtTouchGrp_L.FTTcgRmk AS rtTcgRmk");
                        oSql.AppendLine("FROM TCNMPdtTouchGrp_L with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtTouchGrp_L.FTTcgCode = TCNMPdt.FTTcgCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtTouchGrp_L.FTTcgCode = PDT.FTTcgCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtTouchGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTouchGrpLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtTouchGrpLng = oConn.Query<cmlResInfoPdtTouchGrpLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product type
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtType.FTPtyCode AS rtPtyCode,");
                        oSql.AppendLine("TCNMPdtType.FDLastUpdOn AS rdLastUpdOn, TCNMPdtType.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtType.FTLastUpdBy AS rtLastUpdBy, TCNMPdtType.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtType with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtType.FTPtyCode = TCNMPdt.FTPtyCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtType.FTPtyCode = PDT.FTPtyCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtType>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtType = oConn.Query<cmlResInfoPdtType>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product type languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtType_L.FTPtyCode AS rtPtyCode, TCNMPdtType_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtType_L.FTPtyName AS rtPtyName, TCNMPdtType_L.FTPtyRmk AS rtPtyRmk");
                        oSql.AppendLine("FROM TCNMPdtType_L with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtType_L.FTPtyCode = TCNMPdt.FTPtyCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtType_L.FTPtyCode = PDT.FTPtyCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTypeLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtTypeLng = oConn.Query<cmlResInfoPdtTypeLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product unit
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtUnit.FTPunCode AS rtPunCode,");
                        oSql.AppendLine("TCNMPdtUnit.FDLastUpdOn AS rdLastUpdOn, TCNMPdtUnit.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtUnit.FTLastUpdBy AS rtLastUpdBy, TCNMPdtUnit.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtUnit with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtPackSize with(nolock) ON TCNMPdtUnit.FTPunCode = TCNMPdtPackSize.FTPunCode");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtPackSize.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtPackSize.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtUnit = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtUnit>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtUnit = oConn.Query<cmlResInfoPdtUnit>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product unit languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtUnit_L.FTPunCode AS rtPunCode, TCNMPdtUnit_L.FNLngID AS rnLngID, TCNMPdtUnit_L.FTPunName AS rtPunName");
                        oSql.AppendLine("FROM TCNMPdtUnit_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtPackSize with(nolock) ON TCNMPdtUnit_L.FTPunCode = TCNMPdtPackSize.FTPunCode");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtPackSize.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtPackSize.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtUnitLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtUnitLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtUnitLng = oConn.Query<cmlResInfoPdtUnitLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Image Pdt
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMImgPdt.FNImgID AS rnImgID, TCNMImgPdt.FTImgRefID AS rtImgRefID, TCNMImgPdt.FNImgSeq AS rnImgSeq,");
                        oSql.AppendLine("TCNMImgPdt.FTImgTable AS rtImgTable, TCNMImgPdt.FTImgKey AS rtImgKey, TCNMImgPdt.FTImgObj AS rtImgObj,");
                        oSql.AppendLine("TCNMImgPdt.FDLastUpdOn AS rdLastUpdOn, TCNMImgPdt.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMImgPdt.FTLastUpdBy AS rtLastUpdBy, TCNMImgPdt.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMImgPdt with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMImgPdt.FTImgRefID = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMImgPdt.FTImgRefID = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  PDTWah.FTWahCode = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raImgPdt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgPdt>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raImgPdt = oConn.Query<cmlResInfoImgPdt>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //*Em 62-08-17
                        //PdtAge
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtAge.FTPdtCode AS rtPdtCode, TCNMPdtAge.FCPdtAge AS rcPdtAge, TCNMPdtAge.FCPdtMaxDegree AS rcPdtMaxDegree,");
                        oSql.AppendLine("TCNMPdtAge.FCPdtMinDegree AS rcPdtMinDegree, TCNMPdtAge.FTPdtIngredients AS rtPdtIngredients, TCNMPdtAge.FTPdtHowToUse AS rtPdtHowToUse,");
                        oSql.AppendLine("TCNMPdtAge.FTPdtWarning AS rtPdtWarning, TCNMPdtAge.FDPdtMfg AS rdPdtMfg, TCNMPdtAge.FDPdtExp AS rdPdtExp,");
                        oSql.AppendLine("TCNMPdtAge.FTPdtPerVolumn AS rtPdtPerVolumn, TCNMPdtAge.FCPdtPerCalories AS rcPdtPerCalories, TCNMPdtAge.FCPdtCookTime AS rcPdtCookTime,");
                        oSql.AppendLine("TCNMPdtAge.FCPdtCookHeat AS rcPdtCookHeat");
                        oSql.AppendLine("FROM TCNMPdtAge WITH(NOLOCK)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdtAge.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtAge.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        oPdtItemDwn.raTCNMPdtAge = oConn.Query<cmlTCNMPdtAge>(oSql.ToString(), nCmdTme).ToList();
                        //++++++++++++

                        //*Em 62-09-09
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtSpcBch.FTPdtCode AS rtPdtCode,TCNMPdtSpcBch.FTBchCode AS rtBchCode,TCNMPdtSpcBch.FTMerCode AS rtMerCode,");
                        oSql.AppendLine("TCNMPdtSpcBch.FTMgpCode AS rtMgpCode,TCNMPdtSpcBch.FCPdtMin AS rcPdtMin,TCNMPdtSpcBch.FTShpCode AS rtShpCode,");
                        oSql.AppendLine("TCNMPdtSpcBch.FTPdtRmk AS rtPdtRmk");
                        oSql.AppendLine("FROM TCNMPdtSpcBch WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdt.FTPdtCode = TCNMPdtSpcBch.FTPdtCode");
                        oSql.AppendLine("WHERE (ISNULL(TCNMPdtSpcBch.FTBchCode,'') = '' OR ISNULL(TCNMPdtSpcBch.FTBchCode,'') = ISNULL((SELECT TOP 1 FTBchCode FROM TCNMComp WITH(NOLOCK)),''))");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oPdtItemDwn.raTCNMPdtSpcBch = oConn.Query<cmlResTCNMPdtSpcBch>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                        //*Arm 63-01-17 : Query TCNMPdtDrug
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtDrug.FTPdtCode AS rtPdtCode, TCNMPdtDrug.FCPdgAge AS rcPdgAge, TCNMPdtDrug.FDPdgCreate AS rdPdgCreate, TCNMPdtDrug.FDPdgExpired AS rdPdgExpired, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgHowtoUse AS rtPdgHowtoUse, TCNMPdtDrug.FTPdgActIngredient AS rtPdgActIngredient, TCNMPdtDrug.FTPdgProperties AS rtPdgProperties, TCNMPdtDrug.FTPdgCtd AS rtPdgCtd, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgWarn AS rtPdgWarn, TCNMPdtDrug.FTPdgStopUse AS rtPdgStopUse, TCNMPdtDrug.FCPdgDoseSchedule AS rcPdgDoseSchedule, TCNMPdtDrug.FCPdgMaxIntake AS rcPdgMaxIntake, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgBrandName AS rtPdgBrandName, TCNMPdtDrug.FTPdgGenericName AS rtPdgGenericName, TCNMPdtDrug.FTPdgCategory AS rtPdgCategory, TCNMPdtDrug.FTPdgType AS rtPdgType, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgRegNo AS rtPdgRegNo, TCNMPdtDrug.FTPdgStorage AS rtPdgStorage, TCNMPdtDrug.FTPunCode AS rtPunCode, TCNMPdtDrug.FTPdgForm AS rtPdgForm, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgCtrlRole AS rtPdgCtrlRole, TCNMPdtDrug.FTPdgManufacturer AS rtPdgManufacturer, ");
                        oSql.AppendLine("TCNMPdtDrug.FDLastUpdOn AS rdLastUpdOn, TCNMPdtDrug.FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("TCNMPdtDrug.FDCreateOn AS rdCreateOn, TCNMPdtDrug.FTCreateBy AS rtCreateBy ");
                        oSql.AppendLine("FROM TCNMPdtDrug WITH(NOLOCK)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdt.FTPdtCode = TCNMPdtDrug.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        
                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtDrug.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'')  = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19
                        oPdtItemDwn.raTCNMPdtDrug = oConn.Query<cmlResInfoPdtDrug>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                        //*Arm 63-01-17 : Query TCNMPdtSpcWah
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtSpcWah.FTPdtCode AS rtPdtCode, TCNMPdtSpcWah.FTBchCode AS rtBchCode, TCNMPdtSpcWah.FTWahCode AS rtWahCode,");
                        oSql.AppendLine("TCNMPdtSpcWah.FCSpwQtyMin AS rcSpwQtyMin, TCNMPdtSpcWah.FCSpwQtyMax AS rcSpwQtyMax, TCNMPdtSpcWah.FTSpwRmk AS rtSpwRmk");
                        oSql.AppendLine("FROM TCNMPdtSpcWah WITH(NOLOCK) ");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdt.FTPdtCode = TCNMPdtSpcWah.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON TCNMPdtSpcWah.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(TCNMPdtSpcWah.FTWahCode,'') = '' OR  ISNULL(TCNMPdtSpcWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        oSql.AppendLine(tCondition); //*Arm 63-09-19
                        oPdtItemDwn.raTCNMPdtSpcWah = oConn.Query<cmlResInfoPdtSpcWah>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oPdtItemDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch(Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtItemDwn>();
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
        ///     Download product.
        /// </summary>
        /// <param name="poData">information for fillter product.</param>
        /// <returns>
        ///     Product varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        /// </returns>
        [Route("Item/Download")]
        [HttpPost]
        public cmlResItem<cmlResPdtItemDwn> POST_PDToDownloadPdtItem(cmlReqPdtItemDwn poData)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtItemDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtItemDwn oPdtItemDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            string tPathImage;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtItemDwn>();
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
                tPathImage = "";

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    aoResult.rtCode = oMsg.tMS_RespCode904;
                    aoResult.rtDesc = oMsg.tMS_RespDesc904;
                    return aoResult;
                }

                //tKeyCache = "Product" + string.Format("{0:yyyyMMdd}", pdDate);
                //if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                //{
                //    // ถ้ามี key อยุ่ใน cache
                //    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtItemDwn>>(tKeyCache);
                //    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                //    aoResult.rtCode = oMsg.tMS_RespCode001;
                //    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                //    return aoResult;
                //}

                nCmdTme = oFunc.SP_GETnSQLCmdTimeout(); //*Net 63-10-06 เอา nCmdTime มาจาก Web Config
                //nCmdTme = 300; //Arm 63-04-02 เพิ่มเวลาในการ Query

                //*Arm 65-08-30 -[CR-Oversea]
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTCtyCode FROM TCNMAgency WITH(NOLOCK) WHERE FTAgnCode = '" + poData.ptAgnCode + "' ");
                string tCtyCode = new cDatabase().C_DAToSqlQuery<string>(oSql.ToString());
                if(string.IsNullOrEmpty(tCtyCode))
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //+++++++++++++

                // Get data
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPdtStkCode AS rtPdtStkCode, FTPdtStkControl AS rtPdtStkControl, FTPdtGrpControl AS rtPdtGrpControl, FTPdtForSystem AS rtPdtForSystem,");
                oSql.AppendLine("SELECT PDT.FTPdtCode AS rtPdtCode, PDT.FTPdtStkControl AS rtPdtStkControl, PDT.FTPdtGrpControl AS rtPdtGrpControl, PDT.FTPdtForSystem AS rtPdtForSystem,");    //*Em 62-06-18
                oSql.AppendLine("PDT.FCPdtQtyOrdBuy AS rcPdtQtyOrdBuy, PDT.FCPdtCostDef AS rcPdtCostDef, PDT.FCPdtCostOth AS rcPdtCostOth, PDT.FCPdtCostStd AS rcPdtCostStd, PDT.FCPdtMin AS rcPdtMin,");
                oSql.AppendLine("PDT.FCPdtMax AS rcPdtMax, PDT.FTPdtPoint AS rtPdtPoint, PDT.FCPdtPointTime AS rcPdtPointTime, PDT.FTPdtType AS rtPdtType, PDT.FTPdtSaleType AS rtPdtSaleType,");
                oSql.AppendLine("PDT.FTPdtSetOrSN AS rtPdtSetOrSN, PDT.FTPdtStaSetPri AS rtPdtStaSetPri, PDT.FTPdtStaSetShwDT AS rtPdtStaSetShwDT, PDT.FTPdtStaAlwDis AS rtPdtStaAlwDis, PDT.FTPdtStaAlwReturn AS rtPdtStaAlwReturn,");
                oSql.AppendLine("PDT.FTPdtStaVatBuy AS rtPdtStaVatBuy, PDT.FTPdtStaVat AS rtPdtStaVat, PDT.FTPdtStaActive AS rtPdtStaActive, PDT.FTPdtStaAlwReCalOpt AS rtPdtStaAlwReCalOpt, PDT.FTPdtStaCsm AS rtPdtStaCsm,");
                oSql.AppendLine("PDT.FTTcgCode AS rtTcgCode, PDT.FTPgpChain AS rtPgpChain, PDT.FTPtyCode AS rtPtyCode,");
                oSql.AppendLine("PDT.FTPbnCode AS rtPbnCode, PDT.FTPmoCode AS rtPmoCode, PDT.FTVatCode AS rtVatCode, PDT.FTEvhCode AS rtEvhCode, PDT.FDPdtSaleStart AS rdPdtSaleStart,");
                oSql.AppendLine("PDT.FDPdtSaleStop AS rdPdtSaleStop,");
                oSql.AppendLine("PDT.FTPdtRefID AS rtPdtRefID, PDT.FTCtyCode AS rtCtyCode,"); //*Arm 65-08-11 -[CR-Oversea] -เพิ่มฟิลด์รหัสอ้างอิงสินค้า และรหัสประเทศ
                oSql.AppendLine("PDT.FDLastUpdOn AS rdLastUpdOn, PDT.FTLastUpdBy AS rtLastUpdBy,");
                oSql.AppendLine("PDT.FDCreateOn AS rdCreateOn, PDT.FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdt PDT with(nolock)");
                oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode+ "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode+"')"); //*Arm 63-09-19 Comment Code
                //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}",pdDate) +"'"); //*Arm 63-09-19 Comment Code
                ////if (ptShpCode != null) oSql.AppendLine("AND FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03

                //*Arm 63-09-19
                if (string.IsNullOrEmpty(poData.ptWahCode))
                {
                    //ไม่ได้ส่งคลังมาด้วย
                    oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + poData.ptBchCode + "')");
                    oSql.AppendLine("AND (ISNULL(PDTBch.FTAgnCode,'') = '' OR ISNULL(PDTBch.FTAgnCode,'') = '" + poData.ptAgnCode + "')"); //*Arm 63-10-15
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", poData.pdDate) + "'");
                    oSql.AppendLine("AND PDT.FTCtyCode = '" + tCtyCode + "'");  //*Arm 65-08-30 -[CR-Oversea]
                }
                else
                {
                    //ส่งคลังมาด้วย
                    oSql.AppendLine("LEFT JOIN TCNTPdtStkBal PSB ON PDT.FTPdtCode = PSB.FTPdtCode AND PSB.FTBchCode = '" + poData.ptBchCode + "' AND PSB.FTWahCode = '" + poData.ptWahCode + "' "); //*Arm 63-10-14
                    oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + poData.ptBchCode + "')");
                    oSql.AppendLine("AND (ISNULL(PDTBch.FTAgnCode,'') = '' OR ISNULL(PDTBch.FTAgnCode,'') = '" + poData.ptAgnCode + "')"); //*Arm 63-10-15
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), PSB.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", poData.pdDate) + "'");
                    oSql.AppendLine("AND PDT.FTCtyCode = '" + tCtyCode + "'");  //*Arm 65-08-30 -[CR-Oversea]
                    oSql.AppendLine("OR PDT.FTPdtStkControl = '2'"); //*Arm 63-10-14
                }
                //++++++++++++++

                aoResult.roItem = new cmlResPdtItemDwn();
                oPdtItemDwn = new cmlResPdtItemDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //using (DbConnection oConn = oDB.Database.Connection)
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())   //*Em 62-06-09
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oPdtItemDwn.raPdt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdt>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oPdtItemDwn.raPdt = oConn.Query<cmlResInfoPdt>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09
                    if (oPdtItemDwn.raPdt.Count > 0)
                    {

                        //*Arm 63-09-19
                        string tCondition = "";
                        oSql.Clear();
                        if (string.IsNullOrEmpty(poData.ptWahCode))
                        {
                            //ไม่ได้ส่งคลังมาด้วย
                            oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + poData.ptBchCode + "')");
                            oSql.AppendLine("AND (ISNULL(PDTBch.FTAgnCode,'') = '' OR ISNULL(PDTBch.FTAgnCode,'') = '" + poData.ptAgnCode + "')"); //*Arm 63-10-15
                            oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", poData.pdDate) + "'");
                            oSql.AppendLine("AND PDT.FTCtyCode = '" + tCtyCode + "'");  //*Arm 65-08-30 -[CR-Oversea]
                        }
                        else
                        {
                            //ส่งคลังมาด้วย
                            oSql.AppendLine("LEFT JOIN TCNTPdtStkBal PSB ON PDT.FTPdtCode = PSB.FTPdtCode  AND PSB.FTBchCode = '" + poData.ptBchCode + "' AND PSB.FTWahCode = '" + poData.ptWahCode + "' "); //*Arm 63-10-14
                            oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + poData.ptBchCode + "')");
                            oSql.AppendLine("AND (ISNULL(PDTBch.FTAgnCode,'') = '' OR ISNULL(PDTBch.FTAgnCode,'') = '" + poData.ptAgnCode + "')"); //*Arm 63-10-15
                            oSql.AppendLine("AND CONVERT(VARCHAR(10), PSB.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", poData.pdDate) + "'");
                            oSql.AppendLine("AND PDT.FTCtyCode = '" + tCtyCode + "'");  //*Arm 65-08-30 -[CR-Oversea]
                            oSql.AppendLine("OR PDT.FTPdtStkControl = '2'"); //*Arm 63-10-14
                        }
                        tCondition = oSql.ToString();
                        //Product Languague
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT DISTINCT TCNMPdt_L.FTPdtCode AS rtPdtCode, TCNMPdt_L.FNLngID AS rnLngID, TCNMPdt_L.FTPdtName AS rtPdtName,");
                        //oSql.AppendLine("TCNMPdt_L.FTPdtNameOth AS rtPdtNameOth, TCNMPdt_L.FTPdtNameABB AS rtPdtNameABB, TCNMPdt_L.FTPdtRmk AS rtPdtRmk");
                        //oSql.AppendLine("FROM TCNMPdt_L with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdt_L.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdt_L.FTPdtCode AS rtPdtCode, TCNMPdt_L.FNLngID AS rnLngID, TCNMPdt_L.FTPdtName AS rtPdtName,");
                        oSql.AppendLine("TCNMPdt_L.FTPdtNameOth AS rtPdtNameOth, TCNMPdt_L.FTPdtNameABB AS rtPdtNameABB, TCNMPdt_L.FTPdtRmk AS rtPdtRmk");
                        oSql.AppendLine("FROM TCNMPdt_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdt_L.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        oSql.AppendLine(tCondition); //*Arm 63-09-19
                        oPdtItemDwn.raPdtLng = oConn.Query<cmlResInfoPdtLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product packsize
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtPackSize.FTPdtCode AS rtPdtCode, TCNMPdtPackSize.FTPunCode AS rtPunCode, TCNMPdtPackSize.FCPdtUnitFact AS rcPdtUnitFact,");
                        oSql.AppendLine("TCNMPdtPackSize.FCPdtPriceRET AS rcPdtPriceRET, TCNMPdtPackSize.FCPdtPriceWHS AS rcPdtPriceWHS, TCNMPdtPackSize.FCPdtPriceNET AS rcPdtPriceNET,");
                        oSql.AppendLine("TCNMPdtPackSize.FTPdtGrade AS rtPdtGrade, TCNMPdtPackSize.FCPdtWeight AS rcPdtWeight, TCNMPdtPackSize.FTClrCode AS rtClrCode,");
                        oSql.AppendLine("TCNMPdtPackSize.FTPszCode AS rtPszCode, TCNMPdtPackSize.FTPdtUnitDim AS rtPdtUnitDim, TCNMPdtPackSize.FTPdtPkgDim AS rtPdtPkgDim,");
                        oSql.AppendLine("TCNMPdtPackSize.FTPdtStaAlwPick AS rtPdtStaAlwPick, TCNMPdtPackSize.FTPdtStaAlwPoHQ AS rtPdtStaAlwPoHQ,");
                        oSql.AppendLine("TCNMPdtPackSize.FDLastUpdOn AS rdLastUpdOn, TCNMPdtPackSize.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtPackSize.FTLastUpdBy AS rtLastUpdBy, TCNMPdtPackSize.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtPackSize with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtPackSize.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtPackSize.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtPackSize = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPackSize>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtPackSize = oConn.Query<cmlResInfoPdtPackSize>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product barcode
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtBar.FTPdtCode AS rtPdtCode, TCNMPdtBar.FTBarCode AS rtBarCode, TCNMPdtBar.FTPunCode AS rtPunCode,");
                        oSql.AppendLine("TCNMPdtBar.FTBarStaUse AS rtBarStaUse, TCNMPdtBar.FTBarStaAlwSale AS rtBarStaAlwSale, TCNMPdtBar.FTBarStaByGen AS rtBarStaByGen,");
                        oSql.AppendLine("TCNMPdtBar.FTPlcCode AS rtPlcCode, ISNULL(TCNMPdtBar.FNPldSeq,0) AS rnPldSeq,");
                        oSql.AppendLine("TCNMPdtBar.FDLastUpdOn AS rdLastUpdOn, TCNMPdtBar.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtBar.FTLastUpdBy AS rtLastUpdBy, TCNMPdtBar.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtBar with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtBar.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtBar.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19


                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtBar = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBar>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtBar = oConn.Query<cmlResInfoPdtBar>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product brand
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtBrand.FTPbnCode AS rtPbnCode,");
                        oSql.AppendLine("TCNMPdtBrand.FDLastUpdOn AS rdLastUpdOn, TCNMPdtBrand.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtBrand.FTLastUpdBy AS rtLastUpdBy, TCNMPdtBrand.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtBrand with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtBrand.FTPbnCode = TCNMPdt.FTPbnCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtBrand.FTPbnCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtBrand = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBrand>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtBrand = oConn.Query<cmlResInfoPdtBrand>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product brand languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtBrand_L.FTPbnCode AS rtPbnCode, TCNMPdtBrand_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtBrand_L.FTPbnName AS rtPbnName, TCNMPdtBrand_L.FTPbnRmk AS rtPbnRmk");
                        oSql.AppendLine("FROM TCNMPdtBrand_L with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtBrand_L.FTPbnCode = TCNMPdt.FTPbnCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtBrand_L.FTPbnCode  = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'')= '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtBrandLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtBrandLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtBrandLng = oConn.Query<cmlResInfoPdtBrandLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product group
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtGrp.FTPgpCode AS rtPgpCode, TCNMPdtGrp.FNPgpLevel AS rnPgpLevel, TCNMPdtGrp.FTPgpParent AS rtPgpParent,");
                        oSql.AppendLine("TCNMPdtGrp.FTPgpChain AS rtPgpChain,");
                        oSql.AppendLine("TCNMPdtGrp.FDLastUpdOn AS rdLastUpdOn, TCNMPdtGrp.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtGrp.FTLastUpdBy AS rtLastUpdBy, TCNMPdtGrp.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtGrp with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpCode = TCNMPdt.FTPgpCode");

                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpChain = TCNMPdt.FTPgpChain");   //*Em 61-11-06
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtGrp.FTPgpChain  = PDT.FTPgpChain");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtGrp>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtGrp = oConn.Query<cmlResInfoPdtGrp>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product group languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtGrp_L.FTPgpChain AS rtPgpChain, TCNMPdtGrp_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtGrp_L.FTPgpName AS rtPgpName, TCNMPdtGrp_L.FTPgpChainName AS rtPgpChainName, TCNMPdtGrp_L.FTPgpRmk AS rtPgpRmk");
                        oSql.AppendLine("FROM TCNMPdtGrp_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtGrp with(nolock) ON TCNMPdtGrp_L.FTPgpChain = TCNMPdtGrp.FTPgpChain");
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpCode = TCNMPdt.FTPgpCode");

                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtGrp.FTPgpChain = TCNMPdt.FTPgpChain");   //*Em 61-11-06
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtGrp.FTPgpChain  = PDT.FTPgpChain");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtGrpLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtGrpLng = oConn.Query<cmlResInfoPdtGrpLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product model
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtModel.FTPmoCode AS rtPmoCode,");
                        oSql.AppendLine("TCNMPdtModel.FDLastUpdOn AS rdLastUpdOn, TCNMPdtModel.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtModel.FTLastUpdBy AS rtLastUpdBy, TCNMPdtModel.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtModel with(nolock)");
                        //*Arm 63-06-16 Comment Code 
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtModel.FTPmoCode = TCNMPdt.FTPmoCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtModel.FTPmoCode = PDT.FTPmoCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtModel = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtModel>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtModel = oConn.Query<cmlResInfoPdtModel>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product model languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtModel_L.FTPmoCode AS rtPmoCode, TCNMPdtModel_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtModel_L.FTPmoName AS rtPmoName, TCNMPdtModel_L.FTPmoRmk AS rtPmoRmk");
                        oSql.AppendLine("FROM TCNMPdtModel_L with(nolock)");

                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtModel_L.FTPmoCode = TCNMPdt.FTPmoCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtModel_L.FTPmoCode = PDT.FTPmoCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19


                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtModelLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtModelLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtModelLng = oConn.Query<cmlResInfoPdtModelLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product price header
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT DISTINCT TCNTPdtAdjPriHD.FTBchCode AS rtBchCode, TCNTPdtAdjPriHD.FTXphDocNo AS rtXphDocNo, TCNTPdtAdjPriHD.FTXphDocType AS rtXphDocType,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphStaAdj AS rtXphStaAdj, TCNTPdtAdjPriHD.FDXphDocDate AS rdXphDocDate, TCNTPdtAdjPriHD.FTXphDocTime AS rtXphDocTime,");
                        ////oSql.AppendLine("TCNTPdtAdjPriHD.FTXphName AS rtXphName, TCNTPdtAdjPriHD.FTPplCode AS rtPplCode, TCNTPdtAdjPriHD.FTAggCode AS rtAggCode,");//*Arm 63-06-15 Comment Code
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphName AS rtXphName, TCNTPdtAdjPriHD.FTPplCode AS rtPplCode, ");//*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FDXphDStart AS rdXphDStart, TCNTPdtAdjPriHD.FTXphTStart AS rtXphTStart, TCNTPdtAdjPriHD.FDXphDStop AS rdXphDStop,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphTStop AS rtXphTStop, TCNTPdtAdjPriHD.FTXphPriType AS rtXphPriType, TCNTPdtAdjPriHD.FTXphStaDoc AS rtXphStaDoc,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphStaPrcDoc AS rtXphStaPrcDoc, TCNTPdtAdjPriHD.FNXphStaDocAct AS rnXphStaDocAct, TCNTPdtAdjPriHD.FTUsrCode AS rtUsrCode,");
                        ////oSql.AppendLine("TCNTPdtAdjPriHD.FTXphUsrApv AS rtXphUsrApv, TCNTPdtAdjPriHD.FTXphZneTo AS rtXphZneTo, TCNTPdtAdjPriHD.FTXphBchTo AS rtXphBchTo,"); //*Arm 63-06-15 Comment Code
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphUsrApv AS rtXphUsrApv,"); //*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTXphRmk AS rtXphRmk,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FDLastUpdOn AS rdLastUpdOn, TCNTPdtAdjPriHD.FDCreateOn AS rdCreateOn,");
                        //oSql.AppendLine("TCNTPdtAdjPriHD.FTLastUpdBy AS rtLastUpdBy, TCNTPdtAdjPriHD.FTCreateBy AS rtCreateBy");
                        //oSql.AppendLine("FROM TCNTPdtAdjPriHD with(nolock)");
                        //oSql.AppendLine("INNER JOIN TCNTPdtAdjPriDT with(nolock) ON TCNTPdtAdjPriHD.FTBchCode = TCNTPdtAdjPriDT.FTBchCode AND TCNTPdtAdjPriHD.FTXphDocNo = TCNTPdtAdjPriDT.FTXphDocNo");
                        ////*Arm 63-06-16  Comment Code
                        ////oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = TCNMPdt.FTPdtCode");
                        ////oSql.AppendLine("WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'");
                        ////oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        ////*Arm 63-06-16 
                        //oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = PDT.FTPdtCode");
                        //oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        //oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        ////oSql.AppendLine("WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("AND (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine(tCondition); //*Arm 63-09-19
                        //oSql.AppendLine("AND TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19
                        ////++++++++++++++

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtPriHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPriHD>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        //oPdtItemDwn.raPdtPriHD = oConn.Query<cmlResInfoPdtPriHD>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        ////Product price detail
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT DISTINCT TCNTPdtAdjPriDT.FTBchCode AS rtBchCode, TCNTPdtAdjPriDT.FTXphDocNo AS rtXphDocNo, TCNTPdtAdjPriDT.FNXpdSeq AS rnXpdSeq,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FTPdtCode AS rtPdtCode, TCNTPdtAdjPriDT.FTPunCode AS rtPunCode,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FCXpdPriceRet AS rcXpdPriceRet, TCNTPdtAdjPriDT.FCXpdPriceWhs AS rcXpdPriceWhs, TCNTPdtAdjPriDT.FCXpdPriceNet AS rcXpdPriceNet,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FDLastUpdOn AS rdLastUpdOn, TCNTPdtAdjPriDT.FDCreateOn AS rdCreateOn,");
                        //oSql.AppendLine("TCNTPdtAdjPriDT.FTLastUpdBy AS rtLastUpdBy, TCNTPdtAdjPriDT.FTCreateBy AS rtCreateBy");
                        //oSql.AppendLine("FROM TCNTPdtAdjPriDT with(nolock)");
                        //oSql.AppendLine("INNER JOIN");
                        //oSql.AppendLine("    (SELECT DISTINCT TCNTPdtAdjPriHD.FTBchCode, TCNTPdtAdjPriHD.FTXphDocNo FROM TCNTPdtAdjPriHD with(nolock)");
                        //oSql.AppendLine("    INNER JOIN TCNTPdtAdjPriDT with(nolock) ON TCNTPdtAdjPriHD.FTXphDocNo = TCNTPdtAdjPriDT.FTXphDocNo");
                        ////*Arm 63-06-16  Comment Code
                        ////oSql.AppendLine("    INNER JOIN TCNMPdt with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = TCNMPdt.FTPdtCode");
                        ////oSql.AppendLine("    WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'");
                        //////if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        ////oSql.AppendLine("    AND CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "') HD ON TCNTPdtAdjPriDT.FTBchCode = HD.FTBchCode AND TCNTPdtAdjPriDT.FTXphDocNo = HD.FTXphDocNo");

                        ////*Arm 63-06-16 
                        //oSql.AppendLine("   INNER JOIN TCNMPdt PDT with(nolock) ON TCNTPdtAdjPriDT.FTPdtCode = PDT.FTPdtCode");
                        //oSql.AppendLine("   LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        //oSql.AppendLine("   LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        ////oSql.AppendLine("   WHERE TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("   AND (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        ////oSql.AppendLine("   AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("   "+ tCondition); //*Arm 63-09-19
                        //oSql.AppendLine("   AND TCNTPdtAdjPriHD.FTXphStaDoc = '1' AND TCNTPdtAdjPriHD.FTXphStaPrcDoc = '1'"); //*Arm 63-09-19
                        //oSql.AppendLine(") HD ON TCNTPdtAdjPriDT.FTBchCode = HD.FTBchCode AND TCNTPdtAdjPriDT.FTXphDocNo = HD.FTXphDocNo");
                        ////++++++++++++++

                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtPriDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPriDT>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        //oPdtItemDwn.raPdtPriDT = oConn.Query<cmlResInfoPdtPriDT>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product touch group
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtTouchGrp.FTTcgCode AS rtTcgCode, TCNMPdtTouchGrp.FTTcgStaUse AS rtTcgStaUse, ");
                        oSql.AppendLine("TCNMPdtTouchGrp.FDLastUpdOn AS rdLastUpdOn, TCNMPdtTouchGrp.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtTouchGrp.FTLastUpdBy AS rtLastUpdBy, TCNMPdtTouchGrp.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtTouchGrp with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtTouchGrp.FTTcgCode = TCNMPdt.FTTcgCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtTouchGrp.FTTcgCode = PDT.FTTcgCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtTouchGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTouchGrp>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtTouchGrp = oConn.Query<cmlResInfoPdtTouchGrp>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product touch group languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtTouchGrp_L.FTTcgCode AS rtTcgCode, TCNMPdtTouchGrp_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtTouchGrp_L.FTTcgName AS rtTcgName, TCNMPdtTouchGrp_L.FTTcgRmk AS rtTcgRmk");
                        oSql.AppendLine("FROM TCNMPdtTouchGrp_L with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtTouchGrp_L.FTTcgCode = TCNMPdt.FTTcgCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtTouchGrp_L.FTTcgCode = PDT.FTTcgCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtTouchGrpLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTouchGrpLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtTouchGrpLng = oConn.Query<cmlResInfoPdtTouchGrpLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product type
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtType.FTPtyCode AS rtPtyCode,");
                        oSql.AppendLine("TCNMPdtType.FDLastUpdOn AS rdLastUpdOn, TCNMPdtType.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtType.FTLastUpdBy AS rtLastUpdBy, TCNMPdtType.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtType with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtType.FTPtyCode = TCNMPdt.FTPtyCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtType.FTPtyCode = PDT.FTPtyCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtType>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtType = oConn.Query<cmlResInfoPdtType>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product type languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtType_L.FTPtyCode AS rtPtyCode, TCNMPdtType_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMPdtType_L.FTPtyName AS rtPtyName, TCNMPdtType_L.FTPtyRmk AS rtPtyRmk");
                        oSql.AppendLine("FROM TCNMPdtType_L with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtType_L.FTPtyCode = TCNMPdt.FTPtyCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtType_L.FTPtyCode = PDT.FTPtyCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtTypeLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtTypeLng = oConn.Query<cmlResInfoPdtTypeLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product unit
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtUnit.FTPunCode AS rtPunCode,");
                        oSql.AppendLine("TCNMPdtUnit.FDLastUpdOn AS rdLastUpdOn, TCNMPdtUnit.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMPdtUnit.FTLastUpdBy AS rtLastUpdBy, TCNMPdtUnit.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMPdtUnit with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtPackSize with(nolock) ON TCNMPdtUnit.FTPunCode = TCNMPdtPackSize.FTPunCode");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtPackSize.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtPackSize.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtUnit = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtUnit>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtUnit = oConn.Query<cmlResInfoPdtUnit>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Product unit languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtUnit_L.FTPunCode AS rtPunCode, TCNMPdtUnit_L.FNLngID AS rnLngID, TCNMPdtUnit_L.FTPunName AS rtPunName");
                        oSql.AppendLine("FROM TCNMPdtUnit_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMPdtPackSize with(nolock) ON TCNMPdtUnit_L.FTPunCode = TCNMPdtPackSize.FTPunCode");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMPdtPackSize.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtPackSize.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raPdtUnitLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtUnitLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raPdtUnitLng = oConn.Query<cmlResInfoPdtUnitLng>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //Image Pdt
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMImgPdt.FNImgID AS rnImgID, TCNMImgPdt.FTImgRefID AS rtImgRefID, TCNMImgPdt.FNImgSeq AS rnImgSeq,");
                        oSql.AppendLine("TCNMImgPdt.FTImgTable AS rtImgTable, TCNMImgPdt.FTImgKey AS rtImgKey, TCNMImgPdt.FTImgObj AS rtImgObj,");
                        oSql.AppendLine("TCNMImgPdt.FDLastUpdOn AS rdLastUpdOn, TCNMImgPdt.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMImgPdt.FTLastUpdBy AS rtLastUpdBy, TCNMImgPdt.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMImgPdt with(nolock)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt with(nolock) ON TCNMImgPdt.FTImgRefID = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMImgPdt.FTImgRefID = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  PDTWah.FTWahCode = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        //if (ptShpCode != null) oSql.AppendLine("AND TCNMPdt.FTShpCode = '" + ptShpCode + "'");  //*Em 62-04-03
                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oPdtItemDwn.raImgPdt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgPdt>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oPdtItemDwn.raImgPdt = oConn.Query<cmlResInfoImgPdt>(oSql.ToString(), nCmdTme).ToList();    //*Em 62-06-09

                        //*Em 62-08-17
                        //PdtAge
                        oSql.Clear();
                        oSql.AppendLine("SELECT DISTINCT TCNMPdtAge.FTPdtCode AS rtPdtCode, TCNMPdtAge.FCPdtAge AS rcPdtAge, TCNMPdtAge.FCPdtMaxDegree AS rcPdtMaxDegree,");
                        oSql.AppendLine("TCNMPdtAge.FCPdtMinDegree AS rcPdtMinDegree, TCNMPdtAge.FTPdtIngredients AS rtPdtIngredients, TCNMPdtAge.FTPdtHowToUse AS rtPdtHowToUse,");
                        oSql.AppendLine("TCNMPdtAge.FTPdtWarning AS rtPdtWarning, TCNMPdtAge.FDPdtMfg AS rdPdtMfg, TCNMPdtAge.FDPdtExp AS rdPdtExp,");
                        oSql.AppendLine("TCNMPdtAge.FTPdtPerVolumn AS rtPdtPerVolumn, TCNMPdtAge.FCPdtPerCalories AS rcPdtPerCalories, TCNMPdtAge.FCPdtCookTime AS rcPdtCookTime,");
                        oSql.AppendLine("TCNMPdtAge.FCPdtCookHeat AS rcPdtCookHeat");
                        oSql.AppendLine("FROM TCNMPdtAge WITH(NOLOCK)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdtAge.FTPdtCode = TCNMPdt.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtAge.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'') = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19

                        oPdtItemDwn.raTCNMPdtAge = oConn.Query<cmlTCNMPdtAge>(oSql.ToString(), nCmdTme).ToList();
                        //++++++++++++

                        //*Em 62-09-09
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtSpcBch.FTPdtCode AS rtPdtCode,TCNMPdtSpcBch.FTBchCode AS rtBchCode,TCNMPdtSpcBch.FTMerCode AS rtMerCode,");
                        oSql.AppendLine("TCNMPdtSpcBch.FTMgpCode AS rtMgpCode,TCNMPdtSpcBch.FCPdtMin AS rcPdtMin,TCNMPdtSpcBch.FTShpCode AS rtShpCode,");
                        oSql.AppendLine("TCNMPdtSpcBch.FTPdtRmk AS rtPdtRmk");
                        oSql.AppendLine("FROM TCNMPdtSpcBch WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdt.FTPdtCode = TCNMPdtSpcBch.FTPdtCode");
                        oSql.AppendLine("WHERE (ISNULL(TCNMPdtSpcBch.FTBchCode,'') = '' OR ISNULL(TCNMPdtSpcBch.FTBchCode,'') = ISNULL((SELECT TOP 1 FTBchCode FROM TCNMComp WITH(NOLOCK)),''))");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", poData.pdDate) + "'");
                        oPdtItemDwn.raTCNMPdtSpcBch = oConn.Query<cmlResTCNMPdtSpcBch>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                        //*Arm 63-01-17 : Query TCNMPdtDrug
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtDrug.FTPdtCode AS rtPdtCode, TCNMPdtDrug.FCPdgAge AS rcPdgAge, TCNMPdtDrug.FDPdgCreate AS rdPdgCreate, TCNMPdtDrug.FDPdgExpired AS rdPdgExpired, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgHowtoUse AS rtPdgHowtoUse, TCNMPdtDrug.FTPdgActIngredient AS rtPdgActIngredient, TCNMPdtDrug.FTPdgProperties AS rtPdgProperties, TCNMPdtDrug.FTPdgCtd AS rtPdgCtd, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgWarn AS rtPdgWarn, TCNMPdtDrug.FTPdgStopUse AS rtPdgStopUse, TCNMPdtDrug.FCPdgDoseSchedule AS rcPdgDoseSchedule, TCNMPdtDrug.FCPdgMaxIntake AS rcPdgMaxIntake, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgBrandName AS rtPdgBrandName, TCNMPdtDrug.FTPdgGenericName AS rtPdgGenericName, TCNMPdtDrug.FTPdgCategory AS rtPdgCategory, TCNMPdtDrug.FTPdgType AS rtPdgType, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgRegNo AS rtPdgRegNo, TCNMPdtDrug.FTPdgStorage AS rtPdgStorage, TCNMPdtDrug.FTPunCode AS rtPunCode, TCNMPdtDrug.FTPdgForm AS rtPdgForm, ");
                        oSql.AppendLine("TCNMPdtDrug.FTPdgCtrlRole AS rtPdgCtrlRole, TCNMPdtDrug.FTPdgManufacturer AS rtPdgManufacturer, ");
                        oSql.AppendLine("TCNMPdtDrug.FDLastUpdOn AS rdLastUpdOn, TCNMPdtDrug.FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("TCNMPdtDrug.FDCreateOn AS rdCreateOn, TCNMPdtDrug.FTCreateBy AS rtCreateBy ");
                        oSql.AppendLine("FROM TCNMPdtDrug WITH(NOLOCK)");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdt.FTPdtCode = TCNMPdtDrug.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16 
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT with(nolock) ON TCNMPdtDrug.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcWah PDTWah  with(nolock) ON PDT.FTPdtCode = PDTWah.FTPdtCode AND PDTBch.FTBchCode = PDTWah.FTBchCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(PDTWah.FTWahCode,'') = '' OR  ISNULL(PDTWah.FTWahCode,'')  = '" + ptWahCode + "')");  //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");  //*Arm 63-09-19 Comment Code
                        //++++++++++++++
                        oSql.AppendLine(tCondition); //*Arm 63-09-19
                        oPdtItemDwn.raTCNMPdtDrug = oConn.Query<cmlResInfoPdtDrug>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                        //*Arm 63-01-17 : Query TCNMPdtSpcWah
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMPdtSpcWah.FTPdtCode AS rtPdtCode, TCNMPdtSpcWah.FTBchCode AS rtBchCode, TCNMPdtSpcWah.FTWahCode AS rtWahCode,");
                        oSql.AppendLine("TCNMPdtSpcWah.FCSpwQtyMin AS rcSpwQtyMin, TCNMPdtSpcWah.FCSpwQtyMax AS rcSpwQtyMax, TCNMPdtSpcWah.FTSpwRmk AS rtSpwRmk");
                        oSql.AppendLine("FROM TCNMPdtSpcWah WITH(NOLOCK) ");
                        //*Arm 63-06-16 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMPdt WITH(NOLOCK) ON TCNMPdt.FTPdtCode = TCNMPdtSpcWah.FTPdtCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMPdt.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Arm 63-06-16
                        oSql.AppendLine("INNER JOIN TCNMPdt PDT WITH(NOLOCK) ON TCNMPdtSpcWah.FTPdtCode = PDT.FTPdtCode");
                        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PDTBch with(nolock) ON PDT.FTPdtCode = PDTBch.FTPdtCode ");
                        //oSql.AppendLine("WHERE (ISNULL(PDTBch.FTBchCode,'') = '' OR ISNULL(PDTBch.FTBchCode,'') = '" + ptBchCode + "')  AND ( ISNULL(TCNMPdtSpcWah.FTWahCode,'') = '' OR  ISNULL(TCNMPdtSpcWah.FTWahCode,'') = '" + ptWahCode + "')"); //*Arm 63-09-19 Comment Code
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10), PDT.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-19 Comment Code
                        oSql.AppendLine(tCondition); //*Arm 63-09-19
                        oPdtItemDwn.raTCNMPdtSpcWah = oConn.Query<cmlResInfoPdtSpcWah>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oPdtItemDwn;
                // เก็บ KeyApi ลง Cache
                //oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtItemDwn>();
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