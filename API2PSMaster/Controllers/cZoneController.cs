using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Zone;
using API2PSMaster.Models.WebService.Request.Zone;
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
using API2PSMaster.Class;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
	/// <summary>
	///     Zone Information.
	/// </summary>
	[ApiController]
	[Route(cCS.tCS_APIVer + "/Zone")]
	public class cZoneController : ControllerBase
	{
		/// <summary>
		///     Download zone information.
		/// </summary>
		/// <param name="pdDate">date for download (format : yyyy-MM-dd).</param>
		/// <returns></returns>
		[Route("Download")]
		[HttpGet]
		public cmlResItem<cmlResZoneDwn> GET_PDToDownloadZone(DateTime pdDate)
		{
			cDatabase oDB; //*Net 64-10-19
			cSP oFunc;
			cCS oCS;
			cMS oMsg;
			StringBuilder oSql;
			cmlResItem<cmlResZoneDwn> aoResult;
			List<cmlTSysConfig> aoSysConfig;
			cmlResZoneDwn oZoneDwn;
			cCacheFunc oCacheFunc;
			int nRowEff, nCmdTme, nConTme;
			string tFuncName, tModelErr, tKeyApi, tKeyCache;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

				aoResult = new cmlResItem<cmlResZoneDwn>();
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

				tKeyCache = "Zone" + string.Format("{0:yyyyMMdd}", pdDate);
				if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
				{
					// ถ้ามี key อยุ่ใน cache
					aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResZoneDwn>>(tKeyCache);
					//aoResult = oCacheFunc.C_CAHoGetKey<cmlResPdtItemDwn>(tKeyCache);
					aoResult.rtCode = oMsg.tMS_RespCode001;
					aoResult.rtDesc = oMsg.tMS_RespDesc001;
					return aoResult;
				}

				//// Get data
				//oSql = new StringBuilder();
				//oSql.AppendLine("SELECT FTZneChain AS rtZneChain, FTZneCode AS rtZneCode, FNZneLevel AS rnZneLevel,");
				//oSql.AppendLine("FTZneParent AS rtZneParent, FTAreCode AS rtAreCode,");
				//oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
				//oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
				//oSql.AppendLine("FROM TCNMZone with(nolock)");
				//oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

				//aoResult.roItem = new cmlResZoneDwn();
				//oZoneDwn = new cmlResZoneDwn();
				//using (AdaAccEntities oDB = new AdaAccEntities())
				//{
				//    using (DbConnection oConn = oDB.Database.Connection)
				//    {
				//        oConn.Open();
				//        DbCommand oCmd = oConn.CreateCommand();
				//        oCmd.CommandText = oSql.ToString();
				//        using (DbDataReader oDR = oCmd.ExecuteReader())
				//        {
				//            oZoneDwn.raZone = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoZone>(oDR).ToList();
				//            ((IDisposable)oDR).Dispose();
				//        }

				//        if (oZoneDwn.raZone.Count > 0)
				//        {
				//            //Languague
				//            oSql = new StringBuilder();
				//            //oSql.AppendLine("SELECT TCNMZone_L.FTZneChain AS rtZneCode, TCNMZone_L.FNLngID AS rnLngID,"); //*Net 62-12-30 comment out
				//            //oSql.AppendLine("TCNMZone_L.FTZneName AS rtZneName, TCNMZone_L.FTZneRmk AS rtZneRmk"); //*Net 62-12-30 comment out

				//            oSql.AppendLine("SELECT TCNMZone_L.FTZneChain AS rtZneChain, TCNMZone_L.FNLngID AS rnLngID,"); //*Net 62-12-30 Select all field
				//            oSql.AppendLine("TCNMZone_L.FTZneName AS rtZneName, TCNMZone_L.FTZneCode AS rtZneCode,"); //*Net 62-12-30 Select all field
				//            oSql.AppendLine("TCNMZone_L.FTZneChainName AS rtZneChainName, TCNMZone_L.FTZneRmk AS rtZneRmk"); //*Net 62-12-30 Select all field

				//            oSql.AppendLine("FROM TCNMZone_L with(nolock)");
				//            oSql.AppendLine("INNER JOIN TCNMZone with(nolock) ON TCNMZone_L.FTZneChain = TCNMZone.FTZneChain");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMZone.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oZoneDwn.raZoneLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoZoneLng>(oDR).ToList();
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
				aoResult.roItem = new cmlResZoneDwn();
				oZoneDwn = new cmlResZoneDwn();

				// Get data
				oSql = new StringBuilder();
				oSql.AppendLine("SELECT FTZneChain AS rtZneChain, FTZneCode AS rtZneCode, FNZneLevel AS rnZneLevel,");
				oSql.AppendLine("FTZneParent AS rtZneParent, FTAreCode AS rtAreCode,");
				oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
				oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
				oSql.AppendLine("FROM TCNMZone with(nolock)");
				oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				oZoneDwn.raZone = oDB.C_DATaSqlQuery<cmlResInfoZone>(oSql.ToString());
				if (oZoneDwn.raZone.Count > 0)
				{
					//Languague
					oSql = new StringBuilder();
					oSql.AppendLine("SELECT TCNMZone_L.FTZneChain AS rtZneChain, TCNMZone_L.FNLngID AS rnLngID,"); //*Net 62-12-30 Select all field
					oSql.AppendLine("TCNMZone_L.FTZneName AS rtZneName, TCNMZone_L.FTZneCode AS rtZneCode,"); //*Net 62-12-30 Select all field
					oSql.AppendLine("TCNMZone_L.FTZneChainName AS rtZneChainName, TCNMZone_L.FTZneRmk AS rtZneRmk"); //*Net 62-12-30 Select all field

					oSql.AppendLine("FROM TCNMZone_L with(nolock)");
					oSql.AppendLine("INNER JOIN TCNMZone with(nolock) ON TCNMZone_L.FTZneChain = TCNMZone.FTZneChain");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMZone.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oZoneDwn.raZoneLng = oDB.C_DATaSqlQuery<cmlResInfoZoneLng>(oSql.ToString());
				}
				else
				{
					aoResult.rtCode = oMsg.tMS_RespCode800;
					aoResult.rtDesc = oMsg.tMS_RespDesc800;
					return aoResult;
				}
				//++++++++++++++++++++++++++++++++++++++++++++

				aoResult.roItem = oZoneDwn;
				// เก็บ KeyApi ลง Cache
				oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

				aoResult.rtCode = oMsg.tMS_RespCode001;
				aoResult.rtDesc = oMsg.tMS_RespDesc001;
				return aoResult;
			}
			catch (Exception oExcept)
			{
				// Return error.
				aoResult = new cmlResItem<cmlResZoneDwn>();
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

		///// <summary>
		///// Insert Zone
		///// </summary>
		///// <param name="poparam"></param>
		///// <returns></returns>
		//[Route("Insert")]
		//[HttpPost]
		//public cmlResItem<cmlResZoneDwn> POST_PDToInsZone(cmlReqZone poparam)
		//{
		//    cSP oFunc;
		//    cMS oMsg;
		//    cCS oCS;
		//    StringBuilder oSql;
		//    StringBuilder oSQL_L;
		//    List<cmlTSysConfig> aoSysConfig;
		//    cmlResItem<cmlResZoneDwn> aoResult;

		//    int nRowEff, nCmdTme, nConTme;
		//    string tFuncName, tModelErr, tKeyApi, tKeyCache;


		//    try
		//    {

		//        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		//        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
		//        aoResult = new cmlResItem<cmlResZoneDwn>();
		//        oFunc = new cSP();
		//        oMsg = new cMS();
		//        oCS = new cCS();

		//        // Get method name.
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        // Validate parameter.
		//        tModelErr = "";
		//        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
		//        {
		//            // Validate parameter model false.
		//            aoResult.rtCode = oMsg.tMS_RespCode701;
		//            aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
		//            return aoResult;
		//        }

		//        // Load configuration.
		//        aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
		//        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

		//        tKeyApi = "";
		//        // Check KeyApi.
		//        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
		//        {
		//            // Key not allowed to use method.
		//            aoResult.rtCode = oMsg.tMS_RespCode904;
		//            aoResult.rtDesc = oMsg.tMS_RespDesc904;
		//            return aoResult;
		//        }

		//        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
		//        {

		//            oSql = new StringBuilder();
		//            oSql.Clear();
		//            oSql.AppendLine(" INSERT INTO TCNMZone WITH(ROWLOCK)  (FTZneChain, FTZneCode, FNZneLevel,");
		//            oSql.AppendLine("  FTZneParent, FTAreCode, FDDateUpd, FTTimeUpd, ");
		//            oSql.AppendLine("  FTWhoUpd, FDDateIns, FTTimeIns, FTWhoIns)");
		//            oSql.AppendLine(" VALUES ('" + poparam.ptZneChain + "',");
		//            oSql.AppendLine(" '" + poparam.ptZneCode + "',");
		//            oSql.AppendLine(" '" + poparam.pnZneLevel + "',");
		//            oSql.AppendLine("'" + poparam.ptZneParent + "',");
		//            oSql.AppendLine("'" + poparam.ptAreCode +"',");
		//            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
		//            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108), ");
		//            oSql.AppendLine(" '" + poparam.ptWhoUpd + "',");
		//            oSql.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
		//            oSql.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
		//            oSql.AppendLine(" '" + poparam.ptWhoUpd + "')");

		//            oSQL_L = new StringBuilder();
		//            oSQL_L.Clear();
		//            oSQL_L.AppendLine(" INSERT INTO TCNMZone_L WITH(ROWLOCK) ");
		//            oSQL_L.AppendLine(" (FTZneCode, FNLngID, FTZneName, FTZneRmk, FTZneChainName) ");
		//            oSQL_L.AppendLine(" VALUES ('" + poparam.ptZneCode + "',");
		//            oSQL_L.AppendLine(" '" + poparam.pnLngID + "',");
		//            oSQL_L.AppendLine(" '" + poparam.ptZneName + "',");
		//            oSQL_L.AppendLine(" '" + poparam.ptZneRmk + "',");
		//            oSQL_L.AppendLine(" '" + poparam.ptZneChainName + "' ");
		//            oSQL_L.AppendLine(")");


		//            using (DbConnection oConn = oAdaAcc.Database.Connection)
		//            {
		//                oConn.Open();
		//                DbCommand oCmd = oConn.CreateCommand();
		//                DbTransaction oTrans;
		//                oTrans = oConn.BeginTransaction();
		//                oCmd.Connection = oConn;
		//                oCmd.Transaction = oTrans;
		//                try
		//                {
		//                    oCmd.CommandTimeout = nCmdTme;
		//                    oCmd.CommandType = CommandType.Text;
		//                    oCmd.CommandText = oSql.ToString();
		//                    oCmd.ExecuteNonQuery();

		//                    oCmd.CommandTimeout = nCmdTme;
		//                    oCmd.CommandType = CommandType.Text;
		//                    oCmd.CommandText = oSQL_L.ToString();
		//                    oCmd.ExecuteNonQuery();

		//                    oTrans.Commit();
		//                }
		//                catch (SqlException oSqlExn)
		//                {
		//                    oTrans.Rollback();
		//                    switch (oSqlExn.Number)
		//                    {
		//                        case 2627:
		//                            // Data is duplicate.
		//                            aoResult.rtCode = oMsg.tMS_RespCode801;
		//                            aoResult.rtDesc = oMsg.tMS_RespDesc801;
		//                            return aoResult;

		//                        default:
		//                            //Error statement or sql error
		//                            aoResult.rtCode = oMsg.tMS_RespCode999;
		//                            aoResult.rtDesc = oSqlExn.Message;
		//                            return aoResult;
		//                    }
		//                }
		//                catch (EntityException oEtyExn)
		//                {
		//                    oTrans.Rollback();
		//                    switch (oEtyExn.HResult)
		//                    {
		//                        case -2146232060:
		//                            // Cannot connect database..
		//                            aoResult.rtCode = oMsg.tMS_RespCode905;
		//                            aoResult.rtDesc = oMsg.tMS_RespDesc905;
		//                            return aoResult;
		//                    }
		//                }
		//            }
		//        }
		//        aoResult.rtCode = oMsg.tMS_RespCode001;
		//        aoResult.rtDesc = oMsg.tMS_RespDesc001;
		//        return aoResult;


		//    }
		//    catch(Exception ex)
		//    {
		//        // Return error.
		//        aoResult = new cmlResItem<cmlResZoneDwn>();
		//        aoResult.rtCode = new cMS().tMS_RespCode900;
		//        aoResult.rtDesc = new cMS().tMS_RespDesc900;
		//        return aoResult;
		//    }
		//    finally
		//    {
		//        oFunc = null;
		//        oMsg = null;
		//        oSql = null;

		//        //GC.Collect();
		//        //GC.WaitForPendingFinalizers();
		//        //GC.Collect();
		//    }

		//}


		///// <summary>
		///// Delete Zone
		///// </summary>
		///// <param name="ptZneChain"></param>
		///// <returns></returns>
		//[Route("Delete")]
		//[HttpPost]
		//public cmlResItem<cmlResZoneDwn> POST_PDToDelZone(cmlReqZoneDel poparam)
		//{
		//    cSP oFunc;
		//    cCS oCS;
		//    cMS oMsg;
		//    StringBuilder oSql;
		//    StringBuilder oSql_L;
		//    List<cmlTSysConfig> aoSysConfig;
		//    cmlResItem<cmlResZoneDwn> aoResult;
		//    int nRowEff, nCmdTme, nConTme;
		//    string tFuncName, tModelErr, tKeyApi, tKeyCache;


		//    try
		//    {

		//        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		//        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

		//        oFunc = new cSP();
		//        oCS = new cCS();
		//        oMsg = new cMS();
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        aoResult = new cmlResItem<cmlResZoneDwn>();
		//        // Get method name.
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        // Validate parameter.
		//        tModelErr = "";
		//        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
		//        {
		//            // Validate parameter model false.
		//            aoResult.rtCode = oMsg.tMS_RespCode701;
		//            aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
		//            return aoResult;
		//        }

		//        // Load configuration.
		//        aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
		//        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

		//        tKeyApi = "";
		//        // Check KeyApi.
		//        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
		//        {
		//            // Key not allowed to use method.
		//            aoResult.rtCode = oMsg.tMS_RespCode904;
		//            aoResult.rtDesc = oMsg.tMS_RespDesc904;
		//            return aoResult;
		//        }

		//        using (AdaAccEntities oAdaAcc = new AdaAccEntities())
		//        {

		//            oSql = new StringBuilder();
		//            oSql.Clear();
		//            oSql.AppendLine(" DELETE ");
		//            oSql.AppendLine(" FROM TCNMZone  WITH(ROWLOCK) ");
		//            oSql.AppendLine(" WHERE (FTZneChain = '" + poparam.ptZneChain + "')");

		//            oSql_L = new StringBuilder();
		//            oSql_L.Clear();
		//            oSql_L.AppendLine(" DELETE ");
		//            oSql_L.AppendLine(" FROM TCNMZone_L WITH(ROWLOCK)");
		//            oSql_L.AppendLine(" WHERE (FTZneCode = '" + poparam.ptZneCode + "')");
		//            oSql_L.AppendLine(" AND FNLngID = '" + poparam.pnLngID + "' ");


		//            using (DbConnection oConn = oAdaAcc.Database.Connection)
		//            {
		//                oConn.Open();
		//                DbCommand oCmd = oConn.CreateCommand();
		//                DbTransaction oTrans;
		//                oTrans = oConn.BeginTransaction();
		//                oCmd.Connection = oConn;
		//                oCmd.Transaction = oTrans;
		//                try
		//                {
		//                    oCmd.CommandTimeout = nCmdTme;
		//                    oCmd.CommandType = CommandType.Text;
		//                    oCmd.CommandText = oSql.ToString();
		//                    oCmd.ExecuteNonQuery();

		//                    oCmd.CommandTimeout = nCmdTme;
		//                    oCmd.CommandType = CommandType.Text;
		//                    oCmd.CommandText = oSql_L.ToString();
		//                    int nSuccess = oCmd.ExecuteNonQuery();

		//                    if(nSuccess == 0)
		//                    {
		//                        aoResult = new cmlResItem<cmlResZoneDwn>();
		//                        //aoResult = new cmlResPdtItemDwn();
		//                        aoResult.rtCode = "";
		//                        aoResult.rtDesc = "";
		//                        return aoResult;
		//                    }


		//                    oTrans.Commit();
		//                }
		//                catch (SqlException oSqlExn)
		//                {
		//                    oTrans.Rollback();
		//                    switch (oSqlExn.Number)
		//                    {
		//                        case 2627:
		//                            // Data is duplicate.
		//                            aoResult.rtCode = oMsg.tMS_RespCode801;
		//                            aoResult.rtDesc = oMsg.tMS_RespDesc801;
		//                            return aoResult;

		//                        default:
		//                            //Error statement or sql error
		//                            aoResult.rtCode = oMsg.tMS_RespCode999;
		//                            aoResult.rtDesc = oSqlExn.Message;
		//                            return aoResult;
		//                    }
		//                }
		//                catch (EntityException oEtyExn)
		//                {
		//                    oTrans.Rollback();
		//                    switch (oEtyExn.HResult)
		//                    {
		//                        case -2146232060:
		//                            // Cannot connect database..
		//                            aoResult.rtCode = oMsg.tMS_RespCode905;
		//                            aoResult.rtDesc = oMsg.tMS_RespDesc905;
		//                            return aoResult;
		//                    }
		//                }
		//            }
		//        }
		//        aoResult.rtCode = oMsg.tMS_RespCode001;
		//        aoResult.rtDesc = oMsg.tMS_RespDesc001;
		//        return aoResult;



		//    }
		//    catch (Exception ex)
		//    {
		//        // Return error.
		//        aoResult = new cmlResItem<cmlResZoneDwn>();
		//        aoResult.rtCode = new cMS().tMS_RespCode900;
		//        aoResult.rtDesc = new cMS().tMS_RespDesc900;
		//        return aoResult;
		//    }
		//    finally
		//    {
		//        oFunc = null;
		//        oMsg = null;
		//        oSql = null;

		//        //GC.Collect();
		//        //GC.WaitForPendingFinalizers();
		//        //GC.Collect();
		//    }
		//}

		///// <summary>
		///// Udpate Zone
		///// </summary>
		///// <param name="poparam"></param>
		///// <returns></returns>
		//[Route("Update")]
		//[HttpPost]
		//public cmlResItem<cmlResZoneDwn> POST_PDToUpdZone(cmlReqZone poparam)
		//{
		//    cSP oFunc;
		//    cCS oCS;
		//    cMS oMsg;
		//    StringBuilder oSql;
		//    StringBuilder oSql_L;
		//    List<cmlTSysConfig> aoSysConfig;
		//    cmlResItem<cmlResZoneDwn> aoResult;
		//    cDatabase oDatabase;
		//    int nRowEff, nCmdTme, nConTme;
		//    string tFuncName, tModelErr, tKeyApi, tKeyCache;
		//    DataTable oDbTblZone;
		//    DataTable oDbTblZone_L;

		//    try
		//    {

		//        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		//        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
		//        aoResult = new cmlResItem<cmlResZoneDwn>();
		//        oFunc = new cSP();
		//        oMsg = new cMS();
		//        oCS = new cCS();


		//        // Get method name.
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        // Validate parameter.
		//        tModelErr = "";
		//        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
		//        {
		//            // Validate parameter model false.
		//            aoResult.rtCode = oMsg.tMS_RespCode701;
		//            aoResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
		//            return aoResult;
		//        }

		//        // Load configuration.
		//        aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
		//        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

		//        tKeyApi = "";
		//        // Check KeyApi.
		//        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
		//        {
		//            // Key not allowed to use method.
		//            aoResult.rtCode = oMsg.tMS_RespCode904;
		//            aoResult.rtDesc = oMsg.tMS_RespDesc904;
		//            return aoResult;
		//        }

		//        oDatabase = new cDatabase();
		//        oSql = new StringBuilder();
		//        oSql.AppendLine("SELECT TOP(1)  FTZneChain, FTZneCode, FNZneLevel, FTZneParent, FTAreCode  ");
		//        oSql.AppendLine("FROM TCNMZone WITH(NOLOCK)");
		//        oSql.AppendLine("WHERE FTZneChain = '" + poparam.ptZneChain + "'");
		//        oDbTblZone = oDatabase.C_DAToSqlQuery(oSql.ToString());

		//        oSql = new StringBuilder();
		//        oSql.AppendLine(" SELECT TOP(1)  FTZneName, FTZneRmk, FTZneChainName");
		//        oSql.AppendLine(" FROM TCNMZone_L");
		//        oSql.AppendLine(" WHERE ");
		//        oSql.AppendLine(" FTZneCode = '" + poparam.ptZneCode + "' AND ");
		//        oSql.AppendLine(" FNLngID = '" + poparam.pnLngID +"'");
		//        oDbTblZone_L = oDatabase.C_DAToSqlQuery(oSql.ToString());

		//        if (oDbTblZone != null && oDbTblZone.Rows.Count > 0)
		//        {
		//            if (oDbTblZone_L != null && oDbTblZone_L.Rows.Count > 0)
		//            {
		//                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
		//                {
		//                    //string tUpdZneCode = "";
		//                    //string tUpdZneLevel = "";
		//                    //string tUpdZneParent = "";
		//                    //string tAreCode = "";
		//                    //string tZneRmk = "";
		//                    //string tZneChainName = "";


		//                    //if (poparam.ptZneCode != "")
		//                    //{
		//                    //    tUpdZneCode += "FTZneCode = '" + poparam.ptZneCode + "',";
		//                    //}

		//                    //if (poparam.ptZneLevel != 0)
		//                    //{
		//                    //    tUpdZneLevel += "FNZneLevel = '" + poparam.ptZneLevel + "',";
		//                    //}

		//                    //if (poparam.ptZneParent != "")
		//                    //{
		//                    //    tUpdZneParent += "FTZneParent = '" + poparam.ptZneParent + "',";
		//                    //}

		//                    //if (poparam.ptAreCode != "")
		//                    //{
		//                    //    tAreCode += "FTAreCode = '" + poparam.ptAreCode + "',";
		//                    //}

		//                    oSql = new StringBuilder();
		//                    oSql.Clear();
		//                    oSql.AppendLine(" UPDATE ");
		//                    oSql.AppendLine(" TCNMZone");
		//                    oSql.AppendLine(" WITH(ROWLOCK)");
		//                    oSql.AppendLine(" SET " + oFunc.SP_SETtValueUpt(poparam.ptZneCode, "FTZneCode") + "");
		//                    oSql.AppendLine("     " + oFunc.SP_SETtValueUpt(Convert.ToString(poparam.pnZneLevel), "FNZneLevel") + "");
		//                    oSql.AppendLine("     " + oFunc.SP_SETtValueUpt(poparam.ptZneParent, "FTZneParent") + "");
		//                    oSql.AppendLine("     " + oFunc.SP_SETtValueUpt(poparam.ptAreCode, "FTAreCode") + "");
		//                    oSql.AppendLine(" FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),  ");
		//                    oSql.AppendLine(" FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),108) , ");
		//                    oSql.AppendLine(" FTWhoUpd = '" + poparam.ptWhoUpd + "'");
		//                    oSql.AppendLine(" WHERE FTZneChain = '" + poparam.ptZneChain + "'");


		//                    //if(poparam.ptZneRmk != "")
		//                    //{
		//                    //    tZneRmk += "FTZneRmk = '" + poparam.ptZneRmk +"',";
		//                    //}

		//                    //if(poparam.ptZneChainName != "")
		//                    //{
		//                    //    tZneChainName += "FTZneChainName = '" + poparam.ptZneChainName +"',";
		//                    //}

		//                    oSql_L = new StringBuilder();
		//                    oSql_L.Clear();
		//                    oSql_L.AppendLine(" UPDATE ");
		//                    oSql_L.AppendLine(" TCNMZone_L ");
		//                    oSql_L.AppendLine(" WITH(ROWLOCK)");
		//                    oSql_L.AppendLine(" SET " + oFunc.SP_SETtValueUpt(poparam.ptZneRmk, "FTZneRmk") + "");
		//                    oSql_L.AppendLine("     " + oFunc.SP_SETtValueUpt(poparam.ptZneChainName, "FTZneChainName") + "");
		//                    oSql_L.AppendLine("     FTZneName = '" + poparam.ptZneName + "'");
		//                    oSql_L.AppendLine(" WHERE FTZneCode = '" + poparam.ptZneCode + "' ");

		//                    using (DbConnection oConn = oAdaAcc.Database.Connection)
		//                    {
		//                        oConn.Open();
		//                        DbCommand oCmd = oConn.CreateCommand();
		//                        DbTransaction oTrans;
		//                        oTrans = oConn.BeginTransaction();
		//                        oCmd.Connection = oConn;
		//                        oCmd.Transaction = oTrans;
		//                        try
		//                        {
		//                            oCmd.CommandTimeout = nCmdTme;
		//                            oCmd.CommandType = CommandType.Text;
		//                            oCmd.CommandText = oSql.ToString();
		//                            oCmd.ExecuteNonQuery();

		//                            oCmd.CommandTimeout = nCmdTme;
		//                            oCmd.CommandType = CommandType.Text;
		//                            oCmd.CommandText = oSql_L.ToString();
		//                            oCmd.ExecuteNonQuery();

		//                            oTrans.Commit();
		//                        }
		//                        catch (SqlException oSqlExn)
		//                        {
		//                            oTrans.Rollback();
		//                            switch (oSqlExn.Number)
		//                            {
		//                                case 2627:
		//                                    // Data is duplicate.
		//                                    aoResult.rtCode = oMsg.tMS_RespCode801;
		//                                    aoResult.rtDesc = oMsg.tMS_RespDesc801;
		//                                    return aoResult;

		//                                default:
		//                                    //Error statement or sql error
		//                                    aoResult.rtCode = oMsg.tMS_RespCode999;
		//                                    aoResult.rtDesc = oSqlExn.Message;
		//                                    return aoResult;
		//                            }
		//                        }
		//                        catch (EntityException oEtyExn)
		//                        {
		//                            oTrans.Rollback();
		//                            switch (oEtyExn.HResult)
		//                            {
		//                                case -2146232060:
		//                                    // Cannot connect database..
		//                                    aoResult.rtCode = oMsg.tMS_RespCode905;
		//                                    aoResult.rtDesc = oMsg.tMS_RespDesc905;
		//                                    return aoResult;
		//                            }
		//                        }
		//                    }
		//                }

		//                aoResult.rtCode = oMsg.tMS_RespCode001;
		//                aoResult.rtDesc = oMsg.tMS_RespDesc001;
		//                return aoResult;
		//            }
		//            else
		//            {
		//                aoResult = new cmlResItem<cmlResZoneDwn>();
		//                //aoResult = new cmlResPdtItemDwn();
		//                aoResult.rtCode = "";
		//                aoResult.rtDesc = "";
		//                return aoResult;
		//            }

		//            //aoResult.rtCode = oMsg.tMS_RespCode001;
		//            //aoResult.rtDesc = oMsg.tMS_RespDesc001;
		//            //return aoResult;
		//        }
		//        else
		//        {
		//            aoResult = new cmlResItem<cmlResZoneDwn>();
		//            //aoResult = new cmlResPdtItemDwn();
		//            aoResult.rtCode = "";
		//            aoResult.rtDesc = "";
		//            return aoResult;

		//        }



		//    }
		//    catch (Exception oEx)
		//    {
		//        // Return error.
		//        aoResult = new cmlResItem<cmlResZoneDwn>();
		//        aoResult.rtCode = new cMS().tMS_RespCode900;
		//        aoResult.rtDesc = new cMS().tMS_RespDesc900;
		//        return aoResult;
		//    }
		//    finally
		//    {
		//        oFunc = null;
		//        oMsg = null;
		//        oSql = null;

		//        //GC.Collect();
		//        //GC.WaitForPendingFinalizers();
		//        //GC.Collect();
		//    }

		//}
	}
}