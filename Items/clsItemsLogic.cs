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
    /// This will contain all business logic for the Items Window
    /// </summary>
    class clsItemsLogic
    {
        /// <summary>
        /// Class db of clsDataAccess allows connection to the items database
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// Instance of clsItemsSQL class to handle requests from the database
        /// </summary>
        clsItemsSQL sql;

        /// <summary>
        /// Creates a list that contains all of the items
        /// </summary>
        List<clsItem> ArbitraryList;


        /// <summary>
        /// Public constructor of clsItemsLogic
        /// </summary>
        public clsItemsLogic()
        {
            try
            {
                db = new clsDataAccess();
                sql = new clsItemsSQL();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Returns a list of items.
        /// </summary>
        /// <returns></returns>
        public List<clsItem> PopulateItems()
        {
            //Lower level exception handling
            try
            {
                //Create dataset
                DataSet dsItems;

                //Track the number of rows returned
                int iRowsReturned = 0;

                //Create a list that will contain the Items and be returned
                List<clsItem> lstAllItems = new List<clsItem>();

                // Pull SQL statement from the clsItemsSQL class
                string sSQL = sql.PopulateAllItems();

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);

                // For each data row, transpose that rows information into an Item
                // to be added to the list to be returned
                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    lstAllItems.Add(new clsItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));

                ArbitraryList = lstAllItems;

                return lstAllItems;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This will add an item to the itemList
        /// </summary>
        public List<clsItem> AddItem(string ItemCode, string ItemDesc, string Cost, wndItems window)
        {
            try
            {
                //Create dataset
                DataSet dsItems;

                //Track the number of rows returned
                int iRowsReturned = 0;

                //Create a list that will contain the Items and be returned
                List<clsItem> lstAllItems = PopulateItems();

                // Pull SQL statement from the clsItemsSQL class
                string sSQL = sql.PopulateAllItems();

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);

                // For each data row, transpose that rows information into an Item
                // to be added to the list to be returned
                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    lstAllItems.Add(new clsItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));

                // This will check to make sure no duplicate PK's of ItemCode are already in the database
                foreach (clsItem item in lstAllItems)
                {
                    if (item.sItemCode == ItemCode)
                    {
                        window.lblItemCodeError.Content = "Item Code already Exists.";
                        return lstAllItems;
                    }
                }

                if (!Int32.TryParse(Cost, out int parseTest))
                {
                    window.lblItemCodeError.Content = "Cost is needs to be a number.";
                    return lstAllItems;
                }

                // Add a new item to the database
                sSQL = sql.AddOneItem(ItemCode, ItemDesc, Cost);

                // Execute the sql Query and add a new item to the database
                db.ExecuteNonQuery(sSQL);

                // Add the new item to the local list
                lstAllItems.Add(new clsItem(ItemCode, ItemDesc, Cost));


                window.lblSuccess.Content = "Item Added Successfully.";

                return lstAllItems;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// When item is edited, update item.
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="ItemDesc"></param>
        /// <param name="Cost"></param>
        /// <returns></returns>
        public List<clsItem> EditItem(clsItem item, wndItems window, ref string message)
        {
            try
            {
                //Create dataset
                DataSet dsItems;

                //Track the number of rows returned
                int iRowsReturned = 0;

                //Create a list that will contain the Items and be returned
                List<clsItem> lstAllItems = new List<clsItem>();

                // Pull SQL statement from the clsItemsSQL class
                string sSQL = sql.PopulateAllItems();

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                // Add table name
                dsItems.Tables[0].TableName = "ItemDesc";

                // For each data row, transpose that rows information into an Item
                // to be added to the list to be returned
                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    lstAllItems.Add(new clsItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));

                // This will check to make sure the item about to be edited has the same item code
                if (item.sItemCode != window.txtbItemCode.Text)
                {
                    message = "Item Codes do not match";
                    return lstAllItems;
                }

                // This will update the item in the database
                sSQL = sql.EditOneItem(item.sItemCode, window.txtbItemDescription.Text, window.txtbItemCost.Text);
                db.ExecuteNonQuery(sSQL);

                // This will return the updated item in the list
                foreach (clsItem itemInList in lstAllItems)
                {
                    if (itemInList.sItemCode == item.sItemCode)
                    {
                        itemInList.sItemCost = window.txtbItemCost.Text;
                        itemInList.sItemDescription = window.txtbItemDescription.Text;
                    }
                }

                window.lblSuccess.Content = "Item Edited Successfully";

                //Update invoice totals.
                List<string> invoicesToUpdate = new List<string>();

                // Pull SQL statement from the clsItemsSQL class
                sSQL = sql.GetInvoices(item.sItemCode);

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);

                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    invoicesToUpdate.Add(dr[0].ToString());

                foreach (string invoice in invoicesToUpdate)
                {
                    // Pull SQL statement from the clsItemsSQL class
                    sSQL = sql.GetNewInvoiceTotal(invoice);

                    // Store the SQL results into dataset
                    string sTotal = db.ExecuteScalarSQL(sSQL);

                    sSQL = sql.UpdateTotal(invoice, sTotal);

                    db.ExecuteNonQuery(sSQL);
                }

                //Track the number of rows returned
                iRowsReturned = 0;

                //Create a list that will contain the Items and be returned
                lstAllItems = new List<clsItem>();

                // Pull SQL statement from the clsItemsSQL class
                sSQL = sql.PopulateAllItems();

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                // Add table name
                dsItems.Tables[0].TableName = "ItemDesc";

                // For each data row, transpose that rows information into an Item
                // to be added to the list to be returned
                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    lstAllItems.Add(new clsItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));

                return lstAllItems;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// This wil delete an item from the database and the list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public List<clsItem> DeleteItem(clsItem item, wndItems window, ref string message)
        {
            try
            {
                //Create dataset
                DataSet dsItems;

                //Track the number of rows returned
                int iRowsReturned = 0;

                //Create a list that will contain the Items and be returned
                List<clsItem> lstAllItems = PopulateItems();

                // Pull SQL statement from the clsItemsSQL class
                string sSQL = sql.PopulateAllItems();

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);

                // For each data row, transpose that rows information into an Item
                // to be added to the list to be returned
                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    lstAllItems.Add(new clsItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));

                // Pull SQL statement from the clsItemsSQL class
                sSQL = sql.GetInvoices(item.sItemCode);

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);

                //Test if any invoices are tied to the item.
                string sInvoices = "";

                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    sInvoices += dr[0].ToString() + " ";

                if (sInvoices != "")
                {
                    message = "Item can not be deleted because it is in the following invoices: " + sInvoices;
                    return lstAllItems;
                }

                // Remove a the item from the database
                sSQL = sql.DeleteOneItem(item.sItemCode);
                db.ExecuteNonQuery(sSQL);

                int? removeAtIndex = null;

                foreach (clsItem itemInList in lstAllItems)
                {
                    if (itemInList.sItemCode == item.sItemCode)
                    {
                        removeAtIndex = lstAllItems.IndexOf(itemInList);
                    }
                }

                if (removeAtIndex != null)
                {
                    lstAllItems.RemoveAt((int)removeAtIndex);
                }

                lstAllItems = new List<clsItem>();

                // Pull SQL statement from the clsItemsSQL class
                sSQL = sql.PopulateAllItems();

                // Store the SQL results into dataset
                dsItems = db.ExecuteSQLStatement(sSQL, ref iRowsReturned);

                // For each data row, transpose that rows information into an Item
                // to be added to the list to be returned
                foreach (DataRow dr in dsItems.Tables[0].Rows)
                    lstAllItems.Add(new clsItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));

                return lstAllItems;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
