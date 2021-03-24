using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GroupProject
{
    /// <summary>
    /// Class that contains the business logic behind the Search Window UI
    /// </summary>
    class clsSearchLogic
    {
        /// <summary>
        /// Class db of clsDataAccess allows connection to the invoices database
        /// </summary>
        clsDataAccess db;
        /// <summary>
        /// Instance of clsSearchSql as clsSQL to handle requests from the database
        /// </summary>
        clsSearchSQL clsSQL;

        /// <summary>
        /// public constructor of clsSearchLogic
        /// </summary>
        public clsSearchLogic()
        {
            try
            {
                //Initalize instance of SearchSQL to procide the sel querries
                clsSQL = new clsSearchSQL();
                //Initalize instance of data access to interact with invoices database as db
                db = new clsDataAccess();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Method PopulateGrid return all the invoices from the database into a list
        /// </summary>
        /// <returns> List<clsInvoice> </returns>
        public List<clsInvoice> PopulateGrid()
        {
            //Lower level exception handling
            try
            {
                //Create dataset
                DataSet dsInvoices;
                //Track rows returned
                int iRowsReturned = 0;
                //Create list to hold the clsInvoices and be returned
                List<clsInvoice> lSearchResults = new List<clsInvoice>();

                //Pull SQL statement from SQL class
                string sSQL = clsSQL.PopulateInvoices();

                //store SQL results into dataset
                dsInvoices = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                //Add table name
                dsInvoices.Tables[0].TableName = "Invoices";

                //For each data row transpose that rows information into a clsInvoice 
                //to be added to the list to be returned
                foreach (DataRow dr in dsInvoices.Tables[0].Rows)
                {
                    lSearchResults.Add(new clsInvoice(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));
                }

                return lSearchResults;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Method ReturnNumbers returns the invoice numbers in a list
        /// </summary>
        /// <returns> List<clsInvoice> </returns>
        public List<clsInvoice> ReturnNumbers()
        {
            //Lower level exception handling
            try
            {
                //Create dataset
                DataSet dsInvoices;
                //Track rows returned
                int iRowsReturned = 0;
                //Create list to hold the clsInvoices and be returned
                List<clsInvoice> lNumbers = new List<clsInvoice>();

                //Pull SQL statement from SQL class
                string sSQL = clsSQL.PopulateNumbers();

                //store SQL results into dataset
                dsInvoices = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                //Add table name
                dsInvoices.Tables[0].TableName = "InvoiceNum";

                //For each data row transpose that rows information into a clsInvoice 
                //to be added to the list to be returned
                foreach (DataRow dr in dsInvoices.Tables[0].Rows)
                {
                    lNumbers.Add(new clsInvoice(dr[0].ToString()));
                }

                return lNumbers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// method ReturnDates returns the invoice dates in a list
        /// </summary>
        /// <returns> List<clsInvoice> </returns>
        public List<clsInvoice> ReturnDates()
        {
            //Lower level exception handling
            try
            {
                //Create dataset
                DataSet dsInvoices;
                //Track rows returned
                int iRowsReturned = 0;
                //Create list to hold the clsInvoices and be returned
                List<clsInvoice> lDates = new List<clsInvoice>();

                //Pull SQL statement from SQL class
                string sSQL = clsSQL.PopulateDates();

                //store SQL results into dataset
                dsInvoices = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                //Add table name
                dsInvoices.Tables[0].TableName = "InvoiceDate";

                //For each data row transpose that rows information into a clsInvoice 
                //to be added to the list to be returned
                foreach (DataRow dr in dsInvoices.Tables[0].Rows)
                {
                    lDates.Add(new clsInvoice(dr[0].ToString()));
                }

                return lDates;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Method ReturnCharges returned the total cost of invoices in a list
        /// </summary>
        /// <returns> List<clsInvoice> </returns>
        public List<clsInvoice> ReturnCharges()
        {
            //Lower level exception handling
            try
            {
                //Create dataset
                DataSet dsInvoices;
                //Track rows returned
                int iRowsReturned = 0;
                //Create list to hold the clsInvoices and be returned
                List<clsInvoice> lCharges = new List<clsInvoice>();

                //Pull SQL statement from SQL class
                string sSQL = clsSQL.PopulateCharges();

                //store SQL results into dataset
                dsInvoices = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                //Add table name
                dsInvoices.Tables[0].TableName = "InvoiceCharge";

                //For each data row transpose that rows information into a clsInvoice 
                //to be added to the list to be returned
                foreach (DataRow dr in dsInvoices.Tables[0].Rows)
                {
                    lCharges.Add(new clsInvoice(dr[0].ToString()));
                }

                return lCharges;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Method ReturnSearch seperates the string array into strings to pass into 
        /// the sql search method and take the returned dataset and convert into a
        /// list to pass back to the search window
        /// </summary>
        /// <param name="saSearchParameters"></param>
        /// <returns> List<clsinvoice> </returns>
        public List<clsInvoice> ReturnSearch(string[] saSearchParameters)
        {
            //Lower level exception handling
            try
            {
                //Create dataset
                DataSet dsInvoices;
                //Track rows returned
                int iRowsReturned = 0;
                //Create list to hold the clsInvoices and be returned
                List<clsInvoice> lSearch = new List<clsInvoice>();

                //Sperate the array parameters into individual strings
                string sInvNumber = saSearchParameters[0];
                string sInvDate = saSearchParameters[1];
                string sInvCharge = saSearchParameters[2];

                //Pull SQL statement from SQL class
                string sSQL = clsSQL.SearchTable(sInvNumber, sInvDate, sInvCharge);

                //store SQL results into dataset
                dsInvoices = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                //Add table name
                dsInvoices.Tables[0].TableName = "FoundInvoices";

                //For each data row transpose that rows information into a clsInvoice 
                //to be added to the list to be returned
                foreach (DataRow dr in dsInvoices.Tables[0].Rows)
                {
                    lSearch.Add(new clsInvoice(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));
                }

                return lSearch;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the invoice number.
        /// </summary>
        /// <param name="clsInvoice"></param>
        /// <returns></returns>
        public string GetItemNumber(clsInvoice clsInvoice)
        {
            try
            {
                string sInvName = clsInvoice.InvoiceNumber;
                return sInvName;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
