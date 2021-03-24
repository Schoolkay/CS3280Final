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
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        /// <summary>
        /// Creates a usable initialization of clsItemsLogic to use the business logic of the window
        /// </summary>
        clsItemsLogic logic;

        /// <summary>
        /// Default items window
        /// </summary>
        public wndItems()
        {
            try
            {
                InitializeComponent();

                logic = new clsItemsLogic();

                // Populate the Items DataGrid will all current items
                dgItems.ItemsSource = logic.PopulateItems();
                clearAllMessageLabels();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Adds an item to the database. Updates display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearAllMessageLabels();

                //If valid input, add item
                if (txtbItemCode.Text != "" && txtbItemDescription.Text != "" && txtbItemCost.Text != "")
                {
                    if (txtbItemCost.Text.Contains("."))
                    {
                        lblError.Content = "Please enter an Integer for Cost";
                    }
                    else
                    {
                        logic.AddItem(txtbItemCode.Text, txtbItemDescription.Text, txtbItemCost.Text, this);
                        dgItems.ItemsSource = logic.PopulateItems();

                        txtbItemCode.Text = "";
                        txtbItemCost.Text = "";
                        txtbItemDescription.Text = "";

                        btnDeleteItem.IsEnabled = false;
                        btnEditItem.IsEnabled = false;
                    }
                }
                else
                {
                    lblError.Content = "All fields must be filled";
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Edits the selected item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearAllMessageLabels();

                //Makes sure the selection is valid.
                if (dgItems.SelectedItem != null)
                {
                    // Convert selected Item to an item and pass the information through
                    string message = "";
                    logic.EditItem((clsItem)dgItems.SelectedItem, this, ref message);
                    dgItems.ItemsSource = logic.PopulateItems();
                    lblError.Content = message;

                    txtbItemCode.Text = "";
                    txtbItemCost.Text = "";
                    txtbItemDescription.Text = "";

                    btnDeleteItem.IsEnabled = false;
                    btnEditItem.IsEnabled = false;
                }
                else
                {
                    lblError.Content = "No item was selected";
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearAllMessageLabels();

                if (dgItems.SelectedItem != null)
                {
                    // Convert selected Item to an item and pass the information through
                    string message = "";
                    dgItems.ItemsSource = logic.DeleteItem((clsItem)dgItems.SelectedItem, this, ref message);
                    lblError.Content = message;

                    txtbItemCode.Text = "";
                    txtbItemCost.Text = "";
                    txtbItemDescription.Text = "";

                    btnDeleteItem.IsEnabled = false;
                    btnEditItem.IsEnabled = false;
                }
                else
                {
                    lblError.Content = "No item was selected";
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Updates the selected item from the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                clearAllMessageLabels();

                //ERROR HANDLING
                if (dgItems.SelectedIndex == -1) return;

                // Fill in the item's information in the textboxes
                clsItem selectedItem = (clsItem)dgItems.SelectedItem;
                txtbItemCode.Text = selectedItem.sItemCode;
                txtbItemCost.Text = selectedItem.sItemCost;
                txtbItemDescription.Text = selectedItem.sItemDescription;

                // Enable edit and delte buttons
                btnEditItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;

                // Update the textboxes to the current selection
                lblSuccess.Content = dgItems.CurrentItem.ToString();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method to display the handled errors to the user should one occur
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(String sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + "->" + sMessage);
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText("C:\\Error.Txt", Environment.NewLine + "HandleError Exception" + ex.Message);
            }
        }

        /// <summary>
        /// Clears all the message labels
        /// </summary>
        public void clearAllMessageLabels()
        {
            try
            {
                lblError.Content = "";
                lblItemCodeError.Content = "";
                lblItemCostError.Content = "";
                lblItemDescriptionError.Content = "";
                lblSuccess.Content = "";
                lblError.Content = "";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Prevents an existing item from being edited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtbItemCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                btnEditItem.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
                dgItems.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
