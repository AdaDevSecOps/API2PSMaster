using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.RedeemPoint;
using System;
using System.Collections.Generic;
using System.Data;
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
    ///     Redeem point Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/RedeemPoint")]
    public class cRedeemPointController : ControllerBase
    {
        /// <summary>
        ///     Download Redeem Point information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResRedeemDwn> GET_PDToDownloadRedeem(DateTime pdDate, string ptBchCode)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            DataTable odtTmp;       //*Arm 63-09-03
            cmlResItem<cmlResRedeemDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResRedeemDwn oRedeemDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResRedeemDwn>();
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

                tKeyCache = "RedeemPoint" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResRedeemDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //string tSqlX = ""; //*Arm 63-09-03 เก้บ Query test

                //*Arm 63-09-03 หาเอกสารที่ไม่เอา
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRdhDocNo FROM TARTRedeemHDBch WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTRdhStaType = '2' AND FTRdhBchTo = '"+ ptBchCode +"'");
                oSql.AppendLine("UNION");
                oSql.AppendLine("SELECT FTRdhDocNo FROM TARTRedeemHDBch WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTRdhStaType = '1'");
                oSql.AppendLine("AND FTRdhDocNo NOT IN(SELECT FTRdhDocNo FROM TARTRedeemHDBch WITH(NOLOCK) WHERE FTRdhStaType = '1' AND FTRdhBchTo = '"+ ptBchCode +"')");
                odtTmp = new DataTable();
                odtTmp = new cDatabase().C_DAToSqlQuery(oSql.ToString());
                //+++++++++++++

                //tSqlX += oSql.ToString()+Environment.NewLine; //*Arm 63-09-03 

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTRdhDocNo AS rtRdhDocNo, FDRdhDocDate AS rdRdhDocDate, FTRdhDocType AS rtRdhDocType, FTRdhCalType AS rtRdhCalType,");
                //oSql.AppendLine("FDRdhDStart AS rdRdhDStart, FDRdhDStop AS rdRdhDStop, FDRdhTStart AS rdRdhTStart, FDRdhTStop AS rdRdhTStop, FTRdhStaClosed AS rtRdhStaClosed,");
                //oSql.AppendLine("FTRdhStaDoc AS rtRdhStaDoc, FTRdhStaApv AS rtRdhStaApv, FTRdhStaPrcDoc AS rtRdhStaPrcDoc, FNRdhStaDocAct AS rnRdhStaDocAct, FTUsrCode AS rtUsrCode,");
                //oSql.AppendLine("FTRdhUsrApv AS rtRdhUsrApv, FTRdhStaOnTopPmt AS rtRdhStaOnTopPmt, FNRdhLimitQty AS rnRdhLimitQty,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy ");
                //oSql.AppendLine("FROM TARTRedeemHD with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //oSql.AppendLine("AND CONVERT(VARCHAR(10), FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                //if (odtTmp != null && odtTmp.Rows.Count > 0)
                //{
                //    oSql.AppendLine("AND FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                //}

                //aoResult.roItem = new cmlResRedeemDwn();
                //oRedeemDwn = new cmlResRedeemDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oRedeemDwn.raRedeemHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRedeemHD>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oRedeemDwn.raRedeemHD.Count > 0)
                //        {
                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TARTRedeemHD_L.FTBchCode AS rtBchCode, TARTRedeemHD_L.FTRdhDocNo AS rtRdhDocNo, TARTRedeemHD_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TARTRedeemHD_L.FTRdhName AS rtRdhName, TARTRedeemHD_L.FTRdhNameSlip AS rtRdhNameSlip, TARTRedeemHD_L.FTRdhRmk AS rtRdhRmk ");
                //            oSql.AppendLine("FROM TARTRedeemHD_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemHD_L.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemHD_L.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");    //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND TARTRedeemHD_L.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRedeemDwn.raRedeemHDLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRedeemHDLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                //            //TARTRedeemHDBch
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TARTRedeemHDBch.FTBchCode AS rtBchCode, TARTRedeemHDBch.FTRdhDocNo AS rtRdhDocNo, TARTRedeemHDBch.FTRdhBchTo AS rtRdhBchTo,");
                //            oSql.AppendLine("TARTRedeemHDBch.FTRdhMerTo AS rtRdhMerTo, TARTRedeemHDBch.FTRdhShpTo AS rtRdhShpTo, TARTRedeemHDBch.FTRdhStaType AS rtRdhStaType ");
                //            oSql.AppendLine("FROM TARTRedeemHDBch with(nolock)");
                //            oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemHDBch.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemHDBch.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");    //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND TARTRedeemHDBch.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRedeemDwn.raRedeemHDBch = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRedeemHDBch>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03
                //            //TARTRedeemHDCstPri
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TARTRedeemHDCstPri.FTBchCode AS rtBchCode, TARTRedeemHDCstPri.FTRdhDocNo AS rtRdhDocNo,");
                //            oSql.AppendLine("TARTRedeemHDCstPri.FTPplCode AS rtPplCode, TARTRedeemHDCstPri.FTRdhStaType AS rtRdhStaType ");
                //            oSql.AppendLine("FROM TARTRedeemHDCstPri with(nolock)");
                //            oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemHDCstPri.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemHDCstPri.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND TARTRedeemHDCstPri.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRedeemDwn.raRedeemHDCstPri = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRedeemHDCstPri>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                //            //TARTRedeemDT
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TARTRedeemDT.FTBchCode AS rtBchCode, TARTRedeemDT.FTRdhDocNo AS rtRdhDocNo, TARTRedeemDT.FNRddSeq AS rnRddSeq, TARTRedeemDT.FTRddStaType AS rtRddStaType,");
                //            oSql.AppendLine("TARTRedeemDT.FTRddGrpName AS rtRddGrpName, TARTRedeemDT.FTPdtCode AS rtPdtCode, TARTRedeemDT.FTPunCode AS rtPunCode, TARTRedeemDT.FTRddBarCode AS rtRddBarCode");
                //            oSql.AppendLine("FROM TARTRedeemDT with(nolock)");
                //            oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemDT.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemDT.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND TARTRedeemDT.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRedeemDwn.raRedeemDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRedeemDT>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                //            //TARTRedeemCD
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TARTRedeemCD.FTBchCode AS rtBchCode, TARTRedeemCD.FTRdhDocNo AS rtRdhDocNo, TARTRedeemCD.FNRdcSeq AS rnRdcSeq, TARTRedeemCD.FTRddGrpName AS rtRddGrpName,");
                //            oSql.AppendLine("TARTRedeemCD.FTRdcRefCode AS rtRdcRefCode, TARTRedeemCD.FCRdcUsePoint AS rcRdcUsePoint, TARTRedeemCD.FCRdcUseMny AS rcRdcUseMny, TARTRedeemCD.FCRdcMinTotBill AS rcRdcMinTotBill");
                //            oSql.AppendLine("FROM TARTRedeemCD with(nolock)");
                //            oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemCD.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemCD.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND TARTRedeemCD.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                //            }
                            
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oRedeemDwn.raRedeemCD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoRedeemCD>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString();  //*Arm 63-09-03 Get Query Test.. 
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
                aoResult.roItem = new cmlResRedeemDwn();
                oRedeemDwn = new cmlResRedeemDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTRdhDocNo AS rtRdhDocNo, FDRdhDocDate AS rdRdhDocDate, FTRdhDocType AS rtRdhDocType, FTRdhCalType AS rtRdhCalType,");
                oSql.AppendLine("FDRdhDStart AS rdRdhDStart, FDRdhDStop AS rdRdhDStop, FDRdhTStart AS rdRdhTStart, FDRdhTStop AS rdRdhTStop, FTRdhStaClosed AS rtRdhStaClosed,");
                oSql.AppendLine("FTRdhStaDoc AS rtRdhStaDoc, FTRdhStaApv AS rtRdhStaApv, FTRdhStaPrcDoc AS rtRdhStaPrcDoc, FNRdhStaDocAct AS rnRdhStaDocAct, FTUsrCode AS rtUsrCode,");
                oSql.AppendLine("FTRdhUsrApv AS rtRdhUsrApv, FTRdhStaOnTopPmt AS rtRdhStaOnTopPmt, FNRdhLimitQty AS rnRdhLimitQty,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, FTCreateBy AS rtCreateBy ");
                oSql.AppendLine("FROM TARTRedeemHD with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    oSql.AppendLine("AND FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                }
                oRedeemDwn.raRedeemHD = oDB.C_DATaSqlQuery<cmlResInfoRedeemHD>(oSql.ToString());
                if (oRedeemDwn.raRedeemHD.Count > 0)
                {
                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TARTRedeemHD_L.FTBchCode AS rtBchCode, TARTRedeemHD_L.FTRdhDocNo AS rtRdhDocNo, TARTRedeemHD_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TARTRedeemHD_L.FTRdhName AS rtRdhName, TARTRedeemHD_L.FTRdhNameSlip AS rtRdhNameSlip, TARTRedeemHD_L.FTRdhRmk AS rtRdhRmk ");
                    oSql.AppendLine("FROM TARTRedeemHD_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemHD_L.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemHD_L.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");    //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND TARTRedeemHD_L.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                    }
                    oRedeemDwn.raRedeemHDLng = oDB.C_DATaSqlQuery<cmlResInfoRedeemHDLng>(oSql.ToString());
                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                    //TARTRedeemHDBch
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TARTRedeemHDBch.FTBchCode AS rtBchCode, TARTRedeemHDBch.FTRdhDocNo AS rtRdhDocNo, TARTRedeemHDBch.FTRdhBchTo AS rtRdhBchTo,");
                    oSql.AppendLine("TARTRedeemHDBch.FTRdhMerTo AS rtRdhMerTo, TARTRedeemHDBch.FTRdhShpTo AS rtRdhShpTo, TARTRedeemHDBch.FTRdhStaType AS rtRdhStaType ");
                    oSql.AppendLine("FROM TARTRedeemHDBch with(nolock)");
                    oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemHDBch.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemHDBch.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");    //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND TARTRedeemHDBch.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                    }
                    oRedeemDwn.raRedeemHDBch = oDB.C_DATaSqlQuery<cmlResInfoRedeemHDBch>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03
                    //TARTRedeemHDCstPri
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TARTRedeemHDCstPri.FTBchCode AS rtBchCode, TARTRedeemHDCstPri.FTRdhDocNo AS rtRdhDocNo,");
                    oSql.AppendLine("TARTRedeemHDCstPri.FTPplCode AS rtPplCode, TARTRedeemHDCstPri.FTRdhStaType AS rtRdhStaType ");
                    oSql.AppendLine("FROM TARTRedeemHDCstPri with(nolock)");
                    oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemHDCstPri.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemHDCstPri.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND TARTRedeemHDCstPri.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                    }
                    oRedeemDwn.raRedeemHDCstPri = oDB.C_DATaSqlQuery<cmlResInfoRedeemHDCstPri>(oSql.ToString());
                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                    //TARTRedeemDT
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TARTRedeemDT.FTBchCode AS rtBchCode, TARTRedeemDT.FTRdhDocNo AS rtRdhDocNo, TARTRedeemDT.FNRddSeq AS rnRddSeq, TARTRedeemDT.FTRddStaType AS rtRddStaType,");
                    oSql.AppendLine("TARTRedeemDT.FTRddGrpName AS rtRddGrpName, TARTRedeemDT.FTPdtCode AS rtPdtCode, TARTRedeemDT.FTPunCode AS rtPunCode, TARTRedeemDT.FTRddBarCode AS rtRddBarCode");
                    oSql.AppendLine("FROM TARTRedeemDT with(nolock)");
                    oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemDT.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemDT.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND TARTRedeemDT.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                    }
                    oRedeemDwn.raRedeemDT = oDB.C_DATaSqlQuery<cmlResInfoRedeemDT>(oSql.ToString());
                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03

                    //TARTRedeemCD
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TARTRedeemCD.FTBchCode AS rtBchCode, TARTRedeemCD.FTRdhDocNo AS rtRdhDocNo, TARTRedeemCD.FNRdcSeq AS rnRdcSeq, TARTRedeemCD.FTRddGrpName AS rtRddGrpName,");
                    oSql.AppendLine("TARTRedeemCD.FTRdcRefCode AS rtRdcRefCode, TARTRedeemCD.FCRdcUsePoint AS rcRdcUsePoint, TARTRedeemCD.FCRdcUseMny AS rcRdcUseMny, TARTRedeemCD.FCRdcMinTotBill AS rcRdcMinTotBill");
                    oSql.AppendLine("FROM TARTRedeemCD with(nolock)");
                    oSql.AppendLine("INNER JOIN TARTRedeemHD with(nolock) ON TARTRedeemCD.FTBchCode = TARTRedeemHD.FTBchCode AND TARTRedeemCD.FTRdhDocNo = TARTRedeemHD.FTRdhDocNo ");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TARTRedeemHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), TARTRedeemHD.FDRdhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");     //*Arm 63-09-03 กรองไม่เอาเอกสารที่หมดอายุ
                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND TARTRedeemCD.FTRdhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTRdhDocNo"))).ToArray()) + ")"); //*Arm 63-09-03
                    }
                    oRedeemDwn.raRedeemCD = oDB.C_DATaSqlQuery<cmlResInfoRedeemCD>(oSql.ToString());

                    //tSqlX += oSql.ToString();  //*Arm 63-09-03 Get Query Test.. 
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oRedeemDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResRedeemDwn>();
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