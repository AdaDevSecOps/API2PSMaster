using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.SalePerson;
using API2PSMaster.Models.WebService.Request.SalePerson;
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
using System.Data.Entity.Core;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Sale person Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/SalePerson")]
    public class cSalePersonController : ControllerBase
    {

        /// <summary>
        ///     Download sale person information.
        /// </summary>
        /// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResSpnDwn> GET_PDToDownloadSalePerson(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSpnDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResSpnDwn oSpnDwn;
            cCacheFunc oCacheFunc;
            int nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSpnDwn>();
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

                tKeyCache = "SalePerson" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResSpnDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTSpnCode AS rtSpnCode, FTSpnTel AS rtSpnTel, ISNULL(FCSpnSleAmt,0) AS rcSpnSleAmt, FTSpnEmail AS rtSpnEmail,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMSpn with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResSpnDwn();
                //oSpnDwn = new cmlResSpnDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oSpnDwn.raSpn = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSpn>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oSpnDwn.raSpn.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMSpn_L.FTSpnCode AS rtSpnCode, TCNMSpn_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMSpn_L.FTSpnName AS rtSpnName, TCNMSpn_L.FTSpnRmk AS rtSpnRmk");
                //            oSql.AppendLine("FROM TCNMSpn_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSpn with(nolock) ON TCNMSpn_L.FTSpnCode = TCNMSpn.FTSpnCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSpn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSpnDwn.raSpnLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSpnLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Address
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
                //            oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
                //            oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
                //            //oSql.AppendLine("TCNMAddress_L.FTAreCode AS rtAreCode, TCNMAddress_L.FTZneCode AS rtZneCode, TCNMAddress_L.FTAddVersion AS rtAddVersion,");
                //            oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,"); //*Em 62-04-02  ปรับโครงสร้างใหม่
                //            oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
                //            oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
                //            oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
                //            oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
                //            oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
                //            oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSpn with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMSpn.FTSpnCode AND TCNMAddress_L.FTAddGrpType = '3'");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSpn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSpnDwn.raAddrLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoAddrLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Group
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNTSpnGroup.FTSpnCode AS rtSpnCode, TCNTSpnGroup.FTBchCode AS rtBchCode, TCNTSpnGroup.FTSpnStaShop AS rtSpnStaShop,");
                //            oSql.AppendLine("TCNTSpnGroup.FTShpCode AS rtShpCode, TCNTSpnGroup.FDSpnStart AS rdSpnStart, TCNTSpnGroup.FDSpnStop AS rdSpnStop");
                //            oSql.AppendLine("FROM TCNTSpnGroup with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMSpn with(nolock) ON TCNTSpnGroup.FTSpnCode = TCNMSpn.FTSpnCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSpn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oSpnDwn.raSpnGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSpnGrp>(oDR).ToList();
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
                aoResult.roItem = new cmlResSpnDwn();
                oSpnDwn = new cmlResSpnDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSpnCode AS rtSpnCode, FTSpnTel AS rtSpnTel, ISNULL(FCSpnSleAmt,0) AS rcSpnSleAmt, FTSpnEmail AS rtSpnEmail,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMSpn with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oSpnDwn.raSpn = oDB.C_DATaSqlQuery<cmlResInfoSpn>(oSql.ToString());
                if (oSpnDwn.raSpn.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMSpn_L.FTSpnCode AS rtSpnCode, TCNMSpn_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMSpn_L.FTSpnName AS rtSpnName, TCNMSpn_L.FTSpnRmk AS rtSpnRmk");
                    oSql.AppendLine("FROM TCNMSpn_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSpn with(nolock) ON TCNMSpn_L.FTSpnCode = TCNMSpn.FTSpnCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSpn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSpnDwn.raSpnLng = oDB.C_DATaSqlQuery<cmlResInfoSpnLng>(oSql.ToString());

                    //Address
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
                    oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
                    oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
                    //oSql.AppendLine("TCNMAddress_L.FTAreCode AS rtAreCode, TCNMAddress_L.FTZneCode AS rtZneCode, TCNMAddress_L.FTAddVersion AS rtAddVersion,");
                    oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,"); //*Em 62-04-02  ปรับโครงสร้างใหม่
                    oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
                    oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
                    oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
                    oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
                    oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
                    oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSpn with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMSpn.FTSpnCode AND TCNMAddress_L.FTAddGrpType = '3'");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSpn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSpnDwn.raAddrLng = oDB.C_DATaSqlQuery<cmlResInfoAddrLng>(oSql.ToString());

                    //Group
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNTSpnGroup.FTSpnCode AS rtSpnCode, TCNTSpnGroup.FTBchCode AS rtBchCode, TCNTSpnGroup.FTSpnStaShop AS rtSpnStaShop,");
                    oSql.AppendLine("TCNTSpnGroup.FTShpCode AS rtShpCode, TCNTSpnGroup.FDSpnStart AS rdSpnStart, TCNTSpnGroup.FDSpnStop AS rdSpnStop");
                    oSql.AppendLine("FROM TCNTSpnGroup with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMSpn with(nolock) ON TCNTSpnGroup.FTSpnCode = TCNMSpn.FTSpnCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMSpn.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oSpnDwn.raSpnGrp = oDB.C_DATaSqlQuery<cmlResInfoSpnGrp>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oSpnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSpnDwn>();
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
        /// Insert SalePerson
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Insert")]
        [HttpPost]
        public cmlResItem<cmlResSpnDwn> POST_PDToInsSalePerson(cmlReqSalePerson poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;

            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Addr;
            StringBuilder oSql_Grp;
            cmlResItem<cmlResSpnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResSpnDwn>();
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
                            oSql.AppendLine(" INSERT INTO TCNMSpn ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" ( FTSpnCode, FTSpnTel, FCSpnSleAmt, FTSpnEmail, ");
                            oSql.AppendLine(" FDDateUpd, FTTimeUpd,");
                            oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns ");
                            oSql.AppendLine(" )");
                            oSql.AppendLine(" VALUES( '" + poparam.ptSpnCode + "',");
                            oSql.AppendLine(" '" + poparam.ptSpnTel + "',");
                            oSql.AppendLine("'" + poparam.pnSpnSleAmt + "',");
                            oSql.AppendLine("'" + poparam.ptSpnEmail + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "' ");
                            oSql.AppendLine(")");

                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" INSERT INTO TCNMSpn_L");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" (FTSpnCode, FNLngID, FTSpnName, FTSpnRmk) ");
                            oSql_L.AppendLine(" VALUES");
                            oSql_L.AppendLine("('" + poparam.ptSpnCode + "', ");
                            oSql_L.AppendLine("'" + poparam.pnLngID + "', ");
                            oSql_L.AppendLine(" '" + poparam.ptSpnName + "',");
                            oSql_L.AppendLine(" '" + poparam.ptSpnRmk + "'");
                            oSql_L.AppendLine(")");

                            oSql_Grp = new StringBuilder();
                            oSql_Grp.Clear();
                            oSql_Grp.AppendLine(" INSERT ");
                            oSql_Grp.AppendLine(" INTO ");
                            oSql_Grp.AppendLine(" WITH(ROWLOCK)");
                            oSql_Grp.AppendLine(" TCNTSpnGroup (FTSpnCode, FTBchCode, FTSpnStaShop, FTShpCode, FDSpnStart, FDSpnStop)");
                            oSql_Grp.AppendLine(" VALUES");
                            oSql_Grp.AppendLine("(");
                            oSql_Grp.AppendLine(" '" + poparam.ptSpnCode +"',");
                            oSql_Grp.AppendLine(" '" + poparam.poPersonGrp.ptBchCode + "',");
                            oSql_Grp.AppendLine(" '" + poparam.poPersonGrp.ptSpnStaShop + "',");
                            oSql_Grp.AppendLine(" '" + poparam.poPersonGrp.ptShpCode + "',");
                            oSql_Grp.AppendLine(" '" + poparam.poPersonGrp.pdSpnStart + "',");
                            oSql_Grp.AppendLine(" '" + poparam.poPersonGrp.pdSpnStop + "'");
                            oSql_Grp.AppendLine(")");

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
                aoResult = new cmlResItem<cmlResSpnDwn>();
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
        /// Update Sale Person
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public cmlResItem<cmlResSpnDwn> POST_PDToUptSalePerson(cmlReqSalePerson poparam)
        {

            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_M;
            StringBuilder oSql_L;
            StringBuilder oSql_Grp;
            StringBuilder oSql_Addr;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResSpnDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            DataTable oDbTblSp;
            DataTable oDbTblSp_L;
            DataTable oDbTblGrp;

            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResSpnDwn>();
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
                oSql.Clear();
                oSql.AppendLine("SELECT FTSpnCode, FTSpnTel, FCSpnSleAmt, FTSpnEmail  ");
                oSql.AppendLine("FROM TCNMSpn WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSpnCode = '" + poparam.ptSpnCode + "'");
                oDbTblSp = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine(" SELECT TOP(1)   FTSpnCode, FNLngID, FTSpnName, FTSpnRmk");
                oSql.AppendLine(" FROM TCNMSpn_L");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTSpnCode = '" + poparam.ptSpnCode + "' AND ");
                oSql.AppendLine(" FNLngID = '" + poparam.pnLngID + "'");
                oDbTblSp_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql.Clear();
                oSql.AppendLine(" SELECT FTSpnCode, FTBchCode, FTSpnStaShop, FTShpCode, FDSpnStart, FDSpnStop");
                oSql.AppendLine(" FROM ");
                oSql.AppendLine(" TCNTSpnGroup");
                oSql.AppendLine(" WHERE FTSpnCode = '" + poparam.ptSpnCode +"' ");
                oDbTblGrp = oDatabase.C_DAToSqlQuery(oSql.ToString());

                if (oDbTblSp != null && oDbTblSp.Rows.Count > 0)
                {
                    if (oDbTblSp_L != null && oDbTblSp_L.Rows.Count > 0)
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
                                    oSql_M.AppendLine(" TCNMSpn ");
                                    oSql_M.AppendLine(" WITH(ROWLOCK)");
                                    oSql_M.AppendLine(" SET ");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptSpnTel, "FTSpnTel") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(Convert.ToString(poparam.pnSpnSleAmt), "FCSpnSleAmt") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptSpnEmail, "FTSpnEmail") + "");
                                    oSql_M.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                                    oSql_M.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                                    oSql_M.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                                    oSql_M.AppendLine(" WHERE FTSpnCode = '" + poparam.ptSpnCode + "'");

                                    string tSpnName = poparam.ptSpnName;
                                    if (poparam.ptSpnName == "")
                                    {
                                        tSpnName = oDbTblSp_L.Rows[0]["FTSpnName"].ToString();
                                    }

                                    oSql_L = new StringBuilder();
                                    oSql_L.Clear();
                                    oSql_L.AppendLine(" UPDATE ");
                                    oSql_L.AppendLine(" TCNMSpn_L ");
                                    oSql_L.AppendLine(" WITH(ROWLOCK)");
                                    oSql_L.AppendLine(" SET");
                                    oSql_L.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptSpnRmk, "FTSpnRmk") + "");
                                    oSql_L.AppendLine(" FTSpnName = '" + poparam.ptSpnName + "'");
                                    oSql_L.AppendLine(" WHERE FTSpnCode = '" + poparam.ptSpnCode + "' ");
                                    oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");

                                    string tShpName = poparam.poPersonGrp.ptShpCode;
                                    if (poparam.poPersonGrp.ptShpCode == "")
                                    {
                                        tShpName = oDbTblSp_L.Rows[0]["FTShpCode"].ToString();
                                    }

                                    oSql_Grp = new StringBuilder();
                                    oSql_Grp.Clear();
                                    oSql_Grp.AppendLine(" UPDATE ");
                                    oSql_Grp.AppendLine(" TCNTSpnGroup ");
                                    oSql_Grp.AppendLine(" WITH(ROWLOCK)");
                                    oSql_Grp.AppendLine(" SET ");
                                    oSql_Grp.AppendLine(" " + oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.poPersonGrp.pdSpnStart), "FDSpnStart") + "");
                                    oSql_Grp.AppendLine("" + oFunc.SP_SETtValueUpt(string.Format("{0:yyyy-MM-dd}", poparam.poPersonGrp.pdSpnStop), "FDSpnStop") + "");
                                    oSql_Grp.AppendLine( "FTShpCode = '" + tShpName + "'");
                                    oSql_Grp.AppendLine(" WHERE FTSpnCode = '" + poparam.ptSpnCode +"' ");
                                    oSql_Grp.AppendLine(" AND FTBchCode = '" + poparam.poPersonGrp.ptBchCode + "' ");
                                    oSql_Grp.AppendLine(" AND FTSpnStaShop = '" + poparam.poPersonGrp.ptSpnStaShop +"' ");


                                    int nSuccess;
                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_M.ToString();
                                    oCmd.ExecuteNonQuery();

                                    oCmd.CommandTimeout = nCmdTme;
                                    oCmd.CommandType = CommandType.Text;
                                    oCmd.CommandText = oSql_L.ToString();
                                    nSuccess = oCmd.ExecuteNonQuery();
                                    if (nSuccess == 0)
                                    {
                                        aoResult = new cmlResItem<cmlResSpnDwn>();
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
                                        aoResult = new cmlResItem<cmlResSpnDwn>();
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
                        aoResult = new cmlResItem<cmlResSpnDwn>();
                        //aoResult = new cmlResPdtItemDwn();
                        aoResult.rtCode = "";
                        aoResult.rtDesc = "";
                        return aoResult;
                    }

                }
                else
                {
                    aoResult = new cmlResItem<cmlResSpnDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;

                }
            }
            catch(Exception oExn)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSpnDwn>();
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

        /// <summary>
        /// Delete Sale Person
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        public cmlResItem<cmlResSpnDwn>POST_PDTDelSalePerson(cmlReqSalePersonDel poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Grp;
            StringBuilder oSql_Addr;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResSpnDwn> aoResult;
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

                aoResult = new cmlResItem<cmlResSpnDwn>();
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
                    oSql.AppendLine(" FROM TCNMSpn  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTSpnCode = '" + poparam.ptSpnCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMSpn_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTSpnCode = '" + poparam.ptSpnCode + "') ");
                    oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");

                    oSql_Grp = new StringBuilder();
                    oSql_Grp.Clear();
                    oSql_Grp.AppendLine(" DELETE ");
                    oSql_Grp.AppendLine(" FROM TCNTSpnGroup WITH(ROWLOCK) ");
                    oSql_Grp.AppendLine("  WHERE (FTSpnCode = '" + poparam.ptSpnCode + "') ");
                    oSql_Grp.AppendLine(" AND (FTBchCode = '" + poparam.poDelGrp.ptBchCode + "') ");
                    oSql_Grp.AppendLine(" AND (FTSpnStaShop = '" + poparam.poDelGrp.ptSpnStaShop + "')");
                    
                    oSql_Addr = new StringBuilder();
                    oSql_Addr.Clear();
                    oSql_Addr.AppendLine(" DELETE ");
                    oSql_Addr.AppendLine(" FROM TCNMAddress_L WITH(ROWLOCK)");
                    oSql_Addr.AppendLine(" WHERE FTAddRefCode = '" + poparam.poDelAddr.ptRefNo + "'");

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
                            oCmd.CommandText = oSql_Grp.ToString();
                            oCmd.ExecuteNonQuery();


                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_L.ToString();
                            int nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                aoResult = new cmlResItem<cmlResSpnDwn>();
                                //aoResult = new cmlResPdtItemDwn();
                                aoResult.rtCode = "";
                                aoResult.rtDesc = "";
                                return aoResult;
                            }

                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Addr.ToString();
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
                aoResult = new cmlResItem<cmlResSpnDwn>();
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
        /// Address
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Address")]
        [HttpPost]
        public cmlResItem<cmlResSpnDwn> POST_PDToInsAddr(cmlSalePersonAdr poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql_Ins;
            StringBuilder oSql_Upt;
            List<cmlTSysConfig> aoSysConfig;
            cmlResItem<cmlResSpnDwn> aoResult;
            cDatabase oDatabase;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResItem<cmlResSpnDwn>();
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
                            oSql_Upt = new StringBuilder();
                            oSql_Upt.Clear();
                            oSql_Upt.AppendLine(" UPDATE TCNMAddress_L ");
                            oSql_Upt.AppendLine(" SET ");
                            oSql_Upt.AppendLine(" FTAddGrpType = '" + poparam.ptGrptype + "', ");
                            oSql_Upt.AppendLine(" FTAddRefCode = '" + poparam.ptRefCode + "',");
                            oSql_Upt.AppendLine(" FTAddRefNo = '" + poparam.ptRefNo + "',");
                            oSql_Upt.AppendLine(" FTAddName = '" + poparam.ptAddName + "',");
                            oSql_Upt.AppendLine(" FTAddTaxNo = '" + poparam.ptTaxNo + "', ");
                            oSql_Upt.AppendLine(" FTAddRmk = '" + poparam.ptAddRmk + "',  ");
                            oSql_Upt.AppendLine(" FTAddCountry = '" + poparam.ptAddCountry + "', ");
                            oSql_Upt.AppendLine(" FTAreCode = '" + poparam.ptAreCode + "',");
                            oSql_Upt.AppendLine(" FTZneCode = '" + poparam.ptZneCode + "', ");
                            oSql_Upt.AppendLine(" FTAddVersion = '" + poparam.ptAddVersion + "',");
                            oSql_Upt.AppendLine(" FTAddV1No = '" + poparam.ptAddV1No + "',");
                            oSql_Upt.AppendLine(" FTAddV1Soi = '" + poparam.ptAddV1Soi + "',");
                            oSql_Upt.AppendLine(" FTAddV1Village = '" + poparam.ptV1Village + "',");
                            oSql_Upt.AppendLine(" FTAddV1Road = '" + poparam.ptV1Road + "',");
                            oSql_Upt.AppendLine(" FTAddV1SubDist = '" + poparam.ptV1SubDist + "',");
                            oSql_Upt.AppendLine(" FTAddV1DstCode = '" + poparam.ptAddV1DstC + "',");
                            oSql_Upt.AppendLine(" FTAddV1PvnCode = '" + poparam.ptV1PvnC + "',");
                            oSql_Upt.AppendLine(" FTAddV1PostCode = '" + poparam.ptV1PostC + "',  ");
                            oSql_Upt.AppendLine(" FTAddV2Desc1 = '" + poparam.ptAddV2Desc1 + "',");
                            oSql_Upt.AppendLine(" FTAddV2Desc2 = '" + poparam.ptAddV2Desc2 + "', ");
                            oSql_Upt.AppendLine(" FTAddWebsite = '" + poparam.ptAddWebSite + "',");
                            oSql_Upt.AppendLine(" FTAddLongitude = '" + poparam.ptAddLongtitude + "',  ");
                            oSql_Upt.AppendLine(" FTAddLatitude = '" + poparam.ptAddLatitude + "',");
                            oSql_Upt.AppendLine(" FDDateUpd =  CONVERT(VARCHAR(10), GETDATE(), 121) ,");
                            oSql_Upt.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql_Upt.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                            oSql_Upt.AppendLine(" WHERE FNLngID = '" + poparam.pnLngID + "'");
                            oSql_Upt.AppendLine(" AND FTAddGrpType = '" + poparam.ptGrptype + "'");
                            oSql_Upt.AppendLine(" AND FTAddRefCode = '" + poparam.ptRefCode + "'");



                            oSql_Ins = new StringBuilder();
                            oSql_Ins.Clear();
                            oSql_Ins.AppendLine(" INSERT INTO TCNMAddress_L ");
                            oSql_Ins.AppendLine(" (FNLngID, FTAddGrpType, FTAddRefCode,  ");
                            oSql_Ins.AppendLine(" FTAddRefNo, FTAddName, FTAddTaxNo,");
                            oSql_Ins.AppendLine(" FTAddRmk, FTAddCountry, FTAreCode, FTZneCode, ");
                            oSql_Ins.AppendLine(" FTAddVersion, FTAddV1No, FTAddV1Soi, FTAddV1Village,");
                            oSql_Ins.AppendLine(" FTAddV1Road, FTAddV1SubDist, FTAddV1DstCode, ");
                            oSql_Ins.AppendLine(" FTAddV1PvnCode, FTAddV1PostCode, FTAddV2Desc1, ");
                            oSql_Ins.AppendLine(" FTAddV2Desc2, FTAddWebsite, FTAddLongitude, ");
                            oSql_Ins.AppendLine(" FTAddLatitude, FDDateUpd, FTTimeUpd, FTWhoUpd)");
                            oSql_Ins.AppendLine(" VALUES ('" + poparam.pnLngID + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptGrptype + "' ,");
                            oSql_Ins.AppendLine(" '" + poparam.ptRefCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptRefNo + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddName + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptTaxNo + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddRmk + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddCountry + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAreCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptZneCode + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddVersion + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddV1No + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddV1Soi + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1Village + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1Road + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1SubDist + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddV1DstC + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1PvnC + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptV1PostC + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddV2Desc1 + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddV2Desc2 + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddWebSite + "',");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddLongtitude + "', ");
                            oSql_Ins.AppendLine(" '" + poparam.ptAddLatitude + "', ");
                            oSql_Ins.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql_Ins.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql_Ins.AppendLine(" '" + poparam.ptWhoUpd + "'");
                            oSql_Ins.AppendLine(")");

                            int nSuccess;
                            oCmd.CommandTimeout = nCmdTme;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Upt.ToString();
                            nSuccess = oCmd.ExecuteNonQuery();
                            if (nSuccess == 0)
                            {
                                oCmd.CommandTimeout = nCmdTme;
                                oCmd.CommandType = CommandType.Text;
                                oCmd.CommandText = oSql_Ins.ToString();
                                oCmd.ExecuteNonQuery();
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
            catch (Exception oExn)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSpnDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900;
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql_Ins = null;
                oSql_Upt = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}