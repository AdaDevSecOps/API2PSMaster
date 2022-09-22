using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Class.UserGrp;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.UserGrp;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.UserGrp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
    /// User group information.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/UserGrp")]
    public class cUserGrpController : ControllerBase
    {
        /// <summary>
        ///     Insert User Department.
        /// </summary>
        /// 
        /// <param name="poPara">User Department information.</param>
        /// 
        /// <returns>
        ///     User Department varify false.<br/>
        ///     System process status.<br/>
        ///     1   : success.<br/>
        ///     801 : data is duplicate.<br/>
        ///     802 : formate data incorrect.<br/>
        ///     900 : service process false.<br/>
        ///     904 : key not allowed to use method.<br/>
        ///     905 : cannot connect database.<br/>
        ///     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Insert/Item")]
        [HttpPost]
        public cmlResItem<cmlResUsrGrpInsItem> POST_PUNoInsUseGrpItem([FromBody] cmlReqUsrGrpInsItem poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cUserGrp oUserGrp;
            List<cmlTSysConfig> aoSysConfig;
            cmlResUsrGrpInsItem oResUsrErr;
            cmlResItem<cmlResUsrGrpInsItem> oResult;
            int nRowEff, nConTme, nCmdTme;
            string tFuncName, tModelErr, tKeyApi, tErrCode, tErrDesc;
            bool bVerifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResItem<cmlResUsrGrpInsItem>();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(out tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Check range time use function.
                    if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
                    {
                        tKeyApi = "";
                        // Check KeyApi.
                        if (oFunc.SP_CHKbKeyApi(out tKeyApi, aoSysConfig, HttpContext))
                        {
                            // Varify parameter value.
                            oUserGrp = new cUserGrp();
                            bVerifyPara = oUserGrp.C_DATbVerifyInsItemParameterValue(poPara, aoSysConfig, out tErrCode, out tErrDesc, out oResUsrErr);
                            if (bVerifyPara == true)
                            {
                                // Insert.
                                oSql = new StringBuilder();
                                oSql.AppendLine("INSERT INTO TCNTUsrGroup WITH(ROWLOCK)");
                                oSql.AppendLine("(");
                                oSql.AppendLine("	FTUsrCode,FTBchCode,FTUsrStaShop,");
                                oSql.AppendLine("	FTShpCode,FDUsrStart,FDUsrStop");
                                oSql.AppendLine(")");
                                oSql.AppendLine("VALUES");
                                oSql.AppendLine("(");
                                oSql.AppendLine("	'" + poPara.ptUsrCode + "','" + poPara.ptBchCode + "','" + poPara.ptUsrStaShop + "',");
                                oSql.AppendLine("	'" + poPara.ptShpCode + "','" + poPara.pdUsrStart + "','" + poPara.pdUsrStop + "'");
                                oSql.AppendLine(")");

                                try
                                {
                                    // Confuguration database.
                                    nConTme = 0;
                                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, aoSysConfig, "003");
                                    nCmdTme = 0;
                                    oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "004");

                                    oDatabase = new cDatabase(nConTme);
                                    nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                                }
                                catch (SqlException oSqlExn)
                                {
                                    switch (oSqlExn.Number)
                                    {
                                        case 2627:
                                            // Data is duplicate.
                                            oResult.roItem = new cmlResUsrGrpInsItem();
                                            oResult.roItem.rtUsrCode = poPara.ptUsrCode;
                                            oResult.rtCode = oMsg.tMS_RespCode801;
                                            oResult.rtDesc = oMsg.tMS_RespDesc801 + " (User Group).";
                                            return oResult;
                                    }
                                }
                                catch (EntityException oEtyExn)
                                {
                                    switch (oEtyExn.HResult)
                                    {
                                        case -2146232060:
                                            // Cannot connect database..
                                            oResult.rtCode = oMsg.tMS_RespCode905;
                                            oResult.rtDesc = oMsg.tMS_RespDesc905;
                                            return oResult;
                                    }
                                }
                                oResult.rtCode = oMsg.tMS_RespCode001;
                                oResult.rtDesc = oMsg.tMS_RespDesc001;
                                return oResult;
                            }
                            else
                            {
                                // Varify parameter value false.
                                oResult.roItem = oResUsrErr;
                                oResult.rtCode = tErrCode;
                                oResult.rtDesc = tErrDesc;
                                return oResult;
                            }

                        }
                        else
                        {
                            // Key not allowed to use method.
                            oResult.rtCode = oMsg.tMS_RespCode904;
                            oResult.rtDesc = oMsg.tMS_RespDesc904;
                            return oResult;
                        }
                    }
                    else
                    {
                        // This time not allowed to use method.
                        oResult.rtCode = oMsg.tMS_RespCode906;
                        oResult.rtDesc = oMsg.tMS_RespDesc906;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResItem<cmlResUsrGrpInsItem>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                aoSysConfig = null;
                oResUsrErr = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }


    }
}