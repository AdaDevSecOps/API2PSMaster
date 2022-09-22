using API2PSMaster.Class.Standard;
using API2PSMaster.Models;
using API2PSMaster.Models.WebService.Request.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSMaster.Class.Product
{
    /// <summary>
    ///     Function for product.
    /// </summary>
    public class cProduct
    {
        /// <summary>
        ///     Generate statement for create/add item product price.
        /// </summary>
        /// <param name="poPara"></param>
        /// <param name="paoSysConfig"></param>
        /// <returns></returns>
        public string C_GENtStatementPdtPri(cmlReqPdtItemIns poPara,List<cmlTSysConfig> paoSysConfig)
        {
            cSP oFunc;
            StringBuilder oSql;
            cDatabase oDB;
            DataTable odtTmp;
            string tDocNo, tStaPriRet ="2", tStaPriWhs = "2", tStaPriNet ="2";
            bool bGenCode;
            try
            {
                oFunc = new cSP();
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FTPghDocNo ");
                oSql.AppendLine("FROM TCNTPdtPriHD with(nolock)");
                oSql.AppendLine("WHERE FTBchCode = '"+ poPara.ptBchCode +"'");
                oSql.AppendLine("AND FTPghDocType = '1'");
                oSql.AppendLine("AND FTPghStaAdj = '1'");
                oSql.AppendLine("AND FTPghStaDoc = '1'");
                oSql.AppendLine("AND FTPghStaPrcDoc = '1'");
                oSql.AppendLine("AND FNPghStaDocAct = 1");
                if (poPara.pcPgdNewPriRet != 0) { oSql.AppendLine("AND FTPghStaAlwRet = '1'"); }
                if (poPara.pcPgdNewPriWhs != 0) { oSql.AppendLine("AND FTPghStaAlwWhs = '1'"); }
                if (poPara.pcPgdNewPriNet != 0) { oSql.AppendLine("AND FTPghStaAlwNet = '1'"); }
                oSql.AppendLine("ORDER BY FTPghDocNo Desc");
                odtTmp = oDB.C_DAToSqlQuery(oSql.ToString());
                if (odtTmp.Rows.Count > 0)
                {
                    bGenCode = oFunc.SP_GENbAutoFmtCode("TCNTPdtPriHD", "FTPghDocNo", "0", poPara.ptBchCode,paoSysConfig, out tDocNo);
                    if (!bGenCode)
                    {
                        return "";
                    }
                    if (poPara.pcPgdNewPriRet != 0) { tStaPriRet = "1"; }
                    if (poPara.pcPgdNewPriWhs != 0) { tStaPriWhs = "1"; }
                    if (poPara.pcPgdNewPriNet != 0) { tStaPriNet = "1"; }
                    oSql = new StringBuilder();
                    oSql.AppendLine("INSERT INTO TCNTPdtPriHD with(rowlock)");
                    oSql.AppendLine("(");
                    oSql.AppendLine("FTBchCode, FTPghDocNo, FTPghDocType, FTPghStaAdj, FDPghDocDate, ");
                    oSql.AppendLine("FTPghDocTime, FTPghName, FTPplCode, FTAggCode, FDPghDStart, ");
                    oSql.AppendLine("FTPghTStart, FDPghDStop, FTPghTStop, FTPghStaAlwRet, FTPghStaAlwWhs, ");
                    oSql.AppendLine("FTPghStaAlwNet, FTPghStaDoc, FTPghStaPrcDoc, FNPghStaDocAct, FTUsrCode, ");
                    oSql.AppendLine("FTPghUsrApv, FTPghZneTo, FTPghBchTo, FTCphRmk, ");
                    oSql.AppendLine("FDDateUpd, FTTimeUpd, FTWhoUpd,");
                    oSql.AppendLine("FDDateIns, FTTimeIns, FTWhoIns");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES");
                    oSql.AppendLine("(");
                    oSql.AppendLine("'" + poPara.ptBchCode + "','" + tDocNo + "','1','1',CONVERT(VARCHAR(10),GETDATE(),121),");
                    oSql.AppendLine("CONVERT(VARCHAR(8),GETDATE(),108),'New Price','','',CONVERT(VARCHAR(10),GETDATE(),121),");
                    oSql.AppendLine("CONVERT(VARCHAR(8),GETDATE(),108),'9999-12-31','23:59:59','" + tStaPriRet + "','" + tStaPriWhs + "',");
                    oSql.AppendLine("'" + tStaPriNet + "','1','1',1,'System',");
                    oSql.AppendLine("'System','','" + poPara.ptBchCode + "','',");
                    oSql.AppendLine("CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'" + poPara.ptWhoUpd + "',");
                    oSql.AppendLine("CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'" + poPara.ptWhoUpd + "'");
                    oSql.AppendLine("(");
                    oSql.AppendLine("");
                    oSql.AppendLine("INSERT INTO TCNTPdtPriDT with(rowlock)");
                    oSql.AppendLine("(");
                    oSql.AppendLine("FTBchCode, FTPghDocNo, FNPgdSeq, FTPdtCode, FTPunCode,");
                    oSql.AppendLine("FCPgdNewPriRet, FCPgdNewPriWhs, FCPgdNewPriNet,");
                    oSql.AppendLine("FDDateUpd, FTTimeUpd, FTWhoUpd,");
                    oSql.AppendLine("FDDateIns, FTTimeIns, FTWhoIns");
                    oSql.AppendLine(")");
                    oSql.AppendLine("VALUES");
                    oSql.AppendLine("(");
                    oSql.AppendLine("'" + poPara.ptBchCode + "','" + tDocNo + "',1,'"+ poPara.poPdtPackSizeInf.ptPdtCode +"','"+ poPara.poPdtPackSizeInf.ptPunCode +"',");
                    oSql.AppendLine(poPara.pcPgdNewPriRet + "," + poPara.pcPgdNewPriWhs + "," + poPara.pcPgdNewPriNet + ",");
                    oSql.AppendLine("CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'" + poPara.ptWhoUpd + "',");
                    oSql.AppendLine("CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'" + poPara.ptWhoUpd + "'");
                    oSql.AppendLine(")");
                }
                else
                {
                    tDocNo = odtTmp.Rows[0][0].ToString();
                    oSql = new StringBuilder();
                    oSql.AppendLine("INSERT INTO TCNTPdtPriDT with(rowlock)");
                    oSql.AppendLine("(");
                    oSql.AppendLine("FTBchCode, FTPghDocNo, FNPgdSeq, FTPdtCode, FTPunCode,");
                    oSql.AppendLine("FCPgdNewPriRet, FCPgdNewPriWhs, FCPgdNewPriNet,");
                    oSql.AppendLine("FDDateUpd, FTTimeUpd, FTWhoUpd,");
                    oSql.AppendLine("FDDateIns, FTTimeIns, FTWhoIns");
                    oSql.AppendLine(")");
                    oSql.AppendLine("SELECT FTBchCode,FTPghDocNo,(MAX(FNPgdSeq) + 1) AS FNPgdSeq");
                    oSql.AppendLine("'" + poPara.poPdtPackSizeInf.ptPdtCode + "','" + poPara.poPdtPackSizeInf.ptPunCode + "',");
                    oSql.AppendLine(poPara.pcPgdNewPriRet + "," + poPara.pcPgdNewPriWhs + "," + poPara.pcPgdNewPriNet + ",");
                    oSql.AppendLine("CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'" + poPara.ptWhoUpd + "',");
                    oSql.AppendLine("CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'" + poPara.ptWhoUpd + "'");
                    oSql.AppendLine("FROM TCNTPdtPriDT with(nolock)");
                    oSql.AppendLine("WHERE FTBchCode = '" + poPara.ptBchCode + "' AND FTPghDocNo = '" + tDocNo + "'");
                    oSql.AppendLine("GROUP BY FTBchCode,FTPghDocNo");
                }

                return oSql.ToString();
            }
            catch
            {
                return "";
            }
            finally
            {
                oSql = null;
            }
        }
    }
}