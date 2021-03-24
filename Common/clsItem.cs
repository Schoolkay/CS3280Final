using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    /// <summary>
    /// Item data holder.
    /// </summary>
    class clsItem
    {
        /// <summary>
        /// This will hold the Item Description
        /// </summary>
        private string sIDescription;

        /// <summary>
        /// This will hold the cost of the Item
        /// </summary>
        private string sICost;

        /// <summary>
        /// This will hold the code of the Item
        /// </summary>
        private string sICode;

        public clsItem(string sCode = "", string sDescription = "", string sCost = "")
        {
            try
            {
                sICode = sCode;
                sIDescription = sDescription;
                sICost = sCost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Public property to get and set sIDescription
        /// </summary>
        public string sItemDescription
        {
            get
            {
                return sIDescription;
            }
            set
            {
                sIDescription = value;
            }
        }

        /// <summary>
        /// Public property to get and set sICost
        /// </summary>
        public string sItemCost
        {
            get
            {
                return sICost;
            }
            set
            {
                sICost = value;
            }
        }

        /// <summary>
        /// Public property to get and set sICode
        /// </summary>
        public string sItemCode
        {
            get
            {
                return sICode;
            }
            set
            {
                sICode = value;
            }
        }

        /// <summary>
        /// Override the toString method to display the only itemDescription and itemCost
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return sICode + " " + sIDescription + " " + sICost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the code of an item.
        /// </summary>
        /// <returns></returns>
        public string getItemCode()
        {
            try
            {
                return sICode;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the description of an item.
        /// </summary>
        /// <returns></returns>
        public string getItemDescription()
        {
            try
            {
                return sItemDescription;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the cost of an item.
        /// </summary>
        /// <returns></returns>
        public string getItemCost()
        {
            try
            {
                return sICost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

    }
}
