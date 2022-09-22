using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.UserGrp;
using API2PSMaster.Models.WebService.Response.UserGrp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSMaster.Class.UserGrp
{
    /// <summary>
    /// 
    /// </summary>
    public class cUserGrp
    {

        /// <summary>
        ///     Verify pameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="paoSysConfig">System configuration.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrUsrGrp">out Error product.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsItemParameterValue(cmlReqUsrGrpInsItem poPara, List<cmlTSysConfig> paoSysConfig,
            out string ptErrCode, out string ptErrDesc, out cmlResUsrGrpInsItem poErrUsrGrp)
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
                oSql.AppendLine("FROM TCNTUsrGroup WITH(NOLOCK) ");
                oSql.AppendLine("WHERE FTUsrCode='" + poPara.ptUsrCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // User duplicate.
                    poErrUsrGrp = new cmlResUsrGrpInsItem();
                    poErrUsrGrp.rtUsrCode = poPara.ptUsrCode;
                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801 + " (User Group).";
                    return false;
                }

                // Check สถานะร้านค้า  ถ้าสถานะเป็น (3:ร้านค้า) ptShpCode ต้องมีค่า
                if (poPara.ptUsrStaShop == "3")
                {
                    if (poPara.ptShpCode == "")
                    {
                        // Formate in incorrect
                        poErrUsrGrp = new cmlResUsrGrpInsItem();
                        poErrUsrGrp.rtUsrCode = poPara.ptUsrCode;
                        ptErrCode = oMsg.tMS_RespCode802;
                        ptErrDesc = oMsg.tMS_RespDesc802 + " (User Group).";
                        return false;
                    }
                }
                ptErrCode = "";
                ptErrDesc = "";
                poErrUsrGrp = null;
                return true;
            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrUsrGrp = null;
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