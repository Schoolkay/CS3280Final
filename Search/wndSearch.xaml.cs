using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        /// <summary>
        /// Class clsSLogic of clsSearchLogic to runs the business logic of the window
        /// </summary>
        clsSearchLogic clsSLogic;
        /// <summary>
        /// string array saSearchModifiers hold the search parameters for the window 
        /// selected in combobox and passed to business logic to get search results
        /// </summary>
        string[] saSearchModifiers;
        /// <summary>
        /// bool bIsNumberSelected tracks when an item has been selected
        /// from null in the combo box for invoice numbers
        /// </summary>
        bool bIsNumberSelected;
        /// <summary>
        /// bool bIsDateSelected tracks when an item has been selected
        /// from null in the combo box for invoice dates
        /// </summary>
        bool bIsDateSelected;
        /// <summary>
        /// bool bIsChargeSelected tracks when an item has been selected
        /// from null in the combo box for invoice charges
        /// </summary>
        bool bIsChargeSelected;
        /// <summary>
        /// bool bIsReseting tracks when a use is reseting a combo box parameter 
        /// so that other method do not fire off during the process
        /// </summary>
        bool bIsReseting;
        /// <summary>
        //// bool bIsReseting tracks when a use is leaving the window and the 
        /// window is reseting for next use so that other method do not fire 
        /// off during the process
        /// </summary>
        bool bIsWindowReseting;
        /// <summary>
        /// string holds the selected invoice that was searched used by the
        /// main window -1 indicates nothing was selected
        /// </summary>
        public string SelectedInvoiceNumber { get; private set; }

        /// <summary>
        /// Constuctor for the Seach window
        /// </summary>
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                //initialize business logic for window
                clsSLogic = new clsSearchLogic();
                //initialize default view of all invoices when window is created
                dgResults.ItemsSource = clsSLogic.PopulateGrid();
                //initialize the combo box of invoice numbers 
                cboInvoiceNumber.ItemsSource = clsSLogic.ReturnNumbers();
                //initialize the combo box of invoice dates
                cboInvoiceDate.ItemsSource = clsSLogic.ReturnDates();
                //initialize the combo box of invoice total costs
                cboInvoiceCharge.ItemsSource = clsSLogic.ReturnCharges();
                //initialize the string array for gathering search modifiers
                saSearchModifiers = new string[3];
                //initialize bool to check if there is a selection of the searchbox
                bIsNumberSelected = false;
                //initialize bool to check if there is a selection of the searchbox
                bIsDateSelected = false;
                //initialize bool to check if there is a selection of the searchbox
                bIsChargeSelected = false;
                //initialize bool to check if the user is reseting a search parameter
                bIsReseting = false;
                //initialize bool to check if the window is being reset for being hidden 
                //and to be use later
                bIsWindowReseting = false;
                //initialize string for selected invoice number
                SelectedInvoiceNumber = "-1";
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method cboInvoiceNumber_SelectionChanged passes the selected value of
        /// an invoice number into the business logic thorugh the collectparameters 
        /// function and returns the results of the search 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboInvoiceNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //High level exception handling
            try
            {
                //Do not fire selection change events while reseting combo box to defualt -1
                if (bIsReseting == false && bIsWindowReseting == false)
                {
                    //avoid pulling a null object from the combobox
                    if (bIsNumberSelected == false) { bIsNumberSelected = true; }
                    //Pull perameters for search into class string array
                    CollectSearchParameters();
                    //assign the datagrid the results of the search from the givin perameters
                    dgResults.ItemsSource = clsSLogic.ReturnSearch(saSearchModifiers);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method cboInvoiceDate_SelectionChanged passes the selected value of
        /// an invoice date into the business logic thorugh the collectparameters 
        /// function and returns the results of the search 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboInvoiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //High level exception handling
            try
            {
                //Do not fire selection change events while reseting combo box to defualt -1
                if (bIsReseting == false && bIsWindowReseting == false)
                {
                    //avoid pulling a null object from the combobox
                    if (bIsDateSelected == false) { bIsDateSelected = true; }
                    //Pull perameters for search into class string array
                    CollectSearchParameters();
                    //assign the datagrid the results of the search from the givin perameters
                    dgResults.ItemsSource = clsSLogic.ReturnSearch(saSearchModifiers);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method cboInvoiceCharge_SelectionChanged passes the selected value of
        /// an invoice charge into the business logic thorugh the collectparameters 
        /// function and returns the results of the search 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboInvoiceCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //High level exception handling
            try
            {
                //Do not fire selection change events while reseting combo box to defualt -1
                if (bIsReseting == false && bIsWindowReseting == false)
                {
                    //avoid pulling a null object from the combobox
                    if (bIsChargeSelected == false) { bIsChargeSelected = true; }
                    //Pull perameters for search into class string array
                    CollectSearchParameters();
                    //assign the datagrid the results of the search from the givin perameters
                    dgResults.ItemsSource = clsSLogic.ReturnSearch(saSearchModifiers);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method to handle the button click of submit, sending info on the selected 
        /// invoice back to the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //High level exception handling
            try
            {
                if (dgResults.SelectedIndex == -1) return;

                //Pull selected invoice object from data grid to find the invoice number
                string sReturnumber = clsSLogic.GetItemNumber((clsInvoice)dgResults.SelectedItem);
                SelectedInvoiceNumber = sReturnumber;
                //Pass to public properity
                ResetWindow();
                this.Hide();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method to handle the button click of the cancel button to reset the window 
        /// and hide this window in order to return to the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //High level exception handling
            try
            {
                //pass selected item back to main window to be interacted with
                SelectedInvoiceNumber = "-1";
                ResetWindow();
                this.Hide();

            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method to handle invoce number combo box reset button. Resets the combo box
        /// to a -1 index to remove the search parameter from the logic and thus allow
        /// the user to return to the full table or the table minus that search parameter
        /// and select others or reselect another parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Restrict what happens when reseting the combo box
                bIsReseting = true;
                //tack that the number is no longer selected when creating a search querry
                bIsNumberSelected = false;
                //remove the currenet search parameter from the string array
                saSearchModifiers[0] = null;
                //return combo box to -1 index selected
                cboInvoiceNumber.SelectedIndex = -1;
                //no long needs to restrict other methods firing, reset done
                bIsReseting = false;
                //collect search parameters and display the table with this parameter reset to nothing
                CollectSearchParameters();
                dgResults.ItemsSource = clsSLogic.ReturnSearch(saSearchModifiers);
            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Method to handle invoce date combo box reset button. Resets the combo box
        /// to a -1 index to remove the search parameter from the logic and thus allow
        /// the user to return to the full table or the table minus that search parameter
        /// and select others or reselect another parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Restrict what happens when reseting the combo box
                bIsReseting = true;
                //tack that the date is no longer selected when creating a search querry
                bIsDateSelected = false;
                //remove the currenet search parameter from the string array
                saSearchModifiers[1] = null;
                //return combo box to -1 index selected
                cboInvoiceDate.SelectedIndex = -1;
                //no long needs to restrict other methods firing, reset done
                bIsReseting = false;
                //collect search parameters and display the table with this parameter reset to nothing
                CollectSearchParameters();
                dgResults.ItemsSource = clsSLogic.ReturnSearch(saSearchModifiers);
            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// /// Method to handle invoce charge combo box reset button. Resets the combo box
        /// to a -1 index to remove the search parameter from the logic and thus allow
        /// the user to return to the full table or the table minus that search parameter
        /// and select others or reselect another parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Restrict what happens when reseting the combo box
                bIsReseting = true;
                //tack that the charge is no longer selected when creating a search querry
                bIsChargeSelected = false;
                //remove the currenet search parameter from the string array
                saSearchModifiers[2] = null;
                //return combo box to -1 index selected
                cboInvoiceCharge.SelectedIndex = -1;
                //no long needs to restrict other methods firing, reset done
                bIsReseting = false;
                //collect search parameters and display the table with this parameter reset to nothing
                CollectSearchParameters();
                dgResults.ItemsSource = clsSLogic.ReturnSearch(saSearchModifiers);
            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Prevents the window from closing to preserve data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                SelectedInvoiceNumber = "-1";
                this.Hide();
                e.Cancel = true;
            }
            catch (Exception ex)
            {

                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Private method CollectSearchParameters pulls the search perameters from the combo 
        /// boxes into a string array to pass to logic
        /// </summary>
        private void CollectSearchParameters()
        {
            //Lower level exception handling
            try
            {
                //do not fire while the window is reseting
                if (bIsWindowReseting == false)
                {
                    //gather each search parameter that exists
                    if (bIsNumberSelected == true)
                    {
                        saSearchModifiers[0] = cboInvoiceNumber.SelectedItem.ToString();
                    }
                    if (bIsDateSelected == true)
                    {
                        saSearchModifiers[1] = cboInvoiceDate.SelectedItem.ToString();
                    }
                    if (bIsChargeSelected == true)
                    {
                        saSearchModifiers[2] = cboInvoiceCharge.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Private mehtod ResetWindow used to reset the display of the window after being 
        /// hidden to return to the main screen
        /// </summary>
        private void ResetWindow()
        {
            //lower level exception handling
            try
            {
                //reset all modifiable values and display the default invoice table
                bIsWindowReseting = true;
                bIsNumberSelected = false;
                saSearchModifiers[0] = null;
                cboInvoiceNumber.SelectedIndex = -1;
                bIsDateSelected = false;
                saSearchModifiers[1] = null;
                cboInvoiceDate.SelectedIndex = -1;
                bIsChargeSelected = false;
                saSearchModifiers[2] = null;
                cboInvoiceCharge.SelectedIndex = -1;
                dgResults.ItemsSource = clsSLogic.PopulateGrid();
                bIsWindowReseting = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
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
    }
}
