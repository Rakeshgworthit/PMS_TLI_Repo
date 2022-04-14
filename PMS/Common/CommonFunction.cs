using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;
using PMS.Database;
using System.Data.SqlClient;

namespace PMS.Common
{
    public static class CommonFunction
    {
        public static string ConvertAmoun_to_Decimal(decimal? amount)
        {
            return String.Format("{0:0.00}", Convert.ToDecimal(amount));
        }
        public static string ConvertAmountoDecimal(decimal? amount)
        {
            return Convert.ToDecimal(amount).ToString("#,##0.00");
        }
        public static string GetCustomerData(Int32 customerid)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                customer objCust = objDB.customers.Where(o => o.id == customerid).SingleOrDefault();
                if (objCust != null)
                {
                    return Convert.ToString(objCust.block_no + " " + objCust.job_site);
                }

            }
            return "";
        }

        public static decimal GetSalemanCommision(Int32 salemanid)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                salesman objCust = objDB.salesmen.Where(o => o.id == salemanid).SingleOrDefault();
                if (objCust != null)
                {
                    return Convert.ToDecimal(objCust.saleman_commission);
                }

            }
            return 0;
        }


        public static List<SelectListItem> CountryList()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = objDB.countries.ToList();
                foreach (var items in sList)
                {
                    countryList.Add(new SelectListItem { Text = items.name, Value = items.id.ToString() });
                }
            }

            countryList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return countryList;
        }
        //public static List<SelectListItem> GetSalesmenAndUser()
        //{
        //    List<SelectListItem> objList = new List<SelectListItem>();
        //    using (PMSEntities objDB = new PMSEntities())
        //    {
        //        var sList = (from s in objDB.SSP_GetSalesmenAndUser(SessionManagement.SelectedBranchID)
        //                     select new SelectListItem
        //                     {
        //                         Text = s.Name.ToString(),
        //                         Value = s.id.ToString()
        //                     }
        //                    ).ToList();

        //        foreach (var items in sList)
        //        {
        //            objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
        //        }
        //    }
        //    objList = objList.OrderBy(o => o.Text).ToList();
        //    objList.Insert(0, new SelectListItem { Text = "Select Employee", Value = "" });
        //    return objList;
        //}
        //public static List<SelectListItem> GetRecordTypeList()
        //{
        //    List<SelectListItem> objList = new List<SelectListItem>();            
        //    objList.Insert(0, new SelectListItem { Text = "Select Type", Value = "0" });
        //    objList.Insert(1, new SelectListItem { Text = "Loan Payout", Value = "1" });
        //    objList.Insert(2, new SelectListItem { Text = "File Closed", Value = "2" });
        //    return objList;
        //}
        /// <summary>
        /// 16042019
        /// Advanced item will work for some dynamic controls functionality to add under ADD/EDIT loan Popup.
        /// Bind dropdown from static to enum
        /// </summary>
        /// <returns></returns> 
        public static List<SelectListItem> GetRecordTypeList()
        {

            List<SelectListItem> objList = new List<SelectListItem>();
            objList.AddRange(Enum.GetValues(typeof(CommonHelper.RecordType)).Cast<CommonHelper.RecordType>()
                .Select(v => new SelectListItem()
                {
                    Text = CommonHelper.GetEnumDescription(v),
                    Value = Convert.ToInt32(v).ToString()
                }).OrderBy(o => o.Value));
            // set 'other' type in bottom
            if (objList.Count > 5)
            {
                Move(4, 5, objList);
            }
            return objList;
        }


        public static void Move(int oldIndex, int newIndex, List<SelectListItem> objList)
        {
            var item = objList[oldIndex];
            objList.RemoveAt(oldIndex);
            objList.Insert(newIndex, item);
        }
        public static string GetRecordTypeById(int rectype)
        {
            switch (rectype)
            {
                case 1:
                    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.PersonalLoan);
                case 2:
                    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.LoanReturn);
                case 3:
                    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.ProjectAdvance);
                case 4:
                    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.Others);
                case 5:
                    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.ProjectClosure);
                default:
                    return string.Empty;
            }


            //if (rectype == 1)
            //{
            //    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.ProjectAdvance);
            //}
            //else if (rectype == 2)
            //{
            //    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.PersonalLoan);
            //}
            ////Code Will uncomment after Client Confirmation
            ////else if (rectype == 3)
            ////{
            ////    return CommonHelper.GetEnumDescription(CommonHelper.RecordType.Advanced); //"File Closed";
            ////}
            //else
            //{
            //    return "";
            //}

        }

        public static int GetUserType(string userType)
        {
            switch (userType)
            {
                case "U":
                    return Convert.ToInt32(CommonHelper.UserType.U);
                case "M":
                    return Convert.ToInt32(CommonHelper.UserType.M);
                case "S":
                    return Convert.ToInt32(CommonHelper.UserType.S);
                default:
                    return 0;
            }
        }

        public class SuccessMessage
        {
            public string Result { get; set; }
            public string Errormessage { get; set; }
            public string Id { get; set; }
            public decimal Amount { get; set; }
            public decimal gst_percentage { get; set; }
            public decimal gst_amount { get; set; }
            public decimal SubTotal { get; set; }

            public decimal addition_Amount { get; set; }
            public decimal omission_Amount { get; set; }
            public decimal TasksTotal_Amount { get; set; }

            public string Vo_Id { get; set; }

            public string Evo_Id { get; set; }
            public string Internal_Evo_Number { get; set; }
            public string evo_det_id { get; set; }
            public decimal Evo_Amount { get; set; }
            public decimal Evo_GstAmount { get; set; }
            public decimal Evo_total_Amount { get; set; }
            public decimal Evo_gstpercentage { get; set; }
            public string Evo_Date { get; set; }
            public string Evo_Status { get; set; }
            public string Property_Type { get; set; }

            public decimal TotalSelectedItemsAmount { get; set; }
            public decimal DiscountPercentage { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal GstAmount { get; set; }
            public decimal GrandTotal { get; set; }
            public string project_budget_id { get; set; }
        }

        public static List<SSP_UsersRoles_Result> UserRoleList(string uid)
        {
            List<SSP_UsersRoles_Result> countryList = new List<SSP_UsersRoles_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                countryList = objDB.SSP_UsersRoles(uid).ToList();
            }
            return countryList;
        }

        public static List<SelectListItem> BranchListByCode(string code)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.branches
                             join c in objDB.companies on p.company_id equals c.id
                             where p.branch_name.Contains(code)
                             && p.isactive == true
                             select new SelectListItem
                             {
                                 Text = p.branch_name, //+ ", " + c.company_name
                                 Value = p.id.ToString()
                             }
                             ).ToList();

                //objDB.branches.Where(o => o.branch_name.Contains(code) && o.isactive == true).ToList();
                foreach (var items in sList)
                {
                    branchList.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                }
            }

            return branchList;
        }

        public static List<SelectListItem> BranchList(bool valblnk = false)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.branches
                             join c in objDB.companies on p.company_id equals c.id
                             where p.isactive == true
                             select new SelectListItem
                             {
                                 Text = p.branch_name,//+ ", " + c.company_name
                                 Value = p.id.ToString()
                             }
                             ).ToList();

                //objDB.branches.Where(o => o.branch_name.Contains(code) && o.isactive == true).ToList();
                foreach (var items in sList)
                {
                    branchList.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                }
            }
            if (valblnk == true)
            {
                branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "" });
            }
            else
            {
                branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            }


            return branchList;
        }

        public static List<SelectListItem> Active_Salesmen(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {

                var sList = (from s in objDB.salesmen
                             where s.isactive == true
                             orderby s.salesmen_name
                             select new SelectListItem
                             {
                                 Text = s.salesmen_name,
                                 Value = s.id.ToString()
                             }
                            ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> InActive_Salesmen(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {

                var sList = (from s in objDB.salesmen
                             where s.isactive == false
                             orderby s.salesmen_name
                             select new SelectListItem
                             {
                                 Text = s.salesmen_name,
                                 Value = s.id.ToString()
                             }
                            ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> AllSalesmen()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {

                var sList = (from s in objDB.salesmen
                             orderby s.salesmen_name
                             select new SelectListItem
                             {
                                 Text = s.salesmen_name,
                                 Value = s.id.ToString()
                             }
                            ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> UserBranchList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.branches
                             join c in objDB.user_access_rights on p.id equals c.branch_id
                             where p.isactive == true && c.user_id == new Guid(uid)
                             && p.isactive == true
                             select new SelectListItem
                             {
                                 Text = p.branch_name,
                                 Value = p.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> UserBranchListNew(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.branches
                             join c in objDB.user_access_rights on p.id equals c.branch_id
                             join ca in objDB.companies on p.company_id equals ca.id
                             where p.isactive == true && c.user_id == new Guid(uid)
                             && p.isactive == true
                             select new SelectListItem
                             {
                                 Text = p.branch_name + ", " + ca.company_name,
                                 Value = p.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> UserProjectListWithID(string uid, Int32 projectid = 0)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.projects
                             where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                             && ((p.status_id == 3 && p.id == projectid && p.id != 0) || (p.status_id != 3))
                             select new SelectListItem
                             {
                                 Text = p.project_name + " - " + p.project_number,
                                 Value = p.id.ToString()
                             }
                            ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
            return objList;
        }

        public static List<SSP_GetInvoiceList_Result> SupplierInvoiceList(Int32 id)
        {
            List<SSP_GetInvoiceList_Result> objList = new List<SSP_GetInvoiceList_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_GetInvoiceList(id, SessionManagement.SelectedBranchID).ToList();
            }
            objList.Insert(0, new SSP_GetInvoiceList_Result { Id = 0, agreed_amt = 0, invoice_number = "Please Select" });
            return objList;
        }


        public static List<SelectListItem> UserProjectList(string uid, bool isNeedToConcatenate = true)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.projects
                                 //where p.isactive == true && (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID && p.status_id!=3
                             select new SelectListItem
                             {
                                 Text = isNeedToConcatenate ? p.project_name + " - " + p.project_number : p.project_name,
                                 Value = p.id.ToString()
                             }
                            ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
            return objList;
        }
        public static List<SelectListItem> LoanUserProjectList(string uid, bool isNeedToConcatenate = true)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.projects
                                 //where p.isactive == true && (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             where p.status_id != 3 && p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID 
                             select new SelectListItem
                             {
                                 Text = isNeedToConcatenate ? p.project_name + " - " + p.project_number : p.project_name,
                                 Value = p.id.ToString()
                             }
                            ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> ModeofPaymentList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = objDB.mode_of_payment.OrderBy(o => o.mode_of_payment1).ToList();
                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.mode_of_payment1, Value = items.id.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> DesignerList(string designer_name = "")
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var DesList = objDB.tbl_designer_invoice.OrderByDescending(x => x.Id).Select(o => o.Designer_name).Distinct().ToList();
                foreach (var items in DesList)
                {
                    if (!String.IsNullOrWhiteSpace(designer_name) && designer_name.Equals(items))
                    {
                        objList.Add(new SelectListItem { Text = items, Value = items, Selected = true });
                    }
                    else
                    {
                        objList.Add(new SelectListItem { Text = items, Value = items });
                    }
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Designer", Value = "" });
            return objList;
        }

        public static List<SelectListItem> CustomerList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = objDB.customers.Where(o => o.isactive == true).OrderBy(o => o.name1).ToList();
                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.name1, Value = items.id.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> BankList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = objDB.SSP_GetAllBank().ToList();

                //var sList = objDB.banks.Where(o => o.isactive == true && o.branch_id == SessionManagement.SelectedBranchID).ToList();
                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Bank_Name, Value = items.id.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Bank", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> ProjectStatusList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = objDB.project_status.ToList();
                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.project_status1, Value = items.id.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Status", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> UserSupplierList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from s in objDB.suppliers
                             where s.isactive == true //&& (s.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             orderby s.supplier_name
                             select new SelectListItem
                             {
                                 Text = s.supplier_name,
                                 Value = s.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
            return objList;
        }       

        public static List<SelectListItem> StatusList()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();

            statusList.Insert(0, new SelectListItem { Text = "Yes", Value = "true" });
            statusList.Insert(1, new SelectListItem { Text = "No", Value = "false" });
            return statusList;
        }

        public static List<SelectListItem> GSTRegistered(bool gstregistered = true)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (gstregistered)
            {
                result.Add(new SelectListItem { Text = "Yes", Value = "true", Selected = true });
                result.Add(new SelectListItem { Text = "No", Value = "false" });
            }
            else if (gstregistered == false)
            {
                result.Add(new SelectListItem { Text = "Yes", Value = "true" });
                result.Add(new SelectListItem { Text = "No", Value = "false", Selected = true });


            }

            return result;
        }

        public static List<SelectListItem> AdditionOmissionTypeList()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();

            statusList.Insert(0, new SelectListItem { Text = "Addition", Value = "1" });
            statusList.Insert(1, new SelectListItem { Text = "Omission", Value = "2" });
            return statusList;
        }

        public static List<SelectListItem> MonthList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem { Text = "January", Value = "1" });
            objList.Add(new SelectListItem { Text = "February", Value = "2" });
            objList.Add(new SelectListItem { Text = "March", Value = "3" });
            objList.Add(new SelectListItem { Text = "April", Value = "4" });
            objList.Add(new SelectListItem { Text = "May", Value = "5" });
            objList.Add(new SelectListItem { Text = "June", Value = "6" });
            objList.Add(new SelectListItem { Text = "July", Value = "7" });
            objList.Add(new SelectListItem { Text = "August", Value = "8" });
            objList.Add(new SelectListItem { Text = "September", Value = "9" });
            objList.Add(new SelectListItem { Text = "October", Value = "10" });
            objList.Add(new SelectListItem { Text = "November", Value = "11" });
            objList.Add(new SelectListItem { Text = "December", Value = "12" });

            return objList;
        }

        public static List<SelectListItem> YearList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            for (int i = 2016; i <= DateTime.Now.Year + 1; i++)
            {
                objList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return objList;
        }
        public static List<SelectListItem> CompanyList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from s in objDB.companies
                             where s.isactive == true
                             orderby s.company_name
                             select new SelectListItem
                             {
                                 Text = s.company_name,
                                 Value = s.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Company", Value = "0" });
            return objList;
        }




        public static List<SelectListItem> UserProjectListByYear(string uid, Int32 year, Int32 salesmenId)
        {
            DateTime start = Convert.ToDateTime("01/01/" + year.ToString());
            DateTime endDate = start.AddYears(1);
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.projects
                             where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                             && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                             && p.contract_date >= start && p.contract_date < endDate
                             && (p.salesmen_id == salesmenId || salesmenId == 0)
                             select new SelectListItem
                             {
                                 Text = p.project_name + " - " + p.project_number,
                                 Value = p.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> AlphabetList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            for (char c = 'A'; c < 'Z'; c++)
            {
                objList.Add(new SelectListItem { Text = c.ToString(), Value = c.ToString() });
            }
            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            objList.Insert(1, new SelectListItem { Text = "0-9", Value = "1" });
            return objList;
        }


        public static List<SelectListItem> PaymentTermsList()
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.tbl_payment_terms
                             select new SelectListItem
                             {
                                 Text = p.payment_term,
                                 Value = p.Id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    _List.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                }
            }

            return _List;
        }

        public static List<SelectListItem> ContractTypeList()
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.tbl_contract_types
                             select new SelectListItem
                             {
                                 Text = p.contract_type,
                                 Value = p.Id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    _List.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                }
            }

            return _List;
        }


        public static List<SelectListItem> SalesmenCustomerList(Int32 salesmen_id)
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from p in objDB.projects
                             join c in objDB.customers on p.customer_id equals c.id
                             where p.salesmen_id == salesmen_id && c.isactive == true 
                             select new SelectListItem
                             {
                                 Text = c.name1,
                                 Value = c.id.ToString()
                             }
                             ).Distinct().ToList();

                foreach (var items in sList)
                {
                    _List.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                }
            }
            _List.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return _List;
        }

        public static List<SelectListItem> ExpenseCategoryList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from s in objDB.ExpenseCategories
                             where s.Parent == 0
                             orderby s.ExpenseCategory1
                             select new SelectListItem
                             {
                                 Text = s.ExpenseCategory1,
                                 Value = s.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return objList;
        }

        public static List<SelectListItem> ExpenseItemist(int categoryid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            using (PMSEntities objDB = new PMSEntities())
            {
                List<SelectListItem> sList = new List<SelectListItem>();
                if (categoryid > 0)
                {
                    sList = (from s in objDB.ExpenseCategories
                             where s.Parent == categoryid
                             && s.IsActive == true
                             orderby s.ExpenseCategory1
                             select new SelectListItem
                             {
                                 Text = s.ExpenseCategory1,
                                 Value = s.id.ToString()
                             }
                                ).ToList();
                }
                else
                {
                    sList = (from s in objDB.ExpenseCategories
                             where s.Parent > 0
                             && s.IsActive == true
                             orderby s.ExpenseCategory1
                             select new SelectListItem
                             {
                                 Text = s.ExpenseCategory1,
                                 Value = s.id.ToString()
                             }
                                ).ToList();
                }
                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                 }
            }

            objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            return objList;
        }

        #region 
        public static Boolean MergeObjects(Object originalObj, Object modifiedObj, Boolean updateNullValues = false)
        {
            try
            {
                // Uncomment in case objects of different type are not allowed to be merged
                //if (originalObj.GetType() != modifiedObj.GetType()) return false;

                PropertyInfo[] propertyInfos = originalObj.GetType().GetProperties();
                Object originalVal;
                Object modifiedVal;

                //loop through each property of the originalObj
                foreach (PropertyInfo objProperty in propertyInfos)
                {
                    //retrieve the value of the current property
                    originalVal = originalObj.GetType().GetProperty(objProperty.Name).GetValue(originalObj, null);

                    //check if a similar property exists in the modifiedObj
                    if (modifiedObj.GetType().GetProperty(objProperty.Name) != null &&
                        Convert.ToString(objProperty.Name) != "EntityState" &&
                        Convert.ToString(objProperty.Name) != "EntityKey")
                    {
                        //retrieve the modified value of the current property from the modifiedVal
                        modifiedVal = modifiedObj.GetType().GetProperty(objProperty.Name).GetValue(modifiedObj, null);

                        //update the origional object if the origional and the modified value are different and the modified value is not null
                        if ((Convert.ToString(originalVal) != Convert.ToString(modifiedVal)) && (modifiedVal != null || updateNullValues == true))
                            originalObj.GetType().GetProperty(objProperty.Name).SetValue(originalObj, modifiedVal, null);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static Boolean MergeObjects(Object originalObj, Object modifiedObj, String[] ignoreProperties)
        {
            try
            {
                // Uncomment in case objects of different type are not allowed to be merged
                //if (originalObj.GetType() != modifiedObj.GetType()) return false;

                PropertyInfo[] propertyInfos = originalObj.GetType().GetProperties();
                Object originalVal;
                Object modifiedVal;

                //loop through each property of the originalObj
                foreach (PropertyInfo objProperty in propertyInfos)
                {
                    //retrieve the value of the current property
                    originalVal = originalObj.GetType().GetProperty(objProperty.Name).GetValue(originalObj, null);

                    //check if a similar property exists in the modifiedObj
                    if (modifiedObj.GetType().GetProperty(objProperty.Name) != null)
                    {
                        //retrieve the modified value of the current property from the modifiedVal
                        modifiedVal = modifiedObj.GetType().GetProperty(objProperty.Name).GetValue(modifiedObj, null);

                        //check if the origional and the modified value are different and the modified value is not null
                        if ((Convert.ToString(originalVal) != Convert.ToString(modifiedVal)) && modifiedVal != null)
                        {
                            //update the origional object if the current property is not specified to be ignored
                            if (!ignoreProperties.Contains(objProperty.Name))
                                originalObj.GetType().GetProperty(objProperty.Name).SetValue(originalObj, modifiedVal, null);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SelectListItem> ActiveInactive()
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            _List.Insert(0, new SelectListItem { Text = "Please Select", Value = "-1" });
            _List.Insert(1, new SelectListItem { Text = "Active", Value = "1" });
            _List.Insert(2, new SelectListItem { Text = "Inactive", Value = "0" });
            return _List;

        }

        // Get Designer
        //public static List<SelectListItem> GetDesignerList()
        //{
        //    List<SelectListItem> _List = new List<SelectListItem>();
        //    using (PMSEntities objDB = new PMSEntities())
        //    {
        //        var sList = (from p in objDB.salesmen
        //                     select new SelectListItem
        //                     {
        //                         Text = p.salesmen_name,
        //                         Value = p.id.ToString()
        //                     }
        //                     ).Distinct().ToList();

        //        foreach (var items in sList)
        //        {
        //            _List.Add(new SelectListItem { Text = items.Text, Value = items.Value });
        //        }
        //    }
        //    _List.Insert(0, new SelectListItem { Text = "Please Select Designer", Value = "0" });
        //    return _List;
        //}
        #endregion

        public static string SupplierMobile(long id)
        {
            string mobile = string.Empty;
            using (PMSEntities objDB = new PMSEntities())
            {

                mobile = objDB.Database.SqlQuery<string>("select mobile from supplier where id=" + id)
                               .FirstOrDefault();
            }
            return mobile;
        }

        public static List<SelectListItem> SalesmenListStatusWise(string status)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (status == "")
                {
                    var sList = (from s in objDB.salesmen
                                 //where s.branch_Id == SessionManagement.SelectedBranchID
                                 orderby s.salesmen_name
                                 select new SelectListItem
                                 {
                                     Text = s.salesmen_name,
                                     Value = s.id.ToString()
                                 }
                           ).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }
                else
                {
                    bool isactive = true;
                    if (status == "0")
                    {
                        isactive = false;
                    }
                    var sList = (from s in objDB.salesmen
                                 where s.isactive == isactive 
                                 //&& s.branch_Id == SessionManagement.SelectedBranchID
                                 orderby s.salesmen_name
                                 select new SelectListItem
                                 {
                                     Text = s.salesmen_name,
                                     Value = s.id.ToString()
                                 }
                           ).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }
            }

            objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
            return objList;
        }
        public static List<SelectListItem> SalesmenStatusList()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            statusList.Insert(1, new SelectListItem { Text = "Active", Value = "1" });
            statusList.Insert(2, new SelectListItem { Text = "Inactive", Value = "0" });
            return statusList;
        }
        public static List<SelectListItem> GetSalesmenAndUser(string status = null)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (status == "")
                {
                    status = null;
                    var sList = (from s in objDB.SSP_GetSalesmenAndUser(SessionManagement.SelectedBranchID, status)
                                 select new SelectListItem
                                 {
                                     Text = s.Name.ToString(),
                                     Value = s.id.ToString()
                                 }
                            ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }
                else
                {
                    var sList = (from s in objDB.SSP_GetSalesmenAndUser(SessionManagement.SelectedBranchID, status)
                                 select new SelectListItem
                                 {
                                     Text = s.Name.ToString(),
                                     Value = s.id.ToString()
                                 }
                           ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }
            }
            objList = objList.OrderBy(o => o.Text).ToList();
            objList.Insert(0, new SelectListItem { Text = "Select Employee", Value = "" });
            return objList;
        }

        public static bool IsSalesmanLogin(string UserId)
        {
            bool IsSalesmanLogin = false;
            try
            {              
                SqlConnection Conn = GetConnection();
                SqlCommand Cmd = new SqlCommand();

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return IsSalesmanLogin;

        }

        public static SqlConnection GetConnection()
        {
            SqlConnection Conn = new SqlConnection();
            Conn.Open();

            return Conn;
        }
        public static void CloseConnection(SqlConnection Conn)
        {
            Conn.Close();
        }
    }
}
