using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroupProject
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        /// <summary>
        /// Default main window.
        /// </summary>
        public wndMain()
        {
            //Initial values.
            InitializeComponent();

            objMainLogic = new clsMainLogic();
            objSearchWindow = new wndSearch();

            //Get the items updated.
            objMainLogic.UpdateItems();
            cboItemChooser.ItemsSource = objMainLogic.ItemList;

            //Because we have persistant windows, the shutdown mode must be changed.
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        /// <summary>
        /// Holds the main logic for the main window.
        /// </summary>
        private clsMainLogic objMainLogic;

        /// <summary>
        /// Holds the search window, this allows us to pass a seach value back.
        /// </summary>
        private wndSearch objSearchWindow;

        /// <summary>
        /// Activates all controls relative to an invoice.
        /// </summary>
        private void InvoiceEditControls(bool bIsOn)
        {
            try
            {
                datepInvoiceDate.IsEnabled = bIsOn;
                cboItemChooser.IsEnabled = bIsOn;
                btnDeleteInvoice.IsEnabled = bIsOn;
                btnSaveInvoice.IsEnabled = bIsOn;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Activates all controls relative to deleting an item.
        /// </summary>
        private void ItemDeleteControls(bool bIsOn)
        {
            try
            {
                btnDeleteItem.IsEnabled = bIsOn;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Activates all controls relative to adding an item.
        /// </summary>
        private void ItemAddControls(bool bIsOn)
        {
            try
            {
                btnAddItem.IsEnabled = bIsOn;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Resets all inputs to the default state.
        /// </summary>
        private void RestoreDefaults()
        {
            try
            {
                lblInvoiceNumber.Content = "Invoice #: N/A";

                datepInvoiceDate.SelectedDate = DateTime.Today;
                datepInvoiceDate.SelectedDate = DateTime.Today;

                lblItemNumber.Content = "Selected Item #: N/A";

                cboItemChooser.SelectedIndex = -1;
                txtbItemCost.Text = "";

                lblTotal.Content = "Total Cost:";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Enables and disables edit mode.
        /// </summary>
        private void FlipMode()
        {
            try
            {
                //Disable edit mode.
                if (objMainLogic.EditMode)
                {
                    btnEdit.Content = "Edit Invoice";

                    objMainLogic.EditMode = false;
                    objMainLogic.ResetEdits();

                    InvoiceEditControls(false);
                    ItemAddControls(false);

                    //Refresh data.
                    lblInvoiceNumber.Content = "Invoice #: " + objMainLogic.SelectedInvoiceNumber;
                    dgLineItems.ItemsSource = objMainLogic.LineItemList;
                    lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;

                    menuBtnEditItems.IsEnabled = true;
                }
                else //Enable edit mode.
                {
                    btnEdit.Content = "Cancel Edit Invoice";

                    objMainLogic.EditMode = true;

                    InvoiceEditControls(true);

                    menuBtnEditItems.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Displays an error trace.
        /// </summary>
        /// <param name="sClass">Last calling class.</param>
        /// <param name="sMethod">Last calling method.</param>
        /// <param name="sMessage">Last error message.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                    "HandleError Exception: " + e.Message);
            }
        }

        /// <summary>
        /// When clicked, open items window. Updates displays.
        /// </summary>
        private void MenuBtnEditItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //No data is needed from this window, so it is disposed of as
                //soon as we are done with it.
                wndItems objItemsWindow = new wndItems();
                this.IsEnabled = false;
                objItemsWindow.ShowDialog();

                //Update items chooser.
                objMainLogic.UpdateItems();
                cboItemChooser.ItemsSource = objMainLogic.ItemList;

                //Update line item display.
                objMainLogic.UpdateData();
                dgLineItems.ItemsSource = objMainLogic.LineItemList;
                lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;

                this.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// When the search button is clicked, open the search window. Update
        /// if a good search was made.
        /// </summary>
        private void MenuBtnSearchInvoices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objSearchWindow = new wndSearch();

                this.IsEnabled = false;
                objSearchWindow.ShowDialog();

                //Gets search number.
                string sInvoiceNumber = objSearchWindow.SelectedInvoiceNumber;

                //Makes sure it's good.
                if(objMainLogic.SearchFor(sInvoiceNumber))
                {
                    //Break up date.
                    string[] saInvoiceDate = objMainLogic.SelectedInvoiceDate.Split('/');

                    //Display invoice info.
                    dgLineItems.ItemsSource = objMainLogic.LineItemList;
                    lblInvoiceNumber.Content = "Invoice #: " + sInvoiceNumber;

                    datepInvoiceDate.SelectedDate = new DateTime(Int32.Parse(saInvoiceDate[2].Substring(0, 4)), Int32.Parse(saInvoiceDate[0]), Int32.Parse(saInvoiceDate[1]));

                    lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;
                    InvoiceEditControls(false);
                    ItemAddControls(false);
                    cboItemChooser.SelectedIndex = -1;
                    if (objMainLogic.EditMode) FlipMode();
                    btnEdit.IsEnabled = true;
                }

                this.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Enables and disables edit mode.
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FlipMode();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// When the datagrid's selection changes, update selected item.
        /// </summary>
        private void DgLineItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(dgLineItems.SelectedIndex == -1 || !objMainLogic.EditMode)
                {
                    lblItemNumber.Content = "Selected Item #: N/A";
                    ItemDeleteControls(false);
                    dgLineItems.SelectedIndex = -1;
                    return;
                }

                lblItemNumber.Content = "Selected Item #: " + ((clsLineItem)(dgLineItems.SelectedItem)).Number;
                ItemDeleteControls(true);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// When the add item combo box changes, update cost.
        /// </summary>
        private void CboItemChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboItemChooser.SelectedIndex == -1)
                {
                    txtbItemCost.Text = "";
                    ItemAddControls(false);
                    return;
                }

                txtbItemCost.Text = ((clsItem)(cboItemChooser.SelectedItem)).getItemCost();
                ItemAddControls(true);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new invoice.
        /// </summary>
        private void BtnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RestoreDefaults();
                if (objMainLogic.EditMode) FlipMode();
                FlipMode();

                objMainLogic.AddInvoice();

                //Refresh data.
                lblInvoiceNumber.Content = "Invoice #: " + objMainLogic.SelectedInvoiceNumber;
                dgLineItems.ItemsSource = objMainLogic.LineItemList;
                lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;

                btnEdit.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Removes an item from the selected invoice.
        /// </summary>
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Make sure a line item is selected.
                if (dgLineItems.SelectedIndex == -1) return;

                objMainLogic.DeleteItem(((clsLineItem)dgLineItems.SelectedItem).Number);

                //Refresh data.
                lblInvoiceNumber.Content = "Invoice #: " + objMainLogic.SelectedInvoiceNumber;
                dgLineItems.ItemsSource = new List<clsLineItem>(); //Forces an update, other methods were unreliable.
                dgLineItems.ItemsSource = objMainLogic.LineItemList;
                lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;

                ItemDeleteControls(false);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Adds an item from the selected invoice.
        /// </summary>
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Make sure a line item is selected.
                if (cboItemChooser.SelectedIndex == -1) return;

                objMainLogic.AddItem(((clsItem)cboItemChooser.SelectedItem).sItemCode);

                //Refresh data.
                lblInvoiceNumber.Content = "Invoice #: " + objMainLogic.SelectedInvoiceNumber;
                dgLineItems.ItemsSource = new List<clsLineItem>(); //Forces an update, other methods were unreliable.
                dgLineItems.ItemsSource = objMainLogic.LineItemList;
                lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Update invoice date.
        /// </summary>
        private void DatepInvoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                objMainLogic.UpdateDate(datepInvoiceDate.SelectedDate.ToString());
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Saves the selected invoice.
        /// </summary>
        private void BtnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objMainLogic.SaveChanges();

                //Refresh data.
                lblInvoiceNumber.Content = "Invoice #: " + objMainLogic.SelectedInvoiceNumber;
                dgLineItems.ItemsSource = new List<clsLineItem>(); //Forces an update, other methods were unreliable.
                dgLineItems.ItemsSource = objMainLogic.LineItemList;
                lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;

                InvoiceEditControls(false);
                ItemAddControls(false);
                cboItemChooser.SelectedIndex = -1;
                FlipMode();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the selected invoice.
        /// </summary>
        private void BtnDeleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objMainLogic.DeleteInvoice();

                //Refresh data.
                lblInvoiceNumber.Content = "Invoice #: " + objMainLogic.SelectedInvoiceNumber;
                dgLineItems.ItemsSource = objMainLogic.LineItemList;
                lblTotal.Content = "Total Cost: " + objMainLogic.SelectedInvoiceTotal;

                InvoiceEditControls(false);
                ItemAddControls(false);
                cboItemChooser.SelectedIndex = -1;
                FlipMode();
                btnEdit.IsEnabled = false;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
