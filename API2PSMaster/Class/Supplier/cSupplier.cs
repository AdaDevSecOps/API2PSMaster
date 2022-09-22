using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Supplier;
using API2PSMaster.Models.WebService.Response.Supplier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSMaster.Class.Supplier
{
    /// <summary>
    ///     Verify Supplier.
    /// </summary>
    public class cSupplier
    {
        /// <summary>
        ///     Verify parameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrSpl">out Error supplier.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsItemParamValue(cmlReqSplInsItem poPara, out string ptErrCode, out string ptErrDesc, out cmlResSplInsItem poErrSpl)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSplCode");
                oSql.AppendLine("FROM TCNMSpl WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // Supplier duplicate.
                    poErrSpl = new cmlResSplInsItem();
                    poErrSpl.rtSplCode = poPara.ptSplCode;

                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801;

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrSpl = null;
                    return true;
                }
            }
            catch (Exception oExcep)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = oExcep.Message;
                poErrSpl = null;
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
        ///     Verify parameter value of delete function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrSpl">out Error supplier.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyDelItemParamValue(cmlReqSplInsItem poPara, out string ptErrCode, out string ptErrDesc, out cmlResSplInsItem poErrSpl)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSplCode");
                oSql.AppendLine("FROM TCNMPdtSpl WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // Supplier reference.
                    poErrSpl = new cmlResSplInsItem();
                    poErrSpl.rtSplCode = poPara.ptSplCode;

                    ptErrCode = oMsg.tMS_RespCode813;
                    ptErrDesc = oMsg.tMS_RespDesc813;

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrSpl = null;
                    return true;
                }
            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrSpl = null;
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
        ///     Verify Supplier Languague for insert
        /// </summary>
        /// <param name="poPara">Parmeter.</param>
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifySplLanguagueInsValue(string ptSplCode,Int64 pnLngID)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSplCode");
                oSql.AppendLine("FROM TCNMSpl_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + ptSplCode + "'");
                oSql.AppendLine("AND FNLngID = " + pnLngID + "");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    //duplicate.
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        ///     Verify Supplier card for insert
        /// </summary>
        /// <param name="poPara">Parmeter.</param>
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifySplCardInsValue(string ptSplCode)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSplCode");
                oSql.AppendLine("FROM TCNMSplCard WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSplCode = '" + ptSplCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    //duplicate.
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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

        #region Group
        /// <summary>
        ///     Verify parameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrSpl">out Error supplier.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsSplGrpParamValue(cmlReqSplGrpIns poPara, out string ptErrCode, out string ptErrDesc, out cmlResSplGrpIns poErrSpl)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSgpCode");
                oSql.AppendLine("FROM TCNMSplGrp WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSgpCode = '" + poPara.ptSgpCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // Supplier duplicate.
                    poErrSpl = new cmlResSplGrpIns();
                    poErrSpl.rtSgpCode = poPara.ptSgpCode;

                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801;

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrSpl = null;
                    return true;
                }
            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrSpl = null;
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
        ///     Verify Supplier group Languague for insert
        /// </summary>
        /// <param name="poPara">Parmeter.</param>
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifySplGrpLanguagueInsValue(string ptSgpCode, Int64 pnLngID)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSgpCode");
                oSql.AppendLine("FROM TCNMSplGrp_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSgpCode = '" + ptSgpCode + "'");
                oSql.AppendLine("AND FNLngID = " + pnLngID + "");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    //duplicate.
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        ///     Verify parameter value of delete function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyDelSplGrpParamValue(cmlReqSplGrpDel poPara)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSgpCode");
                oSql.AppendLine("FROM TCNMSpl WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSgpCode = '" + poPara.ptSgpCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        #endregion

        #region Type
        /// <summary>
        ///     Verify parameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrSpl">out Error supplier.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsSplTypeParamValue(cmlReqSplTypeIns poPara, out string ptErrCode, out string ptErrDesc, out cmlResSplTypeIns poErrSpl)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTStyCode");
                oSql.AppendLine("FROM TCNMSplType WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // Supplier duplicate.
                    poErrSpl = new cmlResSplTypeIns();
                    poErrSpl.rtStyCode = poPara.ptStyCode;

                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801;

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrSpl = null;
                    return true;
                }
            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrSpl = null;
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
        ///     Verify Supplier type Languague for insert
        /// </summary>
        /// <param name="poPara">Parmeter.</param>
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifySplTypeLanguagueInsValue(string ptStyCode,Int64 pnLngID)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTStyCode");
                oSql.AppendLine("FROM TCNMSplType_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTStyCode = '" + ptStyCode + "'");
                oSql.AppendLine("AND FNLngID = " + pnLngID + "");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    //duplicate.
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        ///     Verify parameter value of delete function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyDelSplTypeParamValue(cmlReqSplTypeDel poPara)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTStyCode");
                oSql.AppendLine("FROM TCNMSpl WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTStyCode = '" + poPara.ptStyCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        #endregion

        #region Level
        /// <summary>
        ///     Verify parameter value of insert function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// <param name="ptErrCode">out Error code.</param>
        /// <param name="ptErrDesc">out Error descript.</param>
        /// <param name="poErrSpl">out Error supplier.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyInsSplLevelParamValue(cmlReqSplLevIns poPara, out string ptErrCode, out string ptErrDesc, out cmlResSplLevIns poErrSpl)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSlvCode");
                oSql.AppendLine("FROM TCNMSplLev WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSlvCode = '" + poPara.ptSlvCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    // Supplier duplicate.
                    poErrSpl = new cmlResSplLevIns();
                    poErrSpl.rtSlvCode = poPara.ptSlvCode;

                    ptErrCode = oMsg.tMS_RespCode801;
                    ptErrDesc = oMsg.tMS_RespDesc801;

                    return false;
                }
                else
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    poErrSpl = null;
                    return true;
                }
            }
            catch (Exception)
            {
                ptErrCode = new cMS().tMS_RespCode900;
                ptErrDesc = "";
                poErrSpl = null;
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
        ///     Verify Supplier type Languague for insert
        /// </summary>
        /// <param name="poPara">Parmeter.</param>
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifySplLevelLanguagueInsValue(string ptSlvCode,Int64 pnLngID)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTSlvCode");
                oSql.AppendLine("FROM TCNMSplLev_L WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSlvCode = '" + ptSlvCode + "'");
                oSql.AppendLine("AND FNLngID = " + pnLngID + "");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    //duplicate.
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        ///     Verify parameter value of delete function.
        /// </summary>
        /// 
        /// <param name="poPara">Parmeter.</param>
        /// 
        /// <returns>
        ///     true : Varify pass.<br/>
        ///     false : Varify false.
        /// </returns>
        public bool C_DATbVerifyDelSplLevelParamValue(cmlReqSplLevDel poPara)
        {
            cSP oFunc;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oDbTblTmp;

            try
            {
                oFunc = new cSP();
                oMsg = new cMS();

                oDatabase = new cDatabase();

                // Check unit code duplicate.
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTStyCode");
                oSql.AppendLine("FROM TCNMSpl WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTSlvCode = '" + poPara.ptSlvCode + "'");
                oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
                if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
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
        #endregion

        #region Contact
        #endregion

        #region Address
        ///// <summary>
        /////     Verify Supplier address for insert
        ///// </summary>
        ///// <param name="poPara">Parmeter.</param>
        ///// <returns>
        /////     true : Varify pass.<br/>
        /////     false : Varify false.
        ///// </returns>
        //public bool C_DATbVerifySplAddrInsValue(cmlReqSplInsItem poPara)
        //{
        //    cSP oFunc;
        //    cMS oMsg;
        //    cDatabase oDatabase;
        //    StringBuilder oSql;
        //    DataTable oDbTblTmp;

        //    try
        //    {
        //        oFunc = new cSP();
        //        oMsg = new cMS();

        //        oDatabase = new cDatabase();

        //        // Check unit code duplicate.
        //        oSql = new StringBuilder();
        //        oSql.AppendLine("SELECT TOP 1 FTSplCode");
        //        oSql.AppendLine("FROM TCNMSplAddress_L WITH(NOLOCK)");
        //        oSql.AppendLine("WHERE FTSplCode = '" + poPara.ptSplCode + "'");
        //        oSql.AppendLine("AND FNLngID = " + poPara.pnLngID + "");
        //        oSql.AppendLine("AND FTAddGrpType = '" + poPara.ptAddGrpType + "'");
        //        oDbTblTmp = oDatabase.C_DAToSqlQuery(oSql.ToString());
        //        if (oDbTblTmp != null && oDbTblTmp.Rows.Count > 0)
        //        {
        //            //duplicate.
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        oFunc = null;
        //        oMsg = null;
        //        oDatabase = null;
        //        oSql = null;
        //        oDbTblTmp = null;

        //        //GC.Collect();
        //        //GC.WaitForPendingFinalizers();
        //        //GC.Collect();
        //    }
        //}
        #endregion
    }
}