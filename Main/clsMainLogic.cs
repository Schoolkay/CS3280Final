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
    /// All the logic for the main window.
    /// </summary>
    class clsMainLogic
    {
        /// <summary>
        /// Default logic.
        /// </summary>
        public clsMainLogic()
        {
            //Initial values.
            SelectedInvoiceNumber = "-1";
            objDataBaseAccess = new clsDataAccess();
            objMainSQL = new clsMainSQL();
            LineItemList = new List<clsLineItem>();
            ItemList = new List<clsItem>();
            lstEditList = new List<string>();
            EditMode = false;
        }

        /// <summary>
        /// Returns the current invoice number.
        /// </summary>
        public string SelectedInvoiceNumber { get; private set; }

        /// <summary>
        /// Returns the current invoice date.
        /// </summary>
        public string SelectedInvoiceDate { get; private set; }

        /// <summary>
        /// Returns the current invoice total.
        /// </summary>
        public string SelectedInvoiceTotal { get; private set; }

        /// <summary>
        /// Returns the list of line items.
        /// </summary>
        public List<clsLineItem> LineItemList { get; private set; }

        /// <summary>
        /// Returns the list items.
        /// </summary>
        public List<clsItem> ItemList { get; private set; }

        /// <summary>
        /// Flag which states if the program is adding a new invoice.
        /// </summary>
        public bool AddingNewInvoice { get; private set; }

        /// <summary>
        /// Flag to show if the program is in edit mode.
        /// </summary>
        public bool EditMode { get; set; }

        /// <summary>
        /// Holds the list of edits to be saved to the database.
        /// </summary>
        private List<string> lstEditList;

        /// <summary>
        /// Database logic class.
        /// </summary>
        private clsDataAccess objDataBaseAccess;

        /// <summary>
        /// Used to build all our used sql statements.
        /// </summary>
        private clsMainSQL objMainSQL;

        /// <summary>
        /// Updates line item list to relect what's in the database.
        /// </summary>
        /// <param name="sInvoiceNumber">The invoice number for the line items to grab.</param>
        public void UpdateData()
        {
            try
            {
                if (AddingNewInvoice || SelectedInvoiceNumber == "N/A") return;

                //Reset list.
                LineItemList = new List<clsLineItem>();

                //Store query info.
                DataSet dsLineItems;
                int iRowsReturned = 0;

                //Get SQL statement.
                string sSQL = objMainSQL.GetItemsForInvoice(SelectedInvoiceNumber);

                //Get and normalize table info.
                dsLineItems = objDataBaseAccess.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                dsLineItems.Tables[0].TableName = "Line Items";

                //Organize SQL info into a notmalized list.
                foreach (DataRow drCurrent in dsLineItems.Tables[0].Rows)
                {
                    LineItemList.Add(new clsLineItem(drCurrent[0].ToString(), drCurrent[1].ToString(), drCurrent[2].ToString(), drCurrent[3].ToString()));
                }

                //Get SQL statement 2.
                sSQL = objMainSQL.GetInvoiceDate(SelectedInvoiceNumber);

                //Get invoice date.
                SelectedInvoiceDate = objDataBaseAccess.ExecuteScalarSQL(sSQL);

                //Get SQL statement 3.
                sSQL = objMainSQL.GetInvoiceTotal(SelectedInvoiceNumber);

                //Get invoice total.
                SelectedInvoiceTotal = objDataBaseAccess.ExecuteScalarSQL(sSQL);
                if (SelectedInvoiceTotal == "") SelectedInvoiceTotal = "0";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Updates the items list.
        /// </summary>
        public void UpdateItems()
        {
            try
            {
                //Reset list.
                ItemList = new List<clsItem>();

                //Store query info.
                DataSet dsItems;
                int iRowsReturned = 0;

                //Get SQL statement.
                string sSQL = objMainSQL.GetItems();

                //Get and normalize table info.
                dsItems = objDataBaseAccess.ExecuteSQLStatement(sSQL, ref iRowsReturned);
                dsItems.Tables[0].TableName = "Line Items";

                //Organize SQL info into a notmalized list.
                foreach (DataRow drCurrent in dsItems.Tables[0].Rows)
                {
                    ItemList.Add(new clsItem(drCurrent[0].ToString(), drCurrent[1].ToString(), drCurrent[2].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Checks if an invoice exists in the database.
        /// </summary>
        /// <param name="sInvoiceNumber">The invoice number to test.</param>
        /// <returns>bool: Is the invoice real?</returns>
        private bool InvoiceExists(string sInvoiceNumber)
        {
            try
            {
                //Store query info.
                string sInvoice;

                //Get SQL statement.
                string sSQL = objMainSQL.GetInvoiceCheck(sInvoiceNumber);

                //Get invoice validity.
                sInvoice = objDataBaseAccess.ExecuteScalarSQL(sSQL);

                //Return if the invoice exists or not.
                return sInvoice != "" ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Searches for an invoice and updates the line items.
        /// </summary>
        /// <param name="sInvoiceNumber">The invoice number who's line items we need.</param>
        /// <returns>bool: Was the search successful?</returns>
        public bool SearchFor(string sInvoiceNumber)
        {
            try
            {
                //Make sure the invoice exists.
                if (InvoiceExists(sInvoiceNumber))
                {
                    //Update line items.
                    SelectedInvoiceNumber = sInvoiceNumber;
                    UpdateData();
                    lstEditList = new List<string>();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Adds a new invoice to be edited and saved later.
        /// </summary>
        public void AddInvoice()
        {
            try
            {
                //Set new invoice data.
                SelectedInvoiceNumber = "TBD";
                AddingNewInvoice = true;
                LineItemList = new List<clsLineItem>();
                SelectedInvoiceTotal = "0";
                SelectedInvoiceDate = DateTime.Today.ToString();

                lstEditList = new List<string>();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Deletes an item from the invoice.
        /// </summary>
        /// <param name="sLineItemNumber">The line item number to delete.</param>
        public void DeleteItem(string sLineItemNumber)
        {
            try
            {
                //If line number is real...
                if (LineItemList.Exists(lI => lI.Number == sLineItemNumber))
                {
                    //Update total.
                    SelectedInvoiceTotal = (Int32.Parse(SelectedInvoiceTotal) - Int32.Parse(LineItemList.Find(lI => lI.Number == sLineItemNumber).Cost)).ToString();

                    //Remove line item.
                    LineItemList.RemoveAll(lI => lI.Number == sLineItemNumber);

                    //Remove the record of the line item, this changes between
                    //new invoices, items, and existing invoices.
                    if (lstEditList.Exists(s => s == "aItem;" + sLineItemNumber)) lstEditList.RemoveAll(s => s == "aItem:" + sLineItemNumber);
                    else if (!AddingNewInvoice) lstEditList.Add("dItem;" + sLineItemNumber);
                    lstEditList.Add("total; ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Adds an item from the invoice.
        /// </summary>
        /// <param name="sItemCode">The item code to add.</param>
        public void AddItem(string sItemCode)
        {
            try
            {
                //Get selected item.
                clsItem objSelectedItem = ItemList.Find(i => i.sItemCode == sItemCode);

                //Update total.
                SelectedInvoiceTotal = (Int32.Parse(SelectedInvoiceTotal) + Int32.Parse(objSelectedItem.sItemCost)).ToString();

                //Set up a max tester to find the new line number.
                Func<clsLineItem, int> fMaxFunc = (lI) => Int32.Parse(lI.Number);

                //Add new line item to display, number is determined by max or if there is no data.
                LineItemList.Add(new clsLineItem(LineItemList.Count > 0 ? (LineItemList.Max(fMaxFunc) + 1).ToString() : "1", sItemCode, objSelectedItem.sItemDescription, objSelectedItem.sItemCost));

                //Log change.
                lstEditList.Add("aItem;" + (LineItemList.Max(fMaxFunc) + 1).ToString() + ";" + objSelectedItem.sItemCode);
                lstEditList.Add("total; ");
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Updates the date of an invoice.
        /// </summary>
        /// <param name="sNewDate">The new date string.</param>
        public void UpdateDate(string sNewDate)
        {
            try
            {
                SelectedInvoiceDate = sNewDate;

                //Log change.
                lstEditList.Add("date;" + sNewDate);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Saves changes on the current invoice.
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                List<string> tempEditList = lstEditList;

                if (AddingNewInvoice)
                {
                    //Get SQL statement.
                    string sSQL = objMainSQL.AddInvoice(SelectedInvoiceDate, SelectedInvoiceTotal);

                    //Add invoice.
                    int goodInsert = objDataBaseAccess.ExecuteNonQuery(sSQL);

                    if (goodInsert == 0) throw new Exception("New invoice was not added!");

                    //Get invoice number.
                    sSQL = objMainSQL.GetNewestInvoiceNum;

                    SearchFor(objDataBaseAccess.ExecuteScalarSQL(sSQL));

                    AddingNewInvoice = false;
                }

                string[] sArrComParts;

                foreach (string command in tempEditList)
                {
                    sArrComParts = command.Split(';');

                    //First check the command type.
                    if (sArrComParts[0] == "total") //Update total.
                    {
                        //Get SQL statement.
                        string sSQL = objMainSQL.UpdateTotal(SelectedInvoiceNumber, SelectedInvoiceTotal);

                        //Update total.
                        objDataBaseAccess.ExecuteNonQuery(sSQL);
                    }
                    else if (sArrComParts[0] == "date") //Update date.
                    {
                        //Get SQL statement.
                        string sSQL = objMainSQL.UpdateDate(SelectedInvoiceNumber, SelectedInvoiceDate);

                        //Update date.
                        objDataBaseAccess.ExecuteNonQuery(sSQL);
                    }
                    else if (sArrComParts[0] == "dItem") //Delete item.
                    {
                        //Get SQL statement.
                        string sSQL = objMainSQL.RemoveItem(SelectedInvoiceNumber, sArrComParts[1]);

                        //Delete item.
                        objDataBaseAccess.ExecuteNonQuery(sSQL);
                    }
                    else if (sArrComParts[0] == "aItem") //Add item.
                    {
                        //Get SQL statement.
                        string sSQL = objMainSQL.AddItem(SelectedInvoiceNumber, sArrComParts[1], sArrComParts[2]);

                        //Delete item.
                        objDataBaseAccess.ExecuteNonQuery(sSQL);
                    }
                }

                lstEditList = new List<string>();

                //Get latest data.
                SearchFor(SelectedInvoiceNumber);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Deletes the invoice.
        /// </summary>
        public void DeleteInvoice()
        {
            try
            {
                if (!AddingNewInvoice)
                {
                    SearchFor(SelectedInvoiceNumber);

                    //Remove all line items.
                    foreach (clsLineItem objLineItem in LineItemList)
                    {
                        //Get SQL statement.
                        string sSSQL = objMainSQL.RemoveItem(SelectedInvoiceNumber, objLineItem.Number);

                        //Delete item.
                        objDataBaseAccess.ExecuteNonQuery(sSSQL);
                    }

                    //Get SQL statement.
                    string sSQL = objMainSQL.RemoveInvoice(SelectedInvoiceNumber);

                    //Delete invoice.
                    objDataBaseAccess.ExecuteNonQuery(sSQL);
                }

                SelectedInvoiceNumber = "N/A";
                SelectedInvoiceDate = DateTime.Today.ToString();
                SelectedInvoiceTotal = "";
                LineItemList = new List<clsLineItem>();
                AddingNewInvoice = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Resets edits.
        /// </summary>
        public void ResetEdits()
        {
            try
            {
                lstEditList = new List<string>();

                //Reset data.
                if(SelectedInvoiceNumber != "N/A" && SelectedInvoiceNumber != "TBD") SearchFor(SelectedInvoiceNumber);
                else
                {
                    LineItemList = new List<clsLineItem>();
                    SelectedInvoiceDate = DateTime.Today.ToString();
                    SelectedInvoiceNumber = "N/A";
                    SelectedInvoiceTotal = "";
                    AddingNewInvoice = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
