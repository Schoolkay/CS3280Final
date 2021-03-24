using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;

namespace GroupProject
{
    /// <summary>
    /// Used to access the database.
    /// </summary>
    public class clsDataAccess
    {
        #region constructors

        /// <summary>
        /// Default constructor that sets the default connection string.
        /// </summary>
		public clsDataAccess()
        {
            try
            {
                sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data source= " + Directory.GetCurrentDirectory() + "\\Invoice.mdb";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region variables

        /// <summary>
        /// Connection string for the database.
        /// </summary>
        private string sConnectionString;

        #endregion

        #region functions

        /// <summary>
        /// Executes a sql statement and passes back the data set.
        /// </summary>
        /// <param name="sSQLStatement">The SQL statement string</param>
        /// <param name="iRowsReturned">int to return the number of selected rows to.</param>
        /// <returns>DataSet: Contains all data returned from the sql statement.</returns>
        public DataSet ExecuteSQLStatement(string sSQLStatement, ref int iRowsReturned)
        {
            try
            {
                //Create a new DataSet.
                DataSet dataset = new DataSet();

                using (OleDbConnection connection = new OleDbConnection(sConnectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {

                        //Open the connection to the database.
                        connection.Open();

                        //Execute SQL.
                        adapter.SelectCommand = new OleDbCommand(sSQLStatement, connection);
                        adapter.SelectCommand.CommandTimeout = 0;

                        //Fill up the DataSet with data.
                        adapter.Fill(dataset);
                    }
                }

                //Set the number of values returned.
                iRowsReturned = dataset.Tables[0].Rows.Count;

                //Return the DataSet.
                return dataset;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Executes a SQL statment and returns one value.
        /// </summary>
        /// <param name="sSQLStatement">The SQL statement string.</param>
        /// <returns>string: The result of the SQL statement.</returns>
		public string ExecuteScalarSQL(string sSQLStatement)
        {
            try
            {
                //Holds the return value.
                object returnScalar;

                using (OleDbConnection connection = new OleDbConnection(sConnectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {

                        //Open the connection to the database.
                        connection.Open();

                        //Execute SQL.
                        adapter.SelectCommand = new OleDbCommand(sSQLStatement, connection);
                        adapter.SelectCommand.CommandTimeout = 0;

                        //Execute the scalar SQL statement.
                        returnScalar = adapter.SelectCommand.ExecuteScalar();
                    }
                }

                //See if the object is null.
                if (returnScalar == null)
                {
                    //Return a blank.
                    return "";
                }
                else
                {
                    //Return the value.
                    return returnScalar.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Executes a non-query SQL statment.
        /// </summary>
        /// <param name="sSQLStatement">The SQL statement string.</param>
        /// <returns>int: Number of rows affected.</returns>
        public int ExecuteNonQuery(string sSQLStatement)
        {
            try
            {
                //Number of rows affected.
                int iNumRows;

                using (OleDbConnection connection = new OleDbConnection(sConnectionString))
                {
                    //Open the connection to the database.
                    connection.Open();

                    //Prepare to execute SQL.
                    OleDbCommand command = new OleDbCommand(sSQLStatement, connection);
                    command.CommandTimeout = 0;

                    //Execute SQL.
                    iNumRows = command.ExecuteNonQuery();
                }

                //Return the number of rows affected.
                return iNumRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion
    }
}
