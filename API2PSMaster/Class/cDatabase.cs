using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;   //*Em 62-06-08
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using System.Data.Entity.Core;

namespace API2PSMaster.Class
{
    /// <summary>
    /// 
    /// </summary>
    public class cDatabase:IDisposable 
    {
        //*Ton 64-05-21
        //private AdaAccEntities oC_AdaAcc;

        /// <summary>
        ///     Constructor
        /// </summary>
        //public cDatabase()
        //{
        //    EntityConnectionStringBuilder oEntityConnStr;
        //    SqlConnectionStringBuilder oSqlConnStr;
        //    string tConnStr;

        //    try
        //    {
        //        string tCon = "metadata=res://*/EF.AdaAcc.csdl|res://*/EF.AdaAcc.ssdl|res://*/EF.AdaAcc.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=202.44.55.96;initial catalog=SKC_Fullloop2;user id=sa;password=GvFhk@61;MultipleActiveResultSets=True;App=EntityFramework&quot;\" name = \"AdaAccEntities\" providerName = \"System.Data.EntityClient\"";
        //        tConnStr = tCon;
        //        //tConnStr = ConfigurationManager.ConnectionStrings["AdaAccEntities"].ConnectionString;
        //        oEntityConnStr = new EntityConnectionStringBuilder(tConnStr);
        //        oSqlConnStr = new SqlConnectionStringBuilder(oEntityConnStr.ProviderConnectionString);
        //        oSqlConnStr.ConnectTimeout = cCS.nCS_ConTme;
        //        oEntityConnStr.ProviderConnectionString = oSqlConnStr.ConnectionString;

        //        oC_AdaAcc = new AdaAccEntities(oEntityConnStr.ConnectionString);
        //        oC_AdaAcc.Database.Connection.Open();
        //    }
        //    catch (SqlException oSqlEct)
        //    {
        //        throw oSqlEct;
        //    }
        //    catch (EntityException oEtyExn)
        //    {
        //        throw oEtyExn;
        //    }
        //    catch (Exception oExn)
        //    {
        //        throw oExn;
        //    }
        //}

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="pnConTme">Connection time out.</param>
        public cDatabase(int pnConTme = cCS.nCS_ConTme, int pnCmdTme = cCS.nCS_CmdTme)
        {
            //EntityConnectionStringBuilder oEntityConnStr;
            //SqlConnectionStringBuilder oSqlConnStr;
            //string tConnStr;

            //try
            //{
            //    tConnStr = ConfigurationManager.ConnectionStrings["AdaAccEntities"].ConnectionString;
            //    oEntityConnStr = new EntityConnectionStringBuilder(tConnStr);
            //    oSqlConnStr = new SqlConnectionStringBuilder(oEntityConnStr.ProviderConnectionString);
            //    oSqlConnStr.ConnectTimeout = pnConTme;
            //    oEntityConnStr.ProviderConnectionString = oSqlConnStr.ConnectionString;

            //    oC_AdaAcc = new AdaAccEntities(oEntityConnStr.ConnectionString);
            //    oC_AdaAcc.Database.Connection.Open();
            //}
            //catch (SqlException oSqlEct)
            //{
            //    throw oSqlEct;
            //}
            //catch (EntityException oEtyExn)
            //{
            //    throw oEtyExn;
            //}
            //catch (Exception oExn)
            //{
            //    throw oExn;
            //}
        }

        /// <summary>
        /// Connect database
        /// </summary>
        public SqlConnection C_CONoDatabase()
        {
            SqlConnection oConn = null;
            string tConnString;

            try
            {
                //*Ton 64-05-21
                //oConn = new SqlConnection(new AdaAccEntities().Database.Connection.ConnectionString.ToString());
                oConn = new SqlConnection(cAppSetting.Default.tConnDB);
                oConn.Open();
            }
            catch (Exception oEx) { throw oEx; }

            return oConn;
        }

