using AlphaApp.Repositories.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlphaApp.Repositories.Helpers
{
   
    public class SQLHelper
    {
        #region Member Variables
        public SqlConnection _objConnection;
        public SqlCommand _objCommand;
        public SqlTransaction _sqlTrans;
        public string _connectionString;
        #endregion

        #region Constructor
        /// <summary>
        /// This is a constructor it takes the connection string from web.config 
        /// and create instantiation of sqlcommand.
        /// </summary>
        public SQLHelper(AlphaAppContext _context)
        {

            _connectionString = _context.Database.GetConnectionString();
            //config.GetConnectionString("AlphaAppDB");
            _objConnection = new SqlConnection(_connectionString);
            _objCommand = new SqlCommand();
            _objCommand.CommandTimeout = 40000;
        }

     
        #endregion Constructor

        #region Private Methods
        /// <summary>
        /// intializing the connection and opening the connection
        /// </summary>
        private void Open()
        {
            try
            {
                _objConnection = new SqlConnection(_connectionString);
                if (_objConnection.State != ConnectionState.Open)
                    _objConnection.Open();
                if (_objCommand == null)
                    _objCommand = new SqlCommand();
                _objCommand.Connection = _objConnection;
            }
            catch (Exception ex)
            {
                Close();
                throw new ApplicationException("Error occured while Calling DataAccess:SQLHelper:Open()" + ex.Message, ex);
            }
        }
        /// <summary>
        /// Close the connection 
        /// </summary>
        private void Close()
        {
            try
            {
                _objCommand.Dispose();
                if (_sqlTrans == null)
                {
                    if (_objConnection.State != ConnectionState.Closed)
                    {
                        _objConnection.Close();
                        _objConnection.Dispose();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while Calling DataAccess:SQLHelper:Close()" + ex.Message, ex);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public string StoredProcedureName
        {
            get
            {
                if (_objCommand != null)
                    return _objCommand.CommandText;
                else
                    return null;
            }
            set
            {
                if (_objCommand == null)
                {
                    _objCommand = new SqlCommand();
                }
                _objCommand.Parameters.Clear();
                _objCommand.CommandText = value;
                _objCommand.CommandType = CommandType.StoredProcedure;
            }
        }

        /// <summary>
        /// Retrieve a multi-table set of data from the database.
        /// UntypedDataset filling
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ExecDataSet()
        {
            DataSet dataSet = new DataSet();
            try
            {
#if DEBUG
                var script = GetProcedureScript(_objCommand);
#endif
                Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(_objCommand);
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecDataSet()" + ex.Message, ex);
            }
            finally
            {
                Close();
            }
        }

        public DataTable ExecDataTable(string objsqlConnectionsString, SqlCommand objSqlCommand)
        {
#if DEBUG
            var script = GetProcedureScript(_objCommand);
#endif

            DataTable dataTable = new DataTable();
            try
            {

                using (SqlConnection objsqlConnection = new SqlConnection(objsqlConnectionsString))
                {
                    objsqlConnection.Open();
                    objSqlCommand.Connection = objsqlConnection;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(objSqlCommand);
                    dataAdapter.Fill(dataTable);
                    return dataTable;

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecDataTable(string objsqlConnectionsString, SqlCommand objSqlCommand)" + ex.ToString(), ex);
            }
            finally
            {
                Close();
            }
        }

        public bool ExecuteNonQuery(string objsqlConnectionsString, SqlCommand objSqlCommand)
        {


#if DEBUG
            var script = GetProcedureScript(_objCommand);
#endif

            bool Rtrn = false;

            try
            {
                Open();
                var res = objSqlCommand.ExecuteNonQuery();

                Rtrn = true;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecuteNonQuery()" + ex.Message, ex);

                //MessageBox.Show(ex.Message) 
                Rtrn = false;

            }
            finally
            {
                Close();
            }
            return Rtrn;
        }


        private string GetProcedureScript(SqlCommand cmd)
        {
            string strQry = cmd.CommandText + "\n\r";
            foreach (SqlParameter param in cmd.Parameters)
            {
                if (param.DbType == DbType.String || param.DbType == DbType.DateTime || param.DbType == DbType.AnsiString || param.DbType == DbType.AnsiStringFixedLength)
                    strQry += string.Format("{0} = '{1}',-- {2} Type  \n\r", param, param.Value, param.DbType);
                else if (param.DbType == DbType.Boolean)
                    strQry += string.Format("{0} = {1},-- {2} Type  \n\r", param, ((bool)param.Value) ? 1 : 0, param.DbType);
                else
                    strQry += string.Format("{0} = {1},-- {2} Type  \n\r", param, param.Value, param.DbType);


            }
            return strQry.Trim();
        }
        public DataSet ExecDataSet(string objsqlConnectionsString, SqlCommand objSqlCommand)
        {
            DataSet dataSet = new DataSet();
            try
            {
#if DEBUG
                var script = GetProcedureScript(objSqlCommand);
#endif

                using (SqlConnection objsqlConnection = new SqlConnection(objsqlConnectionsString))
                {
                    objsqlConnection.Open();
                    objSqlCommand.Connection = objsqlConnection;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(objSqlCommand);
                    dataAdapter.Fill(dataSet);
                    return dataSet;

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecDataTable(string objsqlConnectionsString, SqlCommand objSqlCommand)" + ex.ToString(), ex);
            }
            finally
            {
                Close();
            }
        }

     
     
        /// <summary>
        /// Typed Dataset filling
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="mapping"></param>
        /// <returns>example:  sqlHelper.ExecDataSet<PhaseDS>(ds);</returns>
        public void ExecDataSet<T>(T dataSet)
             where T : DataSet
        {

            try
            {
                Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(_objCommand);
                dataAdapter.Fill(dataSet);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecDataSet<T>(T dataSet)" + ex.Message, ex);
            }
            finally
            {
                if (_sqlTrans != null)
                    Close();
            }
        }

        /// <summary>
        /// Retrieve a single table of data from the database.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ExecDataTable()
        {
            DataTable dataTable = new DataTable();
            try
            {
#if DEBUG
                var script = GetProcedureScript(_objCommand);
#endif
                Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(_objCommand);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecDataTable()" + ex.Message, ex);
            }
            finally
            {
                Close();
            }
        }
        /// <summary>
        /// Get or Set the internal SQL Parameters collection.
        /// The following example adds two parameter items to provide input
        /// values for a stored procedure used to update a user record.
        /// C#:
        /// SQLHelper obj = new SQLHelper();
        ///obj.StoredProcedureName = "dt_dept";
        ///SqlParameter param;
        ///param = new SqlParameter("@dept", SqlDbType.Int);
        ///param.Direction = ParameterDirection.Input;
        ///param.Value = deptno;
        ///obj.parameters.Add(param);
        ///param = new SqlParameter("@deptName", SqlDbType.VarChar);
        ///param.Direction = ParameterDirection.Input;
        ///param.Value = Name;
        ///obj.parameters.Add(param);
        ///param = new SqlParameter("@deptloc", SqlDbType.VarChar);
        ///param.Direction = ParameterDirection.Input;
        ///param.Value = Location;	
        ///obj.parameters.Add(param);
        /// </summary>
        public SqlParameterCollection Parameters
        {
            get
            {
                return _objCommand.Parameters;
            }
        }
        /// <summary>
        /// Perform DML operations on the DataBase based on Supplied Stored procedure.
        /// </summary>
        /// <returns></returns>
        public int ExecDML()
        {
            int intCount;
            try
            {
                Open();
                intCount = _objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                intCount = 0;
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecDML()" + ex.Message, ex);
            }
            finally
            {
                Close();
            }
            return intCount;
        }
        /// <summary>
        /// Opens a connection for DataReader and returns retrieved DataReader object 
        /// from DataBase.
        /// Explicitly close() shold be called after closing the DataReader object.
        /// </summary>
        /// <returns></returns>
        public SqlDataReader ExecReader()
        {
            SqlDataReader dataReader = null;
            try
            {
                Open();
                dataReader = _objCommand.ExecuteReader();
                return dataReader;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecReader()" + ex.Message, ex);
            }
            finally
            {
                Close();
            }
        }

        public string getSingleValue()
        {
            SqlDataReader dataReader = null;
            try
            {
                Open();
                dataReader = _objCommand.ExecuteReader();
                string Value = "";
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Value = Convert.ToString(dataReader[0]);
                }
                dataReader.Close();
                return Value;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecReader()" + ex.Message, ex);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Retrieves a scalar (string, double, etc.) from the database.
        /// </summary>
        /// <returns></returns>
        public object ExecScalar()
        {
            try
            {
#if DEBUG
                var script = GetProcedureScript(_objCommand);
#endif
                Open();
                return _objCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while calling DataAccess:SQLHelper:ExecScalar()" + ex.Message, ex);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Use this method prior to executing a series of database operations that
        /// must be treated as a single unit of changes
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                Open();
                _sqlTrans = this._objConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                _objCommand.Transaction = _sqlTrans;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while Begining the transaction:SQLHelper:BeginTransaction()" + ex.Message, ex);
            }
        }
        /// <summary>
        /// Rolls back the currently open transaction if
        /// one exists.
        /// </summary>
        public void RollBack()
        {
            try
            {
                if (_sqlTrans != null)
                {
                    _sqlTrans.Rollback();
                    _sqlTrans = null;
                }
                Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while Rollback:SQLHelper:RollBack()" + ex.Message, ex);
            }
        }
        /// <summary>
        /// Commits the currently open transaction if one exists.  Also closes
        /// the connection, if it is open, for re-use in the connection pool.
        /// </summary>
        public void Commit()
        {
            try
            {
                if (_sqlTrans != null)
                {
                    _sqlTrans.Commit();
                    _sqlTrans = null;
                }
                Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while commiting the Transaction:SQLHelper:Commit()" + ex.Message, ex);
            }

        }


        #endregion
    }

}
