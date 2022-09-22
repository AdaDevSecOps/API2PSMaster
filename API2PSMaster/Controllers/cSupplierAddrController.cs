using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Class.Supplier;
using API2PSMaster.Models;
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
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Supplier Address information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Supplier")]
    public class cSupplierAddrController : ControllerBase
    {
        /// <summary>
        ///     Save Supplier Address.
        /// </summary>
        /// 
        /// <param name="poPara"> list Supplier Address information.</param>
        /// 
        /// <returns>
        ///     Supplier varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     701 : validate parameter model false.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        ///     999 : Other error from message error.<br/>
        /// </returns>
        [Route("Address/Save")]
        [HttpPost]
        public cmlResItem<cmlResSplAddrIns> POST_SPLoInsSplSaveItem([FromBody] cmlReqSplAddrIns poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cmlResItem<cmlResSplAddrIns> oResult;
            Boolean bFirst;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplAddrIns>();
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


                if (poPara.paItem.Count > 0)
                {
                    // Delete.
                    oSql = new StringBuilder();
                    oSql.AppendLine("DELETE TCNMSplAddress_L WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID + " AND FTAddGrpType = '" + poPara.ptAddGrpType + "'");
                    try
                    {
                        oDB = new cDatabase();
                        nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                    }
                    catch (SqlException oSqlExn)
                    {
                        switch (oSqlExn.Number)
                        {
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

                    //Insert
                    oSql = new StringBuilder();
                    oSql.AppendLine("INSERT INTO TCNMSplAddress_L WITH(ROWLOCK)");
                    oSql.AppendLine("(");
                    //oSql.AppendLine("   FTSplCode, FNLngID, FTAddGrpType, FNAddSeqNo, FTAddName, ");
                    oSql.AppendLine("   FTSplCode, FNLngID, FTAddGrpType, FTAddName, ");
                    oSql.AppendLine("   FTAddTaxNo, FTAddRmk, FTAddCountry, FTAreCode, FTZneCode,");
                    oSql.AppendLine("   FTAddVersion, FTAddWebsite, FTAddLongitude, FTAddLatitude,");
                    oSql.AppendLine("   FTAddV1No, FTAddV1Soi, FTAddV1Village, FTAddV1Road, ");
                    oSql.AppendLine("   FTAddV1SubDist, FTAddV1DstCode, FTAddV1PvnCode, FTAddV1PostCode,");
                    oSql.AppendLine("   FTAddV2Desc1, FTAddV2Desc2,");
                    oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES");

                    bFirst = true;
                    foreach (cmlReqSplAddrInfo poItem in poPara.paItem)
                    {
                        if (bFirst == false)
                        {
                            oSql.AppendLine(",");
                        }
                        oSql.AppendLine("(");
                        //oSql.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + ",'" + poPara.ptAddGrpType + "'," + poItem.pnAddSeqNo + ",'" + poItem.ptAddName + "',");
                        oSql.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + ",'" + poPara.ptAddGrpType + "','" + poItem.ptAddName + "',");
                        oSql.AppendLine("   '" + poItem.ptAddTaxNo + "','" + poItem.ptAddRmk + "','" + poItem.ptAddCountry + "','" + poItem.ptAreCode + "','" + poItem.ptZneCode + "',");
                        oSql.AppendLine("   '" + poItem.ptAddVersion + "','" + poItem.ptAddWebSite + "','" + poItem.ptAddLongitude + "','" + poItem.ptAddLatitude + "',");
                        oSql.AppendLine("   '" + poItem.ptAddV1No + "','" + poItem.ptAddV1Soi + "','" + poItem.ptAddV1Village + "','" + poItem.ptAddV1Road + "',");
                        oSql.AppendLine("   '" + poItem.ptAddV1SubDist + "','" + poItem.ptAddV1DstCode + "','" + poItem.ptAddV1PvnCode + "','" + poItem.ptAddV1PostCode + "',");
                        oSql.AppendLine("   '" + poItem.ptAddV2Desc1 + "','" + poItem.ptAddV2Desc2 + "',");
                        oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + poItem.ptWhoUpd + "'");
                        oSql.AppendLine(")");
                        if (bFirst == true)
                        {
                            bFirst = false;
                        }
                    }



                    try
                    {
                        oDB = new cDatabase();
                        nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
                    }
                    catch (SqlException oSqlExn)
                    {
                        switch (oSqlExn.Number)
                        {
                            case 2627:
                                // Data is duplicate.
                                //oResult.roItem.rtSplCode = poPara.ptSplCode;
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
                    oResult.rtCode = oMsg.tMS_RespCode001;
                    oResult.rtDesc = oMsg.tMS_RespDesc001;
                    return oResult;
                }
                else
                {
                    oResult.rtCode = oMsg.tMS_RespCode800;
                    oResult.rtDesc = oMsg.tMS_RespDesc800;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplAddrIns>();
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
        ///     Delete Supplier Address.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier address information.</param>
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
        [Route("Address/Delete")]
        [HttpPost]
        public cmlResItem<cmlResSplAddrDel> POST_SPLoDelSplAddrItem([FromBody] cmlReqSplAddrDel poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cmlResItem<cmlResSplAddrDel> oResult;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplAddrDel>();
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
                // Delete.
                oSql = new StringBuilder();
                oSql.AppendLine("DELETE TCNMSplAddress_L WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID + " AND FTAddGrpType = '" + poPara.ptAddGrpType + "'");

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
                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplAddrDel>();
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
        ///     Get supplier address.
        /// </summary>
        /// <param name="pdDate">Date update.</param>
        /// <returns>
        ///     Supplier address information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Address/Download")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplAddrDwn> GET_SPLoDownloadSplItem(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResList<cmlResSplAddrDwn> oResult;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplAddrDwn>();
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
                
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FNLngID AS rnLngID, FTAddGrpType AS rtAddGrpType, FNAddSeqNo AS rnAddSeqNo,	FTAddRefNo AS rtAddRefNo,"); //*Arm 63-04-09
                //oSql.AppendLine("FTAddName AS rtAddName,	FTAddTaxNo AS rtAddTaxNo, FTAddRmk AS rtAddRmk,	FTAddCountry AS rtAddCountry,");
                //oSql.AppendLine("FTAreCode AS rtAreCode,	FTZneCode AS rtZneCode,	FTAddVersion AS rtAddVersion, FTAddV1No AS rtAddV1No,	");
                //oSql.AppendLine("FTAddV1Soi AS rtAddV1Soi, FTAddV1Village AS rtAddV1Village,	FTAddV1Road AS rtAddV1Road,	FTAddV1SubDist AS rtAddV1SubDist,	");
                //oSql.AppendLine("FTAddV1DstCode AS rtAddV1DstCode, FTAddV1PvnCode AS rtAddV1PvnCode,	FTAddV1PostCode AS rtAddV1PostCode,	");
                //oSql.AppendLine("FTAddV2Desc1 AS rtAddV2Desc1, FTAddV2Desc2 AS rtAddV2Desc2,	FTAddWebsite AS rtAddWebsite, FTAddLongitude AS rtAddLongitude,	");
                //oSql.AppendLine("FTAddLatitude AS rtAddLatitude,	");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMSplAddress_L with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oAdaAcc.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplAddrDwn>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //    }


                //    if (oResult.raItems.Count > 0)
                //    {
                //        int nCount = oResult.raItems.Count;
                //        //oResult.rnAllPage = (int)Math.Ceiling((double)nCount / oCons.nCS_CNRowPerPagePic);
                //        //oResult.rnCurrentPage = pnPage;
                //        oResult.rtCode = oMsg.tMS_RespCode001;
                //        oResult.rtDesc = oMsg.tMS_RespDesc001;
                //    }
                //    else
                //    {
                //        oResult.rtCode = oMsg.tMS_RespCode800;
                //        oResult.rtDesc = oMsg.tMS_RespDesc800;
                //        return oResult;
                //    }
                //}
                //*Net 64-10-19 ปรับไปใช้ Dapper
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FNLngID AS rnLngID, FTAddGrpType AS rtAddGrpType, FNAddSeqNo AS rnAddSeqNo,	FTAddRefNo AS rtAddRefNo,"); //*Arm 63-04-09
                oSql.AppendLine("FTAddName AS rtAddName,	FTAddTaxNo AS rtAddTaxNo, FTAddRmk AS rtAddRmk,	FTAddCountry AS rtAddCountry,");
                oSql.AppendLine("FTAreCode AS rtAreCode,	FTZneCode AS rtZneCode,	FTAddVersion AS rtAddVersion, FTAddV1No AS rtAddV1No,	");
                oSql.AppendLine("FTAddV1Soi AS rtAddV1Soi, FTAddV1Village AS rtAddV1Village,	FTAddV1Road AS rtAddV1Road,	FTAddV1SubDist AS rtAddV1SubDist,	");
                oSql.AppendLine("FTAddV1DstCode AS rtAddV1DstCode, FTAddV1PvnCode AS rtAddV1PvnCode,	FTAddV1PostCode AS rtAddV1PostCode,	");
                oSql.AppendLine("FTAddV2Desc1 AS rtAddV2Desc1, FTAddV2Desc2 AS rtAddV2Desc2,	FTAddWebsite AS rtAddWebsite, FTAddLongitude AS rtAddLongitude,	");
                oSql.AppendLine("FTAddLatitude AS rtAddLatitude,	");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMSplAddress_L with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oResult.raItems = oDB.C_DATaSqlQuery<cmlResSplAddrDwn>(oSql.ToString());
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

                oResult.rtCode = oMsg.tMS_RespCode001;
                oResult.rtDesc = oMsg.tMS_RespDesc001;
                return oResult;

            }
            catch (Exception oExcep)
            {
                // Return error.
                oResult = new cmlResList<cmlResSplAddrDwn>();
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
        ///     Get supplier address by supplier code.
        /// </summary>
        /// <param name="ptCode">Supplier Code.</param>
        /// <returns>
        ///     Supplier address information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Address/SearchByID")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplAddrDwn> GET_SPLoSeachIDSplItem(String ptCode)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResList<cmlResSplAddrDwn> oResult;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplAddrDwn>();
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
                oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FNLngID AS rnLngID, FTAddGrpType AS rtAddGrpType, FNAddSeqNo AS rnAddSeqNo,	");
                oSql.AppendLine("FTAddName AS rtAddName,	FTAddTaxNo AS rtAddTaxNo, FTAddRmk AS rtAddRmk,	FTAddCountry AS rtAddCountry,");
                oSql.AppendLine("FTAreCode AS rtAreCode,	FTZneCode AS rtZneCode,	FTAddVersion AS rtAddVersion, FTAddV1No AS rtAddV1No,	");
                oSql.AppendLine("FTAddV1Soi AS rtAddV1Soi, FTAddV1Village AS rtAddV1Village,	FTAddV1Road AS rtAddV1Road,	FTAddV1SubDist AS rtAddV1SubDist,	");
                oSql.AppendLine("FTAddV1DstCode AS rtAddV1DstCode, FTAddV1PvnCode AS rtAddV1PvnCode,	FTAddV1PostCode AS rtAddV1PostCode,	");
                oSql.AppendLine("FTAddV2Desc1 AS rtAddV2Desc1, FTAddV2Desc2 AS rtAddV2Desc2,	FTAddWebsite AS rtAddWebsite, FTAddLongitude AS rtAddLongitude,	");
                oSql.AppendLine("FTAddLatitude AS rtAddLatitude,	");
                oSql.AppendLine("FDDateUpd AS rdDateUpd,	FTTimeUpd AS rtTimeUpd,	FTWhoUpd AS rtWhoUpd ");
                oSql.AppendLine("FROM TCNMSplAddress_L with(nolock)");
                oSql.AppendLine("WHERE FTSplCode = '" + ptCode + "'");
                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {
                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplAddrDwn>(oDR).ToList();
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
                oResult = new cmlResList<cmlResSplAddrDwn>();
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