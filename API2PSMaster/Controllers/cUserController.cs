using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Class.User;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.User;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.Center;
using API2PSMaster.Models.WebService.Response.Image;
using API2PSMaster.Models.WebService.Response.User;
using API2PSMaster.Models.WebService.Response.UserGrp;
using API2PSMaster.Models.WebService.Response.UserRole;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
	/// <summary>
	/// User information.
	/// </summary>
	[ApiController]
	[Route(cCS.tCS_APIVer + "/User")]
	public class cUserController : ControllerBase
	{

		///// <summary>
		/////     Insert User.
		///// </summary>
		///// 
		///// <param name="poPara">User information.</param>
		///// 
		///// <returns>
		/////     User varify false.<br/>
		/////     System process status.<br/>
		/////     1   : success.<br/>
		/////     801 : data is duplicate.<br/>
		/////     900 : service process false.<br/>
		/////     904 : key not allowed to use method.<br/>
		/////     905 : cannot connect database.<br/>
		/////     906 : this time not allowed to use method.<br/>
		///// </returns>
		//[Route("Insert/Item")]
		//[HttpPost]
		//public cmlResItem<cmlResUsrInsItem> POST_PUNoInsUsrItem([FromBody] cmlReqUsrInsItem poPara)
		//{
		//    cSP oFunc;
		//    cCS oCons;
		//    cMS oMsg;
		//    cDatabase oDatabase;
		//    StringBuilder oSql;
		//    cUser oUser;
		//    List<cmlTSysConfig> aoSysConfig;
		//    cmlResUsrInsItem oResUsrErr;
		//    cmlResItem<cmlResUsrInsItem> oResult;
		//    int nRowEff, nConTme, nCmdTme;
		//    string tFuncName, tModelErr, tKeyApi, tErrCode, tErrDesc;
		//    bool bVerifyPara;
		//    try
		//    {
		//        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		//        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

		//        oResult = new cmlResItem<cmlResUsrInsItem>();
		//        oFunc = new cSP();
		//        oCons = new cCS();
		//        oMsg = new cMS();

		//        // Get method name.
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        // Validate parameter.
		//        tModelErr = "";
		//        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
		//        {
		//            // Load configuration.
		//            aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

		//            // Check range time use function.
		//            if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
		//            {
		//                tKeyApi = "";
		//                // Check KeyApi.
		//                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
		//                {
		//                    // Varify parameter value.
		//                    oUser = new cUser();
		//                    bVerifyPara = oUser.C_DATbVerifyInsItemParameterValue(poPara, aoSysConfig, out tErrCode, out tErrDesc, out oResUsrErr);
		//                    if (bVerifyPara == true)
		//                    {
		//                        // Insert.
		//                        oSql = new StringBuilder();
		//                        oSql.AppendLine("INSERT INTO TCNMUser WITH(ROWLOCK)");
		//                        oSql.AppendLine("(");
		//                        oSql.AppendLine("	FTUsrCode, FTDptCode,");
		//                        oSql.AppendLine("	FTRolCode, FTUsrTel,");
		//                        oSql.AppendLine("	FTUsrPwd, FTUsrEmail,");
		//                        oSql.AppendLine("	FDDateUpd, FTTimeUpd, FTWhoUpd,");
		//                        oSql.AppendLine("	FDDateIns, FTTimeIns, FTWhoIns");
		//                        oSql.AppendLine(")");
		//                        oSql.AppendLine("VALUES");
		//                        oSql.AppendLine("(");
		//                        oSql.AppendLine("	'" + poPara.ptUsrCode + "', '" + poPara.ptDptCode + "',");
		//                        oSql.AppendLine("	'" + poPara.ptRolCode + "', '" + poPara.ptUsrTel + "',");
		//                        oSql.AppendLine("	'" + poPara.ptUsrPwd + "', '" + poPara.ptUsrEmail + "',");
		//                        oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), 'AdaLink',");
		//                        oSql.AppendLine("	CONVERT(VARCHAR(10), GETDATE(), 121), CONVERT(VARCHAR(8),GETDATE(),108), 'AdaLink'");
		//                        oSql.AppendLine(")");

		//                        try
		//                        {
		//                            // Confuguration database.
		//                            nConTme = 0;
		//                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "003");
		//                            nCmdTme = 0;
		//                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "004");

		//                            oDatabase = new cDatabase(nConTme);
		//                            nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
		//                        }
		//                        catch (SqlException oSqlExn)
		//                        {
		//                            switch (oSqlExn.Number)
		//                            {
		//                                case 2627:
		//                                    // Data is duplicate.
		//                                    oResult.roItem = new cmlResUsrInsItem();
		//                                    oResult.roItem.rtUsrCode = poPara.ptUsrCode;
		//                                    oResult.rtCode = oMsg.tMS_RespCode801;
		//                                    oResult.rtDesc = oMsg.tMS_RespDesc801 + " (User).";
		//                                    return oResult;
		//                            }
		//                        }
		//                        catch (EntityException oEtyExn)
		//                        {
		//                            switch (oEtyExn.HResult)
		//                            {
		//                                case -2146232060:
		//                                    // Cannot connect database..
		//                                    oResult.rtCode = oMsg.tMS_RespCode905;
		//                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
		//                                    return oResult;
		//                            }
		//                        }
		//                        oResult.rtCode = oMsg.tMS_RespCode001;
		//                        oResult.rtDesc = oMsg.tMS_RespDesc001;
		//                        return oResult;
		//                    }
		//                    else
		//                    {
		//                        // Varify parameter value false.
		//                        oResult.roItem = oResUsrErr;
		//                        oResult.rtCode = tErrCode;
		//                        oResult.rtDesc = tErrDesc;
		//                        return oResult;
		//                    }

		//                }
		//                else
		//                {
		//                    // Key not allowed to use method.
		//                    oResult.rtCode = oMsg.tMS_RespCode904;
		//                    oResult.rtDesc = oMsg.tMS_RespDesc904;
		//                    return oResult;
		//                }
		//            }
		//            else
		//            {
		//                // This time not allowed to use method.
		//                oResult.rtCode = oMsg.tMS_RespCode906;
		//                oResult.rtDesc = oMsg.tMS_RespDesc906;
		//                return oResult;
		//            }
		//        }
		//        else
		//        {
		//            // Validate parameter model false.
		//            oResult.rtCode = oMsg.tMS_RespCode701;
		//            oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
		//            return oResult;
		//        }
		//    }
		//    catch (Exception)
		//    {
		//        // Return error.
		//        oResult = new cmlResItem<cmlResUsrInsItem>();
		//        oResult.rtCode = new cMS().tMS_RespCode900;
		//        oResult.rtDesc = new cMS().tMS_RespDesc900;
		//        return oResult;
		//    }
		//    finally
		//    {
		//        oFunc = null;
		//        oCons = null;
		//        oMsg = null;
		//        oDatabase = null;
		//        oSql = null;
		//        aoSysConfig = null;
		//        oResUsrErr = null;

		//        //GC.Collect();
		//        //GC.WaitForPendingFinalizers();
		//        //GC.Collect();
		//    }
		//}

		///// <summary>
		/////     Update User.
		///// </summary>
		///// 
		///// <param name="poPara">User information.</param>
		///// 
		///// <returns>
		/////     Product varify false.<br/>
		/////     System process status.<br/>
		/////     1   : success.<br/>
		/////     802 : formate data incorrect..<br/>
		/////     900 : service process false.<br/>
		/////     904 : key not allowed to use method.<br/>
		/////     905 : cannot connect database.<br/>
		/////     906 : this time not allowed to use method.<br/>
		///// </returns>
		//[Route("Update/Item")]
		//[HttpPost]
		//public cmlResItem<cmlResUsrUpdItem> POST_PUNoUpdateUsrItem([FromBody] cmlReqUsrUpdItem poPara)
		//{
		//    cSP oFunc;
		//    cCS oCons;
		//    cMS oMsg;
		//    cUser oUser;
		//    cDatabase oDatabase;
		//    StringBuilder oSql;
		//    List<cmlTSysConfig> aoSysConfig;
		//    cmlResItem<cmlResUsrUpdItem> oResult;
		//    cmlResUsrUpdItem oResPgpErr;
		//    int nRowEff, nConTme, nCmdTme;
		//    string tFuncName, tModelErr, tKeyApi, tErrCode, tErrDesc;
		//    bool bVarifyPara;
		//    try
		//    {
		//        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		//        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

		//        oResult = new cmlResItem<cmlResUsrUpdItem>();
		//        oFunc = new cSP();
		//        oCons = new cCS();
		//        oMsg = new cMS();

		//        // Get method name.
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        // Validate parameter.
		//        tModelErr = "";
		//        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
		//        {
		//            // Load configuration.
		//            aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

		//            // Check range time use function.
		//            if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
		//            {
		//                tKeyApi = "";
		//                // Check KeyApi.
		//                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
		//                {
		//                    oUser = new cUser();
		//                    bVarifyPara = oUser.C_DATbVerifyUpdItemParameterValue(poPara, aoSysConfig, out tErrCode, out tErrDesc, out oResPgpErr);
		//                    if (bVarifyPara == true)
		//                    {
		//                        // Update.


		//                        oSql = new StringBuilder();
		//                        oSql.AppendLine("UPDATE TCNMUser WITH(ROWLOCK) SET ");

		//                        if (poPara.ptDptCode != null)
		//                        {
		//                            oSql.AppendLine("FTDptCode='" + poPara.ptDptCode + "',");
		//                        }

		//                        if (poPara.ptRolCode != null)
		//                        {
		//                            oSql.AppendLine("FTRolCode='" + poPara.ptRolCode + "',");
		//                        }

		//                        if (poPara.ptUsrTel != null)
		//                        {
		//                            oSql.AppendLine("FTUsrTel='" + poPara.ptUsrTel + "',");
		//                        }

		//                        if (poPara.ptUsrPwd != null)
		//                        {
		//                            oSql.AppendLine("FTUsrPwd = '" + poPara.ptUsrPwd + "',");
		//                        }

		//                        if (poPara.ptUsrEmail != null)
		//                        {
		//                            oSql.AppendLine("FTUsrEmail = '" + poPara.ptUsrEmail + "',");
		//                        }

		//                        oSql.AppendLine("FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),");
		//                        oSql.AppendLine("FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),114),");
		//                        oSql.AppendLine("FTWhoUpd = 'AdaLink'");
		//                        oSql.AppendLine("WHERE FTUsrCode = '" + poPara.ptUsrCode + "'");

		//                        try
		//                        {
		//                            // Confuguration database.
		//                            nConTme = 0;
		//                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "003");
		//                            nCmdTme = 0;
		//                            oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "004");

		//                            oDatabase = new cDatabase(nConTme);
		//                            nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);


		//                            if (nRowEff == 0)
		//                            {
		//                                // Data not  found.
		//                                oResult.rtCode = oMsg.tMS_RespCode800;
		//                                oResult.rtDesc = oMsg.tMS_RespDesc800 + "(User)";
		//                                return oResult;
		//                            }
		//                        }
		//                        catch (EntityException oEtyExn)
		//                        {
		//                            switch (oEtyExn.HResult)
		//                            {
		//                                case -2146232060:
		//                                    // Cannot connect database..
		//                                    oResult.rtCode = oMsg.tMS_RespCode905;
		//                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
		//                                    return oResult;
		//                            }
		//                        }

		//                        // Success.
		//                        oResult.rtCode = oMsg.tMS_RespCode001;
		//                        oResult.rtDesc = oMsg.tMS_RespDesc001;
		//                        return oResult;
		//                    }
		//                    else
		//                    {
		//                        // Varify parameter value false.
		//                        oResult.roItem = oResPgpErr;
		//                        oResult.rtCode = tErrCode;
		//                        oResult.rtDesc = tErrDesc;
		//                        return oResult;
		//                    }

		//                }
		//                else
		//                {
		//                    // Key not allowed to use method.
		//                    oResult.rtCode = oMsg.tMS_RespCode904;
		//                    oResult.rtDesc = oMsg.tMS_RespDesc904;
		//                    return oResult;
		//                }
		//            }
		//            else
		//            {
		//                // This time not allowed to use method.
		//                oResult.rtCode = oMsg.tMS_RespCode906;
		//                oResult.rtDesc = oMsg.tMS_RespDesc906;
		//                return oResult;
		//            }
		//        }
		//        else
		//        {
		//            // Validate parameter model false.
		//            oResult.rtCode = oMsg.tMS_RespCode701;
		//            oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
		//            return oResult;
		//        }
		//    }
		//    catch (Exception)
		//    {
		//        // Return error.
		//        oResult = new cmlResItem<cmlResUsrUpdItem>();
		//        oResult.rtCode = new cMS().tMS_RespCode900;
		//        oResult.rtDesc = new cMS().tMS_RespDesc900;
		//        return oResult;
		//    }
		//    finally
		//    {
		//        oFunc = null;
		//        oCons = null;
		//        oMsg = null;
		//        oDatabase = null;
		//        oSql = null;
		//        aoSysConfig = null;

		//        //GC.Collect();
		//        //GC.WaitForPendingFinalizers();
		//        //GC.Collect();
		//    }
		//}

		///// <summary>
		/////     Delete User(Item).
		///// </summary>
		///// 
		///// <param name="poPara">User information.</param>
		///// 
		///// <returns>
		/////     Product varify false.<br/>
		/////     System process status.<br/>
		/////     1   : success.<br/>
		/////     701 : validate parameter model false.<br/>
		/////     800 : data not found.<br/>
		/////     813 : data is referent in another data.<br/>
		/////     900 : service process false.<br/>
		/////     904 : key not allowed to use method.<br/>
		/////     905 : cannot connect database.<br/>
		/////     906 : this time not allowed to use method.<br/>
		///// </returns>
		//[Route("Delete/Item")]
		//[HttpPost]
		//public cmlResStatus POST_PDToDelUsrItem([FromBody] cmlReqUsrDelItem poPara)
		//{
		//    cSP oFunc;
		//    cCS oCons;
		//    cMS oMsg;
		//    cDatabase oDatabase;
		//    StringBuilder oSql;
		//    List<cmlTSysConfig> aoSysConfig;
		//    cmlResStatus oResult;
		//    int nRowEff, nConTme, nCmdTme;
		//    string tFuncName, tModelErr, tKeyApi;

		//    try
		//    {
		//        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		//        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

		//        oResult = new cmlResStatus();
		//        oFunc = new cSP();
		//        oCons = new cCS();
		//        oMsg = new cMS();

		//        // Get method name.
		//        tFuncName = MethodBase.GetCurrentMethod().Name;

		//        // Validate parameter.
		//        tModelErr = "";
		//        if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
		//        {
		//            // Load configuration.
		//            aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

		//            // Check range time use function.
		//            if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
		//            {
		//                tKeyApi = "";
		//                // Check KeyApi.
		//                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
		//                {
		//                    ////oSql = new StringBuilder();
		//                    ////oSql.AppendLine("SELECT TOP 1 G.FTPgpChain ");
		//                    ////oSql.AppendLine("FROM TCNMPdt P ");
		//                    ////oSql.AppendLine("LEFT JOIN TCNMPdtGrp ");
		//                    ////oSql.AppendLine("G ON(P.FTPgpChain=G.FTPgpChain) ");
		//                    ////oSql.AppendLine("WHERE G.FTPgpCode='" + poPara.ptPgpCode + "'");

		//                    try
		//                    {
		//                        // Confuguration database.
		//                        nConTme = 0;
		//                        oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "003");
		//                        nCmdTme = 0;
		//                        oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "004");

		//                        oDatabase = new cDatabase(nConTme);
		//                   ////     tPdtCode = oDatabase.C_DAToSqlQuery<string>(oSql.ToString(), nCmdTme);

		//                   ////     if (string.IsNullOrEmpty(tPdtCode))
		//                   ////     {
		//                            oSql = new StringBuilder();
		//                            oSql.AppendLine("DELETE");
		//                            oSql.AppendLine("FROM TCNMUser WITH(ROWLOCK)");
		//                            oSql.AppendLine("WHERE FTUsrCode = '" + poPara.ptUsrCode + "'");

		//                            nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
		//                            if (nRowEff > 0)
		//                            {
		//                                // Success.
		//                                oResult.rtCode = oMsg.tMS_RespCode001;
		//                                oResult.rtDesc = oMsg.tMS_RespDesc001;
		//                                return oResult;
		//                            }
		//                            else
		//                            {
		//                                // Data not  found.
		//                                oResult.rtCode = oMsg.tMS_RespCode800;
		//                                oResult.rtDesc = oMsg.tMS_RespDesc800 + "(Product Group)";
		//                                return oResult;
		//                            }
		//                    ////    }
		//                    ////    else
		//                    ////    {
		//                     ////       // Product reference in sale process.
		//                     ////       oResult.rtCode = oMsg.tMS_RespCode813;
		//                     ////       oResult.rtDesc = oMsg.tMS_RespDesc813;
		//                     ////       return oResult;
		//                     ////   }
		//                    }
		//                    catch (SqlException oSqlExn)
		//                    {
		//                        throw oSqlExn;
		//                    }
		//                    catch (EntityException oEtyExn)
		//                    {
		//                        switch (oEtyExn.HResult)
		//                        {
		//                            case -2146232060:
		//                                // Cannot connect database..
		//                                oResult.rtCode = oMsg.tMS_RespCode905;
		//                                oResult.rtDesc = oMsg.tMS_RespDesc905;
		//                                break;
		//                        }

		//                        return oResult;
		//                    }
		//                    catch (Exception oExn)
		//                    {
		//                        throw oExn;
		//                    }
		//                }
		//                else
		//                {
		//                    // Key not allowed to use method.
		//                    oResult.rtCode = oMsg.tMS_RespCode904;
		//                    oResult.rtDesc = oMsg.tMS_RespDesc904;
		//                    return oResult;
		//                }
		//            }
		//            else
		//            {
		//                // This time not allowed to use method.
		//                oResult.rtCode = oMsg.tMS_RespCode906;
		//                oResult.rtDesc = oMsg.tMS_RespDesc906;
		//                return oResult;
		//            }
		//        }
		//        else
		//        {
		//            // Validate parameter model false.
		//            oResult.rtCode = oMsg.tMS_RespCode701;
		//            oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
		//            return oResult;
		//        }
		//    }
		//    catch (Exception)
		//    {
		//        // Return error.
		//        oResult = new cmlResStatus();
		//        oResult.rtCode = new cMS().tMS_RespCode900;
		//        oResult.rtDesc = new cMS().tMS_RespDesc900;
		//        return oResult;
		//    }
		//    finally
		//    {
		//        oFunc = null;
		//        oCons = null;
		//        oMsg = null;
		//        oDatabase = null;
		//        oSql = null;
		//        aoSysConfig = null;

		//        //GC.Collect();
		//        //GC.WaitForPendingFinalizers();
		//        //GC.Collect();
		//    }
		//}

		/// <summary>
		///     Download user information.
		/// </summary>
		/// <param name="pdDate">date last update (format : yyyy-MM-dd).</param>
		/// <returns></returns>
		[Route("Download")]
		[HttpGet]
		public cmlResItem<cmlResUserDwn> GET_USRoDownloadUser(DateTime pdDate)
		{
			cDatabase oDB; //*Net 64-10-19
			cSP oFunc;
			cCS oCS;
			cMS oMsg;
			StringBuilder oSql;
			cmlResItem<cmlResUserDwn> aoResult;
			List<cmlTSysConfig> aoSysConfig;
			cmlResUserDwn oUserDwn;
			cCacheFunc oCacheFunc;
			int nCmdTme, nConTme;
			string tFuncName, tModelErr, tKeyApi, tKeyCache;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

				aoResult = new cmlResItem<cmlResUserDwn>();
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

				tKeyCache = "User" + string.Format("{0:yyyyMMdd}", pdDate);
				if (oCacheFunc.C_CAHbExistsKey(tKeyCache))
				{
					// ถ้ามี key อยุ่ใน cache
					aoResult = oCacheFunc.C_CAHoGetKey<cmlResItem<cmlResUserDwn>>(tKeyCache);
					aoResult.rtCode = oMsg.tMS_RespCode001;
					aoResult.rtDesc = oMsg.tMS_RespDesc001;
					return aoResult;
				}

				// Get data
				//oSql = new StringBuilder();

				////*Arm 63-06-15 Comment Code
				////oSql.AppendLine("SELECT FTUsrCode AS rtUsrCode, FTDptCode AS rtDptCode, FTRolCode AS rtRolCode,");
				////oSql.AppendLine("FTUsrTel AS rtUsrTel, FTUsrPwd AS rtUsrPwd, FTUsrEmail AS rtUsrEmail,");
				////oSql.AppendLine("FTUsrLoginType AS rtUsrLoginType, FDUsrStart AS rdUsrStart, FDUsrFinish AS rdUsrFinish,");

				//oSql.AppendLine("SELECT FTUsrCode AS rtUsrCode, FTDptCode AS rtDptCode, FTUsrTel AS rtUsrTel, FTUsrEmail AS rtUsrEmail,"); //*Arm 63-06-15 ปรับตาม DataBase SKC
				//oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
				//oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
				//oSql.AppendLine("FROM TCNMUser with(nolock)");
				//oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

				//aoResult.roItem = new cmlResUserDwn();
				//oUserDwn = new cmlResUserDwn();
				//using (AdaAccEntities oDB = new AdaAccEntities())
				//{
				//    using (DbConnection oConn = oDB.Database.Connection)
				//    {
				//        oConn.Open();
				//        DbCommand oCmd = oConn.CreateCommand();
				//        oCmd.CommandText = oSql.ToString();
				//        using (DbDataReader oDR = oCmd.ExecuteReader())
				//        {
				//            oUserDwn.raUser = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUser>(oDR).ToList();
				//            ((IDisposable)oDR).Dispose();
				//        }

				//        if (oUserDwn.raUser.Count > 0)
				//        {
				//            //Languague
				//            oSql.Clear();
				//            oSql.AppendLine("SELECT DISTINCT TCNMUser_L.FTUsrCode AS rtUsrCode, TCNMUser_L.FNLngID AS rnLngID,");
				//            oSql.AppendLine("TCNMUser_L.FTUsrName AS rtUsrName, TCNMUser_L.FTUsrRmk AS rtUsrRmk");
				//            oSql.AppendLine("FROM TCNMUser_L with(nolock)");
				//            oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMUser_L.FTUsrCode = TCNMUser.FTUsrCode");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oUserDwn.raUserLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUserLng>(oDR).ToList();
				//                ((IDisposable)oDR).Dispose();
				//            }

				//            //Image
				//            oSql.Clear();
				//            oSql.AppendLine("SELECT DISTINCT TCNMImgPerson.FNImgID AS rnImgID, TCNMImgPerson.FTImgRefID AS rtImgRefID, TCNMImgPerson.FNImgSeq AS rnImgSeq,");
				//            oSql.AppendLine("TCNMImgPerson.FTImgTable AS rtImgTable, TCNMImgPerson.FTImgKey AS rtImgKey, TCNMImgPerson.FTImgObj AS rtImgObj,");
				//            oSql.AppendLine("TCNMImgPerson.FDLastUpdOn AS rdLastUpdOn, TCNMImgPerson.FDCreateOn AS rdCreateOn,");
				//            oSql.AppendLine("TCNMImgPerson.FTLastUpdBy AS rtLastUpdBy, TCNMImgPerson.FTCreateBy AS rtCreateBy");
				//            oSql.AppendLine("FROM TCNMImgPerson with(nolock)");
				//            oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMImgPerson.FTImgRefID = TCNMUser.FTUsrCode AND TCNMImgPerson.FTImgTable = 'TCNMUser'");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oUserDwn.raImage = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoImgPerson>(oDR).ToList();
				//                ((IDisposable)oDR).Dispose();
				//            }

				//            //Address
				//            oSql.Clear();
				//            oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
				//            oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
				//            oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
				//            //oSql.AppendLine("TCNMAddress_L.FTAreCode AS rtAreCode, TCNMAddress_L.FTZneCode AS rtZneCode, TCNMAddress_L.FTAddVersion AS rtAddVersion,");
				//            oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,"); //*Em 62-04-02  ปรับโครงสร้าง
				//            oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
				//            oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
				//            oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
				//            oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
				//            oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
				//            oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
				//            oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
				//            oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
				//            oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMUser.FTUsrCode AND TCNMAddress_L.FTAddGrpType = '2'");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oUserDwn.raAddrLng = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoAddrLng>(oDR).ToList();
				//                ((IDisposable)oDR).Dispose();
				//            }

				//            //Group
				//            oSql.Clear();
				//            //*Arm 63-06-15 Comment Code
				//            //oSql.AppendLine("SELECT DISTINCT TCNTUsrGroup.FTUsrCode AS rtUsrCode, TCNTUsrGroup.FTBchCode AS rtBchCode, TCNTUsrGroup.FTUsrStaShop AS rtUsrStaShop,");
				//            //oSql.AppendLine("TCNTUsrGroup.FTShpCode AS rtShpCode, TCNTUsrGroup.FDUsrStart AS rdUsrStart, TCNTUsrGroup.FDUsrStop AS rdUsrStop");
				//            //oSql.AppendLine("FROM TCNTUsrGroup with(nolock)");
				//            //oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNTUsrGroup.FTUsrCode = TCNMUser.FTUsrCode");
				//            //oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");

				//            //*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC
				//            oSql.AppendLine("SELECT TCNTUsrGroup.FTUsrCode AS rtUsrCode, TCNTUsrGroup.FTAgnCode AS rtAgnCode, TCNTUsrGroup.FTBchCode AS rtBchCode, ");
				//            oSql.AppendLine("TCNTUsrGroup.FTShpCode AS rtShpCode, TCNTUsrGroup.FTMerCode AS rtMerCode,");
				//            oSql.AppendLine("TCNTUsrGroup.FDLastUpdOn AS rdLastUpdOn, TCNTUsrGroup.FDCreateOn AS rdCreateOn,");
				//            oSql.AppendLine("TCNTUsrGroup.FTLastUpdBy AS rtLastUpdBy, TCNTUsrGroup.FTCreateBy AS rtCreateBy ");
				//            oSql.AppendLine("FROM TCNTUsrGroup with(nolock)");
				//            oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNTUsrGroup.FTUsrCode = TCNMUser.FTUsrCode");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oUserDwn.raUserGrp = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUserGrp>(oDR).ToList();
				//                ((IDisposable)oDR).Dispose();
				//            }

				//            //User Login
				//            oSql.Clear();
				//            oSql.AppendLine("SELECT TCNMUsrLogin.FTUsrCode AS rtUsrCode,TCNMUsrLogin.FTUsrLogType AS rtUsrLogType,TCNMUsrLogin.FDUsrPwdStart AS rdUsrPwdStart,");
				//            oSql.AppendLine("TCNMUsrLogin.FDUsrPwdExpired AS rdUsrPwdExpired,TCNMUsrLogin.FTUsrLogin AS rtUsrLogin,TCNMUsrLogin.FTUsrLoginPwd AS rtUsrLoginPwd,");
				//            oSql.AppendLine("TCNMUsrLogin.FTUsrRmk AS rtUsrRmk,TCNMUsrLogin.FTUsrStaActive AS rtUsrStaActive");
				//            oSql.AppendLine("FROM TCNMUsrLogin WITH(NOLOCK)");
				//            oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMUsrLogin.FTUsrCode = TCNMUser.FTUsrCode");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMUsrLogin.FDUsrPwdExpired,121) >= CONVERT(VARCHAR(10),GETDATE(),121)");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oUserDwn.raTCNMUsrLogin = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResTCNMUsrLogin>(oDR).ToList();
				//                ((IDisposable)oDR).Dispose();
				//            }

				//            //User Act Role Arm 63-05-15
				//            oSql.Clear();
				//            oSql.AppendLine("SELECT TCNMUsrActRole.FTRolCode AS rtRolCode, TCNMUsrActRole.FTUsrCode AS rtUsrCode, TCNMUsrActRole.FDLastUpdOn AS rdLastUpdOn, ");
				//            oSql.AppendLine("TCNMUsrActRole.FTLastUpdBy AS rtLastUpdBy, TCNMUsrActRole.FDCreateOn AS rdCreateOn, TCNMUsrActRole.FTCreateBy AS rtCreateBy");
				//            oSql.AppendLine(" FROM TCNMUsrActRole WITH(NOLOCK)");
				//            oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMUsrActRole.FTUsrCode = TCNMUser.FTUsrCode");
				//            oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				//            oCmd.CommandText = oSql.ToString();
				//            using (DbDataReader oDR = oCmd.ExecuteReader())
				//            {
				//                oUserDwn.raTCNMUsrActRole = ((IObjectContextAdapter)oDB).ObjectContext.Translate<cmlResInfoUsrActRole>(oDR).ToList();
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
				aoResult.roItem = new cmlResUserDwn();
				oUserDwn = new cmlResUserDwn();
				oSql = new StringBuilder();
				oSql.AppendLine("SELECT FTUsrCode AS rtUsrCode, FTDptCode AS rtDptCode, FTUsrTel AS rtUsrTel, FTUsrEmail AS rtUsrEmail,"); //*Arm 63-06-15 ปรับตาม DataBase SKC
				oSql.AppendLine("FDLastUpdOn AS rdLastUpdOn, FDCreateOn AS rdCreateOn,");
				oSql.AppendLine("FTLastUpdBy AS rtLastUpdBy, FTCreateBy AS rtCreateBy");
				oSql.AppendLine("FROM TCNMUser with(nolock)");
				oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDLastUpdOn, 121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
				oUserDwn.raUser = oDB.C_DATaSqlQuery<cmlResInfoUser>(oSql.ToString());
				if (oUserDwn.raUser.Count > 0)
				{
					//Languague
					oSql.Clear();
					oSql.AppendLine("SELECT DISTINCT TCNMUser_L.FTUsrCode AS rtUsrCode, TCNMUser_L.FNLngID AS rnLngID,");
					oSql.AppendLine("TCNMUser_L.FTUsrName AS rtUsrName, TCNMUser_L.FTUsrRmk AS rtUsrRmk");
					oSql.AppendLine("FROM TCNMUser_L with(nolock)");
					oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMUser_L.FTUsrCode = TCNMUser.FTUsrCode");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oUserDwn.raUserLng = oDB.C_DATaSqlQuery<cmlResInfoUserLng>(oSql.ToString());

					//Image
					oSql.Clear();
					oSql.AppendLine("SELECT DISTINCT TCNMImgPerson.FNImgID AS rnImgID, TCNMImgPerson.FTImgRefID AS rtImgRefID, TCNMImgPerson.FNImgSeq AS rnImgSeq,");
					oSql.AppendLine("TCNMImgPerson.FTImgTable AS rtImgTable, TCNMImgPerson.FTImgKey AS rtImgKey, TCNMImgPerson.FTImgObj AS rtImgObj,");
					oSql.AppendLine("TCNMImgPerson.FDLastUpdOn AS rdLastUpdOn, TCNMImgPerson.FDCreateOn AS rdCreateOn,");
					oSql.AppendLine("TCNMImgPerson.FTLastUpdBy AS rtLastUpdBy, TCNMImgPerson.FTCreateBy AS rtCreateBy");
					oSql.AppendLine("FROM TCNMImgPerson with(nolock)");
					oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMImgPerson.FTImgRefID = TCNMUser.FTUsrCode AND TCNMImgPerson.FTImgTable = 'TCNMUser'");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oUserDwn.raImage = oDB.C_DATaSqlQuery<cmlResInfoImgPerson>(oSql.ToString());

					//Address
					oSql.Clear();
					oSql.AppendLine("SELECT TCNMAddress_L.FNLngID AS rnLngID, TCNMAddress_L.FTAddGrpType AS rtAddGrpType, TCNMAddress_L.FTAddRefCode AS rtAddRefCode,");
					oSql.AppendLine("TCNMAddress_L.FNAddSeqNo AS rnAddSeqNo, TCNMAddress_L.FTAddRefNo AS rtAddRefNo, TCNMAddress_L.FTAddName AS rtAddName,");
					oSql.AppendLine("TCNMAddress_L.FTAddTaxNo AS rtAddTaxNo, TCNMAddress_L.FTAddRmk AS rtAddRmk, TCNMAddress_L.FTAddCountry AS rtAddCountry,");
					//oSql.AppendLine("TCNMAddress_L.FTAreCode AS rtAreCode, TCNMAddress_L.FTZneCode AS rtZneCode, TCNMAddress_L.FTAddVersion AS rtAddVersion,");
					oSql.AppendLine("TCNMAddress_L.FTAddVersion AS rtAddVersion,"); //*Em 62-04-02  ปรับโครงสร้าง
					oSql.AppendLine("TCNMAddress_L.FTAddV1No AS rtAddV1No, TCNMAddress_L.FTAddV1Soi AS rtAddV1Soi, TCNMAddress_L.FTAddV1Village AS rtAddV1Village,");
					oSql.AppendLine("TCNMAddress_L.FTAddV1Road AS rtAddV1Road, TCNMAddress_L.FTAddV1SubDist AS rtAddV1SubDist, TCNMAddress_L.FTAddV1DstCode AS rtAddV1DstCode,");
					oSql.AppendLine("TCNMAddress_L.FTAddV1PvnCode AS rtAddV1PvnCode, TCNMAddress_L.FTAddV1PostCode AS rtAddV1PostCode, TCNMAddress_L.FTAddV2Desc1 AS rtAddV2Desc1,");
					oSql.AppendLine("TCNMAddress_L.FTAddV2Desc2 AS rtAddV2Desc2, TCNMAddress_L.FTAddWebsite AS rtAddWebsite, TCNMAddress_L.FTAddLongitude AS rtAddLongitude,");
					oSql.AppendLine("TCNMAddress_L.FTAddLatitude AS rtAddLatitude,");
					oSql.AppendLine("TCNMAddress_L.FDLastUpdOn AS rdLastUpdOn, TCNMAddress_L.FDCreateOn AS rdCreateOn,");
					oSql.AppendLine("TCNMAddress_L.FTLastUpdBy AS rtLastUpdBy, TCNMAddress_L.FTCreateBy AS rtCreateBy");
					oSql.AppendLine("FROM TCNMAddress_L with(nolock)");
					oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMAddress_L.FTAddRefCode = TCNMUser.FTUsrCode AND TCNMAddress_L.FTAddGrpType = '2'");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oUserDwn.raAddrLng = oDB.C_DATaSqlQuery<cmlResInfoAddrLng>(oSql.ToString());

					//Group
					oSql.Clear();
					//*Arm 63-06-15 ปรับตามโครงสร้าง DataBase SKC
					oSql.AppendLine("SELECT TCNTUsrGroup.FTUsrCode AS rtUsrCode, TCNTUsrGroup.FTAgnCode AS rtAgnCode, TCNTUsrGroup.FTBchCode AS rtBchCode, ");
					oSql.AppendLine("TCNTUsrGroup.FTShpCode AS rtShpCode, TCNTUsrGroup.FTMerCode AS rtMerCode,");
					oSql.AppendLine("TCNTUsrGroup.FDLastUpdOn AS rdLastUpdOn, TCNTUsrGroup.FDCreateOn AS rdCreateOn,");
					oSql.AppendLine("TCNTUsrGroup.FTLastUpdBy AS rtLastUpdBy, TCNTUsrGroup.FTCreateBy AS rtCreateBy ");
					oSql.AppendLine("FROM TCNTUsrGroup with(nolock)");
					oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNTUsrGroup.FTUsrCode = TCNMUser.FTUsrCode");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oUserDwn.raUserGrp = oDB.C_DATaSqlQuery<cmlResInfoUserGrp>(oSql.ToString());

					//User Login
					oSql.Clear();
					oSql.AppendLine("SELECT TCNMUsrLogin.FTUsrCode AS rtUsrCode,TCNMUsrLogin.FTUsrLogType AS rtUsrLogType,TCNMUsrLogin.FDUsrPwdStart AS rdUsrPwdStart,");
					oSql.AppendLine("TCNMUsrLogin.FDUsrPwdExpired AS rdUsrPwdExpired,TCNMUsrLogin.FTUsrLogin AS rtUsrLogin,TCNMUsrLogin.FTUsrLoginPwd AS rtUsrLoginPwd,");
					oSql.AppendLine("TCNMUsrLogin.FTUsrRmk AS rtUsrRmk,TCNMUsrLogin.FTUsrStaActive AS rtUsrStaActive");
					oSql.AppendLine("FROM TCNMUsrLogin WITH(NOLOCK)");
					oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMUsrLogin.FTUsrCode = TCNMUser.FTUsrCode");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oSql.AppendLine("AND CONVERT(VARCHAR(10),TCNMUsrLogin.FDUsrPwdExpired,121) >= CONVERT(VARCHAR(10),GETDATE(),121)");
					oUserDwn.raTCNMUsrLogin = oDB.C_DATaSqlQuery<cmlResTCNMUsrLogin>(oSql.ToString());

					//User Act Role Arm 63-05-15
					oSql.Clear();
					oSql.AppendLine("SELECT TCNMUsrActRole.FTRolCode AS rtRolCode, TCNMUsrActRole.FTUsrCode AS rtUsrCode, TCNMUsrActRole.FDLastUpdOn AS rdLastUpdOn, ");
					oSql.AppendLine("TCNMUsrActRole.FTLastUpdBy AS rtLastUpdBy, TCNMUsrActRole.FDCreateOn AS rdCreateOn, TCNMUsrActRole.FTCreateBy AS rtCreateBy");
					oSql.AppendLine(" FROM TCNMUsrActRole WITH(NOLOCK)");
					oSql.AppendLine("INNER JOIN TCNMUser with(nolock) ON TCNMUsrActRole.FTUsrCode = TCNMUser.FTUsrCode");
					oSql.AppendLine("WHERE CONVERT(VARCHAR(10),TCNMUser.FDLastUpdOn,121) >= '" + string.Format("{0:yyyy-MM-dd}", pdDate) + "'");
					oUserDwn.raTCNMUsrActRole = oDB.C_DATaSqlQuery<cmlResInfoUsrActRole>(oSql.ToString());
				}
				else
				{
					aoResult.rtCode = oMsg.tMS_RespCode800;
					aoResult.rtDesc = oMsg.tMS_RespDesc800;
					return aoResult;
				}


				aoResult.roItem = oUserDwn;
				// เก็บ KeyApi ลง Cache
				oCacheFunc.C_CAHxAddKey(tKeyCache, aoResult);

				aoResult.rtCode = oMsg.tMS_RespCode001;
				aoResult.rtDesc = oMsg.tMS_RespDesc001;
				return aoResult;
			}
			catch (Exception oExcept)
			{
				// Return error.
				aoResult = new cmlResItem<cmlResUserDwn>();
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