        /// <summary>
        ///     Execute sql command insert, update, delete etc.
        /// </summary>
        /// 
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Row effect of command.
        /// </returns>
        public int C_DATnExecuteSql(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            int nRowEff = 0;
            SqlConnection oConn;
            try
            {
                //oC_AdaAcc.Database.CommandTimeout = pnCmdTme;
                //nRowEff = oC_AdaAcc.Database.ExecuteSqlCommand(ptSqlCmd);

                //*Em 62-06-08
                oConn = C_CONoDatabase();
                nRowEff = oConn.Execute(ptSqlCmd, pnCmdTme);
                //+++++++++++++
            }
            catch (SqlException oSqlExn)
            {
                throw oSqlExn;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
            finally
            {
                oConn = null;
            }

            return nRowEff;
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Result of sql command in list of class model.
        /// </returns>
        public List<T> C_DATaSqlQuery<T>(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            SqlConnection oConn;
            try
            {
                List<T> aoResult = new List<T>();
                //oC_AdaAcc.Database.CommandTimeout = pnCmdTme;
                //aoResult = oC_AdaAcc.Database.SqlQuery<T>(ptSqlCmd).ToList();

                //*Em 62-06-08
                oConn = C_CONoDatabase();
                aoResult = oConn.Query<T>(ptSqlCmd,null,null,true,pnCmdTme).ToList();
                //+++++++++++++
                return aoResult;
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {
                oConn = null;
            }
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Result of sql command in class model.
        /// </returns>
        public T C_DAToSqlQuery<T>(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            SqlConnection oConn;
            try
            {
                T oResult = default(T);
                //oC_AdaAcc.Database.CommandTimeout = pnCmdTme;
                //oResult = oC_AdaAcc.Database.SqlQuery<T>(ptSqlCmd).FirstOrDefault();

                //*Em 62-06-08  
                oConn = C_CONoDatabase();
                oResult = oConn.Query<T>(ptSqlCmd, null, null, true, pnCmdTme).FirstOrDefault();
                //+++++++++++++

                //T oResult = (T)Activator.CreateInstance(typeof(T)); //*[ANUBIS][][2018-05-03] - ใช้ default(T) แทน เพราะใช้ได้ทั้ง string, int, model class.
                return oResult;
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {
                oConn =null;
            }
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// <param name="ptTblName">Table name.</param>
        /// 
        /// <returns>
        ///     Result of sql command in DataTable.
        /// </returns>
        public DataTable C_DAToSqlQuery(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme, string ptTblName = "TableTemp")
        {
            DataTable oDbTblResult = new DataTable();
            SqlConnection oConn;
            try
            {
                //DbProviderFactory oDbFactory = DbProviderFactories.GetFactory(oC_AdaAcc.Database.Connection);
                //using (DbCommand oDbCmd = oDbFactory.CreateCommand())
                //{
                //    oDbCmd.Connection = oC_AdaAcc.Database.Connection;
                //    oDbCmd.CommandType = CommandType.Text;
                //    oDbCmd.CommandText = ptSqlCmd;
                //    using (DbDataAdapter oDbAdp = oDbFactory.CreateDataAdapter())
                //    {
                //        oDbAdp.SelectCommand = oDbCmd;

                //        oDbTblResult = new DataTable();
                //        oDbTblResult.TableName = ptTblName;
                //        oDbAdp.Fill(oDbTblResult);
                //    }
                //}

                //*Em 62-06-08
                oConn = C_CONoDatabase();
                //oDbTblResult = (DataTable)oConn.Query(ptSqlCmd, null, null, true, pnCmdTme).Cast<DataTable>() ;
                IDataReader oDR = oConn.ExecuteReader(ptSqlCmd, pnCmdTme);
                oDbTblResult.Load(oDR);
                //++++++++++++++
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {
                oConn = null;
            }

            return oDbTblResult;
        }

        /// <summary>
        ///     Bulk copy table.
        /// </summary>
        /// 
        /// <param name="poDbTblData">Data.</param>
        /// <param name="pnBcpTme">Bulk copy time out.</param>
        /// 
        /// <returns>
        ///     true : Bulk copy success.<br/>
        ///     false : Bulk copy false.
        /// </returns>
        public bool C_DAToBulkCopyTable(DataTable poDbTblData, int pnBcpTme = cCS.nCS_BcpTme)
        {
            try
            {
                using (SqlBulkCopy oSqlBcp = new SqlBulkCopy(cAppSetting.Default.tConnDB))
                {
                    oSqlBcp.BulkCopyTimeout = pnBcpTme;
                    oSqlBcp.DestinationTableName = poDbTblData.TableName;
                    oSqlBcp.WriteToServer(poDbTblData);

                    oSqlBcp.Close();
                }

                return true;
            }
            catch (SqlException oSqlEct)
            {
                throw oSqlEct;
            }
            catch (Exception oExn)
            {
                throw oExn;
            }
            finally
            {

            }
        }

        /// <summary>
        ///  Interface IDisposable.
        /// </summary>
        public void Dispose()
        {
        }
        
        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="poConfigDb">Config database.</param>
        /// <param name="ptStoreName">Store procedure name.</param>
        /// <param name="paPara">ref Store procedure parameter.</param>
        /// <param name="poResult">ref Rusult.</param>
        /// 
        /// <returns>
        /// true: Execute success.
        /// false: Execute false.
        /// </returns>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-01-30] - add new function/method.
        /// </remarks>
        public bool C_DATbExecuteQueryStoreProcedure(string ptConStr, string ptStoreName, ref SqlParameter[] paPara, ref DataTable poResult)
        {

            SqlConnection oDbConn;
            try
            {
                oDbConn = new SqlConnection(ptConStr);
                using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                {
                    oDbCmd.CommandType = CommandType.StoredProcedure;
                    oDbCmd.CommandTimeout = 60;

                    if (paPara != null && paPara.Count() > 0)
                    {
                        oDbCmd.Parameters.AddRange(paPara);
                    }

                    using (SqlDataAdapter oDbAdp = new SqlDataAdapter())
                    {
                        oDbAdp.SelectCommand = oDbCmd;

                        poResult = new DataTable();
                        oDbAdp.Fill(poResult);

                    }

                    return true;
                }
            }
            catch (Exception oExp)
            {
                return false;

            }
            finally
            {
                oDbConn = null;
                
            }
        }

    }
}