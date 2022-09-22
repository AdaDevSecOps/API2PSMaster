using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.User;
using API2PSMaster.Models.WebService.Response.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSMaster.Class.User
{
    /// <summary>
    /// Class User
    /// </summary>
    public class cUser
    {

        /// <summary>
        ///     Verify pameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="paoSysConfig">System configuration.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrPdt">out Error product.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsItemParameterValue(cmlReqUsrInsItem poPara, List<cmlTSysConfig> paoSysConfig,
            out string ptErrCode, out string ptErrDesc, out cmlResUsrInsItem poErrPdt)
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
                oSql.AppendLine("SELECT FTUsrCode ");
                oSql.AppendLine("FROM TCNMUser WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTUsrCode='" + poPara.ptUsrCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // User duplicate.
                    poErrPdt = new cmlResUsrInsItem();
                    poErrPdt.rtUsrCode = poPara.ptUsrCode;

                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801 + " (User).";

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrPdt = null;
                    return true;
                }


            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrPdt = null;
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

        /// <summary>
        ///     Verify parameter item value of update function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="paoSysConfig">System configuration.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrPgp">out Error product.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyUpdItemParameterValue(cmlReqUsrUpdItem poPara, List<cmlTSysConfig> paoSysConfig,
            out string ptErrCode, out string ptErrDesc, out cmlResUsrUpdItem poErrPgp)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
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

                ptErrCode = "";
                ptErrDesc = "";
                poErrPgp = null;
                return true;
            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrPgp = null;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
    }
}