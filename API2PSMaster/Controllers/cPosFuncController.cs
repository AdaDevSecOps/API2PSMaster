using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.POS;
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
    ///     Manage POS Funciton.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Pos/Func")]
    public class cPosFuncController : ControllerBase
    {
        /// <summary>
        ///     Download Pos function information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResFuncDwn> GET_PDToDownloadPosFunc(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResFuncDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResFuncDwn oFuncDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResFuncDwn>();
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

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    aoResult.rtCode = oMsg.tMS_RespCode904;
                    aoResult.rtDesc = oMsg.tMS_RespDesc904;
                    return aoResult;
                }

                tKeyCache = "PosFunc" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResFuncDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTGhdCode AS rtGhdCode, FTGhdApp AS rtGhdApp, FTKbdScreen AS rtKbdScreen, FTKbdGrpName AS rtKbdGrpName,");
                oSql.AppendLine("FNGhdMaxPerPage AS rnGhdMaxPerPage, FTGhdLayOut AS rtGhdLayOut, FNGhdMaxLayOutX AS rnGhdMaxLayOutX,");
                oSql.AppendLine("FNGhdMaxLayOutY AS rnGhdMaxLayOutY, FTGhdStaAlwChg AS rtGhdStaAlwChg,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TPSMFuncHD with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResFuncDwn();
                oFuncDwn = new cmlResFuncDwn();

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
                //            oFuncDwn.raFuncHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoFuncHD>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                oFuncDwn.raFuncHD = oDB.C_DATaSqlQuery<cmlResInfoFuncHD>(oSql.ToString()); //*Arm 64-10-19
                if (oFuncDwn.raFuncHD.Count > 0)
                {
                    //Function Detail
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TPSMFuncDT.FTGhdCode AS rtGhdCode, TPSMFuncDT.FTSysCode AS rtSysCode, TPSMFuncDT.FNGdtPage AS rnGdtPage,");
                    oSql.AppendLine("TPSMFuncDT.FNGdtDefSeq AS rnGdtDefSeq, TPSMFuncDT.FNGdtUsrSeq AS rnGdtUsrSeq, TPSMFuncDT.FNGdtBtnSizeX AS rnGdtBtnSizeX,");
                    oSql.AppendLine("TPSMFuncDT.FNGdtBtnSizeY AS rnGdtBtnSizeY, TPSMFuncDT.FTGdtCallByName AS rtGdtCallByName, TPSMFuncDT.FTGdtStaUse AS rtGdtStaUse,");
                    oSql.AppendLine("TPSMFuncDT.FNGdtFuncLevel AS rnGdtFuncLevel,TPSMFuncDT.FTGdtSysUse AS rtGdtSysUse");   //*Em 62-09-02
                    oSql.AppendLine("FROM TPSMFuncDT with(nolock)");
                    oSql.AppendLine("INNER JOIN TPSMFuncHD with(nolock) ON TPSMFuncDT.FTGhdCode = TPSMFuncHD.FTGhdCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TPSMFuncHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oFuncDwn.raFuncDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoFuncDT>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oFuncDwn.raFuncDT = oDB.C_DATaSqlQuery<cmlResInfoFuncDT>(oSql.ToString()); //*Arm 64-10-19

                    //Function Detail Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TPSMFuncDT_L.FTGhdCode AS rtGhdCode, TPSMFuncDT_L.FTSysCode AS rtSysCode,");
                    oSql.AppendLine("TPSMFuncDT_L.FNLngID AS rnLngID, TPSMFuncDT_L.FTGdtName AS rtGdtName");
                    oSql.AppendLine("FROM TPSMFuncDT_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TPSMFuncHD with(nolock) ON TPSMFuncDT_L.FTGhdCode = TPSMFuncHD.FTGhdCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TPSMFuncHD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oFuncDwn.raFuncDTLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoFuncDTLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oFuncDwn.raFuncDTLng = oDB.C_DATaSqlQuery<cmlResInfoFuncDTLng>(oSql.ToString()); //*Arm 64-10-19

                    //*Net 63-10-02 Comment
                    ////*Arm 63-09-10 TPSMFuncDTSpc
                    //oSql.Clear();
                    //oSql.AppendLine("SELECT SPC.FTGhdCode AS rtGhdCode, SPC.FTSysCode AS rtSysCode, SPC.FTAppCode AS rtAppCode, SPC.FNGdtPage AS rnGdtPage, SPC.FNGdtDefSeq AS rnGdtDefSeq,");
                    //oSql.AppendLine("SPC.FNGdtUsrSeq AS rnGdtUsrSeq, SPC.FNGdtBtnSizeX AS rnGdtBtnSizeX, SPC.FNGdtBtnSizeY AS rnGdtBtnSizeY, SPC.FTGdtCallByName AS rtGdtCallByName, SPC.FTGdtStaUse AS rtGdtStaUse,");
                    //oSql.AppendLine("SPC.FNGdtFuncLevel AS rnGdtFuncLevel, SPC.FTGdtSysUse AS rtGdtSysUse");
                    //oSql.AppendLine("FROM TPSMFuncDTSpc SPC WITH(NOLOCK)");
                    //oSql.AppendLine("INNER JOIN TPSMFuncHD HD WITH(NOLOCK) ON HD.FTGhdCode = SPC.FTGhdCode");
                    //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "' ");
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oFuncDwn.raFuncDTSpc = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoFuncDTSpc>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    ////++++++++++++++
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                aoResult.roItem = oFuncDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResFuncDwn>();
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