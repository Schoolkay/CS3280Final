//using java.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    /// <summary>
    /// Class that contains all the SQL statements for the Items Window
    /// </summary>
    class clsItemsSQL
    {
        /// <summary>
        /// Public constructor for the clsItemsSQL class
        /// </summary>
        public clsItemsSQL()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will load all the items from the Database
        /// </summary>
        /// <returns></returns>
        public string PopulateAllItems()
        {
            try
            {
                // Returns all columns and rows from ItemDesc
                return "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc ORDER BY ItemCode";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will add an item to the Database
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="ItemDesc"></param>
        /// <param name="Cost"></param>
        /// <returns></returns>
        public string AddOneItem(string ItemCode, string ItemDesc, string Cost)
        {
            try
            {
                // This will pass in the item to be inserted into the database
                return $"INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) VALUES ('{ItemCode}', '{ItemDesc}', {Int32.Parse(Cost)})";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will edit one item from the Database
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string EditOneItem(string ItemCode, string ItemDesc, string Cost)
        {
            try
            {
                // This will pass in the item to be inserted into the databaseS
                return $"UPDATE ItemDesc SET ItemDesc = '{ItemDesc}', Cost = {Int32.Parse(Cost)} WHERE ItemCode = '{ItemCode}'";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will delete one item from the Database
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public string DeleteOneItem(string ItemCode)
        {
            try
            {
                // This will delete the row with the passed in ItemCode
                return $"DELETE FROM ItemDesc WHERE ItemCode = '{ItemCode}'";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will get all invoices tied with an item from the Database
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public string GetInvoices(string ItemCode)
        {
            try
            {
                return $"SELECT DISTINCT InvoiceNum FROM LineItems WHERE ItemCode = '{ItemCode}'";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will get the new invoice total
        /// </summary>
        /// <param name="InvoiceNumber"></param>
        /// <returns></returns>
        public string GetNewInvoiceTotal(string InvoiceNumber)
        {
            try
            {
                return "SELECT Sum(ic.Cost) AS [TotalCost] " +
                    "FROM LineItems AS ln INNER JOIN ItemDesc AS ic " +
                    "ON ln.ItemCode = ic.ItemCode " +
                    "WHERE ln.InvoiceNum = " + InvoiceNumber;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This method will update an invoice total
        /// </summary>
        /// <param name="sInvoiceNumber">The number of the invoice to update.</param>
        /// <param name="sInvoiceTotal">The new total.</param>
        /// <returns></returns>
        public string UpdateTotal(string InvoiceNumber, string InvoiceTotal)
        {
            try
            {
                return "UPDATE Invoices " +
                    "SET TotalCost = " + InvoiceTotal + " " +
                    "WHERE InvoiceNum = " + InvoiceNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
