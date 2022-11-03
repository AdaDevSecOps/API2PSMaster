using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using API2PSMaster.Models.WebService.Response.System;
using API2PSMaster.Models.WebService.Request.System;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Service other.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Service")]
    public class cServiceController : ControllerBase
    {
        /// <summary>
        /// Check task for download
        /// </summary>
        /// <param name="paTSysSyncData"></param>
        /// <returns></returns>
        [Route("CheckTaskDownload")]
        [HttpPost]
        public cmlResItem<cmlResSyncDataDwn> GET_CHKoTaskDownload([FromBody] List<cmlReqSyncData> paTSysSyncData)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResSyncDataDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            List<cmlResInfoSyncData> aSyncData;
            //List<cmlTSysSyncData> aDataLocal;
            List<cmlResInfoSyncDataLng> aSyncDataLng;
            cmlResSyncDataDwn oResInfo;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResSyncDataDwn>();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (paTSysSyncData == null)
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

                //tKeyCache = "ProductPromotion" + string.Format("{0:yyyyMMdd}", pdDate);
                //if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                //{
                //    // ถ้ามี key อยุ่ใน cache
                //    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlTSysSyncData>>(tKeyCache);
                //    aoResult.rtCode = oMsg.tMS_RespCode001;
                //    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                //    return aoResult;
                //}

                //aDataLocal = JsonConvert.DeserializeObject<List<cmlTSysSyncData>>(ptTSysSyncData);

                C_PRCxUpdateTaskSync(); //*Em 61-12-24  Water Park

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FNSynSeqNo AS rnSynSeqNo, FTSynGroup AS rtSynGroup, FTSynTable AS rtSynTable, FTSynTable_L AS rtSynTable_L,");
                //oSql.AppendLine("FTSynType AS rtSynType, FDSynLast AS rdSynLast, ISNULL(FNSynSchedule,5) AS rnSynSchedule, FTSynStaUse AS rtSynStaUse,");
                //oSql.AppendLine("FTSynUriDwn AS rtSynUriDwn, FTSynUriUld AS rtSynUriUld");
                //oSql.AppendLine("FROM TSysSyncData");
                //oSql.AppendLine("WHERE FTSynStaUse = '1'");

                //aoResult.roItem = new cmlResSyncDataDwn();
                //oResInfo = new cmlResSyncDataDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            aSyncData = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSyncData>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (aSyncData.Count > 0)
                //        {
                //            List<cmlResInfoSyncData> aResult = (from oSvr in aSyncData
                //                                                join oLcl in paTSysSyncData on new { oCol1 = oSvr.rnSynSeqNo, oCol2 = oSvr.rtSynTable } equals new { oCol1 = oLcl.pnSynSeqNo, oCol2 = oLcl.ptSynTable }
                //                                                where (oSvr.rdSynLast != null ? oSvr.rdSynLast : DateTime.MinValue) > (oLcl.pdSynLast != null ? oLcl.pdSynLast : DateTime.MinValue)
                //                                                //select oSvr).ToList();
                //                                                //*Em 63-10-29
                //                                                select (new cmlResInfoSyncData() {
                //                                                    rdSynLast = oLcl.pdSynLast,
                //                                                    rnSynSeqNo = oSvr.rnSynSeqNo,
                //                                                    rtSynTable = oSvr.rtSynTable,
                //                                                    rtSynTable_L = oSvr.rtSynTable_L,
                //                                                    rnSynSchedule = oSvr.rnSynSchedule,
                //                                                    rtSynGroup = oSvr.rtSynGroup,
                //                                                    rtSynStaUse = oSvr.rtSynStaUse,
                //                                                    rtSynType = oSvr.rtSynType,
                //                                                    rtSynUriDwn = oSvr.rtSynUriDwn,
                //                                                    rtSynUriUld = oSvr.rtSynUriUld
                //                                                })).ToList();
                //                                                //+++++++++++++++++++++

                //            if (aResult.Count > 0)
                //            {
                //                oResInfo.raSyncData = aResult.ToList();

                //                //Languague
                //                oSql = new StringBuilder();
                //                oSql.AppendLine("SELECT FNSynSeqNo AS rnSynSeqNo, FNLngID AS rnLngID,");
                //                oSql.AppendLine("FTSynName AS rtSynName, FTSynRmk AS rtSynRmk");
                //                oSql.AppendLine("FROM TSysSyncData_L with(nolock)");
                //                oCmd.CommandText = oSql.ToString();
                //                using (DbDataReader oDR = oCmd.ExecuteReader())
                //                {
                //                    aSyncDataLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoSyncDataLng>(oDR).ToList();
                //                    ((IDisposable)oDR).Dispose();
                //                }

                //                if (aSyncDataLng.Count > 0)
                //                {
                //                    var aDataLng = (from oMain in aResult
                //                                    join oLng in aSyncDataLng on oMain.rnSynSeqNo equals oLng.rnSynSeqNo
                //                                    select oLng).ToList();
                //                    if (aDataLng.Count > 0)
                //                    {
                //                        oResInfo.raSyncDataLng = aDataLng.ToList();
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                aoResult.rtCode = oMsg.tMS_RespCode800;
                //                aoResult.rtDesc = oMsg.tMS_RespDesc800;
                //                return aoResult;
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
                aoResult.roItem = new cmlResSyncDataDwn();
                oResInfo = new cmlResSyncDataDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNSynSeqNo AS rnSynSeqNo, FTSynGroup AS rtSynGroup, FTSynTable AS rtSynTable, FTSynTable_L AS rtSynTable_L,");
                oSql.AppendLine("FTSynType AS rtSynType, FDSynLast AS rdSynLast, ISNULL(FNSynSchedule,5) AS rnSynSchedule, FTSynStaUse AS rtSynStaUse,");
                oSql.AppendLine("FTSynUriDwn AS rtSynUriDwn, FTSynUriUld AS rtSynUriUld");
                oSql.AppendLine("FROM TSysSyncData");
                oSql.AppendLine("WHERE FTSynStaUse = '1'");
                aSyncData = oDB.C_DATaSqlQuery<cmlResInfoSyncData>(oSql.ToString());
                if (aSyncData.Count > 0)
                {
                    List<cmlResInfoSyncData> aResult = (from oSvr in aSyncData
                                                        join oLcl in paTSysSyncData on new { oCol1 = oSvr.rnSynSeqNo, oCol2 = oSvr.rtSynTable } equals new { oCol1 = oLcl.pnSynSeqNo, oCol2 = oLcl.ptSynTable }
                                                        where (oSvr.rdSynLast != null ? oSvr.rdSynLast : DateTime.MinValue) > (oLcl.pdSynLast != null ? oLcl.pdSynLast : DateTime.MinValue)
                                                        //select oSvr).ToList();
                                                        //*Em 63-10-29
                                                        select (new cmlResInfoSyncData()
                                                        {
                                                            rdSynLast = oLcl.pdSynLast,
                                                            rnSynSeqNo = oSvr.rnSynSeqNo,
                                                            rtSynTable = oSvr.rtSynTable,
                                                            rtSynTable_L = oSvr.rtSynTable_L,
                                                            rnSynSchedule = oSvr.rnSynSchedule,
                                                            rtSynGroup = oSvr.rtSynGroup,
                                                            rtSynStaUse = oSvr.rtSynStaUse,
                                                            rtSynType = oSvr.rtSynType,
                                                            rtSynUriDwn = oSvr.rtSynUriDwn,
                                                            rtSynUriUld = oSvr.rtSynUriUld
                                                        })).ToList();
                    //+++++++++++++++++++++

                    if (aResult.Count > 0)
                    {
                        oResInfo.raSyncData = aResult.ToList();

                        //Languague
                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT FNSynSeqNo AS rnSynSeqNo, FNLngID AS rnLngID,");
                        oSql.AppendLine("FTSynName AS rtSynName, FTSynRmk AS rtSynRmk");
                        oSql.AppendLine("FROM TSysSyncData_L with(nolock)");
                        aSyncDataLng = oDB.C_DATaSqlQuery<cmlResInfoSyncDataLng>(oSql.ToString());

                        if (aSyncDataLng.Count > 0)
                        {
                            var aDataLng = (from oMain in aResult
                                            join oLng in aSyncDataLng on oMain.rnSynSeqNo equals oLng.rnSynSeqNo
                                            select oLng).ToList();
                            if (aDataLng.Count > 0)
                            {
                                oResInfo.raSyncDataLng = aDataLng.ToList();
                            }
                        }
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oResInfo;
                // เก็บ KeyApi ลง Cache
                //oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResSyncDataDwn>();
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

        private void C_PRCxUpdateTaskSync()
        {
            StringBuilder oSql;
            DataTable odtTemp;
            try
            {
                using (AdaAccEntities oDB = new AdaAccEntities())
                {
                    using (DbConnection oConn = oDB.Database.Connection)
                    {
                        oConn.Open();

                        oSql = new StringBuilder();
                        oSql.AppendLine("SELECT TABLE_NAME,COLUMN_NAME ");
                        oSql.AppendLine("FROM INFORMATION_SCHEMA.COLUMNS");
                        oSql.AppendLine("WHERE TABLE_NAME IN (SELECT FTSynTable FROM TSysSyncData WITH(NOLOCK) WHERE FTSynStaUse = '1')");
                        oSql.AppendLine("AND COLUMN_NAME = 'FDLastUpdOn'");

                        DbCommand oCmd = oConn.CreateCommand();
                        oCmd.CommandText = oSql.ToString();
                        using (DbDataReader oDR = oCmd.ExecuteReader())
                        {
                            odtTemp = new DataTable();
                            odtTemp.Load(oDR);
                            if(odtTemp != null)
                            {
                                foreach (DataRow oRow in odtTemp.Rows)
                                {
                                    oSql = new StringBuilder();
                                    oSql.AppendLine("UPDATE TSysSyncData WITH(ROWLOCK)");
                                    oSql.AppendLine("SET FDSynLast = ISNULL((SELECT MAX("+ oRow.Field<string>("COLUMN_NAME") +") AS FDValue FROM "+ oRow.Field<string>("TABLE_NAME") + " WITH(NOLOCK)),FDSynLast) ");
                                    oSql.AppendLine("WHERE FTSynTable = '"+ oRow.Field<string>("TABLE_NAME") + "'");
                                    oCmd.CommandText = oSql.ToString();
                                    oCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch
            { }
            finally
            {
                oSql = null;
                odtTemp = null;
            }
        }
    }
}