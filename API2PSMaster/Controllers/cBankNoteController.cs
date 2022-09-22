using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.BankNote;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Image;
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
    ///     Manage BankNote.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PAY/BankNote")]
    public class cBankNoteController : ControllerBase
    {
        /// <summary>
        ///     BankNote information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResBankNoteDwn> GET_PDToDownloadBankNote(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResBankNoteDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResBankNoteDwn oBankNoteDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResBankNoteDwn>();
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

                tKeyCache = "PAYRCV" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResBankNoteDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRteCode AS rtRteCode, FTBntCode AS rtBntCode, FTBntStaShw AS rtBntStaShw, FCBntRateAmt AS rcBntRateAmt,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TFNMBankNote with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResBankNoteDwn();
                oBankNoteDwn = new cmlResBankNoteDwn();

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
                //            oBankNoteDwn.raBankNote = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoBankNote>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }
                oBankNoteDwn.raBankNote = oDB.C_DATaSqlQuery<cmlResInfoBankNote>(oSql.ToString()); //*Arm 64-10-19
                if (oBankNoteDwn.raBankNote.Count > 0)
                {
                    //Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMBankNote_L.FTRteCode AS rtRteCode, TFNMBankNote_L.FTBntCode AS rtBntCode, TFNMBankNote_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMBankNote_L.FTBntName AS rtBntName, TFNMBankNote_L.FTBntRmk AS rtBntRmk");
                    oSql.AppendLine("FROM TFNMBankNote_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMBankNote with(nolock) ON TFNMBankNote_L.FTBntCode = TFNMBankNote.FTBntCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMBankNote.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oBankNoteDwn.raBankNoteLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoBankNoteLng>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oBankNoteDwn.raBankNoteLng = oDB.C_DATaSqlQuery<cmlResInfoBankNoteLng>(oSql.ToString()); //*Arm 64-10-19

                    // Image Object BankNote // Zen 28-04-2020
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMImgObj.FNImgID AS rnImgID, TCNMImgObj.FTImgRefID AS rtImgRefID, TCNMImgObj.FNImgSeq AS rnImgSeq,");
                    oSql.AppendLine("TCNMImgObj.FTImgTable AS rtImgTable, TCNMImgObj.FTImgKey AS rtImgKey, TCNMImgObj.FTImgObj AS rtImgObj,");
                    oSql.AppendLine("TCNMImgObj.FDLastUpdOn AS rdLastUpdOn, TCNMImgObj.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMImgObj.FTLastUpdBy AS rtLastUpdBy, TCNMImgObj.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMImgObj with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMBankNote with(nolock) ON TCNMImgObj.FTImgRefID = TFNMBankNote.FTBntCode AND TCNMImgObj.FTImgTable = 'TFNMBankNote'");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMBankNote.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oBankNoteDwn.raImage = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgObj>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oBankNoteDwn.raImage = oDB.C_DATaSqlQuery<cmlResInfoImgObj>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}

                aoResult.roItem = oBankNoteDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResBankNoteDwn>();
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