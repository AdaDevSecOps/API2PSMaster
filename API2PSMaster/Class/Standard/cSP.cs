using API2PSMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using API2PSMaster.Models.Database;
using API2PSMaster.Models.Other;
using System.Threading;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using System.Data.Entity.Core;

namespace API2PSMaster.Class.Standard
{
    public class cSP
    {
        private static AdaAccEntities oC_AdaAccDB = new AdaAccEntities();
        private static TripleDESCryptoServiceProvider oC_TripleDes = new TripleDESCryptoServiceProvider();
        public static Model SP_DAToAES128Decrypt<Model>(string ptEncryptedText, string ptKey, string ptIV)
        {
            try
            {
                ptEncryptedText.Replace(@"\", "");

                var tEncryptedBytes = Convert.FromBase64String(ptEncryptedText);
                var oRijndaelManaged = SP_GEToRijndaelManaged(ptKey, ptIV);
                if ((oRijndaelManaged != null))
                {
                    var aDecrypt = SP_CALaASE128Decrypt(tEncryptedBytes, SP_GEToRijndaelManaged(ptKey, ptIV));
                    if ((aDecrypt != null))
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Model>(Encoding.UTF8.GetString(aDecrypt));
                }
            }
            catch (Exception ex)
            {
            }

            return default(Model);
        }
        public static string SP_DATtAES128Decrypt(string ptEncryptedText, string ptKey)
        {
            try
            {
                var tEncryptedBytes = Convert.FromBase64String(ptEncryptedText);
                var oRijndaelManaged = SP_GEToRijndaelManaged(ptKey);
                if ((oRijndaelManaged != null))
                {
                    var aDecrypt = SP_CALaASE128Decrypt(tEncryptedBytes, SP_GEToRijndaelManaged(ptKey));
                    if ((aDecrypt != null))
                        return Encoding.UTF8.GetString(aDecrypt);
                }
            }
            catch (Exception ex)
            {
            }

            return "";
        }
        private static RijndaelManaged SP_GEToRijndaelManaged(string ptSecretKey, string ptSecretIV)
        {
            byte[] aKeyBytes;
            byte[] aIVBytes;
            byte[] aSecretKeyBytes;
            byte[] aSecretIVBytes;

            RijndaelManaged oRijndaelManaged;

            try
            {
                aKeyBytes = new byte[16];
                aSecretKeyBytes = Encoding.UTF8.GetBytes(ptSecretKey);
                Array.Copy(aSecretKeyBytes, aKeyBytes, Math.Min(aKeyBytes.Length, aSecretKeyBytes.Length));

                aIVBytes = new byte[16];
                aSecretIVBytes = Encoding.UTF8.GetBytes(ptSecretIV);
                Array.Copy(aSecretIVBytes, aIVBytes, Math.Min(aIVBytes.Length, aSecretIVBytes.Length));

                oRijndaelManaged = new RijndaelManaged();
                oRijndaelManaged.Mode = CipherMode.CBC;
                // oRijndaelManaged.Padding = PaddingMode.Zeros
                oRijndaelManaged.Padding = PaddingMode.PKCS7;
                oRijndaelManaged.KeySize = 128;
                oRijndaelManaged.BlockSize = 128;
                oRijndaelManaged.Key = aKeyBytes;
                oRijndaelManaged.IV = aIVBytes;

                return oRijndaelManaged;
            }
            catch (Exception ex)
            {
            }

            return null/* TODO Change to default(_) if this is not a reference type */;
        }
        private static RijndaelManaged SP_GEToRijndaelManaged(string ptSecretKey)
        {
            byte[] aKeyBytes;
            byte[] aSecretKeyBytes;
            RijndaelManaged oRijndaelManaged;

            try
            {
                aKeyBytes = new byte[16];
                aSecretKeyBytes = Encoding.UTF8.GetBytes(ptSecretKey);
                Array.Copy(aSecretKeyBytes, aKeyBytes, Math.Min(aKeyBytes.Length, aSecretKeyBytes.Length));

                oRijndaelManaged = new RijndaelManaged();
                oRijndaelManaged.Mode = CipherMode.CBC;
                oRijndaelManaged.Padding = PaddingMode.PKCS7;
                oRijndaelManaged.KeySize = 128;
                oRijndaelManaged.BlockSize = 128;
                oRijndaelManaged.Key = aKeyBytes;
                oRijndaelManaged.IV = aKeyBytes;

                return oRijndaelManaged;
            }
            catch (Exception ex)
            {
            }

            return null/* TODO Change to default(_) if this is not a reference type */;
        }
        private static byte[] SP_CALaASE128Decrypt(byte[] paEncryptedData, RijndaelManaged poRijndaelManaged)
        {
            try
            {
                return poRijndaelManaged.CreateDecryptor().TransformFinalBlock(paEncryptedData, 0, paEncryptedData.Length);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        ///     Validate model.
        /// </summary>
        /// 
        /// <param name="ptModelErr">ref Error message.</param>
        /// <param name="poModelState">Parameter model.</param>
        /// 
        /// <returns>
        ///     true : validate pass.<br/>
        ///     false : validate false.
        /// </returns>
        public bool SP_CHKbParaModel(out string ptModelErr, ModelStateDictionary poModelState)
        {
            try
            {
                if (poModelState.IsValid)
                {
                    // Validate pass.
                    ptModelErr = "";
                    return true;
                }
                else
                {
                    // Validate false.
                    IEnumerable<string> atErrList = from oState in poModelState.Values
                                                    from oError in oState.Errors
                                                    where !string.IsNullOrEmpty(oError.ErrorMessage)
                                                    select oError.ErrorMessage;

                    ptModelErr = string.Join("|", atErrList);
                    return false;
                }
            }
            catch (Exception)
            {

            }

            ptModelErr = "";
            return false;
        }

        /// <summary>
        ///     Load configuration.
        /// </summary>
        /// 
        /// <returns>
        ///     Configuration.
        /// </returns>
        public List<cmlTSysConfig> SP_SYSaLoadConfiguration()
        {
            cDatabase oDatabase;
            cCacheFunc oCache;
            List<cmlTSysConfig> aoSysConfig;
            StringBuilder oSql;

            try
            {
                oCache = new cCacheFunc();

                // Check cache.
                if (oCache.C_CAHbExistsKey("TSysConfig"))
                {
                    aoSysConfig = oCache.C_CAHoGetKey<List<cmlTSysConfig>>("TSysConfig");
                }
                else
                {
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FTSysCode, FTSysSeq, FTSysStaUsrValue, FTSysStaUsrRef");
                    oSql.AppendLine("FROM TSysConfig WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTSysCode = 'PApi2Master'");
                    oSql.AppendLine("WHERE FTSysCode = 'tCN_API2PSMaster'"); //*Em 62-05-30 AdaFC
                    oSql.AppendLine("ORDER BY FTSysSeq");

                    oDatabase = new cDatabase();
                    aoSysConfig = oDatabase.C_DATaSqlQuery<cmlTSysConfig>(oSql.ToString());

                    if (aoSysConfig != null && aoSysConfig.Count > 0)
                    {
                        // Add cache.
                        oCache.C_CAHxAddKey("TSysConfig", aoSysConfig);
                    }
                }

                return aoSysConfig;
            }
            catch (Exception oEx)
            {
                Console.WriteLine(oEx.Message);
            }
            finally
            {
                oDatabase = null;
                oCache = null;
                oSql = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return null;
        }

        /// <summary>
        ///     Get configuration from memory.
        /// </summary>
        /// 
        /// <typeparam name="T">Type data user value.</typeparam>
        /// <typeparam name="E">Type data user default.</typeparam>
        /// <param name="poUsrValue">ref Data user value.</param>
        /// <param name="poDefValue">Default data user value.</param>
        /// <param name="poUsrRef">ref Data user referent.</param>
        /// <param name="poDefRef">Default data user referent.</param>
        /// <param name="paoSysConfig">Valiable TSysConfig in memory.</param>
        /// <param name="ptSysSeq">Sequence.</param>
        public void SP_DATxGetConfigurationFromMem<T, E>(ref T poUsrValue, T poDefValue, ref E poUsrRef, E poDefRef,
            List<cmlTSysConfig> paoSysConfig, string ptSysSeq)
        {
            try
            {
                // UsrValue.
                try
                {
                    poUsrValue = (T)Convert.ChangeType(paoSysConfig.Where(
                        oItem => string.Equals(oItem.FTSysSeq, ptSysSeq)).Select(oItem => oItem.FTSysUsrValue).FirstOrDefault(), typeof(T));
                }
                catch (Exception)
                {
                    poUsrValue = poDefValue;
                }

                // UsrRef.
                try
                {
                    poUsrRef = (E)Convert.ChangeType(paoSysConfig.Where(
                        oItem => string.Equals(oItem.FTSysSeq, ptSysSeq)).Select(oItem => oItem.FTSysUsrRef).FirstOrDefault(), typeof(E));
                }
                catch (Exception)
                {
                    poUsrRef = poDefRef;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        ///     Get configuration from memory.
        /// </summary>
        /// 
        /// <typeparam name="T">Type data user value.</typeparam>
        /// <param name="poUsrValue">ref Data user value.</param>
        /// <param name="poDefValue">Default data user value.</param>
        /// <param name="paoSysConfig">Valiable TSysConfig in memory.</param>
        /// <param name="ptSysSeq">Sequence.</param>
        public void SP_DATxGetConfigurationFromMem<T>(out T poUsrValue, T poDefValue, List<cmlTSysConfig> paoSysConfig, string ptSysSeq)
        {
            try
            {
                // UsrValue.
                try
                {
                    poUsrValue = (T)Convert.ChangeType(paoSysConfig.Where(
                        oItem => string.Equals(oItem.FTSysSeq, ptSysSeq)).Select(oItem => oItem.FTSysUsrValue).FirstOrDefault(), typeof(T));
                }
                catch (Exception)
                {
                    poUsrValue = poDefValue;
                }
            }
            catch (Exception)
            {
                poUsrValue = poDefValue;
            }
        }

        /// <summary>
        ///     Check allow to user function in range time.
        /// </summary>
        /// 
        /// <param name="paoSysConfig">Configuration.</param>
        /// 
        /// <returns>
        ///     true : Allow to use.<br/>
        ///     false : Not allow to use.
        /// </returns>
        public bool SP_CHKbAllowRangeTime(List<cmlTSysConfig> paoSysConfig)
        {
            TimeSpan oTmeStart, oTmeEnd, oTmeNow;
            string tTmeStart, tTmeEnd, tTmeNow;

            try
            {
                oTmeNow = DateTime.Now.TimeOfDay;
                tTmeNow = oTmeNow.ToString("hh\\:mm");
                tTmeStart = "";
                tTmeEnd = "";

                SP_DATxGetConfigurationFromMem<string, string>(ref tTmeStart, tTmeNow, ref tTmeEnd, tTmeNow, paoSysConfig, "002");

                oTmeStart = TimeSpan.Parse(tTmeStart);
                oTmeEnd = TimeSpan.Parse(tTmeEnd);

                // Check time use function.
                if ((oTmeNow >= oTmeStart) && (oTmeNow <= oTmeEnd))
                {
                    return true;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return false;
        }

        /// <summary>
        ///     Check key API.
        /// </summary>
        /// 
        /// <param name="ptKeyApi">ref Key API.</param>
        /// <param name="ptFuncName">Function name.</param>
        /// <param name="poHttpContext">HttpContext.</param>
        /// 
        /// <returns>
        ///     true : Verify key API pass.
        ///     false : Verify key API false.
        /// </returns>
        public bool SP_CHKbKeyApi(out string ptKeyApi, List<cmlTSysConfig> paoSysConfig, HttpContext poHttpContext)
        {
            cDatabase oDatabase;
            cCacheFunc oCacheFunc;
            StringBuilder oSql;
            //*Ton 64-05-21
            //NameValueCollection oReqHeaders;
            IHeaderDictionary oReqHeaders;

            string tKeyApi;
            string tConfigAPIKey = ""; //*Arm 65-09-20
            int nConTme, nCmdTme;

            try
            {
                //*Ton 64-05-21
                //oReqHeaders = poHttpContext.Request.Headers;
                oReqHeaders = poHttpContext.Request.Headers;
                //tKeyApi = oReqHeaders.Get("X-Api-Key");
                tKeyApi = oReqHeaders["X-Api-Key"];

                //tAgnPwd = oReqHeaders.Get("AgencyPwd");

                // ถ้ามี Key Api ส่งมาใน Header
                if (!string.IsNullOrEmpty(tKeyApi))
                {
                    oCacheFunc = new cCacheFunc();
                    //tCacheKey = tKeyApi + "|" + ptFuncName;

                    if (oCacheFunc.C_CAHbExistsKey(tKeyApi))
                    {
                        // ถ้ามี key อยุ่ใน cache
                        ptKeyApi = tKeyApi;
                        return oCacheFunc.C_CAHoGetKey<bool>(tKeyApi);
                    }

                    // ถ้าไม่มี key อยู่ใน cache
                    oSql = new StringBuilder();
                    //oSql.AppendLine("SELECT FTAgnKeyAPI");
                    //oSql.AppendLine("FROM TCNMAgency WITH(NOLOCK)");
                    //oSql.AppendLine("WHERE FTAgnKeyAPI = '" + tKeyApi + "'");
                    //oSql.AppendLine("AND FTAgnStaApv = '1'");
                    //oSql.AppendLine("AND FTAgnStaActive = '1'");

                    //*Arm 65-09-20
                    oSql.AppendLine("SELECT ISNULL(FTSysStaUsrValue,'') AS FTSysStaUsrValue ");
                    oSql.AppendLine("FROM  TSysConfig WITH (NOLOCK) WHERE FTSysKey='POS' AND FTSysSeq='1' AND FTSysCode='tCN_AgnKeyAPI'");
                    //+++++++++++++

                    SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, paoSysConfig, "1");
                    SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, paoSysConfig, "2");

                    oDatabase = new cDatabase(nConTme, nCmdTme);
                    //*Arm 65-09-20 - Comment Code
                    ////tKeyApi = oDatabase.C_DAToSqlQuery<string>(oSql.ToString());
                    //Guid oKeyApi = oDatabase.C_DAToSqlQuery<Guid>(oSql.ToString());
                    //tKeyApi = oKeyApi.ToString();
                    //++++++++++++++

                    tConfigAPIKey = oDatabase.C_DAToSqlQuery<string>(oSql.ToString()); //*Arm 65-09-20

                    //if (string.IsNullOrEmpty(tKeyApi)) //*Arm 65-09-20 - Comment Code
                    if (string.IsNullOrEmpty(tConfigAPIKey))
                    {
                        // ไม่มีข้อมูล KeyApi ในฐานข้อมูล หรือไม่อนุญาตให้ใช้งาน
                        ptKeyApi = "";
                        return false;
                    }
                    //*Arm 65-09-20
                    else
                    {
                        if(tKeyApi != tConfigAPIKey)
                        {
                            ptKeyApi = "";
                            return false;
                        }
                    }
                    //+++++++++++++

                    // เก็บ KeyApi ลง Cache
                    //ptKeyApi = tKeyApi; //*Arm 65-09-20 - Comment Code
                    ptKeyApi = tConfigAPIKey; //*Arm 65-09-20
                    oCacheFunc.C_CAHxAddKey(ptKeyApi, true);
                    return true;
                }
            }
            catch (Exception)
            {
                ptKeyApi = "";
                return false;
            }
            finally
            {
                oCacheFunc = null;
                oReqHeaders = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
            ptKeyApi = "";
            return false;
        }
        public string SP_SQLtGetKeyAPI(string ptKey, string ptPwd)
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT AGN.FTAgnKeyAPI");
                oSql.AppendLine("FROM TCNMAgency AGN");
                oSql.AppendLine("WHERE AGN.FTAgnKeyAPI = '" + ptKey + "'");
                oSql.AppendLine("AND AGN.FTAgnPwd = '"+ ptPwd +"'");
                oSql.AppendLine("AND AGN.FTAgnStaApv = '1'");
                oSql.AppendLine("AND AGN.FTAgnStaActive = '1'");

                return oSql.ToString();
            }
            catch (Exception)
            {

            }
            finally
            {
                oSql = null;
            }

            return "";
        }

        /// <summary>
        ///     Save image from encoded string base64.
        /// </summary>
        /// 
        /// <param name="ptImgName">Image name.</param>
        /// <param name="ptPathSave">Path to save image.</param>
        /// <param name="ptImgEncStr">Encoded string base64.</param>
        /// <param name="ptFullImg">out Full path image.</param>
        /// <param name="pbDelOld">Status delete old image.</param>
        /// 
        /// <returns>
        ///     true : Save complete.<br/>
        ///     false : Save false.
        /// </returns>
        public bool SP_IMGbSaveStr2Img(string ptImgName, string ptPathSave, string ptImgEncStr, string ptFullImg, bool pbDelOld = false)
        {
            System.Drawing.Image oImage;
            MemoryStream oMemStream;
            byte[] anBuffer;

            try
            {
                if (!string.IsNullOrEmpty(ptImgEncStr))
                {
                    if (ptImgEncStr.Split(',').Count() == 2)
                    {
                        ptImgEncStr = ptImgEncStr.Split(',')[1];
                    }

                    anBuffer = Convert.FromBase64String(ptImgEncStr);
                    using (oMemStream = new MemoryStream(anBuffer, 0, anBuffer.Length))
                    {
                        oImage = System.Drawing.Image.FromStream(oMemStream, true);

                        if (!ptPathSave.EndsWith("\\"))
                        {
                            ptPathSave += "\\";
                        }

                        ptFullImg = ptPathSave + ptImgName + ".png";

                        if (pbDelOld)
                        {
                            if (File.Exists(ptFullImg))
                            {
                                File.Delete(ptFullImg);
                            }
                        }

                        oImage.Save(ptFullImg, System.Drawing.Imaging.ImageFormat.Png);

                        oMemStream.Dispose();
                        oImage.Dispose();
                    }

                    return true;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                oImage = null;
                oMemStream = null;
                anBuffer = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            ptFullImg = "";
            return false;
        }

        /// <summary>
        ///     Delete image.
        /// </summary>
        /// 
        /// <param name="ptPathImg">Path image.</param>
        /// 
        /// <returns>
        ///     true : Delete success.<br/>
        ///     false : Delete false.
        /// </returns>
        public bool SP_IMGbDeleteImage(string ptPathImg)
        {
            try
            {
                if (File.Exists(ptPathImg))
                {
                    File.Delete(ptPathImg);
                }

                return true;
            }
            catch (Exception)
            {

            }
            finally
            {

            }

            return false;
        }

        /// <summary>
        ///     Generate code with format.
        /// </summary>
        /// 
        /// <param name="ptTableName">Table name.</param>
        /// <param name="ptField">Field code.</param>
        /// <param name="ptDocType">Document type.</param>
        /// <param name="ptBchCode">Branch code.</param>
        /// <param name="paoSysConfig">System configuration.</param>
        /// <param name="ptOutCode">
        ///     out Code in success.<br/>
        ///     Case false code out.<br/>
        ///         -1 : function error.<br/>
        ///         -2 : format code not found.<br/>
        ///         -3 : format code is maximum running number.<br/>
        /// </param>
        /// 
        /// <returns>
        ///     true : generate success.<br/>
        ///     false : genreate false.
        /// </returns>
        public bool SP_GENbAutoFmtCode(
           string ptTableName, string ptField, string ptDocType, string ptBchCode, List<cmlTSysConfig> paoSysConfig, out string ptOutCode)
        {
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlTCNTAuto oTCNTAuto;
            cmlAutoFmtCode oAutoFmtCode;
            string tYear, tMonth, tDay, tCYMDSep, tLeftCode, tMaxNum, tNextCode, tResult;
            int nConTme, nCmdTme, nLenFTCode, nLenRight, nLenLeft, nNextCode;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTSatUsrStaBch,FTSatStaDefUsage,FTSatDefChar,FTSatDefYear,");
                oSql.AppendLine("	FTSatDefMonth,FTSatDefDay,FTSatDefNum,FTSatDefFmtAll,");
                oSql.AppendLine("	FTSatUsrFmtChar,FTSatUsrFmtYear,FTSatUsrFmtMonth,FTSatUsrFmtDay,");
                oSql.AppendLine("	FTSatUsrFmtNum,FTSatUsrFmtAll,FTSatStaResetBill");
                oSql.AppendLine("FROM TCNTAuto WITH(NOLOCK)");
                oSql.AppendLine("WHERE UPPER(FTSatTblName) = UPPER('" + ptTableName + "')");
                oSql.AppendLine("AND UPPER(FTSatFedCode) = UPPER('" + ptField + "')");
                oSql.AppendLine("AND (FTSatStaDocType = '" + ptDocType + "' OR FTSatStaDocType = '0'");
                oSql.AppendLine("	OR FTSatStaDocType LIKE ('" + ptDocType + ",%')");
                oSql.AppendLine("	OR FTSatStaDocType LIKE ('%," + ptDocType + ",%')");
                oSql.AppendLine("	OR FTSatStaDocType LIKE ('%," + ptDocType + "'))");

                SP_DATxGetConfigurationFromMem<int>(out nConTme, cCS.nCS_ConTme, paoSysConfig, "1");
                SP_DATxGetConfigurationFromMem<int>(out nCmdTme, cCS.nCS_CmdTme, paoSysConfig, "2");

                oDatabase = new cDatabase(nConTme, nCmdTme);
                oTCNTAuto = oDatabase.C_DAToSqlQuery<cmlTCNTAuto>(oSql.ToString());

                if (oTCNTAuto != null)
                {
                    oAutoFmtCode = new cmlAutoFmtCode();

                    // check use branch in format.
                    oAutoFmtCode.FTBchCode = "";
                    if (string.Equals(oTCNTAuto.FTSatUsrStaBch, "1"))
                    {
                        oAutoFmtCode.FTBchCode = ptBchCode;
                    }

                    if (string.Equals(oTCNTAuto.FTSatStaDefUsage, "1"))
                    {
                        // default value.
                        oAutoFmtCode.FTSatRetFmtChar = oTCNTAuto.FTSatDefChar;
                        oAutoFmtCode.FTSatRetFmtYear = oTCNTAuto.FTSatDefYear;
                        oAutoFmtCode.FTSatRetFmtMonth = oTCNTAuto.FTSatDefMonth;
                        oAutoFmtCode.FTSatRetFmtDay = oTCNTAuto.FTSatDefDay;
                        oAutoFmtCode.FTSatRetFmtNum = oTCNTAuto.FTSatDefNum;
                        oAutoFmtCode.FTSatRetFmtAll = oTCNTAuto.FTSatDefFmtAll;
                    }
                    else
                    {
                        // user value.
                        oAutoFmtCode.FTSatRetFmtChar = oTCNTAuto.FTSatUsrFmtChar;
                        oAutoFmtCode.FTSatRetFmtYear = oTCNTAuto.FTSatUsrFmtYear;
                        oAutoFmtCode.FTSatRetFmtMonth = oTCNTAuto.FTSatUsrFmtMonth;
                        oAutoFmtCode.FTSatRetFmtDay = oTCNTAuto.FTSatUsrFmtDay;
                        oAutoFmtCode.FTSatRetFmtNum = oTCNTAuto.FTSatUsrFmtNum;
                        oAutoFmtCode.FTSatRetFmtAll = oTCNTAuto.FTSatUsrFmtAll;
                    }

                    oAutoFmtCode.FTPKField = ptField;
                    oAutoFmtCode.FTTableName = ptTableName;
                    oAutoFmtCode.FTDocType = ptDocType;
                    oAutoFmtCode.FTSatStaResetBill = oTCNTAuto.FTSatStaResetBill;

                    tYear = "";
                    if (string.Equals(oAutoFmtCode.FTSatRetFmtYear, "1"))
                    {
                        tYear = DateTime.Now.ToString("yy");
                    }
                    else if (string.Equals(oAutoFmtCode.FTSatRetFmtYear, "2"))
                    {
                        tYear = DateTime.Now.ToString("yyyy");
                    }

                    tMonth = "";
                    if (string.Equals(oAutoFmtCode.FTSatRetFmtMonth, "1"))
                    {
                        tMonth = DateTime.Now.ToString("MM");
                    }

                    tDay = "";
                    if (string.Equals(oAutoFmtCode.FTSatRetFmtDay, "1"))
                    {
                        tDay = DateTime.Now.ToString("dd");
                    }

                    tCYMDSep = string.Concat(oAutoFmtCode.FTSatRetFmtChar, oAutoFmtCode.FTBchCode, tYear, tMonth, tDay);
                    if (!string.IsNullOrEmpty(tCYMDSep))
                    {
                        tCYMDSep = string.Concat(tCYMDSep, "-");
                    }

                    nLenFTCode = oAutoFmtCode.FTSatRetFmtAll.Length;
                    nLenRight = oAutoFmtCode.FTSatRetFmtNum.Length;

                    if (string.Equals(oAutoFmtCode.FTSatStaResetBill, "0"))
                    {
                        tLeftCode = string.Concat(oAutoFmtCode.FTSatRetFmtChar, oAutoFmtCode.FTBchCode);
                        nLenLeft = tLeftCode.Length;
                    }
                    else
                    {
                        tLeftCode = string.Concat(oAutoFmtCode.FTSatRetFmtChar, oAutoFmtCode.FTBchCode, tYear);
                        nLenLeft = tLeftCode.Length;
                    }

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT MAX(" + ptField + ") AS MaxValue");
                    oSql.AppendLine("FROM " + ptTableName);
                    oSql.AppendLine("WHERE LEN(" + ptField + ") = " + nLenFTCode);
                    oSql.AppendLine("AND LEFT(" + ptField + "," + nLenLeft + ") = '" + tLeftCode + "'");
                    oSql.AppendLine("AND ISNUMERIC(RIGHT(" + ptField + "," + nLenRight + ")) = 1");

                    tResult = oDatabase.C_DAToSqlQuery<string>(oSql.ToString());
                    if (string.IsNullOrEmpty(tResult))
                    {
                        ptOutCode = string.Concat(tCYMDSep, "1".PadLeft(nLenRight, '0'));
                        return true;
                    }
                    else
                    {
                        tMaxNum = "";
                        tMaxNum = tMaxNum.PadLeft(oAutoFmtCode.FTSatRetFmtNum.Length, '9');
                        tNextCode = tResult.Substring(tResult.Length - nLenRight);

                        if (string.Equals(tMaxNum, tNextCode))
                        {
                            // maximum running number.
                            ptOutCode = "-3";
                            return false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tNextCode))
                            {
                                tNextCode = "0";
                            }
                            nNextCode = int.Parse(tNextCode) + 1;
                            ptOutCode = string.Concat(tCYMDSep, nNextCode.ToString().PadLeft(nLenRight, '0'));
                            return true;
                        }
                    }

                }
                else
                {
                    // format code not found.
                    ptOutCode = "-2";
                    return false;
                }

            }
            catch (Exception)
            {
                ptOutCode = "-1";
                return false;
            }
            finally
            {
                oDatabase = null;
                oSql = null;
                oTCNTAuto = null;
                oAutoFmtCode = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
        }
        /// <summary>
        /// Generate sql command update by model
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="poModel">Model to generate sql command</param>
        /// <param name="ptWhoUpd"> Who update</param>
        /// <returns></returns>
        public string SP_SQLGeneralCmdUpdate<T>(T poModel)
        {
            return null;
        }

        //*Ton 64-05-22 Bypass for testing.
        //public string SP_SQLGeneralCmdUpdate<T>(T poModel)
        //{
        //    Type oType;
        //    StringBuilder oSqlUpd, oSql;
        //    TableNameAttribute oTblNameAtb;
        //    string tSqlUpd, tSqlFmt, tValue;
        //    try
        //    {
        //        oTblNameAtb = (TableNameAttribute)poModel.GetType().GetCustomAttributes(false).FirstOrDefault();
        //        oSql = new StringBuilder();
        //        oSql.AppendLine(" UPDATE");
        //        oSql.AppendLine(" " + oTblNameAtb.Name + " WITH(ROWLOCK)");
        //        oSql.AppendLine(" SET");

        //        tSqlFmt = " {0} = '{1}',";
        //        oType = poModel.GetType();

        //        oSqlUpd = new StringBuilder();
        //        foreach (PropertyInfo oProperty in oType.GetProperties())
        //        {
        //            string tName = oProperty.Name.Substring(0, 2);
        //            string tField = oProperty.Name.Substring(2);
        //            switch (oProperty.Name.Substring(0, 2))
        //            {
        //                case "pn":
        //                    tField = "FN" + tField;
        //                    tValue = oProperty.GetValue(poModel)?.ToString() ?? "null";
        //                    break;
        //                case "pc":
        //                    tField = "FC" + tField;
        //                    tValue = oProperty.GetValue(poModel)?.ToString() ?? "null";
        //                    break;
        //                case "pd":
        //                    tField = "FD" + tField;
        //                    tValue = oProperty.GetValue(poModel)?.ToString() ?? "null";
        //                    if (!string.Equals(tValue, "null"))
        //                    {
        //                        tValue = DateTime.Parse(tValue).ToString("yyyy-MM-dd HH:mm:ss:fff");
        //                    }
        //                    break;
        //                case "pt":
        //                    tField = "FT" + tField;
        //                    tValue = oProperty.GetValue(poModel)?.ToString();
        //                    break;
        //                default:
        //                    tValue = "";
        //                    break;
        //            }

        //            if (!string.IsNullOrEmpty(tValue) && !string.Equals(tValue, null))
        //            {
        //                oSqlUpd.AppendLine(string.Format(tSqlFmt, tField, tValue));
        //            }

        //        }
        //        //tSqlUpd = oSqlUpd.Remove(tSqlUpd.Length - 6);
        //        tSqlUpd = oSqlUpd.ToString().Remove(oSqlUpd.Length - 1);

        //        return tSqlUpd;
        //    }
        //    catch (Exception oExn)
        //    {
        //        return "";
        //    }
        //    finally
        //    {

        //    }
        //}

        /// <summary>
        /// Generate sql insert command.
        /// </summary>
        /// 
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="poData">Model data.</param>
        /// <param name="ptTblName">Table name.</param>
        /// <param name="ptWhoUpd">User code save data.</param>
        /// <param name="pbStaWhoIns">Status save user code and data time.</param>
        /// <param name="pbStaWhoUpd">Status save user code and data time.</param>
        /// 
        /// <returns>
        /// Sql insert command.
        /// </returns>
        public string SP_GENtSqlCmdInsert<T>(T poData, string ptTblName, string ptWhoUpd, bool pbStaWhoIns = true, bool pbStaWhoUpd = true)
        {
            DefaultValueAttribute oDefValueAttr;
            StringBuilder oSqlIns, oSqlField, oSqlValue;
            string tSqlCmd, tFmtDate;

            try
            {
                oSqlIns = new StringBuilder();
                oSqlField = new StringBuilder();
                oSqlValue = new StringBuilder();

                tSqlCmd = "";
                oSqlIns.AppendLine("INSERT INTO " + ptTblName + " WITH(ROWLOCK)");
                oSqlIns.AppendLine("(");
                oSqlIns.AppendLine("{0}");

                if (pbStaWhoUpd)
                {
                    //oSqlIns.AppendLine("FDDateUpd,");
                    //oSqlIns.AppendLine("FTTimeUpd,");
                    oSqlIns.AppendLine("FDLastUpdOn,"); //*Em 61-10-01 ปรับฟิดล์ update
                    if (pbStaWhoIns)
                    {
                        //oSqlIns.AppendLine("FTWhoUpd,");
                        oSqlIns.AppendLine("FTCreateBy,");  //*Em 61-10-01 ปรับฟิดล์ update
                    }
                    else
                    {
                        //oSqlIns.AppendLine("FTWhoUpd");
                        oSqlIns.AppendLine("FTLastUpdBy");  //*Em 61-10-01 ปรับฟิดล์ update
                    }
                }
                if (pbStaWhoIns)
                {
                    //oSqlIns.AppendLine("FDDateIns,");
                    //oSqlIns.AppendLine("FTTimeIns,");
                    //oSqlIns.AppendLine("FTWhoIns");
                    oSqlIns.AppendLine("FDCreateOn,");  //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("FTCreateBy");   //*Em 61-10-01 ปรับฟิดล์ update
                }

                oSqlIns.AppendLine(")");
                oSqlIns.AppendLine("VALUES");
                oSqlIns.AppendLine("(");
                oSqlIns.AppendLine("{1}");

                if (pbStaWhoUpd)
                {
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(10), GETDATE(), 121),");
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(8),GETDATE(),108),");
                    oSqlIns.AppendLine("GETDATE(),");   //*Em 61-10-01 ปรับฟิดล์ update
                    if (pbStaWhoIns)
                    {
                        oSqlIns.AppendLine("'" + ptWhoUpd + "',");
                    }
                    else
                    {
                        oSqlIns.AppendLine("'" + ptWhoUpd + "'");
                    }

                }
                if (pbStaWhoIns)
                {
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(10), GETDATE(), 121),");
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(8),GETDATE(),108),");
                    oSqlIns.AppendLine("GETDATE(),");   //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("'" + ptWhoUpd + "'");
                }

                oSqlIns.AppendLine(");");
                oSqlIns.AppendLine("");

                foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poData))
                {
                    oDefValueAttr = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                    switch (oProperty.Name.Substring(0, 2).ToUpper())
                    {
                        case "PT":
                            oSqlField.AppendLine(oProperty.Name.ToString().Replace("pt", "FT") + ",");

                            if (oProperty.GetValue(poData) == null || Convert.ToString(oProperty.GetValue(poData)) == "")
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlValue.AppendLine("'" + oDefValueAttr.Value + "',");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlValue.AppendLine("'" + oProperty.GetValue(poData) + "',");
                            break;
                        case "PN":
                            oSqlField.AppendLine(oProperty.Name.ToString().Replace("pn", "FN") + ",");

                            if (oProperty.GetValue(poData) == null)
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlValue.AppendLine(oDefValueAttr.Value + ",");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlValue.AppendLine(oProperty.GetValue(poData) + ",");
                            break;
                        case "PC":
                            oSqlField.AppendLine(oProperty.Name.ToString().Replace("pc", "FC") + ",");

                            if (oProperty.GetValue(poData) == null)
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlValue.AppendLine(oDefValueAttr.Value + ",");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlValue.AppendLine(oProperty.GetValue(poData) + ",");
                            break;
                        case "PD":
                            oSqlField.AppendLine(oProperty.Name.ToString().Replace("pd", "FD") + ",");

                            if (string.IsNullOrEmpty(oProperty.Description))
                            {
                                //tFmtDate = "yyyy-MM-dd";
                                tFmtDate = "yyyy-MM-dd HH:mm:ss";   //*Em 61-10-01 ปรับฟิดล์ update
                            }
                            else
                            {
                                tFmtDate = oProperty.Description;
                            }

                            if (oProperty.GetValue(poData) == null)
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlValue.AppendLine("'" + Convert.ToDateTime(oDefValueAttr.Value).ToString(tFmtDate) + "',");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlValue.AppendLine("'" + Convert.ToDateTime(oProperty.GetValue(poData)).ToString(tFmtDate) + "',");
                            break;
                    }
                }

                if (pbStaWhoIns || pbStaWhoUpd)
                {
                    tSqlCmd = string.Format(oSqlIns.ToString(), oSqlField.ToString(), oSqlValue.ToString());
                }
                else
                {
                    tSqlCmd = string.Format(oSqlIns.ToString(),
                        oSqlField.ToString().Remove(oSqlField.Length - 3), oSqlValue.ToString().Remove(oSqlValue.Length - 3));
                }

                return tSqlCmd;
            }
            catch (Exception)
            {

            }
            finally
            {
                oDefValueAttr = null;
                oSqlIns = null;
                oSqlField = null;
                oSqlValue = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return "";
        }
        /// <summary>
        /// Generate sql update master command.
        /// </summary>
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="poData">Model data.</param>
        /// <param name="ptTblName">Table name.</param>
        /// <param name="ptFedWhe">Field condition.</param>
        /// <param name="ptCode">Code value use in condition.</param>
        /// <param name="ptWhoUpd">User code save data.</param>
        /// <param name="pbStaWhoUpd">Status save user code and data time.</param>
        /// 
        /// <returns>
        /// Sql update command.
        /// </returns>
        public string SP_GENtSqlCmdUpdateMaster<T>(
            T poData, string ptTblName, string ptFedWhe, string ptCode, string ptWhoUpd, bool pbStaWhoUpd = true)
        {
            StringBuilder oSqlUpd, oSqlValue;
            string tSqlCmd, tFmtDate, tFieldName;

            try
            {
                oSqlUpd = new StringBuilder();
                oSqlValue = new StringBuilder();

                tSqlCmd = "";
                oSqlUpd.AppendLine("UPDATE " + ptTblName + " WITH(ROWLOCK)");
                oSqlUpd.AppendLine("SET");
                oSqlUpd.AppendLine("{0}");

                if (pbStaWhoUpd)
                {
                    //oSqlUpd.AppendLine("FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),");
                    //oSqlUpd.AppendLine("FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),114),");
                    //oSqlUpd.AppendLine("FTWhoUpd = '" + ptWhoUpd + "'");
                    oSqlUpd.AppendLine("FDLastUpdOn = GETDATE(),");     //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlUpd.AppendLine("FTLastUpdBy = '" + ptWhoUpd + "'"); //*Em 61-10-01 ปรับฟิดล์ update
                }

                oSqlUpd.AppendLine("WHERE " + ptFedWhe + " = '" + ptCode + "'");
                oSqlUpd.AppendLine("IF @@ROWCOUNT > 0");
                oSqlUpd.AppendLine("BEGIN");
                oSqlUpd.AppendLine("{1}");
                oSqlUpd.AppendLine("END");
                oSqlUpd.AppendLine("");

                foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poData))
                {
                    tFieldName = oProperty.Name.ToString();
                    switch (tFieldName.Substring(0, 2).ToUpper())
                    {
                        case "PT":
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                oSqlValue.AppendLine("FT" + tFieldName.Substring(2, tFieldName.Length - 2) + " = '" + oProperty.GetValue(poData) + "',");
                            }

                            break;
                        case "PN":
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                oSqlValue.AppendLine("FN" + tFieldName.Substring(2, tFieldName.Length - 2) + " = " + oProperty.GetValue(poData) + ",");
                            }

                            break;
                        case "PC":
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                oSqlValue.AppendLine("FC" + tFieldName.Substring(2, tFieldName.Length - 2) + " = " + oProperty.GetValue(poData) + ",");
                            }

