using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.UserDep;
using API2PSMaster.Models.WebService.Response.UserDep;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSMaster.Class.UserDep
{
    /// <summary>
    /// 
    /// </summary>
    public class cUserDep
    {
        /// <summary>
        ///     Verify pameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="paoSysConfig">System configuration.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrUsrDep">out Error product.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsItemParameterValue(cmlReqUsrDepInsItem poPara, List<cmlTSysConfig> paoSysConfig,
            out string ptErrCode, out string ptErrDesc, out cmlResUsrDepInsItem poErrUsrDep)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;
            int nConTme, nCmdTme;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                // Confuguration database.
                nConTme = 0;
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, paoSysConfig, "003");
                nCmdTme = 0;
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, paoSysConfig, "004");

                oDatabase = new cDatabase(nConTme);

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT FTDptCode ");
                oSql.AppendLine("FROM TCNMUsrDepart WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTDptCode='" + poPara.ptDptCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // User duplicate.
                    poErrUsrDep = new cmlResUsrDepInsItem();
                    poErrUsrDep.rtDptCode= poPara.ptDptCode;

                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801 + " (User Department).";

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrUsrDep = null;
                    return true;
                }


            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrUsrDep = null;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                oDbTblTmp = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

        }
    }
}