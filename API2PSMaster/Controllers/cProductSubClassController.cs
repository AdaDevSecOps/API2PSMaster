using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.ProductFhn;
using System;
using System.Collections.Generic;
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
    ///     Manage Product sub class.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product/SubClass")]
    public class cProductSubClassController : ControllerBase
    {
        /// <summary>
        ///     Download product fashion information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtSubClassDwn> GET_PDToDownloadPdtSubClass(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtSubClassDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtSubClassDwn oPdtSubClassDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtSubClassDwn>();
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

                tKeyCache = "ProductSubClass" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtSubClassDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTSclCode AS rtSclCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtF3SubClass with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtSubClassDwn();
                //oPdtSubClassDwn = new cmlResPdtSubClassDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtSubClassDwn.raPdtSubClass = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtSubClass>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtSubClassDwn.raPdtSubClass.Count > 0)
                //        {
                //            //Languague
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF3SubClass_L.FTSclCode AS rtSclCode, TCNMPdtF3SubClass_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtF3SubClass_L.FTSclName AS rtSclName, TCNMPdtF3SubClass_L.FTSclRmk AS rtSclRmk");
                //            oSql.AppendLine("FROM TCNMPdtF3SubClass_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtF3SubClass with(nolock) ON TCNMPdtF3SubClass_L.FTSclCode = TCNMPdtF3SubClass.FTSclCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtF3SubClass.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtSubClassDwn.raPdtSubClassLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtSubClassLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtSubClassDwn();
                oPdtSubClassDwn = new cmlResPdtSubClassDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSclCode AS rtSclCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtF3SubClass with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtSubClassDwn.raPdtSubClass = oDB.C_DATaSqlQuery<cmlResInfoPdtSubClass>(oSql.ToString());

                if (oPdtSubClassDwn.raPdtSubClass.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF3SubClass_L.FTSclCode AS rtSclCode, TCNMPdtF3SubClass_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtF3SubClass_L.FTSclName AS rtSclName, TCNMPdtF3SubClass_L.FTSclRmk AS rtSclRmk");
                    oSql.AppendLine("FROM TCNMPdtF3SubClass_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtF3SubClass with(nolock) ON TCNMPdtF3SubClass_L.FTSclCode = TCNMPdtF3SubClass.FTSclCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtF3SubClass.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtSubClassDwn.raPdtSubClassLng = oDB.C_DATaSqlQuery<cmlResInfoPdtSubClassLng>(oSql.ToString());

                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oPdtSubClassDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtSubClassDwn>();
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