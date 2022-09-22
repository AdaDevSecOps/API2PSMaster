using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Product;
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
    ///     Product promotion information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/Product")]
    public class cProductPromotionController : ControllerBase
    {
        /// <summary>
        ///     Download product rental information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <param name="ptBchCode">รหัสสาขา</param>
        /// <returns></returns>
        [Route("Promotion/Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtPmtDwn> GET_PDToDownloadPdtPmt(DateTime pdDate, string ptBchCode)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            DataTable odtTmp;       //*Arm 63-09-03
            cmlResItem<cmlResPdtPmtDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtPmtDwn oPdtPmtDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtPmtDwn>();
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

                tKeyCache = "ProductPromotion" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtPmtDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }
                //string tSqlX = ""; //*Arm 63-09-03 เก้บ Query test

                //Step : 
                //1. หาเลขที่เอกสารที่ไม่ต้องโหลดไปก่อน
                //2. Query -ข้อมูล เพื่อส่งกลับไป โดยไม่เอาเอกสาร ที่ได้จากข้อ 1 ไป


                //*Arm 63-09-02 หาเอกสารที่ไม่เอา
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPmhDocNo FROM TCNTPdtPmtHDBch WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTPmhStaType = '2' AND FTPmhBchTo = '" + ptBchCode + "'");
                oSql.AppendLine("UNION ");
                oSql.AppendLine("SELECT FTPmhDocNo FROM TCNTPdtPmtHDBch WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTPmhStaType = '1'");
                oSql.AppendLine("AND FTPmhDocNo NOT IN(SELECT FTPmhDocNo FROM TCNTPdtPmtHDBch WITH(NOLOCK) WHERE FTPmhStaType = '1' AND FTPmhBchTo = '" + ptBchCode + "')");
                odtTmp = new DataTable();
                odtTmp = new cDatabase().C_DAToSqlQuery(oSql.ToString());
                //+++++++++++++

                //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //*Arm 63-03-25
                //// Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTBchCode AS rtBchCode,FTPmhDocNo AS rtPmhDocNo,FDPmhDStart AS rdPmhDStart,FDPmhDStop AS rdPmhDStop");
                //oSql.AppendLine(",FDPmhTStart AS rdPmhTStart ,FDPmhTStop AS rdPmhTStop,FTPmhStaLimitCst AS rtPmhStaLimitCst,FTPmhStaClosed AS rtPmhStaClosed");
                //oSql.AppendLine(",FTPmhStaDoc AS rtPmhStaDoc,FTPmhStaApv AS rtPmhStaApv,FTPmhStaPrcDoc AS rtPmhStaPrcDoc,FNPmhStaDocAct AS rnPmhStaDocAct");
                //oSql.AppendLine(",FTUsrCode AS rtUsrCode,FTPmhUsrApv AS rtPmhUsrApv,FTPmhStaOnTopPmt AS rtPmhStaOnTopPmt,FTPmhStaAlwCalPntStd AS rtPmhStaAlwCalPntStd");
                //oSql.AppendLine(",FTPmhStaRcvFree AS rtPmhStaRcvFree,FTPmhStaLimitGet AS rtPmhStaLimitGet,FTPmhStaLimitTime AS rtPmhStaLimitTime,FTPmhStaGetPdt AS rtPmhStaGetPdt");
                //oSql.AppendLine(",FTPmhRefAccCode AS rtPmhRefAccCode, FTPmhStaPdtExc AS rtPmhStaPdtExc");   //*Arm 63-03-25
                //oSql.AppendLine(",FTRolCode AS rtRolCode,FNPmhLimitQty AS rnPmhLimitQty,FTPmhStaChkLimit AS rtPmhStaChkLimit,FTPmhStaChkCst AS rtPmhStaChkCst");
                //oSql.AppendLine(",FTPmhStaGrpPriority AS rtPmhStaGrpPriority, FTPmhStaChkQuota AS rtPmhStaChkQuota, FTPmhStaGetPri AS rtPmhStaGetPri, FTPmhStaOnTopDis AS rtPmhStaOnTopDis, FTPmhStaSpcGrpDis AS rtPmhStaSpcGrpDis "); //*Arm 63-06-18 เพิ่มตามโครงสร้าง SKC
                //oSql.AppendLine(",FDLastUpdOn AS rdLastUpdOn,FTLastUpdBy AS rtLastUpdBy,FDCreateOn AS rdCreateOn,FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNTPdtPmtHD with(nolock)");
                //oSql.AppendLine("WHERE ISNULL(FTPmhStaApv, '') = '1'");
                //oSql.AppendLine("AND CONVERT(VARCHAR(10), FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //oSql.AppendLine("AND CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //if (odtTmp != null && odtTmp.Rows.Count > 0)
                //{
                //    oSql.AppendLine("AND FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 เอาเอกสารที่ไม่เอาออก
                //}
                ////++++++++++++++

                //aoResult.roItem = new cmlResPdtPmtDwn();
                //oPdtPmtDwn = new cmlResPdtPmtDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtPmtDwn.raPdtPmtHD = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtHD>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtPmtDwn.raPdtPmtHD.Count > 0)
                //        {
                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //*Arm 63-03-25 ปรับ Standard
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT DT.FTBchCode AS rtBchCode ,DT.FTPmhDocNo AS rtPmhDocNo,DT.FNPmdSeq AS rnPmdSeq,DT.FTPmdStaType AS rtPmdStaType");
                //            oSql.AppendLine(",DT.FTPmdGrpName AS rtPmdGrpName,DT.FTPmdRefCode AS rtPmdRefCode,DT.FTPmdSubRef AS rtPmdSubRef,DT.FTPmdBarCode AS rtPmdBarCode");
                //            oSql.AppendLine("FROM TCNTPdtPmtDT DT with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON DT.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND DT.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }
                //            //++++++++++++++

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtDT = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtDT>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //*Arm 63-03-26
                //            //Promotion CB
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT PmtCB.FTBchCode AS rtBchCode ,PmtCB.FTPmhDocNo AS rtPmhDocNo,PmtCB.FNPbySeq AS rnPbySeq,PmtCB.FTPmdGrpName AS rtPmdGrpName");
                //            oSql.AppendLine(",PmtCB.FTPbyStaCalSum AS rtPbyStaCalSum,PmtCB.FTPbyStaBuyCond AS rtPbyStaBuyCond,PmtCB.FTPbyStaPdtDT AS rtPbyStaPdtDT,PmtCB.FCPbyPerAvgDis AS rcPbyPerAvgDis");
                //            oSql.AppendLine(",PmtCB.FCPbyMinSetPri AS rcPbyMinSetPri,PmtCB.FCPbyMinValue AS rcPbyMinValue,PmtCB.FCPbyMaxValue AS rcPbyMaxValue,PmtCB.FTPbyMinTime AS rtPbyMinTime");
                //            oSql.AppendLine(",PmtCB.FTPbyMaxTime AS rtPbyMaxTime");
                //            oSql.AppendLine("FROM TCNTPdtPmtCB PmtCB with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON PmtCB.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND PmtCB.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtCB = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtCB>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //Promotion CG
                //            //*Arm 63-03-26
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT PmtCG.FTBchCode AS rtBchCode ,PmtCG.FTPmhDocNo AS rtPmhDocNo, PmtCG.FNPgtSeq AS rnPgtSeq ,PmtCG.FTPmdGrpName AS rtPmdGrpName");
                //            oSql.AppendLine(",PmtCG.FTPgtStaGetEffect AS rtPgtStaGetEffect ,PmtCG.FTPgtStaGetType AS rtPgtStaGetType ,PmtCG.FTPgtStaGetPdt AS rtPgtStaGetPdt ,PmtCG.FTRolCode AS rtRolCode");
                //            oSql.AppendLine(",PmtCG.FCPgtGetvalue AS rcPgtGetvalue ,PmtCG.FTPplCode AS rtPplCode ,PmtCG.FCPgtGetQty AS rcPgtGetQty ,PmtCG.FCPgtPerAvgDis AS rcPgtPerAvgDis");
                //            oSql.AppendLine(",PmtCG.FTPgtStaPoint AS rtPgtStaPoint ,FTPgtStaPntCalType AS rtPgtStaPntCalType ,PmtCG.FTPgtStaPdtDT AS rtPgtStaPdtDT ,PmtCG.FNPgtPntGet AS rnPgtPntGet");
                //            oSql.AppendLine(",PmtCG.FNPgtPntBuy AS rnPgtPntBuy ,PmtCG.FTPgtStaCoupon AS rtPgtStaCoupon,PmtCG.FTPgtCpnText AS rtPgtCpnText,PmtCG.FTCphDocNo AS rtCphDocNo");
                //            oSql.AppendLine("FROM TCNTPdtPmtCG PmtCG with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON PmtCG.FTPmhDocNo = HD.FTPmhDocNo"); //PmtCG.FCPgtStaPntCalType
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND PmtCG.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtCG = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtCG>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //*Arm 63-03-26
                //            //Promotion HD_L
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT HDL.FTBchCode AS rtBchCode,HDL.FTPmhDocNo AS rtPmhDocNo ,HDL.FNLngID AS rnLngID ,HDL.FTPmhName AS rtPmhName");
                //            oSql.AppendLine(",HDL.FTPmhNameSlip AS rtPmhNameSlip ,HDL.FTPmhRmk AS rtPmhRmk");
                //            oSql.AppendLine("FROM TCNTPdtPmtHD_L HDL with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON HDL.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND HDL.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtHD_L = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtHD_L>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //*Arm 63-03-26
                //            //Promotion HDBch
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT HDBch.FTBchCode AS rtBchCode,HDBch.FTPmhDocNo AS rtPmhDocNo,HDBch.FTPmhBchTo AS rtPmhBchTo,HDBch.FTPmhMerTo AS rtPmhMerTo");
                //            oSql.AppendLine(",HDBch.FTPmhShpTo AS rtPmhShpTo,HDBch.FTPmhStaType AS rtPmhStaType");
                //            oSql.AppendLine("FROM TCNTPdtPmtHDBch HDBch with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON HDBch.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND HDBch.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtHDBch = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtHDBch>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //*Arm 63-03-26
                //            //Promotion HDCst
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT HDCst.FTBchCode AS rtBchCode,HDCst.FTPmhDocNo AS rtPmhDocNo,HDCst.FTSpmStaLimitCst AS rtSpmStaLimitCst,HDCst.FNSpmMemAgeLT AS rnSpmMemAgeLT");
                //            oSql.AppendLine(",HDCst.FTSpmStaChkCstDOB AS rtSpmStaChkCstDOB,HDCst.FNPmhCstDobPrev AS rnPmhCstDobPrev,HDCst.FNPmhCstDobNext AS rnPmhCstDobNext");
                //            oSql.AppendLine("FROM TCNTPdtPmtHDCst HDCst with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON HDCst.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND HDCst.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtHDCst = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtHDCst>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                //            //*Arm 63-03-26
                //            //Promotion CstPri
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT CstPri.FTBchCode AS rtBchCode,CstPri.FTPmhDocNo AS rtPmhDocNo,CstPri.FTPplCode AS rtPplCode ,CstPri.FTPmhStaType AS rtPmhStaType");
                //            oSql.AppendLine("FROM TCNTPdtPmtHDCstPri CstPri with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON CstPri.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND CstPri.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtHDCstPri = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtHDCstPri>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //*Net 63-12-25
                //            //Promotion Channel
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT CHN.FTBchCode AS rtBchCode, CHN.FTPmhDocNo AS rtPmhDocNo, ");
                //            oSql.AppendLine("CHN.FTChnCode AS rtChnCode, CHN.FTPmhStaType AS rtPmhStaType");
                //            oSql.AppendLine(" FROM TCNTPdtPmtHDChn CHN WITH(NOLOCK)");
                //            oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON CHN.FTPmhDocNo = HD.FTPmhDocNo");
                //            oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                //            oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //            if (odtTmp != null && odtTmp.Rows.Count > 0)
                //            {
                //                oSql.AppendLine("AND CHN.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                //            }

                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtPmtDwn.raPdtPmtHDChn = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtPmtHDChn>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }
                //            //++++++++++++++++++++++++++++++++++++++

                //            //tSqlX += oSql.ToString();  //*Arm 63-09-03 Get Query Test.. 
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
                aoResult.roItem = new cmlResPdtPmtDwn();
                oPdtPmtDwn = new cmlResPdtPmtDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTBchCode AS rtBchCode,FTPmhDocNo AS rtPmhDocNo,FDPmhDStart AS rdPmhDStart,FDPmhDStop AS rdPmhDStop");
                oSql.AppendLine(",FDPmhTStart AS rdPmhTStart ,FDPmhTStop AS rdPmhTStop,FTPmhStaLimitCst AS rtPmhStaLimitCst,FTPmhStaClosed AS rtPmhStaClosed");
                oSql.AppendLine(",FTPmhStaDoc AS rtPmhStaDoc,FTPmhStaApv AS rtPmhStaApv,FTPmhStaPrcDoc AS rtPmhStaPrcDoc,FNPmhStaDocAct AS rnPmhStaDocAct");
                oSql.AppendLine(",FTUsrCode AS rtUsrCode,FTPmhUsrApv AS rtPmhUsrApv,FTPmhStaOnTopPmt AS rtPmhStaOnTopPmt,FTPmhStaAlwCalPntStd AS rtPmhStaAlwCalPntStd");
                oSql.AppendLine(",FTPmhStaRcvFree AS rtPmhStaRcvFree,FTPmhStaLimitGet AS rtPmhStaLimitGet,FTPmhStaLimitTime AS rtPmhStaLimitTime,FTPmhStaGetPdt AS rtPmhStaGetPdt");
                oSql.AppendLine(",FTPmhRefAccCode AS rtPmhRefAccCode, FTPmhStaPdtExc AS rtPmhStaPdtExc");   //*Arm 63-03-25
                oSql.AppendLine(",FTRolCode AS rtRolCode,FNPmhLimitQty AS rnPmhLimitQty,FTPmhStaChkLimit AS rtPmhStaChkLimit,FTPmhStaChkCst AS rtPmhStaChkCst");
                oSql.AppendLine(",FTPmhStaGrpPriority AS rtPmhStaGrpPriority, FTPmhStaChkQuota AS rtPmhStaChkQuota, FTPmhStaGetPri AS rtPmhStaGetPri, FTPmhStaOnTopDis AS rtPmhStaOnTopDis, FTPmhStaSpcGrpDis AS rtPmhStaSpcGrpDis "); //*Arm 63-06-18 เพิ่มตามโครงสร้าง SKC
                oSql.AppendLine(",FDLastUpdOn AS rdLastUpdOn,FTLastUpdBy AS rtLastUpdBy,FDCreateOn AS rdCreateOn,FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNTPdtPmtHD with(nolock)");
                oSql.AppendLine("WHERE ISNULL(FTPmhStaApv, '') = '1'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                oSql.AppendLine("AND CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                if (odtTmp != null && odtTmp.Rows.Count > 0)
                {
                    oSql.AppendLine("AND FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 เอาเอกสารที่ไม่เอาออก
                }
                oPdtPmtDwn.raPdtPmtHD = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtHD>(oSql.ToString());
                if (oPdtPmtDwn.raPdtPmtHD.Count > 0)
                {
                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //*Arm 63-03-25 ปรับ Standard
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT DT.FTBchCode AS rtBchCode ,DT.FTPmhDocNo AS rtPmhDocNo,DT.FNPmdSeq AS rnPmdSeq,DT.FTPmdStaType AS rtPmdStaType");
                    oSql.AppendLine(",DT.FTPmdGrpName AS rtPmdGrpName,DT.FTPmdRefCode AS rtPmdRefCode,DT.FTPmdSubRef AS rtPmdSubRef,DT.FTPmdBarCode AS rtPmdBarCode");
                    oSql.AppendLine("FROM TCNTPdtPmtDT DT with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON DT.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND DT.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    //++++++++++++++
                    oPdtPmtDwn.raPdtPmtDT = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtDT>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //*Arm 63-03-26
                    //Promotion CB
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT PmtCB.FTBchCode AS rtBchCode ,PmtCB.FTPmhDocNo AS rtPmhDocNo,PmtCB.FNPbySeq AS rnPbySeq,PmtCB.FTPmdGrpName AS rtPmdGrpName");
                    oSql.AppendLine(",PmtCB.FTPbyStaCalSum AS rtPbyStaCalSum,PmtCB.FTPbyStaBuyCond AS rtPbyStaBuyCond,PmtCB.FTPbyStaPdtDT AS rtPbyStaPdtDT,PmtCB.FCPbyPerAvgDis AS rcPbyPerAvgDis");
                    oSql.AppendLine(",PmtCB.FCPbyMinSetPri AS rcPbyMinSetPri,PmtCB.FCPbyMinValue AS rcPbyMinValue,PmtCB.FCPbyMaxValue AS rcPbyMaxValue,PmtCB.FTPbyMinTime AS rtPbyMinTime");
                    oSql.AppendLine(",PmtCB.FTPbyMaxTime AS rtPbyMaxTime");
                    oSql.AppendLine("FROM TCNTPdtPmtCB PmtCB with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON PmtCB.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND PmtCB.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }

                    oPdtPmtDwn.raPdtPmtCB = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtCB>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //Promotion CG
                    //*Arm 63-03-26
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT PmtCG.FTBchCode AS rtBchCode ,PmtCG.FTPmhDocNo AS rtPmhDocNo, PmtCG.FNPgtSeq AS rnPgtSeq ,PmtCG.FTPmdGrpName AS rtPmdGrpName");
                    oSql.AppendLine(",PmtCG.FTPgtStaGetEffect AS rtPgtStaGetEffect ,PmtCG.FTPgtStaGetType AS rtPgtStaGetType ,PmtCG.FTPgtStaGetPdt AS rtPgtStaGetPdt ,PmtCG.FTRolCode AS rtRolCode");
                    oSql.AppendLine(",PmtCG.FCPgtGetvalue AS rcPgtGetvalue ,PmtCG.FTPplCode AS rtPplCode ,PmtCG.FCPgtGetQty AS rcPgtGetQty ,PmtCG.FCPgtPerAvgDis AS rcPgtPerAvgDis");
                    oSql.AppendLine(",PmtCG.FTPgtStaPoint AS rtPgtStaPoint ,FTPgtStaPntCalType AS rtPgtStaPntCalType ,PmtCG.FTPgtStaPdtDT AS rtPgtStaPdtDT ,PmtCG.FNPgtPntGet AS rnPgtPntGet");
                    oSql.AppendLine(",PmtCG.FNPgtPntBuy AS rnPgtPntBuy ,PmtCG.FTPgtStaCoupon AS rtPgtStaCoupon,PmtCG.FTPgtCpnText AS rtPgtCpnText,PmtCG.FTCphDocNo AS rtCphDocNo");
                    oSql.AppendLine("FROM TCNTPdtPmtCG PmtCG with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON PmtCG.FTPmhDocNo = HD.FTPmhDocNo"); //PmtCG.FCPgtStaPntCalType
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND PmtCG.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    oPdtPmtDwn.raPdtPmtCG = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtCG>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //*Arm 63-03-26
                    //Promotion HD_L
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT HDL.FTBchCode AS rtBchCode,HDL.FTPmhDocNo AS rtPmhDocNo ,HDL.FNLngID AS rnLngID ,HDL.FTPmhName AS rtPmhName");
                    oSql.AppendLine(",HDL.FTPmhNameSlip AS rtPmhNameSlip ,HDL.FTPmhRmk AS rtPmhRmk");
                    oSql.AppendLine("FROM TCNTPdtPmtHD_L HDL with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON HDL.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND HDL.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    oPdtPmtDwn.raPdtPmtHD_L = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtHD_L>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //*Arm 63-03-26
                    //Promotion HDBch
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT HDBch.FTBchCode AS rtBchCode,HDBch.FTPmhDocNo AS rtPmhDocNo,HDBch.FTPmhBchTo AS rtPmhBchTo,HDBch.FTPmhMerTo AS rtPmhMerTo");
                    oSql.AppendLine(",HDBch.FTPmhShpTo AS rtPmhShpTo,HDBch.FTPmhStaType AS rtPmhStaType");
                    oSql.AppendLine("FROM TCNTPdtPmtHDBch HDBch with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON HDBch.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND HDBch.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    oPdtPmtDwn.raPdtPmtHDBch = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtHDBch>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //*Arm 63-03-26
                    //Promotion HDCst
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT HDCst.FTBchCode AS rtBchCode,HDCst.FTPmhDocNo AS rtPmhDocNo,HDCst.FTSpmStaLimitCst AS rtSpmStaLimitCst,HDCst.FNSpmMemAgeLT AS rnSpmMemAgeLT");
                    oSql.AppendLine(",HDCst.FTSpmStaChkCstDOB AS rtSpmStaChkCstDOB,HDCst.FNPmhCstDobPrev AS rnPmhCstDobPrev,HDCst.FNPmhCstDobNext AS rnPmhCstDobNext");
                    oSql.AppendLine("FROM TCNTPdtPmtHDCst HDCst with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON HDCst.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND HDCst.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    oPdtPmtDwn.raPdtPmtHDCst = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtHDCst>(oSql.ToString());

                    //tSqlX += oSql.ToString() + Environment.NewLine; //*Arm 63-09-03 

                    //*Arm 63-03-26
                    //Promotion CstPri
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT CstPri.FTBchCode AS rtBchCode,CstPri.FTPmhDocNo AS rtPmhDocNo,CstPri.FTPplCode AS rtPplCode ,CstPri.FTPmhStaType AS rtPmhStaType");
                    oSql.AppendLine("FROM TCNTPdtPmtHDCstPri CstPri with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON CstPri.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND CstPri.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    oPdtPmtDwn.raPdtPmtHDCstPri = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtHDCstPri>(oSql.ToString());

                    //*Net 63-12-25
                    //Promotion Channel
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT CHN.FTBchCode AS rtBchCode, CHN.FTPmhDocNo AS rtPmhDocNo, ");
                    oSql.AppendLine("CHN.FTChnCode AS rtChnCode, CHN.FTPmhStaType AS rtPmhStaType");
                    oSql.AppendLine(" FROM TCNTPdtPmtHDChn CHN WITH(NOLOCK)");
                    oSql.AppendLine("INNER JOIN TCNTPdtPmtHD HD with(nolock) ON CHN.FTPmhDocNo = HD.FTPmhDocNo");
                    oSql.AppendLine("WHERE ISNULL(HD.FTPmhStaApv, '') = '1'");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDPmhDStop, 121) >= CONVERT(VARCHAR(10), GETDATE(), 121)");
                    oSql.AppendLine("AND CONVERT(VARCHAR(10), HD.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                    if (odtTmp != null && odtTmp.Rows.Count > 0)
                    {
                        oSql.AppendLine("AND CHN.FTPmhDocNo NOT IN (" + String.Join(", ", odtTmp.AsEnumerable().Select(oRow => string.Format("'{0}'", oRow.Field<string>("FTPmhDocNo"))).ToArray()) + ")"); //*Arm 63-09-02 - เอาเอกสารที่ไม่เอาออก
                    }
                    oPdtPmtDwn.raPdtPmtHDChn = oDB.C_DATaSqlQuery<cmlResInfoPdtPmtHDChn>(oSql.ToString());
                    //++++++++++++++++++++++++++++++++++++++
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oPdtPmtDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtPmtDwn>();
                aoResult.rtCode = new cMS().tMS_RespCode900;
                aoResult.rtDesc = new cMS().tMS_RespDesc900  + Environment.NewLine + oExcept.Message.ToString();
                return aoResult;
            }
            finally
            {
                oFunc = null;
                oCS = null;
                oMsg = null;
                oSql = null;
                odtTmp = null; //*Arm 63-09-02

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}