using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Response.Shop;
using API2PSMaster.Models.WebService.Request.Shop;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Shop Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Shop")]
    public class cShopController : ControllerBase
    {
        /// <summary>
        ///     Download shop information.
        /// </summary>
        /// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResShopDwn> GET_PDToDownloadShop(DateTime pdDate, string ptBchCode)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResShopDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResShopDwn oShopDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResShopDwn>();
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

                tKeyCache = "Shop" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResShopDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTShpCode AS rtShpCode, FTWahCode AS rtWahCode, FTMerCode AS rtMerCode, FTShpStaShwPrice AS rtShpStaShwPrice, FTPplCode AS rtPplCode,"); //*Arm 63-06-16 -เพิ่ม FTPplCode ตามโครงสร้าง Database SKC
                oSql.AppendLine("FTShpType AS rtShpType, FTShpRegNo AS rtShpRegNo, FTShpRefID AS rtShpRefID, FDShpStart AS rdShpStart, FDShpStop AS rdShpStop,");
                oSql.AppendLine("FDShpSaleStart AS rdShpSaleStart, FDShpSaleStop AS rdShpSaleStop, FTShpStaActive AS rtShpStaActive, FTShpStaClose AS rtShpStaClose,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMShop with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '"+ ptBchCode +"'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                aoResult.roItem = new cmlResShopDwn();
                oShopDwn = new cmlResShopDwn();
                
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    oShopDwn.raShop = oConn.Query<cmlResInfoShop>(oSql.ToString(), nCmdTme).ToList();
                    if (oShopDwn.raShop.Count > 0)
                    {
                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNMShop_L.FTBchCode AS rtBchCode, TCNMShop_L.FTShpCode AS rtShpCode, TCNMShop_L.FNLngID AS rnLngID,");
                        oSql.AppendLine("TCNMShop_L.FTShpName AS rtShpName, TCNMShop_L.FTShpRmk AS rtShpRmk");
                        oSql.AppendLine("FROM TCNMShop_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMShop with(nolock) ON TCNMShop_L.FTBchCode = TCNMShop.FTBchCode AND TCNMShop_L.FTShpCode = TCNMShop.FTShpCode ");
                        oSql.AppendLine("WHERE TCNMShop_L.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMShop.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        oShopDwn.raShopLng = oConn.Query<cmlResInfoShopLng>(oSql.ToString(), nCmdTme).ToList();

                        //Address
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
                        oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
                        oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
                        oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
                        oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
                        oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
                        oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMShop with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMShop.FTShpCode AND TCNMAddress_L.FTAddGrpType = '4'");
                        oSql.AppendLine("WHERE TCNMShop.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMShop.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        oShopDwn.raAddrLng = oConn.Query<cmlResInfoAddrLng>(oSql.ToString(), nCmdTme).ToList();

                        //Image
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TCNMImgObj.FNImgID AS rnImgID, TCNMImgObj.FTImgRefID AS rtImgRefID, TCNMImgObj.FNImgSeq AS rnImgSeq,");
                        oSql.AppendLine("TCNMImgObj.FTImgTable AS rtImgTable, TCNMImgObj.FTImgKey AS rtImgKey, TCNMImgObj.FTImgObj AS rtImgObj,");
                        oSql.AppendLine("TCNMImgObj.FDLastUpdOn AS rdLastUpdOn, TCNMImgObj.FDCreateOn AS rdCreateOn,");
                        oSql.AppendLine("TCNMImgObj.FTLastUpdBy AS rtLastUpdBy, TCNMImgObj.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMImgObj with(nolock)");
                        oSql.AppendLine("INNER JOIN TCNMShop with(nolock) ON TCNMImgObj.FTImgRefID = TCNMShop.FTShpCode AND TCNMImgObj.FTImgTable = 'TCNMShop'");
                        oSql.AppendLine("WHERE TCNMShop.FTBchCode = '"+ ptBchCode +"'");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMShop.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        oShopDwn.raImage = oConn.Query<cmlResInfoImgObj>(oSql.ToString(), nCmdTme).ToList();
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }

                aoResult.roItem = oShopDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResShopDwn>();
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

        [Route("Insert")]
        [HttpPost]
        public cmlResItem<cmlResShopDwn> POST_PDToInsShop(cmlReqShop poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;

            StringBuilder oSql;
            StringBuilder oSql_M;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            cmlResItem<cmlResShopDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResShopDwn>();
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

                            oSql = new StringBuilder();
                            oSql.Clear();
                            oSql.AppendLine(" INSERT INTO TCNMShop ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" (FTBchCode, FTShpCode, FTWahCode, FTShpType,");
                            oSql.AppendLine(" FTShpRegNo, FTShpRefID, FDShpStart, FDShpStop,");
                            oSql.AppendLine(" FDShpSaleStart, FDShpSaleStop, FTShpStaActive, FTShpStaClose,");
                            oSql.AppendLine(" FDDateUpd, FTTimeUpd,");
                            oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns ");
                            oSql.AppendLine(" )");
                            oSql.AppendLine(" VALUES( '" + poparam.ptBchCode + "',");
                            oSql.AppendLine(" '" + poparam.ptShpCode + "',");
                            oSql.AppendLine("'" + poparam.ptWahCode + "',");
                            oSql.AppendLine("'" + poparam.ptShpType + "',");
                            oSql.AppendLine("'" + poparam.ptShpRegNo + "',");
                            oSql.AppendLine(" '" + poparam.ptShpRefID + "' ,");
                            oSql.AppendLine(" '" + string.Format("{0:yyyy-MM-dd}", poparam.pdShpStart) + "',");
                            oSql.AppendLine(" '" + string.Format("{0:yyyy-MM-dd}", poparam.pdShpSaleStop) + "',");
                            oSql.AppendLine("'" + string.Format("{0:yyyy-MM-dd}", poparam.pdShpSaleStart) + "',");
                            oSql.AppendLine(" '" + string.Format("{0:yyyy-MM-dd}", poparam.pdShpSaleStop) + "',");
                            oSql.AppendLine("'" + poparam.ptShpStaActive + "',");
                            oSql.AppendLine(" '" + poparam.ptShpStaClose + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "' ");
                            oSql.AppendLine(")");


                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" INSERT INTO TCNMShop_L");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" (FTBchCode, FTShpCode, FNLngID, FTShpName, FTShpRmk) ");
                            oSql_L.AppendLine(" VALUES");
                            oSql_L.AppendLine("('" + poparam.ptBchCode + "', ");
                            oSql_L.AppendLine("'" + poparam.ptShpCode + "', ");
                            oSql_L.AppendLine(" '" + poparam.pnLngID + "',");
                            oSql_L.AppendLine(" '" + poparam.ptShpName + "',");
                            oSql_L.AppendLine(" '" + poparam.ptShpRmk + "'");
                            oSql_L.AppendLine(")");

                            oSql_Img = new StringBuilder();
                            for (var nList = 0; nList < poparam.roImgUpl.Count; nList++)
                            {

                                if (poparam.roImgUpl[nList].ptImgobj != "")
                                {
                                    oFunc.SP_IMGbInsImgPath(poparam.ptShpCode, "Shop/", poparam.roImgUpl[nList].ptImgobj,
                                                      poparam.roImgUpl[nList].ptImgRefID, poparam.roImgUpl[nList].ptImgKey,
                                                      poparam.ptWhoUpd, "TCNMShop");
                                }

                            }

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql.ToString();
                            oCmd.ExecuteNonQuery();

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
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
                aoResult = new cmlResItem<cmlResShopDwn>();
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

        [Route("Update")]
        [HttpPost]
        public cmlResItem<cmlResShopDwn> POST_PDToUpdShop(cmlReqShop poparam)
        {

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_M;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResShopDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblCmp;
            DataTable oDbTblCmp_L;
            DataTable oDbTblImg;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResShopDwn>();
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
                oSql.AppendLine(" SELECT  FTBchCode, FTShpCode, FTWahCode, FTShpType,  ");
                oSql.AppendLine(" FTShpRegNo, FTShpRefID, FDShpStart, FDShpStop,  ");
                oSql.AppendLine(" FDShpSaleStart, FDShpSaleStop, FTShpStaActive, FTShpStaClose");
                oSql.AppendLine(" FROM TCNMShop WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTBchCode = '" + poparam.ptBchCode + "' AND FTShpCode = '" + poparam.ptShpCode + "'");
                oDbTblCmp = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1)   FTBchCode, FTShpCode, FNLngID, FTShpName, FTShpRmk");
                oSql.AppendLine(" FROM TCNMShop_L");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTBchCode = '" + poparam.ptBchCode + "' AND ");
                oSql.AppendLine(" FTShpCode = '" + poparam.ptShpCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblCmp_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblCmp != null && oDbTblCmp.Rows.Count > 0)
                {
                    if (oDbTblCmp_L != null && oDbTblCmp_L.Rows.Count > 0)
                    {
                        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                        {

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

                                    oSql_M = new StringBuilder();
                                    oSql_M.Clear();
                                    oSql_M.AppendLine(" UPDATE ");
                                    oSql_M.AppendLine(" TCNMShop ");
                                    oSql_M.AppendLine(" WITH(ROWLOCK)");
                                    oSql_M.AppendLine(" SET ");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptWahCode, "FTWahCode") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptShpType, "FTShpType") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptShpRegNo, "FTShpRegNo") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptShpRefID, "FTShpRefID") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdShpStart), "FDShpStart") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdShpStop), "FDShpStop") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdShpSaleStart), "FDShpSaleStart") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.pdShpSaleStop), "FDShpSaleStop") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptShpStaActive, "FTShpStaActive") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptShpStaClose, "FTShpStaClose") + "");
                                    oSql_M.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                                    oSql_M.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                                    oSql_M.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                                    oSql_M.AppendLine(" WHERE FTBchCode = '" + poparam.ptBchCode + "'");
                                    oSql_M.AppendLine(" AND FTShpCode = '" + poparam.ptShpCode + "'");

                                    oSql_L = new StringBuilder();
                                    oSql_L.Clear();
                                    oSql_L.AppendLine(" UPDATE ");
                                    oSql_L.AppendLine(" TCNMShop_L ");
                                    oSql_L.AppendLine(" WITH(ROWLOCK)");
                                    oSql_L.AppendLine(" SET");
                                    oSql_L.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptShpRmk, "FTShpRmk") + "");
                                    oSql_L.AppendLine(" FTShpName = '" + poparam.ptShpName + "'");
                                    oSql_L.AppendLine(" WHERE FTBchCode = '" + poparam.ptBchCode + "' ");
                                    oSql_L.AppendLine(" AND FTShpCode  = '" + poparam.ptShpCode +"'");
                                    oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");

                                    oSql_Img = new StringBuilder();

                                    for (var nList = 0; nList < poparam.roImgUpl.Count; nList++)
                                    {

                                        if (poparam.roImgUpl[nList].ptImgobj != "")
                                        {
                                            oFunc.SP_IMGbInsImgPath(poparam.ptShpCode, "Shop/", poparam.roImgUpl[nList].ptImgobj,
                                                              poparam.roImgUpl[nList].ptImgRefID, poparam.roImgUpl[nList].ptImgKey,
                                                              poparam.ptWhoUpd, "TCNMShop");
                                        }
                                    }

                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_M.ToString();
                                    oCmd.ExecuteNonQuery();

                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_L.ToString();
                                    int nSuccess = oCmd.ExecuteNonQuery();
                                    if (nSuccess == 0)
                                    {
                                        aoResult = new cmlResItem<cmlResShopDwn>();
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
                        aoResult = new cmlResItem<cmlResShopDwn>();
                        //aoResult = new cmlResPdtItemDwn();
                        aoResult.rtCode = "";
                        aoResult.rtDesc = "";
                        return aoResult;
                    }

                }
                else
                {
                    aoResult = new cmlResItem<cmlResShopDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }

            }
            catch(Exception oExn)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResShopDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;
                oSql_L = null;
                oSql_M = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }

        [Route("Delete")]
        [HttpPost]
        public cmlResItem<cmlResShopDwn> POST_PDToDelShop(cmlReqShopDel poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResShopDwn> aoResult;
            cDatabase oDatabase;
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

                aoResult = new cmlResItem<cmlResShopDwn>();
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
                    oSql.AppendLine(" FROM TCNMShop  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTBchCode = '" + poparam.ptBchCode + "')");
                    oSql.AppendLine(" AND FTShpCode = '" + poparam.ptShpCode + "'");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMShop_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTBchCode = '" + poparam.ptBchCode + "') ");
                    oSql_L.AppendLine(" AND (FTShpCode = '" + poparam.ptShpCode + "')");
                    oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");

                    oSql_Img = new StringBuilder();
                    oSql_Img.Clear();
                    oSql_Img.AppendLine(" DELETE ");
                    oSql_Img.AppendLine(" FROM TCNMImgObj WITH(ROWLOCK) ");
                    oSql_Img.AppendLine("  WHERE (FTImgRefID = '" + poparam.ptShpCode + "') ");

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
                            oCmd.CommandText = oSql_Img.ToString();
                            oCmd.ExecuteNonQuery();


                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            int nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                aoResult = new cmlResItem<cmlResShopDwn>();
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
                aoResult = new cmlResItem<cmlResShopDwn>();
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