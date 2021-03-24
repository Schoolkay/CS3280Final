using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    /// <summary>
    /// Creates a line item data holder.
    /// </summary>
    class clsLineItem
    {
        /// <summary>
        /// Constructor for clsLineItem, creates a default line item out of passed in
        /// values.
        /// </summary>
        /// <param name="sLineItemNum">The line item number.</param>
        /// <param name="sItemCode">The line item code.</param>
        /// <param name="sItemDescription">The line item description.</param>
        /// <param name="sItemCost">The line item cost.</param>
        public clsLineItem(string sLineItemNum, string sItemCode, string sItemDescription, string sItemCost)
        {
            Number = sLineItemNum;
            Code = sItemCode;
            Description = sItemDescription;
            Cost = sItemCost;
        }

        /// <summary>
        /// Gets the line number for the item.
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// Gets the line code for the item.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the line description for the item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the line cost for the item.
        /// </summary>
        public string Cost { get; }

        /// <summary>
        /// Override to properly display the line item as a string.
        /// </summary>
        /// <returns>string: The string of the line item.</returns>
        public override string ToString()
        {
            try
            {
                return Number + " " + Code + " " + Description + " " + Cost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
