using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Coupon;
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
    ///     Coupon information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/PAY")]
    public class cCouponController : ControllerBase
    {
        /// <summary>
        ///     Download couponType information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("CouponType/Download")]
        [HttpGet]
        public cmlResItem<cmlResCpnTypeDwn> GET_PDToDownloadCpnType(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResCpnTypeDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCpnTypeDwn oCpnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCpnTypeDwn>();
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

                tKeyCache = "PAYCouponType" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCpnTypeDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                //*Arm  63-01-10  แก้ไข Sync แค่  TFNMCouponType และ TFNMCouponType_L
                // Get data
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTCptCode AS rtCptCode, FTCptStaUse AS rtCptStaUse,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy,");
                oSql.AppendLine("FTCptType AS rtCptType, FTCptStaChk AS rtCptStaChk,");   //*Arm 62-12-20
                oSql.AppendLine("FTCptStaChkHQ AS rtCptStaChkHQ");   //*Arm 62-12-20
                oSql.AppendLine("FROM TFNMCouponType WITH(NOLOCK)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResCpnTypeDwn();
                oCpnDwn = new cmlResCpnTypeDwn();

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
                //            oCpnDwn.raCpnType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnType>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                oCpnDwn.raCpnType = oDB.C_DATaSqlQuery<cmlResInfoCpnType>(oSql.ToString()); //*Arm 64-10-19
                if (oCpnDwn.raCpnType.Count > 0)
                {
                    //Type coupon Type Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMCouponType_L.FTCptCode AS rtCptCode, TFNMCouponType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMCouponType_L.FTCptName AS rtCptName, TFNMCouponType_L.FTCptRemark AS rtCptRemark");
                    oSql.AppendLine("FROM TFNMCouponType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMCouponType with(nolock) ON TFNMCouponType_L.FTCptCode = TFNMCouponType.FTCptCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMCouponType.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnTypeLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnType_L>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnTypeLng = oDB.C_DATaSqlQuery<cmlResInfoCpnType_L>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}


                aoResult.roItem = oCpnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCpnTypeDwn>();
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
        ///     Download coupon information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Coupon/Download")]
        [HttpGet]
        public cmlResItem<cmlResCpnDwn> GET_PDToDownloadCpn(DateTime pdDate)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB; //*Arm 64-10-19
            cmlResItem<cmlResCpnDwn> aoResult;
            //cmlResPdtItemDwn aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResCpnDwn oCpnDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResCpnDwn>();
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

                tKeyCache = "PAYCoupon" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResCpnDwn>>(tKeyCache);
                    //aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                oDB = new cDatabase(); //*Arm 64-10-19
                aoResult.roItem = new cmlResCpnDwn();
                oCpnDwn = new cmlResCpnDwn();

                //*Arm 64-10-19 Comment Code
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();

                //Get TFNTCouponHD
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode, FTCphDocNo AS rtCphDocNo, ");
                oSql.AppendLine("FTCptCode AS rtCptCode, FDCphDocDate AS rdCphDocDate, ");
                oSql.AppendLine("FTCphDisType AS rtCphDisType, FCCphDisValue AS rcCphDisValue, ");
                oSql.AppendLine("FTPplCode AS rtPplCode, FDCphDateStart AS rdCphDateStart, ");
                oSql.AppendLine("FDCphDateStop AS rdCphDateStop, FTCphTimeStart AS rtCphTimeStart, ");
                oSql.AppendLine("FTCphTimeStop AS rtCphTimeStop, FTCphStaClosed AS rtCphStaClosed, ");
                oSql.AppendLine("FTUsrCode AS rtUsrCode, FTCphUsrApv AS rtCphUsrApv, ");
                oSql.AppendLine("FTCphStaDoc AS rtCphStaDoc, FTCphStaApv AS rtCphStaApv, ");
                oSql.AppendLine("FTCphStaPrcDoc AS rtCphStaPrcDoc, FTCphStaDelMQ AS rtCphStaDelMQ, ");
                oSql.AppendLine("FCCphMinValue AS rtCphMinValue, FTCphStaOnTopPmt AS rtCphStaOnTopPmt, ");
                oSql.AppendLine("FNCphLimitUsePerBill AS rnCphLimitUsePerBill, FTCphRefAccCode AS rtCphRefAccCode, ");
                oSql.AppendLine("FTStaChkMember AS rtStaChkMember, FDLastUpdOn AS rdLastUpdOn, ");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, ");
                oSql.AppendLine("FTCreateBy AS rtCreateBy FROM TFNTCouponHD HD WITH(NOLOCK)");
                oSql.AppendLine($"WHERE HD.FTCphStaClosed='1' AND HD.FTCphStaPrcDoc='1' ");
                oSql.AppendLine("AND CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //oCmd.CommandText = oSql.ToString();
                //using (DbDataReader oDR = oCmd.ExecuteReader())
                //{
                //    oCpnDwn.raCpnHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnHD>(oDR).ToList();
                //    ((IDisposable)oDR).Dispose();
                //}
                oCpnDwn.raCpnHD = oDB.C_DATaSqlQuery<cmlResInfoCpnHD>(oSql.ToString()); //*Arm 64-10-19
                if (oCpnDwn.raCpnHD.Count > 0)
                {
                    //Get TFNTCouponHD_L
                    oSql = new StringBuilder();
                    oSql.AppendLine($"SELECT HDL.FTBchCode AS rtBchCode,HDL.FTCphDocNo AS rtCphDocNo, FNLngID AS rnLngID, ");
                    oSql.AppendLine($"FTCpnName AS rtCpnName, FTCpnMsg1 AS rtCpnMsg1, ");
                    oSql.AppendLine($"FTCpnMsg2 AS rtCpnMsg2, FTCpnCond AS rtCpnCond");
                    oSql.AppendLine($" FROM TFNTCouponHD_L HDL WITH(NOLOCK)");
                    oSql.AppendLine($"INNER JOIN TFNTCouponHD HD ON HD.FTCphDocNo=HDL.FTCphDocNo");
                    oSql.AppendLine($"WHERE HD.FTCphStaClosed='1' AND HD.FTCphStaPrcDoc='1' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnHD_L = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnHD_L>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnHD_L = oDB.C_DATaSqlQuery<cmlResInfoCpnHD_L>(oSql.ToString()); //*Arm 64-10-19

                    //Get TFNTCouponHDBch
                    oSql = new StringBuilder();
                    oSql.AppendLine($" SELECT HDBch.FTBchCode AS rtBchCode, HDBch.FTCphDocNo AS rtCphDocNo, ");
                    oSql.AppendLine($"FTCphBchTo AS rtCphBchTo, FTCphMerTo AS rtCphMerTo, ");
                    oSql.AppendLine($"FTCphShpTo AS rtCphShpTo, FTCphStaType AS rtCphStaType");
                    oSql.AppendLine($" FROM TFNTCouponHDBch HDBch WITH(NOLOCK)");
                    oSql.AppendLine($"INNER JOIN TFNTCouponHD HD ON HDBch.FTBchCode=HD.FTBchCode AND HDBch.FTCphDocNo=HD.FTCphDocNo");
                    oSql.AppendLine($"WHERE HD.FTCphStaClosed='1' AND HD.FTCphStaPrcDoc='1' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnHDBch = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnHDBch>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnHDBch = oDB.C_DATaSqlQuery<cmlResInfoCpnHDBch>(oSql.ToString()); //*Arm 64-10-19

                    //Get TFNTCouponHDCstPri
                    oSql = new StringBuilder();
                    oSql.AppendLine($"SELECT HDCP.FTBchCode AS rtBchCode, HDCP.FTCphDocNo AS rtCphDocNo, ");
                    oSql.AppendLine($"HDCP.FTPplCode AS rtPplCode, FTCphStaType AS rtCphStaType");
                    oSql.AppendLine($" FROM TFNTCouponHDCstPri HDCP WITH(NOLOCK)");
                    oSql.AppendLine($"INNER JOIN TFNTCouponHD HD ON HDCP.FTBchCode=HD.FTBchCode AND HDCP.FTCphDocNo=HD.FTCphDocNo");
                    oSql.AppendLine($"WHERE HD.FTCphStaClosed='1' AND HD.FTCphStaPrcDoc='1' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnHDCstPri = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnHDCstPri>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnHDCstPri = oDB.C_DATaSqlQuery<cmlResInfoCpnHDCstPri>(oSql.ToString()); //*Arm 64-10-19

                    //Get TFNTCouponHDPdt
                    oSql = new StringBuilder();
                    oSql.AppendLine($"SELECT HDPDT.FTBchCode AS rtBchCode, HDPDT.FTCphDocNo AS rtCphDocNo, ");
                    oSql.AppendLine($"FTPdtCode AS rtPdtCode, FTPunCode AS rtPunCode, ");
                    oSql.AppendLine($"FTCphStaType AS rtCphStaType FROM TFNTCouponHDPdt HDPDT WITH(NOLOCK)");
                    oSql.AppendLine($"INNER JOIN TFNTCouponHD HD ON HDPDT.FTBchCode=HD.FTBchCode AND HDPDT.FTCphDocNo=HD.FTCphDocNo");
                    oSql.AppendLine($"WHERE HD.FTCphStaClosed='1' AND HD.FTCphStaPrcDoc='1' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnHDPdt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnHDPdt>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnHDPdt = oDB.C_DATaSqlQuery<cmlResInfoCpnHDPdt>(oSql.ToString()); //*Arm 64-10-19

                    //Get TFNTCouponDT
                    oSql = new StringBuilder();
                    oSql.AppendLine($"SELECT DT.FTBchCode AS rtBchCode, DT.FTCphDocNo AS rtCphDocNo, ");
                    oSql.AppendLine($"FTCpdBarCpn AS rtCpdBarCpn, FNCpdSeqNo AS rnCpdSeqNo, ");
                    oSql.AppendLine($"FNCpdAlwMaxUse AS rnCpdAlwMaxUse FROM TFNTCouponDT DT WITH(NOLOCK)");
                    oSql.AppendLine($"INNER JOIN TFNTCouponHD HD ON DT.FTBchCode=HD.FTBchCode AND DT.FTCphDocNo=HD.FTCphDocNo"); oSql.AppendLine($"WHERE HD.FTCphStaClosed='1' AND HD.FTCphStaPrcDoc='1' ");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10),HD.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnDT>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnDT = oDB.C_DATaSqlQuery<cmlResInfoCpnDT>(oSql.ToString()); //*Arm 64-10-19

                    //Get TFNMCouponType
                    oSql = new StringBuilder();
                    oSql.AppendLine($"SELECT FTCptCode AS rtCptCode, FTCptType AS rtCptType, ");
                    oSql.AppendLine($"FTCptStaChk AS rtCptStaChk, FTCptStaChkHQ AS rtCptStaChkHQ, ");
                    oSql.AppendLine($"FTCptStaUse AS rtCptStaUse, FDLastUpdOn AS rdLastUpdOn, ");
                    oSql.AppendLine($"FTLastUpdBy AS rtLastUpdBy, FDCreateOn AS rdCreateOn, ");
                    oSql.AppendLine($"FTCreateBy AS rtCreateBy FROM TFNMCouponType WITH(NOLOCK)");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnType = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnType>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnType = oDB.C_DATaSqlQuery<cmlResInfoCpnType>(oSql.ToString()); //*Arm 64-10-19

                    //Type coupon Type Languague
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TFNMCouponType_L.FTCptCode AS rtCptCode, TFNMCouponType_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TFNMCouponType_L.FTCptName AS rtCptName, TFNMCouponType_L.FTCptRemark AS rtCptRemark");
                    oSql.AppendLine("FROM TFNMCouponType_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TFNMCouponType with(nolock) ON TFNMCouponType_L.FTCptCode = TFNMCouponType.FTCptCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TFNMCouponType.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raCpnType_L = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoCpnType_L>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raCpnType_L = oDB.C_DATaSqlQuery<cmlResInfoCpnType_L>(oSql.ToString()); //*Arm 64-10-19

                    //Coupon Image
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMImgObj.FNImgID AS rnImgID, TCNMImgObj.FTImgRefID AS rtImgRefID, TCNMImgObj.FNImgSeq AS rnImgSeq,");
                    oSql.AppendLine("TCNMImgObj.FTImgTable AS rtImgTable, TCNMImgObj.FTImgKey AS rtImgKey, TCNMImgObj.FTImgObj AS rtImgObj,");
                    oSql.AppendLine("TCNMImgObj.FDLastUpdOn AS rdLastUpdOn, TCNMImgObj.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMImgObj.FTLastUpdBy AS rtLastUpdBy, TCNMImgObj.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMImgObj with(nolock)");
                    oSql.AppendLine("WHERE FTImgTable='TFNTCouponHD' AND CONVERT(VARCHAR(10),TCNMImgObj.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    //*Arm 64-10-19 Comment Code        
                    //oCmd.CommandText = oSql.ToString();
                    //using (DbDataReader oDR = oCmd.ExecuteReader())
                    //{
                    //    oCpnDwn.raImage = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgObj>(oDR).ToList();
                    //    ((IDisposable)oDR).Dispose();
                    //}
                    oCpnDwn.raImage = oDB.C_DATaSqlQuery<cmlResInfoImgObj>(oSql.ToString()); //*Arm 64-10-19
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                //    }
                //}


                aoResult.roItem = oCpnDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResCpnDwn>();
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