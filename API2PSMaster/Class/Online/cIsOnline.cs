using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Response.Online;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSMaster.Class.Online
{
    /// <summary>
    /// Information class Online
    /// </summary>
    public class cIsOnline
    {
        /// <summary>
        /// ตรวจสอบสถานะ Online
        /// </summary>
        /// <param name="paoSysConfig"></param>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <param name="poIsOnlineErr"></param>
        /// <returns></returns>
        public bool C_DATbVerifyIsOnline(List<cmlTSysConfig> paoSysConfig, out string ptErrCode
                                            , out string ptErrDesc, out cmlResIsOnline poIsOnlineErr)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            int nConTme, nCmdTme;
            DataTable oTblComp;
            try
            {
                // Confuguration database.
                nConTme = 0;
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, paoSysConfig, "002");
                nCmdTme = 0;
                oFunc.SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, paoSysConfig, "003");
                oDatabase = new cDatabase(nConTme);

                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTCmpCode FROM TCNMComp WITH (NOLOCK)");
                oTblComp = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);

                if (!(oTblComp.Rows.Count >= 0))
                {
                    poIsOnlineErr = new cmlResIsOnline();
                    ptErrCode = oMsg.tMS_RespCode905;
                    ptErrDesc = oMsg.tMS_RespDesc905;
                    return false;
                }

                ptErrCode = "";
                ptErrDesc = "";
                poIsOnlineErr = null;
                return true;
            }
            catch (Exception oEx)
            {
                poIsOnlineErr = new cmlResIsOnline();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                oTblComp = null;
            }
        }
    }
}