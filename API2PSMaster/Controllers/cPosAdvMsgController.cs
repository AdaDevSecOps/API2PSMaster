using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Response.POS;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Point of sale advertising message infomation.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Pos/Advert")]
    public class cPosAdvMsgController : ControllerBase
    {
        /// <summary>
        ///     Download Pos advertising message information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResAdvMsgDwn> GET_PDToDownloadPosAdvMsg(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResAdvMsgDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResAdvMsgDwn oAdvMsgDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResAdvMsgDwn>();
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

                tKeyCache = "PosAdvMsg" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResAdvMsgDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTAdvCode AS rtAdvCode, FTAdvType AS rtAdvType, FNAdvSeqNo AS rnAdvSeqNo, FTAdvStaUse AS rtAdvStaUse,");
                oSql.AppendLine("SELECT FTAdvCode AS rtAdvCode,FTAdvType AS rtAdvType,FNAdvSeqNo AS rnAdvSeqNo,");  //*Em 62-09-11
                oSql.AppendLine("FTAdvStaUse AS rtAdvStaUse,FDAdvStart AS rdAdvStart,FDAdvStop AS rdAdvStop,"); //*Em 62-09-11
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMAdMsg with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 63-07-31 Download มาทั้งหมดเสมอ (ยกมาจาก Moshi)

                aoResult.roItem = new cmlResAdvMsgDwn();
                oAdvMsgDwn = new cmlResAdvMsgDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                //using (DbConnection oConn = oDB.Database.Connection)
                {
                    //oConn.Open();
                    //DbCommand oCmd = oConn.CreateCommand();
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oAdvMsgDwn.raAdvMsg = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoAdvMsg>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}

                    oAdvMsgDwn.raAdvMsg = oConn.Query<cmlResInfoAdvMsg>(oSql.ToString(), nCmdTme).ToList();
                    if (oAdvMsgDwn.raAdvMsg.Count > 0)
                    {
                        //Languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMAdMsg_L.FTAdvCode AS rtAdvCode, TCNMAdMsg_L.FNLngID AS rnLngID, TCNMAdMsg_L.FTAdvName AS rtAdvName,");
                        oSql.AppendLine("TCNMAdMsg_L.FTAdvMsg AS rtAdvMsg, TCNMAdMsg_L.FTAdvRmk AS rtAdvRmk");
                        oSql.AppendLine("FROM TCNMAdMsg_L with(nolock)");
                        //*Arm 63-07-31 Comment Code ให้ Download มาทั้งหมดเสมอ (ยกมาจาก Moshi)
                        //oSql.AppendLine("INNER JOIN TCNMAdMsg with(nolock) ON TCNMAdMsg_L.FTAdvCode = TCNMAdMsg.FTAdvCode");
                        //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMAdMsg.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //+++++++++++++

                        //oCmd.CommandText = oSql.ToString();
                        //using (DbDataReader oDR = oCmd.ExecuteReader())
                        //{
                        //    oAdvMsgDwn.raAdvMsgLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoAdvMsgLng>(oDR).ToList();
                        //    ((IDisposable)oDR).Dispose();
                        //}
                        oAdvMsgDwn.raAdvMsgLng = oConn.Query<cmlResInfoAdvMsgLng>(oSql.ToString(), nCmdTme).ToList();

                        //MediaObject
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMMediaObj.FNMedID AS rnMedID, TCNMMediaObj.FTMedRefID AS rtMedRefID, TCNMMediaObj.FNMedSeq AS rnMedSeq,");
                        oSql.AppendLine("TCNMMediaObj.FNMedType AS rnMedType, TCNMMediaObj.FTMedFileType AS rtMedFileType, TCNMMediaObj.FTMedTable AS rtMedTable,");
                        oSql.AppendLine("TCNMMediaObj.FTMedKey AS rtMedKey, TCNMMediaObj.FTMedPath AS rtMedPath,");
                        oSql.AppendLine("TCNMMediaObj.FDLastUpdOn AS rdLastUpdOn, TCNMMediaObj.FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("TCNMMediaObj.FDCreateOn AS rdCreateOn, TCNMMediaObj.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMMediaObj WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMAdMsg with(nolock) ON TCNMMediaObj.FTMedRefID = TCNMAdMsg.FTAdvCode AND TCNMMediaObj.FTMedTable = 'TCNMAdMsg'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMAdMsg.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                        //*Ton 64-05-22 ต้องทำให้ rtMedPath ให้เป็น fullpath.
                        //oAdvMsgDwn.raTCNMMediaObj = oConn.Query<resTCNMMediaObj>(oSql.ToString(), nCmdTme).ToList();
                        oAdvMsgDwn.raTCNMMediaObj = oConn.Query<resTCNMMediaObj>(oSql.ToString(), nCmdTme)
                            //update rtMedPath to fullpath.
                            .Select(x=> { x.rtMedPath = cFunc.C_PRCtPrepareFile(x.rtMedPath,HttpContext);return x; }).ToList();

                        //*Em 62-09-11
                        //ImageObj
                        oSql.Clear();
                        oSql.AppendLine("SELECT TCNMImgObj.FNImgID AS rnImgID,TCNMImgObj.FTImgRefID AS rtImgRefID,TCNMImgObj.FNImgSeq AS rnImgSeq,");
                        oSql.AppendLine("TCNMImgObj.FTImgTable AS rtImgTable,TCNMImgObj.FTImgKey AS rtImgKey,TCNMImgObj.FTImgObj AS rtImgObj,");
                        oSql.AppendLine("TCNMImgObj.FDLastUpdOn AS rdLastUpdOn,TCNMImgObj.FTLastUpdBy AS rtLastUpdBy,");
                        oSql.AppendLine("TCNMImgObj.FDCreateOn AS rdCreateOn,TCNMImgObj.FTCreateBy AS rtCreateBy");
                        oSql.AppendLine("FROM TCNMImgObj WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMAdMsg WITH(NOLOCK) ON TCNMAdMsg.FTAdvCode = TCNMImgObj.FTImgRefID AND TCNMImgObj.FTImgTable = 'TCNMAdMsg'");
                        oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMAdMsg.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        oAdvMsgDwn.raTCNMImgObj = oConn.Query<cmlResInfoImgObj>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++++++++++++
                    }
                    else
                    {
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                //}

                aoResult.roItem = oAdvMsgDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResAdvMsgDwn>();
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

        //*Ton 64-05-22 Move to class/cFunc.cs
        //public string C_PRCtPrepareFile(string ptFile)
        //{
        //    string tPathDwn = "";
        //    FileInfo oFile;
        //    try
        //    {
        //        oFile = new FileInfo(ptFile);
        //        if (oFile.Exists)
        //        {
        //            tPathDwn = AppDomain.CurrentDomain.BaseDirectory + "MediaFile";
        //            if (!Directory.Exists(tPathDwn)) Directory.CreateDirectory(tPathDwn);
        //            if (!System.IO.File.Exists(tPathDwn + @"\" + oFile.Name))
        //            {
        //                System.IO.File.Copy(ptFile, tPathDwn + @"\" + oFile.Name);
        //            }

        //            if (System.IO.File.Exists(tPathDwn + @"\" + oFile.Name))
        //            {
        //                //tPathDwn = Request.RequestUri.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath + @"/MediaFile/" + oFile.Name;

        //                //*Ton 64-05-22
        //                //tPathDwn = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath + @"/MediaFile/" + oFile.Name;
        //                string tRqUrl = HttpContext.Request.Host.Value;
        //                string tRqPath = HttpContext.Request.Path.Value;
        //                string tFilePath = @"/MediaFile/" + oFile.Name;
        //                tPathDwn = tRqUrl + tRqPath + tFilePath;
        //            }
        //            else
        //            {
        //                tPathDwn = "";
        //            }
        //        }
        //        else
        //        {
        //            tPathDwn = "";
        //        }
        //    }
        //    catch (Exception oExcept)
        //    {
        //        tPathDwn = oExcept.Message.ToString();
        //    }

        //    return tPathDwn;
        //}
    }

}