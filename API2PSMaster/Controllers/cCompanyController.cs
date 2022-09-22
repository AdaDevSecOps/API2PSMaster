using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Company;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Request.Company;
using API2PSMaster.Models.WebService.Request.Image;
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
using System.IO;
using API2PSMaster.Models.WebService.Response.Branch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Company Information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Company")]
    public class cCompanyController : ControllerBase
    {
        /// <summary>
        ///     Download company information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResCompDwn> GET_PDToDownloadCompany(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResCompDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCompDwn oCompDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCompDwn>();
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

                tKeyCache = "Company" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCompDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCmpCode AS rtCmpCode, FTCmpTel AS rtCmpTel, FTCmpFax AS rtCmpFax, FTBchcode AS rtBchcode,");
                oSql.AppendLine("FTCmpWhsInOrEx AS rtCmpWhsInOrEx, FTCmpRetInOrEx AS rtCmpRetInOrEx, FTCmpEmail AS rtCmpEmail,");
                oSql.AppendLine("FTRteCode AS rtRteCode, FTVatCode AS rtVatCode,");
                oSql.AppendLine("FTCtyCode AS rtCtyCode, ");  //*Arm 65-08-16 [CR-Oversea] เพิ่มรหัสประเทศ
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMComp with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResCompDwn();
                oCompDwn = new cmlResCompDwn();

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
                //            DataTable dt = new DataTable();
                //            dt.Load(oDR);
                //            //List<DataRow> aoRows = (from x in dt.AsEnumerable()
                //            //         select x).ToList();
                //            string jSonResult = JsonConvert.SerializeObject(oDR);
                //            oCompDwn.raComp = JsonConvert.DeserializeObject<List<cmlResInfoComp>>(jSonResult);

                //            //oCompDwn.raComp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoComp>(oDR).ToList();
                //            //((IDisposable)oDR).Dispose();
                //        }

                oCompDwn.raComp = oDB.C_DATaSqlQuery<cmlResInfoComp>(oSql.ToString()); //*Arm 64-10-19
                if (oCompDwn.raComp.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMComp_L.FTCmpCode AS rtCmpCode, TCNMComp_L.FNLngID AS rnLngID, TCNMComp_L.FTCmpName AS rtCmpName,");
                    oSql.AppendLine("TCNMComp_L.FTCmpShop AS rtCmpShop, TCNMComp_L.FTCmpDirector AS rtCmpDirector");
                    oSql.AppendLine("FROM TCNMComp_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMComp ON TCNMComp_L.FTCmpCode = TCNMComp.FTCmpCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMComp.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code         
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCompDwn.raCompLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCompLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCompDwn.raCompLng = oDB.C_DATaSqlQuery<cmlResInfoCompLng>(oSql.ToString()); //*Arm 64-10-19
                    //Image
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMImgObj.FNImgID AS rnImgID, TCNMImgObj.FTImgRefID AS rtImgRefID, TCNMImgObj.FNImgSeq AS rnImgSeq,");
                    oSql.AppendLine("TCNMImgObj.FTImgTable AS rtImgTable, TCNMImgObj.FTImgKey AS rtImgKey, TCNMImgObj.FTImgObj AS rtImgObj,");
                    oSql.AppendLine("TCNMImgObj.FDLastUpdOn AS rdLastUpdOn, TCNMImgObj.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMImgObj.FTLastUpdBy AS rtLastUpdBy, TCNMImgObj.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMImgObj with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMComp with(nolock) ON TCNMImgObj.FTImgRefID = TCNMComp.FTCmpCode AND TCNMImgObj.FTImgTable = 'TCNMComp'");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMComp.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code         
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCompDwn.raImage = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgObj>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCompDwn.raImage = oDB.C_DATaSqlQuery<cmlResInfoImgObj>(oSql.ToString()); //*Arm 64-10-19
                    //*Arm 63-08-17
                    oSql.Clear();
                    oSql.AppendLine("SELECT FNUrlID AS rnUrlID,FTUrlRefID AS rtUrlRefID,FNUrlSeq AS rnUrlSeq,");
                    oSql.AppendLine("FNUrlType AS rnUrlType,FTUrlTable AS rtUrlTable,FTUrlKey AS rtUrlKey,");
                    oSql.AppendLine("FTUrlAddress AS rtUrlAddress,FTUrlPort AS rtUrlPort,FTUrlLogo AS rtUrlLogo,");
                    oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn,FTLastUpdBy AS rtLastUpdBy,");
                    oSql.AppendLine("FDCreateOn AS rdCreateOn,FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNTUrlObject WITH(NOLOCK)");
                    oSql.AppendLine("WHERE FTUrlRefID = 'CENTER' AND FTUrlTable = 'TCNMComp'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code         
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCompDwn.raUrlObject = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResTCNTUrlObject>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCompDwn.raUrlObject = oDB.C_DATaSqlQuery<cmlResTCNTUrlObject>(oSql.ToString()); //*Arm 64-10-19
                    //+++++++++++++
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                aoResult.roItem = oCompDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCompDwn>();
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
        /// Insert Company
        /// </summary>
        /// <param name="poparam"></param>
        /// <returns></returns>
        [Route("Insert")]
        [HttpPost]
        public cmlResList<cmlResCompDwn> POST_PDToInsCompany(cmlReqCompany poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;

            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            cmlResList<cmlResCompDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi;


            try
            {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                aoResult = new cmlResList<cmlResCompDwn>();
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
                            oSql.AppendLine(" INSERT INTO TCNMComp ");
                            oSql.AppendLine(" WITH(ROWLOCK)");
                            oSql.AppendLine(" (FTCmpCode, FTCmpTel, FTCmpFax, FTBchcode,");
                            oSql.AppendLine(" FTCmpWhsInOrEx, FTCmpRetInOrEx, FTCmpEmail,");
                            oSql.AppendLine(" FTRteCode, FTVatCode, FDDateUpd, FTTimeUpd,");
                            oSql.AppendLine(" FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns ");
                            oSql.AppendLine(" )");
                            oSql.AppendLine(" VALUES( '" + poparam.ptCmpCode + "',");
                            oSql.AppendLine(" '" + poparam.ptCmpTel + "',");
                            oSql.AppendLine("'" + poparam.ptCmpFax + "',");
                            oSql.AppendLine("'" + poparam.ptBchcode + "',");
                            oSql.AppendLine("'" + poparam.ptCmpWhsInOrEx + "',");
                            oSql.AppendLine(" '" + poparam.ptCmpRetInOrEx + "' ,");
                            oSql.AppendLine(" '" + poparam.ptCmpEmail + "',");
                            oSql.AppendLine(" '" + poparam.ptRteCode + "',");
                            oSql.AppendLine("'" + poparam.ptVatCode + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
                            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            oSql.AppendLine(" '" + poparam.ptWhoUpd + "' ");
                            oSql.AppendLine(")");

                            oSql_L = new StringBuilder();
                            oSql_L.Clear();
                            oSql_L.AppendLine(" INSERT INTO TCNMComp_L");
                            oSql_L.AppendLine(" WITH(ROWLOCK)");
                            oSql_L.AppendLine(" (FTCmpCode, FNLngID, FTCmpName, FTCmpShop, FTCmpDirector) ");
                            oSql_L.AppendLine(" VALUES");
                            oSql_L.AppendLine("('" + poparam.ptCmpCode + "', ");
                            oSql_L.AppendLine("'" + poparam.pnLngID + "', ");
                            oSql_L.AppendLine(" '" + poparam.ptCmpName + "',");
                            oSql_L.AppendLine(" '" + poparam.ptCmpShop + "',");
                            oSql_L.AppendLine(" '" + poparam.ptCmpDirector + "'");
                            oSql_L.AppendLine(")");

                            oSql_Img = new StringBuilder();
                            for (var nList = 0; nList < poparam.roImgUpl.Count; nList++)
                            {
                                if (poparam.roImgUpl[nList].ptImgobj != "")
                                {
                                    oFunc.SP_IMGbInsImgPath(poparam.ptCmpCode, "Company/", poparam.roImgUpl[nList].ptImgobj,
                                                      poparam.roImgUpl[nList].ptImgRefID, poparam.roImgUpl[nList].ptImgKey,
                                                      poparam.ptWhoUpd, "TCNMComp");
                                }

                                //int nSeq = nList + 1;
                                //string tpath = "D:/Img64/" + "Company/" + poparam.ptCmpCode + "/";


                                ////check have folder... 1 folder | 1 company

                                //if (!(System.IO.Directory.Exists(tpath)))
                                //{
                                //    System.IO.Directory.CreateDirectory(tpath);
                                //}

                                //string tImgName = poparam.ptCmpCode + "_" + nSeq;
                                //oFunc.SP_IMGbSaveStr2Img(tImgName, tpath, poparam.roImgUpl[nList].ptImgobj, tpath + tImgName);

                                ////Insert ImgObj
                                //oSql_Img.Clear();
                                //oSql_Img.AppendLine(" INSERT INTO TCNMImgObj");
                                //oSql_Img.AppendLine(" (FTImgRefID, FNImgSeq, FTImgTable, ");
                                //oSql_Img.AppendLine(" FTImgKey, FTImgObj, FDDateUpd, FTTimeUpd, FTWhoUpd)");
                                //oSql_Img.AppendLine(" VALUES ");
                                //oSql_Img.AppendLine(" (");
                                //oSql_Img.AppendLine(" '" + poparam.roImgUpl[nList].ptImgRefID + "',");
                                //oSql_Img.AppendLine(" '" + nSeq + "',");
                                //oSql_Img.AppendLine(" 'TCNMComp',");
                                //oSql_Img.AppendLine(" '" + poparam.roImgUpl[nList].ptImgKey + "',");
                                //oSql_Img.AppendLine(" '" + oFunc.SP_IMGtGetPath("Company/" + poparam.ptCmpCode + "/", tImgName, tpath) + "',");
                                //oSql_Img.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                                //oSql_Img.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                                //oSql_Img.AppendLine(" '" + poparam.ptWhoUpd + "'");
                                //oSql_Img.AppendLine(")");

                                //oCmd.CommandTimeout = nCmdTme;
                                //oCmd.CommandType = CommandType.Text;
                                //oCmd.CommandText = oSql_Img.ToString();
                                //oCmd.ExecuteNonQuery();
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
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResList<cmlResCompDwn>();
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
        public cmlResList<cmlResCompDwn> POST_PDToUpdCompany(cmlReqCompany poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_M;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResCompDwn> aoResult;
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
                aoResult = new cmlResList<cmlResCompDwn>();
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
                oSql.AppendLine("SELECT FTCmpCode, FTCmpTel, FTCmpFax, FTBchcode, FTCmpWhsInOrEx,  ");
                oSql.AppendLine("FTCmpRetInOrEx, FTCmpEmail, FTRteCode, FTVatCode");
                oSql.AppendLine("FROM TCNMComp WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTCmpCode = '" + poparam.ptCmpCode + "'");
                oDbTblCmp = oDatabase.C_DAToSqlQuery(oSql.ToString());

                oSql = new StringBuilder();
                oSql.AppendLine(" SELECT TOP(1)   FTCmpCode, FNLngID, FTCmpName, FTCmpShop, FTCmpDirector");
                oSql.AppendLine(" FROM TCNMComp_L");
                oSql.AppendLine(" WHERE ");
                oSql.AppendLine(" FTCmpCode = '" + poparam.ptCmpCode + "' AND ");
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
                                    oSql_M.AppendLine(" TCNMComp ");
                                    oSql_M.AppendLine(" WITH(ROWLOCK)");
                                    oSql_M.AppendLine(" SET ");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptCmpTel, "FTCmpTel") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptCmpFax, "FTCmpFax") + "");
                                    oSql_M.AppendLine(" " + oFunc.SP_SETtValueUpt(poparam.ptBchcode, "FTBchcode") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptCmpWhsInOrEx, "FTCmpWhsInOrEx") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptCmpRetInOrEx, "FTCmpRetInOrEx") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptCmpEmail, "FTCmpEmail") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptRteCode, "FTRteCode") + "");
                                    oSql_M.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptVatCode, "FTVatCode") + "");
                                    oSql_M.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
                                    oSql_M.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
                                    oSql_M.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
                                    oSql_M.AppendLine(" WHERE FTCmpCode = '" + poparam.ptCmpCode + "'");

                                    oSql_L = new StringBuilder();
                                    oSql_L.Clear();
                                    oSql_L.AppendLine(" UPDATE ");
                                    oSql_L.AppendLine(" TCNMComp_L ");
                                    oSql_L.AppendLine(" WITH(ROWLOCK)");
                                    oSql_L.AppendLine(" SET");
                                    oSql_L.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptCmpShop, "FTCmpShop") + "");
                                    oSql_L.AppendLine("" + oFunc.SP_SETtValueUpt(poparam.ptCmpDirector, "FTCmpDirector") + "");
                                    oSql_L.AppendLine(" FTCmpName = '" + poparam.ptCmpName + "'");
                                    oSql_L.AppendLine(" WHERE FTCmpCode = '" + poparam.ptCmpCode + "' ");
                                    oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "'");

                                    oSql_Img = new StringBuilder();

                                    for (var nList = 0; nList < poparam.roImgUpl.Count; nList++)
                                    {
                                        if (poparam.roImgUpl[nList].ptImgobj != "")
                                        {
                                            oFunc.SP_IMGbInsImgPath(poparam.ptCmpCode, "Company/", poparam.roImgUpl[nList].ptImgobj,
                                                              poparam.roImgUpl[nList].ptImgRefID, poparam.roImgUpl[nList].ptImgKey,
                                                              poparam.ptWhoUpd, "TCNMComp");
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
                                        aoResult = new cmlResList<cmlResCompDwn>();
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
                        aoResult = new cmlResList<cmlResCompDwn>();
                        //aoResult = new cmlResPdtItemDwn();
                        aoResult.rtCode = "";
                        aoResult.rtDesc = "";
                        return aoResult;
                    }

                }
                else
                {
                    aoResult = new cmlResList<cmlResCompDwn>();
                    //aoResult = new cmlResPdtItemDwn();
                    aoResult.rtCode = "";
                    aoResult.rtDesc = "";
                    return aoResult;
                }

            }
            catch (Exception oEx)
            {
                // Return error.
                aoResult = new cmlResList<cmlResCompDwn>();
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
        public cmlResList<cmlResCompDwn> POST_PDToDel(cmlReqCompanyDel poparam)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            StringBuilder oSql_L;
            StringBuilder oSql_Img;
            List<cmlTSysConfig> aoSysConfig;
            cmlResList<cmlResCompDwn> aoResult;
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

                aoResult = new cmlResList<cmlResCompDwn>();
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
                    oSql.AppendLine(" FROM TCNMComp  WITH(ROWLOCK) ");
                    oSql.AppendLine(" WHERE (FTCmpCode = '" + poparam.ptCmpCode + "')");

                    oSql_L = new StringBuilder();
                    oSql_L.Clear();
                    oSql_L.AppendLine(" DELETE ");
                    oSql_L.AppendLine(" FROM TCNMComp_L WITH(ROWLOCK)");
                    oSql_L.AppendLine(" WHERE (FTCmpCode = '" + poparam.ptCmpCode + "') ");
                    oSql_L.AppendLine(" AND (FNLngID = '" + poparam.pnLngID + "')");

                    oSql_Img = new StringBuilder();
                    oSql_Img.Clear();
                    oSql_Img.AppendLine(" DELETE ");
                    oSql_Img.AppendLine(" FROM TCNMImgObj WITH(ROWLOCK) ");
                    oSql_Img.AppendLine("  WHERE (FTImgRefID = '" + poparam.ptCmpCode + "') ");

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
                                aoResult = new cmlResList<cmlResCompDwn>();
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
            catch (Exception oExn)
            {
                // Return error.
                aoResult = new cmlResList<cmlResCompDwn>();
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