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
    ///     Product fashion.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/ProductFhn/Item")]
    public class cProductFhnController : ControllerBase
    {
        /// <summary>
        ///     Download product fashion information.
        /// </summary>
        /// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResPdtFhnDwn> GET_PDToDownloadPdtFhnItem(DateTime pdDate)
        {
            cDatabase oDB; //*Net 64-10-19
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cmlResItem<cmlResPdtFhnDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResPdtFhnDwn oPdtDwn;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme, nConTme;
            string tFuncName, tModelErr, tKeyApi, tKeyCache;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResPdtFhnDwn>();
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

                tKeyCache = "ProductFhn" + string.Format("{0:yyyyMMdd}", pdDate);
                if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
                {
                    // ถ้ามี key อยุ่ใน cache
                    aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResPdtFhnDwn>>(tKeyCache);
                    aoResult.rtCode = oMsg.tMS_RespCode001;
                    aoResult.rtDesc = oMsg.tMS_RespDesc001;
                    return aoResult;
                }

                // Get data
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPgpChain AS rtPgpChain, FTDcsCode AS rtDcsCode, FTClrCode AS rtClrCode,");
                //oSql.AppendLine("FTPszCode AS rtPszCode, FTPdtArticle AS rtPdtArticle, FTDepCode AS rtDepCode, FTClsCode AS rtClsCode,");
                //oSql.AppendLine("FTSclCode AS rtSclCode,");
                //oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                //oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                //oSql.AppendLine("FROM TCNMPdtFhn with(nolock)");
                //oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

                //aoResult.roItem = new cmlResPdtFhnDwn();
                //oPdtDwn = new cmlResPdtFhnDwn();
                //using (AdaAccEntities oDB = new AdaAccEntities())
                //{
                //    using (DbConnection oConn = oDB.Database.Connection)
                //    {
                //        oConn.Open();
                //        DbCommand oCmd = oConn.CreateCommand();
                //        oCmd.CommandText = oSql.ToString();
                //        using (DbDataReader oDR = oCmd.ExecuteReader())
                //        {
                //            oPdtDwn.raPdt = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtFhn>(oDR).ToList();
                //            ((IDisposable)oDR).Dispose();
                //        }

                //        if (oPdtDwn.raPdt.Count > 0)
                //        {
                //            //Season
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtFSeason.FTPgpCode AS rtPgpCode, TCNMPdtFSeason.FNPgpLevel AS rnPgpLevel, TCNMPdtFSeason.FTPgpParent AS rtPgpParent,");
                //            oSql.AppendLine("TCNMPdtFSeason.FTPgpChain AS rtPgpChain,");
                //            oSql.AppendLine("TCNMPdtFSeason.FDLastUpdOn AS rdLastUpdOn, TCNMPdtFSeason.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMPdtFSeason.FTLastUpdBy AS rtLastUpdBy, TCNMPdtFSeason.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMPdtFSeason with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtFSeason.FTPgpChain = TCNMPdtFhn.FTPgpChain");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtSeason = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtSeason>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtFSeason_L.FTPgpCode AS rtPgpCode, TCNMPdtFSeason_L.FNPgpLevel AS rnPgpLevel, TCNMPdtFSeason_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtFSeason_L.FTPgpName AS rtPgpName, TCNMPdtFSeason_L.FTPgpRmk AS rtPgpRmk");
                //            oSql.AppendLine("FROM TCNMPdtFSeason_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFSeason with(nolock) ON TCNMPdtFSeason_L.FTPgpCode = TCNMPdtFSeason.FTPgpCode");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtFSeason.FTPgpChain = TCNMPdtFhn.FTPgpChain");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtSeasonLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtSeasonLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //DCS
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF0DCS.FTDcsCode AS rtDcsCode, TCNMPdtF0DCS.FTDepCode AS rtDepCode, TCNMPdtF0DCS.FTClsCode AS rtClsCode,");
                //            oSql.AppendLine("TCNMPdtF0DCS.FTSclCode AS rtSclCode,");
                //            oSql.AppendLine("TCNMPdtF0DCS.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF0DCS.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMPdtF0DCS.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF0DCS.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMPdtF0DCS with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF0DCS.FTDcsCode = TCNMPdtFhn.FTDcsCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtDCS = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtDCS>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF0DCS_L.FTDcsCode AS rtDcsCode, TCNMPdtF0DCS_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtF0DCS_L.FTDcsName AS rtDcsName, TCNMPdtF0DCS_L.FTDcsRmk AS rtDcsRmk");
                //            oSql.AppendLine("FROM TCNMPdtF0DCS_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtF0DCS with(nolock) ON TCNMPdtF0DCS_L.FTDcsCode = TCNMPdtF0DCS.FTDcsCode");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF0DCS.FTDcsCode = TCNMPdtFhn.FTDcsCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtDCSLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtDCSLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Depart
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF1Depart.FTDepCode AS rtDepCode,");
                //            oSql.AppendLine("TCNMPdtF1Depart.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF1Depart.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMPdtF1Depart.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF1Depart.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMPdtF1Depart with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF1Depart.FTDepCode = TCNMPdtFhn.FTDepCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtDepart = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtDepart>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF1Depart_L.FTDepCode AS rtDepCode, TCNMPdtF1Depart_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtF1Depart_L.FTDepName AS rtDepName, TCNMPdtF1Depart_L.FTDepRmk AS rtDepRmk");
                //            oSql.AppendLine("FROM TCNMPdtF1Depart_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtF1Depart with(nolock) ON TCNMPdtF1Depart_L.FTDepCode = TCNMPdtF1Depart.FTDepCode");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF1Depart.FTDepCode = TCNMPdtFhn.FTDepCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtDepartLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtDepartLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //Class
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF2Class.FTClsCode AS rtClsCode,");
                //            oSql.AppendLine("TCNMPdtF2Class.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF2Class.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMPdtF2Class.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF2Class.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMPdtF2Class with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF2Class.FTClsCode = TCNMPdtFhn.FTClsCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtClass = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtClass>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF2Class_L.FTClsCode AS rtClsCode, TCNMPdtF2Class_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtF2Class_L.FTClsName AS rtClsName, TCNMPdtF2Class_L.FTClsRmk AS rtClsRmk");
                //            oSql.AppendLine("FROM TCNMPdtF2Class_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtF2Class with(nolock) ON TCNMPdtF2Class_L.FTClsCode = TCNMPdtF2Class.FTClsCode");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF2Class.FTClsCode = TCNMPdtFhn.FTClsCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtClassLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtClassLng>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            //SubClass
                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF3SubClass.FTSclCode AS rtSclCode,");
                //            oSql.AppendLine("TCNMPdtF3SubClass.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF3SubClass.FDCreateOn AS rdCreateOn,");
                //            oSql.AppendLine("TCNMPdtF3SubClass.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF3SubClass.FTCreateBy AS rtCreateBy");
                //            oSql.AppendLine("FROM TCNMPdtF3SubClass with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF3SubClass.FTSclCode = TCNMPdtFhn.FTSclCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtSubClass = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtSubClass>(oDR).ToList();
                //                ((IDisposable)oDR).Dispose();
                //            }

                //            oSql = new StringBuilder();
                //            oSql.AppendLine("SELECT TCNMPdtF3SubClass_L.FTSclCode AS rtSclCode, TCNMPdtF3SubClass_L.FNLngID AS rnLngID,");
                //            oSql.AppendLine("TCNMPdtF3SubClass_L.FTSclName AS rtSclName, TCNMPdtF3SubClass_L.FTSclRmk AS rtSclRmk");
                //            oSql.AppendLine("FROM TCNMPdtF3SubClass_L with(nolock)");
                //            oSql.AppendLine("INNER JOIN TCNMPdtF3SubClass with(nolock) ON TCNMPdtF3SubClass_L.FTSclCode = TCNMPdtF3SubClass.FTSclCode");
                //            oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF3SubClass.FTSclCode = TCNMPdtFhn.FTSclCode");
                //            oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                //            oCmd.CommandText = oSql.ToString();
                //            using (DbDataReader oDR = oCmd.ExecuteReader())
                //            {
                //                oPdtDwn.raPdtSubClassLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoPdtSubClassLng>(oDR).ToList();
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
                aoResult.roItem = new cmlResPdtFhnDwn();
                oPdtDwn = new cmlResPdtFhnDwn();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTPdtCode AS rtPdtCode, FTPgpChain AS rtPgpChain, FTDcsCode AS rtDcsCode, FTClrCode AS rtClrCode,");
                oSql.AppendLine("FTPszCode AS rtPszCode, FTPdtArticle AS rtPdtArticle, FTDepCode AS rtDepCode, FTClsCode AS rtClsCode,");
                oSql.AppendLine("FTSclCode AS rtSclCode,");
                oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
                oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
                oSql.AppendLine("FROM TCNMPdtFhn with(nolock)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                oPdtDwn.raPdt = oDB.C_DATaSqlQuery<cmlResInfoPdtFhn>(oSql.ToString());
                if (oPdtDwn.raPdt.Count > 0)
                {
                    //Season
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtFSeason.FTPgpCode AS rtPgpCode, TCNMPdtFSeason.FNPgpLevel AS rnPgpLevel, TCNMPdtFSeason.FTPgpParent AS rtPgpParent,");
                    oSql.AppendLine("TCNMPdtFSeason.FTPgpChain AS rtPgpChain,");
                    oSql.AppendLine("TCNMPdtFSeason.FDLastUpdOn AS rdLastUpdOn, TCNMPdtFSeason.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMPdtFSeason.FTLastUpdBy AS rtLastUpdBy, TCNMPdtFSeason.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMPdtFSeason with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtFSeason.FTPgpChain = TCNMPdtFhn.FTPgpChain");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtSeason = oDB.C_DATaSqlQuery<cmlResInfoPdtSeason>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtFSeason_L.FTPgpCode AS rtPgpCode, TCNMPdtFSeason_L.FNPgpLevel AS rnPgpLevel, TCNMPdtFSeason_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtFSeason_L.FTPgpName AS rtPgpName, TCNMPdtFSeason_L.FTPgpRmk AS rtPgpRmk");
                    oSql.AppendLine("FROM TCNMPdtFSeason_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtFSeason with(nolock) ON TCNMPdtFSeason_L.FTPgpCode = TCNMPdtFSeason.FTPgpCode");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtFSeason.FTPgpChain = TCNMPdtFhn.FTPgpChain");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtSeasonLng = oDB.C_DATaSqlQuery<cmlResInfoPdtSeasonLng>(oSql.ToString());

                    //DCS
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF0DCS.FTDcsCode AS rtDcsCode, TCNMPdtF0DCS.FTDepCode AS rtDepCode, TCNMPdtF0DCS.FTClsCode AS rtClsCode,");
                    oSql.AppendLine("TCNMPdtF0DCS.FTSclCode AS rtSclCode,");
                    oSql.AppendLine("TCNMPdtF0DCS.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF0DCS.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMPdtF0DCS.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF0DCS.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMPdtF0DCS with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF0DCS.FTDcsCode = TCNMPdtFhn.FTDcsCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtDCS = oDB.C_DATaSqlQuery<cmlResInfoPdtDCS>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF0DCS_L.FTDcsCode AS rtDcsCode, TCNMPdtF0DCS_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtF0DCS_L.FTDcsName AS rtDcsName, TCNMPdtF0DCS_L.FTDcsRmk AS rtDcsRmk");
                    oSql.AppendLine("FROM TCNMPdtF0DCS_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtF0DCS with(nolock) ON TCNMPdtF0DCS_L.FTDcsCode = TCNMPdtF0DCS.FTDcsCode");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF0DCS.FTDcsCode = TCNMPdtFhn.FTDcsCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtDCSLng = oDB.C_DATaSqlQuery<cmlResInfoPdtDCSLng>(oSql.ToString());

                    //Depart
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF1Depart.FTDepCode AS rtDepCode,");
                    oSql.AppendLine("TCNMPdtF1Depart.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF1Depart.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMPdtF1Depart.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF1Depart.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMPdtF1Depart with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF1Depart.FTDepCode = TCNMPdtFhn.FTDepCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtDepart = oDB.C_DATaSqlQuery<cmlResInfoPdtDepart>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF1Depart_L.FTDepCode AS rtDepCode, TCNMPdtF1Depart_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtF1Depart_L.FTDepName AS rtDepName, TCNMPdtF1Depart_L.FTDepRmk AS rtDepRmk");
                    oSql.AppendLine("FROM TCNMPdtF1Depart_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtF1Depart with(nolock) ON TCNMPdtF1Depart_L.FTDepCode = TCNMPdtF1Depart.FTDepCode");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF1Depart.FTDepCode = TCNMPdtFhn.FTDepCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtDepartLng = oDB.C_DATaSqlQuery<cmlResInfoPdtDepartLng>(oSql.ToString());

                    //Class
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF2Class.FTClsCode AS rtClsCode,");
                    oSql.AppendLine("TCNMPdtF2Class.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF2Class.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMPdtF2Class.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF2Class.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMPdtF2Class with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF2Class.FTClsCode = TCNMPdtFhn.FTClsCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtClass = oDB.C_DATaSqlQuery<cmlResInfoPdtClass>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF2Class_L.FTClsCode AS rtClsCode, TCNMPdtF2Class_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtF2Class_L.FTClsName AS rtClsName, TCNMPdtF2Class_L.FTClsRmk AS rtClsRmk");
                    oSql.AppendLine("FROM TCNMPdtF2Class_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtF2Class with(nolock) ON TCNMPdtF2Class_L.FTClsCode = TCNMPdtF2Class.FTClsCode");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF2Class.FTClsCode = TCNMPdtFhn.FTClsCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtClassLng = oDB.C_DATaSqlQuery<cmlResInfoPdtClassLng>(oSql.ToString());

                    //SubClass
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF3SubClass.FTSclCode AS rtSclCode,");
                    oSql.AppendLine("TCNMPdtF3SubClass.FDLastUpdOn AS rdLastUpdOn, TCNMPdtF3SubClass.FDCreateOn AS rdCreateOn,");
                    oSql.AppendLine("TCNMPdtF3SubClass.FTLastUpdBy AS rtLastUpdBy, TCNMPdtF3SubClass.FTCreateBy AS rtCreateBy");
                    oSql.AppendLine("FROM TCNMPdtF3SubClass with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF3SubClass.FTSclCode = TCNMPdtFhn.FTSclCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtSubClass = oDB.C_DATaSqlQuery<cmlResInfoPdtSubClass>(oSql.ToString());

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT TCNMPdtF3SubClass_L.FTSclCode AS rtSclCode, TCNMPdtF3SubClass_L.FNLngID AS rnLngID,");
                    oSql.AppendLine("TCNMPdtF3SubClass_L.FTSclName AS rtSclName, TCNMPdtF3SubClass_L.FTSclRmk AS rtSclRmk");
                    oSql.AppendLine("FROM TCNMPdtF3SubClass_L with(nolock)");
                    oSql.AppendLine("INNER JOIN TCNMPdtF3SubClass with(nolock) ON TCNMPdtF3SubClass_L.FTSclCode = TCNMPdtF3SubClass.FTSclCode");
                    oSql.AppendLine("INNER JOIN TCNMPdtFhn with(nolock) ON TCNMPdtF3SubClass.FTSclCode = TCNMPdtFhn.FTSclCode");
                    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), TCNMPdtFhn.FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
                    oPdtDwn.raPdtSubClassLng = oDB.C_DATaSqlQuery<cmlResInfoPdtSubClassLng>(oSql.ToString());
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }

                aoResult.roItem = oPdtDwn;
                // เก็บ KeyApi ลง Cache
                oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResPdtFhnDwn>();
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