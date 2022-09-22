using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Channel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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
    ///     Channel infomation.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Channel")]
    public class cChannelController : ControllerBase
    {
        /// <summary>
        /// Download Channel information.
        /// </summary>
        /// <param name="pdDate"></param>
        /// <param name="ptBchCode"></param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResChnDwn> GET_PDToDownloadChannel(DateTime pdDate, string ptBchCode)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResChnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResChnDwn oChnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            string tAgnCode = ""; //*Arm 64-01-14
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResChnDwn>();
                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(21600, 21600, false);

                #region Check Para
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
                #endregion

                #region Check APIKey
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

                tKeyCache = "Channel" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResChnDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }
                #endregion

                #region Get Data
                
                    aoResult.roItem = new cmlResChnDwn();
                    oChnDwn = new cmlResChnDwn();
                using (DbConnection oConn = new cDatabase().C_CONoDatabase())
                {
                    // Get data
                    oSql = new StringBuilder();

                    //*Arm 64-01-14 Comment Code
                    //oSql.AppendLine("SELECT CHN.FTBchCode AS rtBchCode, CHN.FTChnCode AS rtChnCode, CHN.FTAppCode AS rtAppCode, ");
                    //oSql.AppendLine("   CHN.FTChnGroup AS rtChnGroup, CHN.FNChnSeq AS rnChnSeq, CHN.FTChnStaUse AS rtChnStaUse, ");
                    //oSql.AppendLine("   CHN.FTChnAppDef AS rtChnAppDef, CHN.FTChnRefCode AS rtChnRefCode, CHN.FTPplCode AS rtPplCode, ");
                    ////oSql.AppendLine("   AGN.FTAgnCode AS rtAgnCode, CHN.FTWahCode AS rtWahCode, "); //*Net 64-01-11 ยกเลิก Field ที่ Agency
                    //oSql.AppendLine("   CHN.FTAgnCode AS rtAgnCode, CHN.FTWahCode AS rtWahCode, ");
                    //oSql.AppendLine("   CHN.FDLastUpdOn AS rdLastUpdOn, CHN.FTLastUpdBy AS rtLastUpdBy, ");
                    //oSql.AppendLine("   CHN.FDCreateOn AS rdCreateOn, CHN.FTCreateBy AS rtCreateBy");
                    //oSql.AppendLine("FROM TCNMChannel CHN");
                    ////oSql.AppendLine("INNER JOIN TCNMBranch BCH ON CHN.FTBchCode=BCH.FTBchCode");
                    ////oSql.AppendLine("LEFT JOIN TCNMAgency AGN ON"); //*Net 64-01-11 ยกเลิก Field ที่ Agency
                    ////oSql.AppendLine("   BCH.FTAgnCode=AGN.FTAgnCode AND CHN.FTChnCode=AGN.FTChnCode");
                    //oSql.AppendLine("WHERE CHN.FTBchCode = '" + ptBchCode + "'");
                    //oSql.AppendLine("AND CONVERT(VARCHAR(10), CHN.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //+++++++++++++
                    //*Arm 64-01-14
                    oSql.Clear();
                    oSql.AppendLine("SELECT FTAgnCode FROM TCNMBranch WHERE FTBchCode = '"+ ptBchCode +"'");
                    tAgnCode = new cDatabase().C_DAToSqlQuery<string>(oSql.ToString());

                    oSql.Clear();
                    oSql.AppendLine("SELECT CHN.FTChnCode AS rtChnCode, CHN.FTAppCode AS rtAppCode, CHN.FNChnSeq AS rnChnSeq, CHN.FTChnStaUse AS rtChnStaUse, ");
                    oSql.AppendLine("   CHN.FTChnRefCode AS rtChnRefCode, CHN.FTPplCode AS rtPplCode, CHN.FTWahCode AS rtWahCode, ");
                    oSql.AppendLine("   CHN.FDLastUpdOn AS rdLastUpdOn, CHN.FTLastUpdBy AS rtLastUpdBy, ");
                    oSql.AppendLine("   CHN.FDCreateOn AS rdCreateOn, CHN.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMChannel CHN WITH(NOLOCK)");
                    oSql.AppendLine("LEFT JOIN TCNMChannelSpc SPC WITH(NOLOCK) ON CHN.FTChnCode = SPC.FTChnCode");
                    oSql.AppendLine("WHERE (ISNULL(SPC.FTAgnCode,'') = '' OR ISNULL(SPC.FTAgnCode,'') = '"+ tAgnCode +"')");
                    oSql.AppendLine("AND(ISNULL(SPC.FTBchCode, '') = '' OR ISNULL(SPC.FTBchCode, '') = '"+ ptBchCode +"')");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), CHN.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //+++++++++++++
                    oChnDwn.raChannel = oConn.Query<cmlResInfoChannel>(oSql.ToString(), nCmdTme).ToList();
                    if (oChnDwn.raChannel.Count > 0)
                    {
                        //Channel Languague
                        oSql.Clear();
                        oSql.AppendLine("SELECT CHNL.FTChnCode AS rtChnCode, CHNL.FNLngID AS rnLngID, CHNL.FTChnName AS rtChnName, ");
                        //oSql.AppendLine("  CHNL.FTChnRmk AS rtChnRmk, CHNL.FTBchCode AS rtBchCode"); //*Arm 64-01-14 Comment Code
                        oSql.AppendLine("  CHNL.FTChnRmk AS rtChnRmk"); //*Arm 64-01-14
                        oSql.AppendLine("FROM TCNMChannel_L CHNL WITH(NOLOCK)");
                        //*Arm 64-01-14 Comment Code
                        //oSql.AppendLine("INNER JOIN TCNMChannel CHN with(nolock) ON CHNL.FTChnCode = CHN.FTChnCode AND CHNL.FTBchCode = CHN.FTBchCode ");
                        //oSql.AppendLine("WHERE CHN.FTBchCode = '" + ptBchCode + "'");
                        //oSql.AppendLine("AND CONVERT(VARCHAR(10),CHN.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                        //+++++++++++++
                        //*Arm 64-01-14 
                        oSql.AppendLine("INNER JOIN TCNMChannel CHN WITH(NOLOCK) ON CHNL.FTChnCode = CHN.FTChnCode");
                        oSql.AppendLine("LEFT JOIN TCNMChannelSpc SPC WITH(NOLOCK) ON CHN.FTChnCode = SPC.FTChnCode");
                        oSql.AppendLine("WHERE (ISNULL(SPC.FTAgnCode,'') = '' OR ISNULL(SPC.FTAgnCode,'') = '" + tAgnCode + "')");
                        oSql.AppendLine("AND(ISNULL(SPC.FTBchCode, '') = '' OR ISNULL(SPC.FTBchCode, '') = '" + ptBchCode + "')");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),CHN.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 64-01-14
                        oChnDwn.raChannelLng = oConn.Query<cmlResInfoChannelLng>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++

                        //Arm 64-01-14 -Channel Spc
                        oSql.Clear();
                        oSql.AppendLine("SELECT SPC.FTChnCode AS rtChnCode, SPC.FTAgnCode AS rtAgnCode, SPC.FTAppCode AS rtAppCode, SPC.FNChnSeq AS rnChnSeq, ");
                        oSql.AppendLine("SPC.FTMerCode AS rtMerCode, SPC.FTShpCode AS rtShpCode, SPC.FTBchCode AS rtBchCode, SPC.FTPosCode AS rtPosCode");
                        oSql.AppendLine("FROM TCNMChannelSpc SPC WITH(NOLOCK)");
                        oSql.AppendLine("INNER JOIN TCNMChannel CHN with(nolock) ON SPC.FTChnCode = CHN.FTChnCode");
                        oSql.AppendLine("WHERE (ISNULL(SPC.FTAgnCode,'') = '' OR ISNULL(SPC.FTAgnCode,'') = '" + tAgnCode + "')");
                        oSql.AppendLine("AND(ISNULL(SPC.FTBchCode, '') = '' OR ISNULL(SPC.FTBchCode, '') = '" + ptBchCode + "')");
                        oSql.AppendLine("AND CONVERT(VARCHAR(10),CHN.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'"); //*Arm 64-01-14
                        oChnDwn.raChannelSpc = oConn.Query<cmlResInfoChannelSpc>(oSql.ToString(), nCmdTme).ToList();
                        //+++++++++++++
                    }
                    else
                    {
                        oConn.Close();
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                    oConn.Close();
                }
                #endregion

                aoResult.roItem = oChnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResChnDwn>();
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