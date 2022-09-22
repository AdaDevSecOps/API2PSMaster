using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.ChangePassword;
using API2PSMaster.Models.WebService.Request.User;
using API2PSMaster.Models.WebService.Response.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace API2PSMaster.Controllers
{
    /// <summary>
    /// Change password for New user.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/USER")]

    public class cChangePasswordController : ControllerBase
    {
        /// <summary>
        ///     Change password for New user.
        /// </summary>
        /// <param name="poData">Requst Data User Login </param>
        /// <returns>
        /// System process status.<br/>
        /// &#8195; 001 : success.<br/>
        /// &#8195; 700 : all parameter is null..<br/>
        /// &#8195; 701 : validate parameter model false.<br/>
        /// &#8195; 800 : data not found.<br/>
        /// &#8195; 900 : function process false.<br/>
        /// &#8195; 904 : key not allowed to use method.<br/>
        /// </returns>
        [Route("ChangePassword")]
        [HttpPost]
        public cmlResBase POST_CHGoChangePassword([FromBody] cmlReqPwdChange poData)
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB;
            cmlResBase oResult;
            List<cmlTSysConfig> aoSysConfig;
            cCacheFunc oCacheFunc;
            int nRowEff, nCmdTme;
            string tFuncName, tModelErr, tKeyApi;
            string tOldPwd = "";
            string tNewPwd = "";
            string FTUsrStaActive = "";

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResBase();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();
                oCacheFunc = new cCacheFunc(43200, 43200, false);

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                if (poData == null)
                {
                    oResult.rtCode = oMsg.tMS_RespCode700;
                    oResult.rtDesc = oMsg.tMS_RespDesc700;
                    return oResult;
                }
                
                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState) == false)
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }

                // Load configuration.
                aoSysConfig = oFunc.SP_SYSaLoadConfiguration();
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "2");

                tKeyApi = "";
                // Check KeyApi.
                if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext) == false)
                {
                    // Key not allowed to use method.
                    oResult.rtCode = oMsg.tMS_RespCode904;
                    oResult.rtDesc = oMsg.tMS_RespDesc904;
                    return oResult;
                }
                
                oSql = new StringBuilder();
                oDB = new cDatabase();

                if(string.IsNullOrEmpty(poData.ptUsrLogin) || string.IsNullOrEmpty(poData.ptUsrCode) || string.IsNullOrEmpty(poData.ptOldPwd) || string.IsNullOrEmpty(poData.ptNewPwd))
                {
                    oResult.rtCode = oMsg.tMS_RespCode700;
                    oResult.rtDesc = oMsg.tMS_RespDesc700;
                    return oResult;
                }

                // เข้ารหัส Password
                tOldPwd = oFunc.C_CALtEncrypt(poData.ptOldPwd);
                tNewPwd = oFunc.C_CALtEncrypt(poData.ptNewPwd);

                // Check UserLogin
                oSql.AppendLine("SELECT FTUsrStaActive FROM TCNMUsrLogin WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTUsrCode = '"+poData.ptUsrCode+ "' AND FTUsrLogin ='" + poData.ptUsrLogin + "' ");
                oSql.AppendLine("AND FTUsrLoginPwd ='"+ tOldPwd + "' ");
                oSql.AppendLine("AND FTUsrLogType ='" + poData.ptLoginType + "' "); //*Arm 63-08-12

                FTUsrStaActive = oDB.C_DAToSqlQuery<string>(oSql.ToString());

                if (!string.IsNullOrEmpty(FTUsrStaActive) && FTUsrStaActive == "3")
                {
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TCNMUsrLogin WITH(ROWLOCK) SET ");
                    oSql.AppendLine("FTUsrLoginPwd = '" + tNewPwd + "',");
                    oSql.AppendLine("FTUsrStaActive = '1' ");
                    oSql.AppendLine("WHERE FTUsrCode = '" + poData.ptUsrCode + "' ");
                    oSql.AppendLine("AND FTUsrLogin ='" + poData.ptUsrLogin + "' ");
                    oSql.AppendLine("AND FTUsrLoginPwd ='" + tOldPwd + "' ");
                    oSql.AppendLine("AND FTUsrLogType ='" + poData.ptLoginType + "' "); //*Arm 63-08-12
                    nRowEff = oDB.C_DATnExecuteSql(oSql.ToString());

                    oResult.rtCode = oMsg.tMS_RespCode001;
                    oResult.rtDesc = oMsg.tMS_RespDesc001;

                }
                else
                {
                    oResult.rtCode = oMsg.tMS_RespCode800;
                    oResult.rtDesc = oMsg.tMS_RespDesc800;
                }
                
                return oResult;
            }
            catch (Exception oExcept)
            {
                oResult = new cmlResStatus();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900 + Environment.NewLine + oExcept.Message.ToString();

                return oResult;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDB = null;
                oSql = null;
                oResult = null;
                aoSysConfig = null;
                oResult = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
        
    }
}