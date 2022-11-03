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
    ///     Supplier Contact.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Supplier")]
    public class cSupplierContactController : ControllerBase
    {
        /// <summary>
        ///     Save Supplier Contact.
        /// </summary>
        /// 
        /// <param name="poPara"> list Supplier Contact information.</param>
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
        [Route("Contact/Save")]
        [HttpPost]
        public cmlResItem<cmlResSplContactIns> POST_SPLoSaveSplContactItem([FromBody] cmlReqSplContactIns poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cmlResItem<cmlResSplContactIns> oResult;
            Boolean bFirst;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplContactIns>();
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
                    oSql.AppendLine("DELETE TCNMSplContact_L WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID);
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


                }
                else
                {
                    oResult.rtCode = oMsg.tMS_RespCode800;
                    oResult.rtDesc = oMsg.tMS_RespDesc800;
                    return oResult;
                }
                //Insert
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT INTO TCNMSplContact_L WITH(ROWLOCK)");
                oSql.AppendLine("(");
                //oSql.AppendLine("   FTSplCode, FNLngID, FNAddSeq, FTCtrName, FTCtrFax,");
                oSql.AppendLine("   FTSplCode, FNLngID, FTCtrName, FTCtrFax,");
                oSql.AppendLine("   FTCtrTel, FTCtrEmail, FTCtrRmk,");
                oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES");

                bFirst = true;
                foreach (cmlReqSplContactInfo oItem in poPara.paItem)
                {
                    if (bFirst == false)
                    {
                        oSql.AppendLine(",");
                    }
                    oSql.AppendLine("(");
                    //oSql.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + "," + oItem.pnAddSeqNo  + ",'" + oItem.ptCtrName + "','" + oItem.ptCtrFax + "',");
                    oSql.AppendLine("   '" + poPara.ptSplCode + "'," + poPara.pnLngID + ",'" + oItem.ptCtrName + "','" + oItem.ptCtrFax + "',");
                    oSql.AppendLine("   '" + oItem.ptCtrTel + "','" + oItem.ptCtrEmail + "','" + oItem.ptCtrRmk + "',");
                    oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), '" + oItem.ptWhoUpd + "'");
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
                            oResult.roItem.rtSplCode = poPara.ptSplCode;
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
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResSplContactIns>();
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

        ///// <summary>
        /////     Update Supplier contact.
        ///// </summary>
        ///// 
        ///// <param name="poPara">Supplier contact information.</param>
        ///// 
        ///// <returns>
        /////     Supplier varify false.<br/>
        /////     System process status.<br/>
        /////     1   : success.<br/>
        /////     801 : data is duplicate.<br/>
        /////     900 : service process false.<br/>
        /////     904 : key not allowed to use method.<br/>
        /////     905 : cannot connect database.<br/>
        /////     906 : this time not allowed to use method.<br/>
        ///// </returns>
        //[Route("Contact/Update")]
        //[HttpPost]
        //public cmlResItem<cmlResSplContactIns> POST_SPLoUpdSplAddrItem([FromBody] cmlReqSplContactIns poPara)
        //{
        //    cSP oFunc;
        //    cCS oCons;
        //    cMS oMsg;
        //    cDatabase oDB;
        //    StringBuilder oSql;
        //    cSupplier oSpl;
        //    cmlResItem<cmlResSplContactIns> oResult;
        //    int nRowEff;
        //    string tFuncName, tModelErr, tKeyApi;
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        //        oResult = new cmlResItem<cmlResSplContactIns>();
        //        oFunc = new cSP();
        //        oCons = new cCS();
        //        oMsg = new cMS();

        //        // Get method name.
        //        tFuncName = MethodBase.GetCurrentMethod().Name;

        //        // Validate parameter.
        //        tModelErr = "";
        //        if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState) == false)
        //        {
        //            // Validate parameter model false.
        //            oResult.rtCode = oMsg.tMS_RespCode701;
        //            oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
        //            return oResult;
        //        }

        //        tKeyApi = "";
        //        // Check KeyApi.
        //        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
        //        {
        //            // Key not allowed to use method.
        //            oResult.rtCode = oMsg.tMS_RespCode904;
        //            oResult.rtDesc = oMsg.tMS_RespDesc904;
        //            return oResult;
        //        }
        //        // Varify parameter value.
        //        oSpl = new cSupplier();

        //        //update
        //        oSql = new StringBuilder();
        //        oSql.AppendLine("UPDATE TCNMSplContact_L WITH(ROWLOCK)");
        //        oSql.AppendLine("SET FNAddSeq = " + poPara.pnAddSeqNo);
        //        oSql.AppendLine("   ,FTCtrName = '" + poPara.ptCtrName + "'");
        //        oSql.AppendLine("   ,FTCtrFax = '" + poPara.ptCtrFax + "'");
        //        oSql.AppendLine("   ,FTCtrTel = '" + poPara.ptCtrTel + "'");
        //        oSql.AppendLine("   ,FTCtrEmail = '" + poPara.ptCtrEmail + "'");
        //        oSql.AppendLine("   ,FTCtrRmk = '" + poPara.ptCtrRmk + "'");
        //        oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID);

        //        try
        //        {
        //            oDB = new cDatabase();
        //            nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());
        //            if (nRowEff == 0)
        //            {
        //                oResult.rtCode = oMsg.tMS_RespCode800;
        //                oResult.rtDesc = oMsg.tMS_RespDesc800;
        //                return oResult;
        //            }
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

        //        oResult.rtCode = oMsg.tMS_RespCode001;
        //        oResult.rtDesc = oMsg.tMS_RespDesc001;
        //        return oResult;
        //    }
        //    catch (Exception)
        //    {
        //        // Return error.
        //        oResult = new cmlResItem<cmlResSplContactIns>();
        //        oResult.rtCode = new cMS().tMS_RespCode900;
        //        oResult.rtDesc = new cMS().tMS_RespDesc900;
        //        return oResult;
        //    }
        //    finally
        //    {
        //        oFunc = null;
        //        oCons = null;
        //        oMsg = null;
        //        oDB = null;
        //        oSql = null;

        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        //GC.Collect();
        //    }
        //}

        /// <summary>
        ///     Delete Supplier contact.
        /// </summary>
        /// 
        /// <param name="poPara">Supplier contact information.</param>
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
        [Route("Contact/Delete")]
        [HttpPost]
        public cmlResItem<cmlResSplContactDel> POST_SPLoDelSplAddrItem([FromBody] cmlReqSplContactDel poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDB;
            StringBuilder oSql;
            cmlResItem<cmlResSplContactDel> oResult;
            int nRowEff;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResSplContactDel>();
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
                oSql.AppendLine("DELETE TCNMSplContact_L WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "' AND FNLngID = " + poPara.pnLngID );

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
                oResult = new cmlResItem<cmlResSplContactDel>();
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
        ///     Download supplier contact.
        /// </summary>
        /// <param name="pdDate">Date update.</param>
        /// <returns>
        ///     Supplier information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Contact/Download")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplContactDwn> GET_SPLoDownloadSplItem(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            StringBuilder oSql;
            cSupplier oSpl;
            cmlResList<cmlResSplContactDwn> oResult;
            string tFuncName, tModelErr, tKeyApi;
            List<cmlTSysConfig> aoSysConfig;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResList<cmlResSplContactDwn>();
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
                //oSql = new StringBuilder();
                ////oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FNLngID AS rtLngID, FNCtrSeq AS rnCtrSeq, FTCtrName rtCtrName,");
                //oSql.AppendLine("select FTSplCode AS rtSplCode, FNLngID AS rnLngID, FNCtrSeq AS rnCtrSeq, FTCtrName AS rtCtrName,");
                //oSql.AppendLine("FTCtrFax AS rtCtrFax, FTCtrTel AS rtCtrTel, FTCtrEmail AS rtCtrEmail, FTCtrRmk AS rtCtrRmk,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMSplContact_L");
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
                //            oResult.raItems = ((IObjectContextAdapter)oAdaAcc).ObjectContext.Translate<cmlResSplContactDwn>(oDR).ToList();
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
                oSql.AppendLine("select FTSplCode AS rtSplCode, FNLngID AS rnLngID, FNCtrSeq AS rnCtrSeq, FTCtrName AS rtCtrName,");
                oSql.AppendLine("FTCtrFax AS rtCtrFax, FTCtrTel AS rtCtrTel, FTCtrEmail AS rtCtrEmail, FTCtrRmk AS rtCtrRmk,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMSplContact_L");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oResult.raItems = oDB.C_DATaSqlQuery<cmlResSplContactDwn>(oSql.ToString());
                if (oResult.raItems.Count > 0)
                {
                    int nCount = oResult.raItems.Count;
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
                oResult = new cmlResList<cmlResSplContactDwn>();
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
        ///     Get supplier contact by supplier code.
        /// </summary>
        /// <param name="ptCode">Supplier contact.</param>
        /// <returns>
        ///     Supplier contact information.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     800 : data not found.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Contact/SearchByID")]
        [HttpGet]
        //[OutputCacheWebApi(serverCacheSecond: 30, clientCacheSeconds: 30, allowAnonymous: true)]
        public cmlResList<cmlResSplAddrDwn> GET_SPLoSeachIDSplContact(String ptCode)
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
                oSql.AppendLine("SELECT FTSplCode AS rtSplCode, FNLngID AS rtLngID, FNCtrSeq AS rnCtrSeq, FTCtrName rtCtrName,");
                oSql.AppendLine("FTCtrFax AS rtCtrFax, FTCtrTel AS rtCtrTel,	FTCtrEmail AS rtCtrEmail, FTCtrRmk rtCtrRmk,");
                oSql.AppendLine("FDDateUpd AS rdDateUpd,	FTTimeUpd AS rtTimeUpd,	FTWhoUpd AS rtWhoUpd");
                oSql.AppendLine("FROM TCNMSplContact_L");
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