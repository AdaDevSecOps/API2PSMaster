using API2PSMaster.Class;
using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Base;
using API2PSMaster.Models.WebService.Response.JobTask;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     Download job task infomation.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/JobTask")]
    public class cJobTaskController : ControllerBase
    {
        /// <summary>
        ///     Download job task (confrim rate) information.
        /// </summary>
        /// <param name="ptAgnCode">AD Code.</param>
        /// <returns></returns>
        [Route("Download")]
        [HttpGet]
        public cmlResItem<cmlResJobTaskDwn> GET_DWNoDownloadJobTask(string ptAgnCode = "")
        {
            cSP oFunc;
            cCS oCS;
            cMS oMsg;
            StringBuilder oSql;
            cDatabase oDB;
            cmlResItem<cmlResJobTaskDwn> aoResult;
            List<cmlTSysConfig> aoSysConfig;
            cmlResJobTaskDwn oJobTaskDwn;
            int nCmdTme;
            string tFuncName, tModelErr, tKeyApi;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                aoResult = new cmlResItem<cmlResJobTaskDwn>();
                aoResult.roItem = new cmlResJobTaskDwn();
                oJobTaskDwn = new cmlResJobTaskDwn();

                oSql = new StringBuilder();
                oDB = new cDatabase();

                oFunc = new cSP();
                oCS = new cCS();
                oMsg = new cMS();

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

                // Get data
                oSql.Clear();
                oSql.AppendLine("SELECT JOB.FTAgnCode AS rtAgnCode, JOB.FTJobRefTbl AS rtJobRefTbl, JOB.FDJobDateCfm AS rdJobDateCfm, JOB.FTJobStaUse AS rtJobStaUse ");
                oSql.AppendLine(", JOB.FDLastUpdOn AS rdLastUpdOn, JOB.FTLastUpdBy AS rtLastUpdBy,	JOB.FDCreateOn AS rdCreateOn, JOB.FTCreateBy AS rtCreateBy"); //*Arm 65-09-10
                oSql.AppendLine("FROM TCNSJobTask JOB WITH(NOLOCK) ");
                oSql.AppendLine("WHERE JOB.FTAgnCode = '" + ptAgnCode + "' ");
                oJobTaskDwn.raJobTask = oDB.C_DATaSqlQuery<cmlResInfoJobTask>(oSql.ToString());

                if (oJobTaskDwn.raJobTask != null && oJobTaskDwn.raJobTask.Count > 0)
                {
                    
                }
                else
                {
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
                aoResult.roItem = oJobTaskDwn;
                aoResult.rtCode = oMsg.tMS_RespCode001;
                aoResult.rtDesc = oMsg.tMS_RespDesc001;
                return aoResult;
            }
            catch (Exception oExcept)
            {
                // Return error.
                aoResult = new cmlResItem<cmlResJobTaskDwn>();
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
            }
        }
    }
}
