using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    /// <summary>
    /// Holds all the SQL used by main logic.
    /// </summary>
    class clsMainSQL
    {
        /// <summary>
        /// Returns a SQL statement that gets the newest invoice number.
        /// </summary>
        public string GetNewestInvoiceNum { get { return "SELECT MAX(InvoiceNum) FROM Invoices"; } }

        /// <summary>
        /// Builds a SQL statement that grabs line items based on an invoice number.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice for the line items.</param>
        /// <returns></returns>
        public string GetItemsForInvoice(string sInvoiceNumber)
        {
            try
            {
                return "SELECT ln.LineItemNum, ln.ItemCode, ic.ItemDesc, ic.Cost "+
                    "FROM LineItems AS ln INNER JOIN ItemDesc AS ic "+
                    "ON ln.ItemCode = ic.ItemCode " +
                    "WHERE ln.InvoiceNum = " + sInvoiceNumber +
                    " ORDER BY ln.LineItemNum ASC";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that grabs items.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice for the line items.</param>
        /// <returns></returns>
        public string GetItems()
        {
            try
            {
                return "SELECT ItemCode, ItemDesc, Cost " +
                    "FROM ItemDesc " +
                    "ORDER BY ItemCode ASC";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that checks if an invoice exists.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice to check.</param>
        /// <returns></returns>
        public string GetInvoiceCheck(string sInvoiceNumber)
        {
            try
            {
                return "SELECT InvoiceNum " +
                    "FROM Invoices " +
                    "WHERE InvoiceNum = " + sInvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that gets an invoice date.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice to check.</param>
        /// <returns></returns>
        public string GetInvoiceDate(string sInvoiceNumber)
        {
            try
            {
                return "SELECT InvoiceDate " +
                    "FROM Invoices " +
                    "WHERE InvoiceNum = " + sInvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that gets an invoice total.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice to check.</param>
        /// <returns></returns>
        public string GetInvoiceTotal(string sInvoiceNumber)
        {
            try
            {
                return "SELECT Sum(ic.Cost) AS [TotalCost] " +
                    "FROM LineItems AS ln INNER JOIN ItemDesc AS ic " +
                    "ON ln.ItemCode = ic.ItemCode " +
                    "WHERE ln.InvoiceNum = " + sInvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that adds a new invoice to the database.
        /// </summary>
        /// <param name="sInvoiceDate">The date of the new invoice.</param>
        /// <param name="sInvoiceTotal">The total of the new invoice.</param>
        /// <returns></returns>
        public string AddInvoice(string sInvoiceDate, string sInvoiceTotal)
        {
            try
            {
                return "INSERT INTO Invoices " +
                    "(InvoiceDate, TotalCost) VALUES " +
                    "(#" + sInvoiceDate + "#, " + sInvoiceTotal + ")";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement updates an invoice total.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice to update.</param>
        /// <param name="sInvoiceTotal">The new total.</param>
        /// <returns></returns>
        public string UpdateTotal(string sInvoiceNumber, string sInvoiceTotal)
        {
            try
            {
                return "UPDATE Invoices " +
                    "SET TotalCost = " + sInvoiceTotal + " " +
                    "WHERE InvoiceNum = " + sInvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement updates an invoice date.
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice to update.</param>
        /// <param name="sInvoiceDate">The new date.</param>
        /// <returns></returns>
        public string UpdateDate(string sInvoiceNumber, string sInvoiceDate)
        {
            try
            {
                return "UPDATE Invoices " +
                    "SET InvoiceDate = #" + sInvoiceDate + "# " +
                    "WHERE InvoiceNum = " + sInvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that adds a new line item to the database.
        /// </summary>
        /// <param name="sInvoiceNumber">The invoice number.</param>
        /// <param name="sLineNumber">The line number.</param>
        /// <param name="sItemCode">The item code.</param>
        /// <returns></returns>
        public string AddItem(string sInvoiceNumber, string sLineNumber, string sItemCode)
        {
            try
            {
                return "INSERT INTO LineItems " +
                    "(InvoiceNum, LineItemNum, ItemCode) VALUES " +
                    "(" + sInvoiceNumber + ", " + sLineNumber + ", '" + sItemCode + "')";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that removes a line item from the database.
        /// </summary>
        /// <param name="sInvoiceNumber">The invoice number.</param>
        /// <param name="sLineNumber">The line number.</param>
        /// <returns></returns>
        public string RemoveItem(string sInvoiceNumber, string sLineNumber)
        {
            try
            {
                return "DELETE LineItems FROM LineItems " +
                    "WHERE InvoiceNum = " + sInvoiceNumber + " AND LineItemNum = " + sLineNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Builds a SQL statement that removes an invoice from the database.
        /// </summary>
        /// <param name="sInvoiceNumber">The invoice number.</param>
        /// <returns></returns>
        public string RemoveInvoice(string sInvoiceNumber)
        {
            try
            {
                return "DELETE Invoices FROM Invoices " +
                    "WHERE InvoiceNum = " + sInvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
