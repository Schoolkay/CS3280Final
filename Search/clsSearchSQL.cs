using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    /// <summary>
    /// Class that contains all SQL statements used for the Search Window
    /// </summary>
    class clsSearchSQL
    {
        /// <summary>
        /// Class db of clsDataAccess allows connection to the invoices database
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// Constructor for the clsSearchSql class
        /// </summary>
        public clsSearchSQL()
        {
            try
            {
                //Initalize instance of data access to interact with invoices database as db
                db = new clsDataAccess();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Method PopulateInvoices runs the sql statment to return all invoices from the 
        /// invoice table in a dataset dsInvoices
        /// </summary>
        /// <returns> All data from the invoice table </returns>
        public string PopulateInvoices()
        {
            //Lower level exception handleing
            try
            {
                //SQL statement to return all rows from Invoices
                string sSQL = "SELECT * FROM Invoices";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Mehtod PopulateNumbers returns a dataset that contains all the invoice numbers
        /// from invoices from the database
        /// </summary>
        /// <returns> All invoice numbers from the invoice table </returns>
        public string PopulateNumbers()
        {
            //Lower level exception handleing
            try
            {
                //SQL statement to return Invoice Numbers from invoices
                string sSQL = "SELECT InvoiceNum FROM Invoices";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Mehtod PopulateDates returns a dataset that contains all the invoice dates
        /// from invoices from the database
        /// </summary>
        /// <returns> All invoice dates from the invoice table </returns>
        public string PopulateDates()
        {
            //Lower level exception handleing
            try
            {
                //SQL statement to return Invoice Dates from invoices
                string sSQL = "SELECT DISTINCT InvoiceDate FROM Invoices";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Mehtod PopulateCharges returns a dataset that contains all the total cost
        /// enteries from invoices from the database
        /// </summary>
        /// <returns> All invoice charges from the invoice table </returns>
        public string PopulateCharges()
        {
            //Lower level exception handleing
            try
            {
                //SQL statement to return total cost Numbers from invoices
                string sSQL = "SELECT DISTINCT TotalCost FROM Invoices";

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Method SearchTable uses the input of strings to construct the sql
        /// statement that will return the results of the invoices as specified
        /// by the parameters of the combo boxes by the user.
        /// </summary>
        /// <param name="sNumber">Invoice ID to pull the specicfic invoice by ID from table, if exist (tested in method)</param>
        /// <param name="sDate">Invoice Date to pull the specicfic invoice by from table, if exist (tested in method)</param>
        /// <param name="sTotalCost">Invoice Total Charge to pull the specicfic invoice by Total Charge from table, if exist (tested in method)</param>
        /// <returns> All invoices based on the search parameters passed in being the inovice number, date, and/or charge </returns>
        public string SearchTable(string sNumber, string sDate, string sTotalCost)
        {
            //Lower level exception handleing
            try
            {
                string sSQL = "";

                //If structure to determine which search parameters exist and returns the corresponding sql querry
                if (!(sNumber == null) && !(sDate == null) && !(sTotalCost == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sNumber + "AND InvoiceDate = " + "#" + sDate + "#" + "AND TotalCost = " + sTotalCost;
                }
                else if (!(sNumber == null) && !(sDate == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sNumber + "AND InvoiceDate = " + "#" + sDate + "#";
                }
                else if (!(sNumber == null) && !(sTotalCost == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sNumber + "AND TotalCost = " + sTotalCost;
                }
                else if (!(sDate == null) && !(sTotalCost == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE InvoiceDate = " + "#" + sDate + "#" + "AND TotalCost = " + sTotalCost;
                }
                else if (!(sNumber == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sNumber;
                }
                else if (!(sDate == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE InvoiceDate = " + "#" + sDate + "#";
                }
                else if (!(sTotalCost == null))
                {
                    sSQL = "SELECT * FROM Invoices WHERE TotalCost = " + sTotalCost;
                }
                else
                {
                    sSQL = "SELECT * FROM Invoices";
                }

                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
