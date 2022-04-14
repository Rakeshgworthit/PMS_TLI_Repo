using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PMS.Common
{
    /// <summary>
    /// This class will use for to Initialize for some client or Application level static  enums... and so on 
    /// </summary>
    public static class CommonHelper
    {
        #region RecordType backup enum OLD
        //public enum RecordType
        //{
        //    [Description("Select Type")]
        //    SelectType = 0,
        //    [Description("Loan Payout")]
        //    LoanPayout = 1,
        //    [Description("File Closed")]
        //    FileClosed = 2,
        //    //[Description("Advance")]
        //    //Advance = 3,
        //}
        #endregion
        /// <summary>
        /// Record type loan only 
        /// </summary>
        public enum RecordType
        {
            [Description("Select Type")]
            SelectType = 0,
            [Description("Personal Loan")]
            PersonalLoan = 1,
            [Description("Loan Return")]
            LoanReturn = 2,
            [Description("Project Advance")]
            ProjectAdvance = 3,
            [Description("Project Closure")]
            ProjectClosure = 5,
            [Description("Others")]
            Others = 4,
            //[Description("Advance")]
            //Advance = 3,
        }
        public enum SalesmenStatus
        {
            [Description("ACTIVE")]
            ACTIVE = 0,
            [Description("INACTIVE")]
            INACTIVE = 1,
        }
        /// <summary>
        /// This enum is related to type of users which are concatenate with S_,U_,M_ {id}
        /// </summary>
        public enum UserType
        {
            [Description("Supplier")]
            S = 0,
            [Description("Users")]
            U = 1,
            [Description("Salesmen")]
            M = 2,
        }
        /// <summary>
        /// Get enum Description by enumName 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        /// <summary>
        /// Calculate Invoice amount with tax is applicable or not with global permission is needed or not
        /// </summary>
        /// <param name="total">DB Value</param>
        /// <param name="tax">Tax applicable</param>
        /// <param name="is_tax">flag</param>
        /// <param name="calcutedAmount">calculation needed or not</param>
        /// <param name="isDefaultDollerIncluded">is required amount with $ or not </param>
        /// <returns></returns>
        internal static string CalcualteInvoiceAmount(string total, decimal tax, bool? is_tax, decimal tax_amount, bool calcutedAmount = false, bool isDefaultDollerIncluded = true)
        {
            decimal TotalAmount = 0;
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(total))
            {
                list = total.Split(',').ToList();
            }
            decimal itemTotal = list.Count() > 0 ? list.Sum(o => Convert.ToDecimal(o)) : 0;
            decimal _tax = 0;

            //if (itemTotal > 0 && calcutedAmount)
            //{
            //    if (Convert.ToBoolean(is_tax))
            //    {
            //        _tax = Math.Round(Convert.ToDecimal(itemTotal * tax) / 100, 2); ;
            //    }
            //}
            //TotalAmount = calcutedAmount ? Math.Round((itemTotal + _tax), 2) : itemTotal;

            //if (itemTotal > 0 && calcutedAmount)
            if (calcutedAmount)
            {
                if (Convert.ToBoolean(is_tax))
                {
                    _tax = tax_amount;
                }
            }
            TotalAmount = calcutedAmount ? itemTotal + _tax : itemTotal;
            return isDefaultDollerIncluded ? string.Format("${0:n}", TotalAmount) : string.Format("{0}", TotalAmount);
        }

        internal static string StringFormat( decimal? tax, bool isDefaultDollerIncluded = true)
        {
            return isDefaultDollerIncluded ? string.Format("${0:n}", tax) : string.Format("{0}", tax);
        }
    }
}