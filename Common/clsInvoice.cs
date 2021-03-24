using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    /// <summary>
    /// Class used to store instances of an invoice from database
    /// </summary>
    class clsInvoice
    {
        /// <summary>
        /// String sInvNumber is the invoice number 
        /// </summary>
        private string sInvNumber;
        /// <summary>
        /// String sInvDate is the date on the invoice
        /// </summary>
        private string sInvDate;
        /// <summary>
        /// String sInvCharge is the totalcost of the invoice
        /// </summary>
        private string sInvCharge;

        /// <summary>
        /// Constructor for clsInvoice, sNumebr, sDate, and sCharge all have default
        /// constructors of ""
        /// </summary>
        /// <param name="sNumber"></param>
        /// <param name="sDate"></param>
        /// <param name="sCharge"></param>
        public clsInvoice(string sNumber = "", string sDate = "", string sCharge = "")
        {
            try
            {
                sInvNumber = sNumber;
                sInvDate = sDate;
                sInvCharge = sCharge;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Public properity InvoiceNumber to get and set sInvNumber
        /// </summary>
        public string InvoiceNumber
        {
            get
            {
                return sInvNumber;
            }
            set
            {
                sInvNumber = value;
            }
        }

        /// <summary>
        /// Public properity InvoiceDate to get and set sInvDate
        /// </summary>
        public string InvoiceDate
        {
            get
            {
                return sInvDate;
            }
            set
            {
                sInvDate = value;
            }
        }

        /// <summary>
        /// Public properity InvoiceCharge to get and set sInvCharge
        /// </summary>
        public string InvoiceCharge
        {
            get
            {
                return sInvCharge;
            }
            set
            {
                sInvCharge = value;
            }
        }

        /// <summary>
        /// Overide funstion to display only the flight number when ToString is called
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return sInvNumber + " " + sInvDate + " " + sInvCharge;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