                            break;
                        case "PD":
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                if (string.IsNullOrEmpty(oProperty.Description))
                                {
                                    //tFmtDate = "yyyy-MM-dd";
                                    tFmtDate = "yyyy-MM-dd HH:mm:ss";   //*Em 61-10-01 ปรับฟิดล์ update
                                }
                                else
                                {
                                    tFmtDate = oProperty.Description;
                                }

                                oSqlValue.Append("FD" + tFieldName.Substring(2, tFieldName.Length - 2) + " = '");
                                oSqlValue.AppendLine(Convert.ToDateTime(oProperty.GetValue(poData)).ToString(tFmtDate) + "',");
                            }

                            break;
                    }
                }

                if (pbStaWhoUpd)
                {
                    tSqlCmd = string.Format(oSqlUpd.ToString(), oSqlValue.ToString(), "{0}");
                }
                else
                {
                    tSqlCmd = string.Format(oSqlUpd.ToString(), oSqlValue.ToString().Remove(oSqlValue.Length - 3), "{0}");
                }

                return tSqlCmd;
            }
            catch (Exception)
            {

            }
            finally
            {
                oSqlUpd = null;
                oSqlValue = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return "";
        }
        /// <summary>
        /// Generate sql update language command.
        /// </summary>
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="poData">Model data.</param>
        /// <param name="ptTblName">Table name.</param>
        /// <param name="ptFedWhe">>Field condition.</param>
        /// <param name="ptCode">Code value use in condition.</param>
        /// <param name="ptWhoUpd">User code save data.</param>
        /// <param name="pnLngID">Language ID.</param>
        /// <param name="pbStaWhoUpd">Status save user code and data time.</param>
        /// <returns></returns>
        public string SP_GENtSqlCmdUpdateLanguage<T>(
            T poData, string ptTblName, string ptFedWhe, string ptCode, string ptWhoUpd, Nullable<long> pnLngID, bool pbStaWhoUpd = false)
        {
            DefaultValueAttribute oDefValueAttr;
            StringBuilder oSqlUpd, oSqlUpdValue, oSqlIns, oSqlInsField, oSqlInsValue;
            string tSqlCmdUpd, tSqlCmdIns, tFmtDate, tFieldName;

            try
            {
                oSqlUpd = new StringBuilder();
                oSqlUpdValue = new StringBuilder();
                oSqlIns = new StringBuilder();
                oSqlInsField = new StringBuilder();
                oSqlInsValue = new StringBuilder();

                tSqlCmdUpd = "";
                tSqlCmdIns = "";

                // Update.
                oSqlUpd.AppendLine("UPDATE " + ptTblName + " WITH(ROWLOCK)");
                oSqlUpd.AppendLine("SET");
                oSqlUpd.AppendLine("{0}");

                // Insert.
                oSqlIns.AppendLine("INSERT INTO " + ptTblName + " WITH(ROWLOCK)");
                oSqlIns.AppendLine("(");
                oSqlIns.AppendLine(ptFedWhe + ",");
                oSqlIns.AppendLine("FNLngID,");
                oSqlIns.AppendLine("{0}");

                if (pbStaWhoUpd)
                {
                    // Update.
                    //oSqlUpd.AppendLine("FDDateUpd = CONVERT(VARCHAR(10), GETDATE(), 121),");
                    //oSqlUpd.AppendLine("FTTimeUpd = CONVERT(VARCHAR(8),GETDATE(),114),");
                    //oSqlUpd.AppendLine("FTWhoUpd = '" + ptWhoUpd + "'");
                    oSqlUpd.AppendLine("FDLastUpdOn = GETDATE(),");     //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlUpd.AppendLine("FTLastUpdBy = '" + ptWhoUpd + "'");     //*Em 61-10-01 ปรับฟิดล์ update

                    // Insert.
                    //oSqlIns.AppendLine("FDDateUpd,");
                    //oSqlIns.AppendLine("FTTimeUpd,");
                    //oSqlIns.AppendLine("FTWhoUpd,");
                    //oSqlIns.AppendLine("FDDateIns,");
                    //oSqlIns.AppendLine("FTTimeIns,");
                    //oSqlIns.AppendLine("FTWhoIns");
                    oSqlIns.AppendLine("FDLastUpdOn,");     //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("FTLastUpdBy,");     //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("FDCreateOn,");      //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("FTCreateBy");       //*Em 61-10-01 ปรับฟิดล์ update
                }

                // Update.
                oSqlUpd.AppendLine("WHERE " + ptFedWhe + " = '" + ptCode + "'");
                oSqlUpd.AppendLine("AND FNLngID  = " + pnLngID + "");
                oSqlUpd.AppendLine("");
                oSqlUpd.AppendLine("IF @@ROWCOUNT = 0");
                oSqlUpd.AppendLine("BEGIN");
                oSqlUpd.AppendLine("{1}");
                oSqlUpd.AppendLine("END");

                // Insert.
                oSqlIns.AppendLine(")");
                oSqlIns.AppendLine("VALUES");
                oSqlIns.AppendLine("(");
                oSqlIns.AppendLine("'" + ptCode + "',");
                oSqlIns.AppendLine(pnLngID + ",");
                oSqlIns.AppendLine("{1}");

                // Insert.
                if (pbStaWhoUpd)
                {
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(10), GETDATE(), 121),");
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(8),GETDATE(),114),");
                    //oSqlIns.AppendLine("'" + ptWhoUpd + "',");
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(10), GETDATE(), 121),");
                    //oSqlIns.AppendLine("CONVERT(VARCHAR(8),GETDATE(),114),");
                    //oSqlIns.AppendLine("'" + ptWhoUpd + "'");

                    oSqlIns.AppendLine("GETDATE(),");       //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("'" + ptWhoUpd + "',");      //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("GETDATE(),");       //*Em 61-10-01 ปรับฟิดล์ update
                    oSqlIns.AppendLine("'" + ptWhoUpd + "'");       //*Em 61-10-01 ปรับฟิดล์ update
                }

                oSqlIns.AppendLine(");");
                oSqlIns.AppendLine("");

                foreach (PropertyDescriptor oProperty in TypeDescriptor.GetProperties(poData))
                {
                    oDefValueAttr = (DefaultValueAttribute)oProperty.Attributes[typeof(DefaultValueAttribute)];
                    tFieldName = oProperty.Name.ToString();
                    switch (tFieldName.Substring(0, 2).ToUpper())
                    {
                        case "PT":
                            // Update.
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                oSqlUpdValue.AppendLine("FT" + tFieldName.Substring(2, tFieldName.Length - 2) + " = '" + oProperty.GetValue(poData) + "',");
                            }

                            // Insert.
                            oSqlInsField.AppendLine("FT" + tFieldName.Substring(2, tFieldName.Length - 2) + ",");
                            if (oProperty.GetValue(poData) == null || Convert.ToString(oProperty.GetValue(poData)) == "")
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlInsValue.AppendLine("'" + oDefValueAttr.Value + "',");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlInsValue.AppendLine("'" + oProperty.GetValue(poData) + "',");

                            break;
                        case "PN":
                            // Update.
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                oSqlUpdValue.AppendLine("FN" + tFieldName.Substring(2, tFieldName.Length - 2) + " = " + oProperty.GetValue(poData) + ",");
                            }

                            // Insert.
                            oSqlInsField.AppendLine("FN" + tFieldName.Substring(2, tFieldName.Length - 2) + ",");
                            if (oProperty.GetValue(poData) == null)
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlInsValue.AppendLine(oDefValueAttr.Value + ",");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlInsValue.AppendLine(oProperty.GetValue(poData) + ",");
                            break;
                        case "PC":
                            // Update.
                            if (oProperty.GetValue(poData) != null)
                            {
                                // Property value.
                                oSqlUpdValue.AppendLine("FC" + tFieldName.Substring(2, tFieldName.Length - 2) + " = " + oProperty.GetValue(poData) + ",");
                            }

                            // Insert.
                            oSqlInsField.AppendLine(oProperty.Name.ToString().Replace("pc", "FC") + ",");
                            if (oProperty.GetValue(poData) == null)
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlInsValue.AppendLine(oDefValueAttr.Value + ",");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlInsValue.AppendLine(oProperty.GetValue(poData) + ",");
                            break;
                        case "PD":
                            if (string.IsNullOrEmpty(oProperty.Description))
                            {
                                //tFmtDate = "yyyy-MM-dd";
                                tFmtDate = "yyyy-MM-dd HH:mm:ss";   //*Em 61-10-01 ปรับฟิดล์ update
                            }
                            else
                            {
                                tFmtDate = oProperty.Description;
                            }

                            // Update.
                            if (oProperty.GetValue(poData) != null)
                            {
                                oSqlUpdValue.AppendLine("FD" + tFieldName.Substring(2, tFieldName.Length - 2) + " = '");
                                oSqlUpdValue.AppendLine(Convert.ToDateTime(oProperty.GetValue(poData)).ToString(tFmtDate) + "',");
                            }

                            // Insert.
                            oSqlInsField.AppendLine("FD" + tFieldName.Substring(2, tFieldName.Length - 2) + ",");
                            if (oProperty.GetValue(poData) == null)
                            {
                                if (oDefValueAttr != null)
                                {
                                    // Default value.
                                    oSqlInsValue.AppendLine("'" + Convert.ToDateTime(oDefValueAttr.Value).ToString(tFmtDate) + "',");
                                    continue;
                                }
                            }

                            // Property value.
                            oSqlInsValue.AppendLine("'" + Convert.ToDateTime(oProperty.GetValue(poData)).ToString(tFmtDate) + "',");
                            break;
                    }
                }

                if (pbStaWhoUpd)
                {
                    tSqlCmdIns = string.Format(oSqlIns.ToString(), oSqlInsField.ToString(), oSqlInsValue.ToString());
                    tSqlCmdUpd = string.Format(oSqlUpd.ToString(), oSqlUpdValue.ToString(), tSqlCmdIns);
                }
                else
                {
                    tSqlCmdIns = string.Format(oSqlIns.ToString(),
                        oSqlInsField.ToString().Remove(oSqlInsField.Length - 3), oSqlInsValue.ToString().Remove(oSqlInsValue.Length - 3));
                    tSqlCmdUpd = string.Format(oSqlUpd.ToString(), oSqlUpdValue.ToString().Remove(oSqlUpdValue.Length - 3), tSqlCmdIns);
                }

                return tSqlCmdUpd;
            }
            catch (Exception)
            {

            }
            finally
            {
                oSqlUpd = null;
                oSqlUpdValue = null;
                oSqlIns = null;
                oSqlInsField = null;
                oSqlInsValue = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return "";
        }
        /// <summary>
        /// Check ค่าของ Model ที่รับมา เพื่อนำไป Update Table
        /// </summary>
        /// <param name="ptModel">ค่าสำหรับ Update</param>
        /// <param name="ptField">ฟิลด์ที่ต้องการให้ Update</param>
        /// <returns></returns>
        public string SP_SETtValueUpt(string ptModel, string ptField)
        {
            string result = "";


            if (ptModel != "")
            {
                result = ptField + "=" + "'" + ptModel + "'" + ",";
            }

            return result;
        }
        /// <summary>
        /// Insert to ImageObject
        /// </summary>
        /// <param name="ptCode"> รหัสระบุประเภทของภาพ </param>
        /// <param name="ptFolderName"> ชื่อโฟลเดอร์ตามประเภท</param>
        /// <param name="ptImgObj"> Base 64 from API</param>
        /// <param name="ptRefID"> Refcode</param>
        /// <param name="ptImgKey">หมวดหมู่ของภาพ</param>
        /// <param name="ptWhoUpt"> Who Update</param>
        /// <param name="ptTable">Table ที่ใช้งาน</param>
        /// <returns></returns>
        public bool SP_IMGbInsImgPath(string ptCode, string ptFolderName, string ptImgObj, string ptRefID, string ptImgKey, string ptWhoUpt, string ptTable)
        {
            StringBuilder oSql;
            StringBuilder oSql_Img;
            cDatabase oDatabase;
            DataTable oDbTblImg;

            try
            {
                using (AdaAccEntities oAdaAcc = new AdaAccEntities())
                {

                    using (DbConnection oConn = oAdaAcc.Database.Connection)
                    {
                        oConn.Open();
                        DbCommand oCmd = oConn.CreateCommand();
                        DbTransaction oTrans;
                        oTrans = oConn.BeginTransaction();
                        oCmd.Connection = oConn;
                        oCmd.Transaction = oTrans;

                        try
                        {

                            oSql_Img = new StringBuilder();
                            oDatabase = new cDatabase();

                            oSql = new StringBuilder();
                            oSql.AppendLine(" SELECT FNImgID FROM TCNMImgObj");
                            oSql.AppendLine(" WHERE FTImgRefID = '" + ptCode + "'");
                            oDbTblImg = oDatabase.C_DAToSqlQuery(oSql.ToString());

                            int nSeq = oDbTblImg.Rows.Count + 1;

                            string tpath = "D:/Img64/" + ptFolderName + ptCode + "/";
                            //check have folder... 1 folder | 1 company

                            if (!(System.IO.Directory.Exists(tpath)))
                            {
                                System.IO.Directory.CreateDirectory(tpath);
                            }

                            string tImgName = ptCode + "_" + nSeq;
                            SP_IMGbSaveStr2Img(tImgName, tpath, ptImgObj, tpath + tImgName);

                            //Insert ImgObj
                            oSql_Img.Clear();
                            oSql_Img.AppendLine(" INSERT INTO TCNMImgObj");
                            oSql_Img.AppendLine(" (FTImgRefID, FNImgSeq, FTImgTable, ");
                            //oSql_Img.AppendLine(" FTImgKey, FTImgObj, FDDateUpd, FTTimeUpd, FTWhoUpd)");
                            oSql_Img.AppendLine(" FTImgKey, FTImgObj, FDLastUpdOn, FDCreateOn, FTLastUpdBy, FTCreateBy)");  //*Em 61-10-01 ปรับฟิดล์ update
                            oSql_Img.AppendLine(" VALUES ");
                            oSql_Img.AppendLine(" (");
                            oSql_Img.AppendLine(" '" + ptRefID + "',");
                            oSql_Img.AppendLine(" '" + nSeq + "',");
                            oSql_Img.AppendLine(" '" + ptTable + "',");
                            oSql_Img.AppendLine(" '" + ptImgKey + "',");
                            oSql_Img.AppendLine(" '" + SP_IMGtGetPath(ptFolderName + ptCode + "/", tImgName, tpath) + "',");
                            //oSql_Img.AppendLine(" CONVERT(VARCHAR(10), GETDATE(), 121),");
                            //oSql_Img.AppendLine(" CONVERT(VARCHAR(8),GETDATE(),108),");
                            //oSql_Img.AppendLine(" '" + ptWhoUpt + "'");
                            oSql_Img.AppendLine(" GETDATE(), GETDATE(),");  //*Em 61-10-01 ปรับฟิดล์ update
                            oSql_Img.AppendLine(" '" + ptWhoUpt + "', '" + ptWhoUpt + "'"); //*Em 61-10-01 ปรับฟิดล์ update
                            oSql_Img.AppendLine(")");

                            oCmd.CommandTimeout = 10;
                            oCmd.CommandType = CommandType.Text;
                            oCmd.CommandText = oSql_Img.ToString();
                            oCmd.ExecuteNonQuery();

                            oTrans.Commit();
                        }
                        catch (SqlException oSqlExn)
                        {
                            oTrans.Rollback();
                            switch (oSqlExn.Number)
                            {
                                case 2627:
                                    // Data is duplicate.
                                    return false;

                                default:
                                    //Error statement or sql error
                                    return false;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            oTrans.Rollback();
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database.. 
                                    return false;
                            }
                        }
                    }
                }
            }
            catch (Exception oExn)
            {
                return false;
            }
            finally
            {
                oSql = null;
                oSql_Img = null;
                oDbTblImg = null;
            }
            return true;
        }
        /// <summary>
        /// Get Path Image
        /// </summary>
        /// <param name="ptObjImg"> Folder Image</param>
        /// <param name="ptImgName">Image Name</param>
        /// <param name="ptPath"> Path </param>
        /// <returns></returns>
        public string SP_IMGtGetPath(string ptObjImg, string ptImgName, string ptPath)
        {
            string tImgSave = "";
            DirectoryInfo tPath = new DirectoryInfo(ptPath);
            FileInfo[] oGetFile = tPath.GetFiles();

            foreach (FileInfo oFile in oGetFile)
            {
                string tImg = ptImgName + ".jpg";

                var oImg = from Img in oGetFile
                           where Img.Name == tImg
                           select Img;

                if (oImg.FirstOrDefault() != null)
                {
                    tImgSave = ptObjImg + oImg.FirstOrDefault();
                }
            }

            return tImgSave;
        }

        /// <summary>
        /// Convert Path Image to Base64
        /// </summary>
        /// <param name="ptPathPic"></param>
        /// <returns></returns>
        public string SP_PRCtConvertImage2Base64(string ptPathPic)
        {
            try
            {
                string tPath;
                tPath = ptPathPic;
                byte[] oImageByte;

                using (System.Drawing.Image oImage = System.Drawing.Image.FromFile(tPath))
                {
                    using (MemoryStream oMem = new MemoryStream())
                    {
                        oImage.Save(oMem, oImage.RawFormat);
                        oImageByte = oMem.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(oImageByte);
                        return base64String;
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptValue"></param>
        /// <param name="pnKeysize"></param>
        /// <param name="pnBlocksize"></param>
        /// <param name="poCipher"></param>
        /// <param name="poPadding"></param>
        /// <returns></returns>
        public string C_CALtEncrypt(string ptValue, int pnKeysize = 128, int pnBlocksize = 128,
                                           CipherMode poCipher = CipherMode.CBC, PaddingMode poPadding = PaddingMode.PKCS7)
        {
            byte[] aEncKey = Encoding.UTF8.GetBytes("5YpPTypXtwMML$u@");
            byte[] aEncIV = Encoding.UTF8.GetBytes("zNhQ$D%arP6U8waL");

            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.BlockSize = pnBlocksize;
            oAes.KeySize = pnKeysize;
            oAes.Mode = poCipher;
            oAes.Padding = poPadding;

            byte[] oSrc = Encoding.UTF8.GetBytes(ptValue);

            using (ICryptoTransform oEncrypt = oAes.CreateEncryptor(aEncKey, aEncIV))
            {
                byte[] oDest = oEncrypt.TransformFinalBlock(oSrc, 0, oSrc.Length);
                oEncrypt.Dispose();

                return Convert.ToBase64String(oDest);
            }
        }

        /// <summary>
        /// Get SQL Cmd Timeout from WebConfig
        /// </summary>
        /// <returns></returns>
        public int SP_GETnSQLCmdTimeout()
        {
            try
            {
                //if (ConfigurationManager.AppSettings.GetValues("nCmdTime") != null)
                //{
                //    return Convert.ToInt32(ConfigurationManager.AppSettings["nCmdTime"].ToString());
                //}

                //*Ton 64-05-21
                if (cAppSetting.Default.nCmdTime != null)
                {
                    return Convert.ToInt32(cAppSetting.Default.nCmdTime);
                }
            }
            catch { }
            return 120;
        }
    }
}