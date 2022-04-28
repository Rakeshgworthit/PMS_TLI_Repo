[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PMS.MVCGridConfig), "RegisterGrids")]

namespace PMS
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using Models;
    using PMS.Repository.Infrastructure;
    using PMS.Repository.DataService;
    using Microsoft.AspNet.Identity;

    public static class MVCGridConfig
    {
        public static void RegisterGrids()
        {

            MVCGridDefinitionTable.Add("Customer", new MVCGridBuilder<PMS.Database.SSP_Customer_Result>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithPaging(true, 20)
                .WithPageParameterNames("customersearch")
                .AddColumns(cols =>
                {
                    cols.Add("name1").WithHeaderText("Name1").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(p => p.name1);
                    cols.Add().WithColumnName("nric1").WithCellCssClassExpression(p => "col-sm-2").WithHeaderText("NRIC").WithSorting(true).WithValueExpression(i => i.nric1);
                    cols.Add("block_no").WithHeaderText("Block no").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(p => p.block_no);
                    cols.Add("job_site").WithHeaderText("Job Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(p => p.job_site);
                    cols.Add().WithColumnName("gst_no").WithCellCssClassExpression(p => "col-sm-2").WithHeaderText("Gst No").WithValueExpression(i => i.gst_no);
                    cols.Add().WithColumnName("isactive").WithHeaderText("IsActive").WithValueExpression(i => i.isactive.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                     .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Customer/_LoadCustomer','id',{Value}); class=btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Customer/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>");

                })
                .WithSorting(true, "name1")
                .WithRetrieveDataMethod((context) =>
                {

                    var options = context.QueryOptions;
                    int totalRecords = 0;
                    ICustomerRepositor repo = new CustomerService();
                    string sortColumn = options.GetSortColumnData<string>();
                    string vCustomerSearch = "";
                    if (options.GetPageParameterString("customersearch") != null)
                    {
                        vCustomerSearch = Convert.ToString(options.GetPageParameterString("customersearch"));
                    }


                    var items = repo.SearchCustomer(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), vCustomerSearch.ToString());
                    if (items != null && items.Count > 0)
                    {
                        totalRecords = Convert.ToInt32(items[0].TotalRecords);
                    }
                    return new QueryResult<PMS.Database.SSP_Customer_Result>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
            );

            /* User Grid Start */

            MVCGridDefinitionTable.Add("Users", new MVCGridBuilder<PMS.Database.SSP_Users_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("uname").WithHeaderText("Name").WithCellCssClassExpression(p => "col-sm-4").WithSorting(true).WithValueExpression(p => p.uname);
                   cols.Add("Email").WithColumnName("Email").WithCellCssClassExpression(p => "col-sm-4").WithSorting(true).WithValueExpression(i => i.email);
                   cols.Add("Country").WithColumnName("Country").WithCellCssClassExpression(p => "col-sm-3").WithValueExpression(i => i.Country);
                   cols.Add().WithColumnName("is_active").WithHeaderText("IsActive").WithValueExpression(i => i.is_active.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a href='/Account/Edit?Id={Value}' class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>");
               })
               .WithSorting(true, "uname")
               .WithFiltering(true)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   string sortColumn = options.GetSortColumnData<string>();
                   bool? active = null;
                   string fa = HttpContext.Current.Request["is_active"];
                   if (!String.IsNullOrWhiteSpace(fa))
                   {
                       active = (String.Compare(fa, "active", true) == 0);
                   }
                   List<PMS.Database.SSP_Users_Result> items = new List<Database.SSP_Users_Result>();
                   using (PMS.Database.PMSEntities objDB = new Database.PMSEntities())
                   {
                       items = objDB.SSP_Users(HttpContext.Current.Request["uname"], HttpContext.Current.Request["email"], active, Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString()).ToList();
                   }
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<PMS.Database.SSP_Users_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            /*  User Grid End   */

            //********************Start Supplier Grid **********************************************//

            MVCGridDefinitionTable.Add("Supplier", new MVCGridBuilder<PMS.Database.SSP_Supplier_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("supplier_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.supplier_name);
                   cols.Add().WithColumnName("Email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email);
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone);
                   cols.Add().WithColumnName("gst_no").WithHeaderText("Gst No").WithValueExpression(i => i.gst_no);
                   cols.Add().WithColumnName("rebate").WithHeaderText("Rebate %").WithValueExpression(i => i.rebate_per.ToString());
                   cols.Add().WithColumnName("isactive").WithHeaderText("IsActive").WithValueExpression(i => i.isactive.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2")
                   .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Supplier/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Supplier/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>");
               })
               .WithSorting(true, "supplier_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ISupplierRepositor repo = new SupplierService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchSupplier(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Supplier_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Supplier Grid **********************************************//

            // ******************* Start My Project Grid   ***********************************************//
            MVCGridDefinitionTable.Add("MyProjects", new MVCGridBuilder<Database.SSP_Projects_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "fromdate", "todate", "ProjectStatusId", "ProjectSalesmenId", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("project_number").WithHeaderText("Project Number").WithCellCssClassExpression(p => "col-sm-2")
                   .WithSorting(true).WithValueExpression(p => p.project_number);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("contract_date").WithHeaderText("Contract Date").WithCellCssClassExpression(p => "col-sm-2")
                       .WithSorting(true).WithValueExpression(i => i.contract_date);
                   cols.Add("name1").WithHeaderText("Customer").WithCellCssClassExpression(p => "col-sm-1")
                      .WithSorting(true).WithValueExpression(i => i.name1);
                   cols.Add("salesmen_name").WithHeaderText("Salesmen").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.salesmen_name);
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                     .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);


                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a title='Edit' onclick=openModelpop('/Projects/_LoadProject','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a><a title='Cost' class='btn-xs' href='/Report/ProjectCostingReport?ProjectId={Value}' ><span class='glyphicon glyphicon-usd'></span></a><a title='Delete' onclick=DeleteConfirm('/Projects/DeleteProjectById','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-trash'></span></a><a onclick=UploadFile({Value}); href='#' class='btn-xs' title='Contract Documents'><i class='fa fa-upload' aria-hidden='true'></i></a>");


               })
               .WithSorting(true, "contract_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IProject repo = new ProjectService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   DateTime fromdate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("fromdate"));
                   DateTime todate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("todate"));
                   Int32 projStatus = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectStatusId"));
                   Int32 salesMenId = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId"));
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.GetMyProjects(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), fromdate, todate, projStatus, salesMenId, globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Projects_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );


            MVCGridDefinitionTable.Add("MyProjectsForSalesman", new MVCGridBuilder<Database.SSP_Projects_Result>()
              .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              .WithPaging(true, 20)
              .WithPageParameterNames("hdnUID", "brId", "fromdate", "todate", "ProjectStatusId", "ProjectSalesmenId", "searchText")
              .AddColumns(cols =>
              {
                  cols.Add("project_number").WithHeaderText("Project Number").WithCellCssClassExpression(p => "col-sm-2")
                  .WithSorting(true).WithValueExpression(p => p.project_number);
                  cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2")
                     .WithSorting(true).WithValueExpression(i => i.project_name);
                  cols.Add("contract_date").WithHeaderText("Contract Date").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.contract_date);
                  cols.Add("name1").WithHeaderText("Customer").WithCellCssClassExpression(p => "col-sm-1")
                     .WithSorting(true).WithValueExpression(i => i.name1);
                  cols.Add("salesmen_name").WithHeaderText("Salesmen").WithCellCssClassExpression(p => "col-sm-2")
                     .WithSorting(true).WithValueExpression(i => i.salesmen_name);
                  cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                    .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);


                  cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a title='Cost' class='btn-xs' href='/Report/ProjectCostingReport?ProjectId={Value}' ><span class='glyphicon glyphicon-usd'></span></a><a  onclick=UploadFile({Value});  href='#' class='btn-xs' title='Contract Documents'><i class='fa fa-upload' aria-hidden='true'></i></a>");


              })
              .WithSorting(true, "contract_date")
              .WithRetrieveDataMethod((context) =>
              {
                  var options = context.QueryOptions;
                  int totalRecords = 0;
                  IProject repo = new ProjectService();
                  string sortColumn = options.GetSortColumnData<string>();
                  string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                  DateTime fromdate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("fromdate"));
                  DateTime todate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("todate"));
                  Int32 projStatus = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectStatusId"));
                  Int32 salesMenId = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId"));
                  string globalSearch = options.GetPageParameterString("searchText");
                  var items = repo.GetMyProjects(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), fromdate, todate, projStatus, salesMenId, globalSearch);
                  if (items != null && items.Count > 0)
                  {
                      totalRecords = Convert.ToInt32(items[0].TotalRecords);
                  }

                  return new QueryResult<Database.SSP_Projects_Result>()
                  {
                      Items = items,
                      TotalRecords = totalRecords
                  };
              })
          );
            //********************End My Project Grid **********************************************//

            // ******************* Start My Project Additions Grid   ***********************************************//
            MVCGridDefinitionTable.Add("MyProjectAdditions", new MVCGridBuilder<Database.SSP_Project_additions_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchProject", "ProjectSalesmenId", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("project_number").WithHeaderText("Project Number").WithCellCssClassExpression(p => "col-sm-1")
                   .WithSorting(true).WithValueExpression(p => p.project_number);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("contract_date").WithHeaderText("Contract Date").WithCellCssClassExpression(p => "col-sm-2")
                       .WithSorting(true).WithValueExpression(i => i.contract_date);
                   cols.Add("addition_date").WithHeaderText("Addition Date").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.addition_date);
                   cols.Add("total_amount").WithHeaderText("Total").WithCellCssClassExpression(p => "col-sm-1 text-right")
                      .WithSorting(true)//.WithValueExpression(i => i.total_amount.ToString());
                      .WithValueExpression(i => string.Format("${0:n}", i.total_amount));
                   cols.Add("record_type").WithHeaderText("Type").WithCellCssClassExpression(p => "col-sm-2")
                     .WithSorting(true).WithValueExpression(i => i.record_type);
                   //   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                   //.WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false)
                    .WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Projects/_LoadAdditions','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Projects/DeleteAdditionById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>");



               })
               .WithSorting(true, "contract_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IProjectAdditionRepository repo = new ProjectAdditionService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.GetMyProjects(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Project_additions_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End My Project Additions Grid **********************************************//


            //********************Start Company Grid **********************************************//

            MVCGridDefinitionTable.Add("Company", new MVCGridBuilder<PMS.Database.SSP_Company_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("company_name").WithHeaderText("Name").WithCellCssClassExpression(p => "col-sm-2")
                    .WithSorting(true).WithValueExpression(p => p.company_name);
                   cols.Add("email").WithHeaderText("Email").WithCellCssClassExpression(p => "col-sm-2")
                   .WithSorting(true).WithValueExpression(p => p.email);
                   cols.Add("phone").WithHeaderText("Phone").WithCellCssClassExpression(p => "col-sm-2")
                    .WithSorting(true).WithValueExpression(p => p.phone);
                   cols.Add("reg_no").WithHeaderText("Reg No").WithCellCssClassExpression(p => "col-sm-2")
                  .WithSorting(true).WithValueExpression(p => p.reg_no);
                   // cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   //.WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Admin/CompanyLoadAddEdit','id',{Value}); class='btn btn-primary btn-xs'>Edit</a>&nbsp;<a onclick=DeleteConfirm('/Admin/CompanyDeleteById','id',{Value}); class='btnDelete btn btn-primary btn-xs'>Delete</a>");
               })
               .WithSorting(true, "company_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ICompanyRepositor repo = new CompanyService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchCompany(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Company_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Company Grid **********************************************//


            //********************Start Branches Grid **********************************************//

            MVCGridDefinitionTable.Add("Branches", new MVCGridBuilder<PMS.Database.SSP_Branches_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("branch_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.branch_name).WithCellCssClassExpression(p => "col-sm-4");
                   //  cols.Add().WithColumnName("CompanyName").WithHeaderText("Company Name").WithSorting(true).WithValueExpression(i => i.company_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("gst_reg_no").WithHeaderText("Gst Reg No").WithValueExpression(i => i.gst_reg_no).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("isactive").WithHeaderText("IsActive").WithValueExpression(i => i.isactive.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Admin/BranchLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/BranchDeleteById','id',{Value}); class='btnDeletew btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>");

               })
               .WithSorting(true, "branch_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IBranchesRepositor repo = new BranchesService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchBranches(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Branches_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );


            MVCGridDefinitionTable.Add("BranchesView", new MVCGridBuilder<PMS.Database.SSP_Branches_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("branch_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("CompanyName").WithHeaderText("Company Name").WithSorting(true).WithValueExpression(i => i.company_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("gst_reg_no").WithHeaderText("Gst Reg No").WithValueExpression(i => i.gst_reg_no).WithCellCssClassExpression(p => "col-sm-2");

               })
               .WithSorting(true, "branch_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IBranchesRepositor repo = new BranchesService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchBranches(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Branches_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Branches Grid **********************************************//

            //********************Start Salesmen Grid **********************************************//

            MVCGridDefinitionTable.Add("Salesmen", new MVCGridBuilder<PMS.Database.SSP_Salesmen_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "Search_Text")
               .AddColumns(cols =>
               {
                   cols.Add("salesmen_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.salesmen_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add("saleman_commission").WithHeaderText("Commission").WithSorting(true).WithValueExpression(p => p.saleman_commission.ToString()).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("branch_name").WithHeaderText("Company Name").WithSorting(true).WithValueExpression(i => i.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add().WithColumnName("zip_code").WithHeaderText("Zip Code").WithValueExpression(i => i.zip_code).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add().WithColumnName("isactive").WithHeaderText("IsActive").WithValueExpression(i => i.isactive.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("")
                   .WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1 actions").WithValueExpression((p, c) => p.id.ToString())
                   .WithValueTemplate("<a onclick=openModelpop('/Admin/SalesmenLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/SalesmenDeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>");
               })
               .WithSorting(true, "salesmen_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ISalesmenRepositor repo = new SalesmenService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string SalesmenSearch = "";
                   if (options.GetPageParameterString("Search_Text") != null)
                   {
                       SalesmenSearch = Convert.ToString(options.GetPageParameterString("Search_Text"));
                   }
                   var items = repo.SearchSalesmen(Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), SalesmenSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Salesmen_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );

            MVCGridDefinitionTable.Add("SalesmenView", new MVCGridBuilder<PMS.Database.SSP_Salesmen_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "Search_Text")
               .AddColumns(cols =>
               {
                   cols.Add("salesmen_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.salesmen_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add("saleman_commission").WithHeaderText("Commission").WithSorting(true).WithValueExpression(p => p.saleman_commission.ToString()).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("branch_name").WithHeaderText("Company Name").WithSorting(true).WithValueExpression(i => i.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("zip_code").WithHeaderText("Zip Code").WithValueExpression(i => i.zip_code).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("")
                   .WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1 actions").WithValueExpression((p, c) => p.id.ToString())
                   .WithValueTemplate("<a onclick=openModelpop('/Admin/SalesmenLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>");

               })
               .WithSorting(true, "salesmen_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ISalesmenRepositor repo = new SalesmenService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string SalesmenSearch = "";
                   if (options.GetPageParameterString("Search_Text") != null)
                   {
                       SalesmenSearch = Convert.ToString(options.GetPageParameterString("Search_Text"));
                   }
                   var items = repo.SearchSalesmen(Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), SalesmenSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Salesmen_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Salesmen Grid **********************************************//

            //********************Start Banks Grid **********************************************//


            MVCGridDefinitionTable.Add("Banks", new MVCGridBuilder<PMS.Database.SSP_Banks_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("bank_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.bank_name);
                   cols.Add().WithColumnName("account_number").WithHeaderText("Account Number").WithSorting(true).WithValueExpression(i => i.account_number);
                   cols.Add().WithColumnName("branch_code").WithHeaderText("Branch Code").WithSorting(true).WithValueExpression(i => i.branch_code);
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone);
                   cols.Add().WithColumnName("branch_name").WithHeaderText("Company").WithSorting(true).WithValueExpression(i => i.branch_name);
                   cols.Add().WithColumnName("isactive").WithHeaderText("IsActive").WithValueExpression(i => i.isactive.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Admin/LoadAddEditBank','id',{Value}); class='btn-xs' title='Delete'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/DeleteBankById','id',{Value}); class='btnDelete btn-xs'><span class='glyphicon glyphicon-trash'></span></a>");
               })
               .WithSorting(true, "bank_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IBanksRepositor repo = new BanksService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchBanks(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Banks_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );

            MVCGridDefinitionTable.Add("BanksView", new MVCGridBuilder<PMS.Database.SSP_Banks_Result>()
              .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              .WithPaging(true, 20)
              .AddColumns(cols =>
              {
                  cols.Add("bank_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.bank_name);
                  cols.Add().WithColumnName("account_number").WithHeaderText("Account Number").WithSorting(true).WithValueExpression(i => i.account_number);
                  cols.Add().WithColumnName("branch_code").WithHeaderText("Branch Code").WithSorting(true).WithValueExpression(i => i.branch_code);
                  cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone);
                  cols.Add().WithColumnName("branch_name").WithHeaderText("Company").WithSorting(true).WithValueExpression(i => i.branch_name);
              })
              .WithSorting(true, "bank_name")
              .WithRetrieveDataMethod((context) =>
              {
                  var options = context.QueryOptions;
                  int totalRecords = 0;
                  IBanksRepositor repo = new BanksService();
                  string sortColumn = options.GetSortColumnData<string>();
                  var items = repo.SearchBanks(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                  if (items != null && items.Count > 0)
                  {
                      totalRecords = Convert.ToInt32(items[0].TotalRecords);
                  }
                  return new QueryResult<PMS.Database.SSP_Banks_Result>()
                  {
                      Items = items,
                      TotalRecords = totalRecords
                  };
              })
              );
            // ******************** End Banks Grid **********************************************//

            MVCGridDefinitionTable.Add("Receipts", new MVCGridBuilder<Database.SSP_Receipts_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchProject", "ProjectSalesmenId", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("receipt_detail").WithHeaderText("Invoice Detail").WithCellCssClassExpression(p => "col-sm-2").WithHtmlEncoding(false).WithSorting(false).WithValueExpression(p => p.receipt_detail.Replace(";", " </br></br> ").Replace("Address", "<strong>Address: </strong>").Replace("Invoice# ", "</br><strong>Invoice#: </strong>").Replace("Amount ", "</br><strong>Amount: </strong>"));
                   cols.Add("receipt_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.receipt_date);
                   cols.Add("billTo").WithHeaderText("Bill To").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.billTo);
                   //cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.project_name);
                   //cols.Add("mode_of_payment").WithHeaderText("Payment Mode").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => i.mode_of_payment);
                   cols.Add("salesmen_name").WithHeaderText("Salesmen").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.salesmen_name);
                   cols.Add("remarks").WithHeaderText("Notes").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.remarks);
                   //cols.Add("bank_name").WithHeaderText("Bank").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => i.bank_name);
                   cols.Add("cheque_details").WithHeaderText("Cheque Details").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.cheque_details);
                   //      cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                   //.WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2").WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Receipts/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Receipts/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Receipts/PrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");
               })
               .WithSorting(true, "receipt_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IReceiptsRepositor repo = new ReceiptsService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.SearchReceipts(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), globalSearch);
                   //var items = repo.SearchReceipts(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), 1,50000, sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), globalSearch);

                   if (items != null && items.Count > 0)
                   {
                       // Paging Code
                       //totalRecords = items.Count;
                       //int page_Index = Convert.ToInt32(options.PageIndex != null ? options.PageIndex : 0); // 
                       //int pageSize = 20;
                       //int skip = pageSize * (page_Index);
                       //if (skip < totalRecords)
                       //    items = items.Skip(skip).Take(pageSize).ToList();

                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Receipts_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            // ******************* Start My Payments Grid   ***********************************************//

            MVCGridDefinitionTable.Add("Payments", new MVCGridBuilder<Database.SSP_Payments_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchProject", "ProjectSalesmenId", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("payment_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.payment_date);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("supplier_name").WithHeaderText("Supplier Name").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.supplier_name);
                   cols.Add("cheque_number").WithHeaderText("Cheque Details").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.cheque_number);
                   cols.Add("remarks").WithHeaderText("Remarks").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.remarks);
                   cols.Add("PaymentDetailItem").WithHeaderText("Invoice").WithCellCssClassExpression(p => "col-sm-2")
             .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.PaymentDetailItem.Replace(";", "</br>").Replace("Invoice#", "<strong>Invoice# </strong>").Replace("Amount ", "<strong>Amount </strong>"));
                   //      cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2"
                   //.WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Payments/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a><a onclick=DeleteConfirm('/Payments/DeleteById','id',{Value});  class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Payments/PrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");
               })
               .WithSorting(true, "payment_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IPaymentsRepositor repo = new PaymentsService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   //var items = repo.SearchPayments(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), globalSearch);
                   var items = repo.SearchPayments(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), 1, 50000, sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       // Paging Code
                       totalRecords = items.Count;
                       int page_Index = Convert.ToInt32(options.PageIndex != null ? options.PageIndex : 0); // 
                       int pageSize = 20;
                       int skip = pageSize * (page_Index);
                       if (skip < totalRecords)
                           items = items.Skip(skip).Take(pageSize).ToList();

                       //totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Payments_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End My Payments Grid **********************************************//


            // ******************* Start Loan Grid   ***********************************************//

            MVCGridDefinitionTable.Add("LoanGrid", new MVCGridBuilder<Database.SSP_Loan_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo")
               .AddColumns(cols =>
               {
                   cols.Add("loan_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => i.loan_date);

                   cols.Add("person_name").WithHeaderText("Person Name").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.person_name);
                   cols.Add("rec_type").WithHeaderText("Type").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => Common.CommonFunction.GetRecordTypeById(i.rec_type));
                   cols.Add("mode_-of_payment").WithHeaderText("Payment Mode").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.mode_of_payment);

                   cols.Add("amount").WithHeaderText("Amount").WithCellCssClassExpression(p => "col-sm-1 text-right").WithSorting(false).WithValueExpression(i => i.amount.ToString("#,##0.00"));

                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
             .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.Id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Loan/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Loan/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Loan/PrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");
               })
               .WithSorting(true, "loan_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ILoanRepositor repo = new LoanService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   var items = repo.SearchLoans(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")));
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Loan_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End Loan Grid **********************************************//


            // ******************* Start Invoice Grid   ***********************************************//

            MVCGridDefinitionTable.Add("Invoices", new MVCGridBuilder<Database.SSP_Invoices_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchBill_to", "SearchSales_person", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("Invoice_number ").WithHeaderText("Invoice number ").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.Invoice_number);

                   cols.Add("invoice_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.Invoice_date);
                   cols.Add("customer_name").WithHeaderText("Bill To").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.customer_name);
                   cols.Add("address").WithHeaderText("Address ").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.address);
                   //cols.Add("company_name").WithHeaderText("Company").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.company_name);
                   cols.Add("salesmen_name").WithHeaderText("Salesmen").WithCellCssClassExpression(p => "col-sm-3").WithSorting(true).WithValueExpression(i => i.salesmen_name);
                   cols.Add("Total ").WithHeaderText("Amount").WithCellCssClassExpression(p => "col-sm-1 text-right").WithSorting(true).WithValueExpression(i => PMS.Common.CommonHelper.CalcualteInvoiceAmount(i.Total, i.tax, i.is_tax, i.tax_amount, true));
                   //cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-3")
                   //     .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.Id.ToString()).WithValueTemplate("<a onclick=ShowCustomerModel({Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a><a onclick=DeleteConfirm('/Invoice/DeleteById','id',{Value});  class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Invoice/PrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");
               })
               //.WithSorting(true, "invoice_date")
               .WithSorting(sorting: true, defaultSortColumn: "Invoice_number", defaultSortDirection: SortDirection.Asc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IInvoice repo = new InvoiceService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.SearchInvoice(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchBill_to")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchSales_person")), false, globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Invoices_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            //********************End Invoice Grid **********************************************//

            // ******************* Start Expense Grid   ***********************************************//
            MVCGridDefinitionTable.Add("Expenses", new MVCGridBuilder<Database.SSP_Expense_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "fromdate", "todate", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("ExpenseCategory").WithHeaderText("Expense Category").WithCellCssClassExpression(p => "col-sm-2")
                   .WithSorting(true).WithValueExpression(p => p.ExpenseCategory);
                   cols.Add("company_name").WithHeaderText("Company Name").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.company_name);
                   cols.Add("Expense_Date").WithHeaderText("Expense Date").WithCellCssClassExpression(p => "col-sm-2")
                       .WithSorting(true).WithValueExpression(i => i.Expense_Date);
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                     .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("Bank").WithHeaderText("Bank").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(false).WithValueExpression(i => i.BankName);
                   cols.Add("Cheque_Number").WithHeaderText("Chq Num.").WithCellCssClassExpression(p => "col-sm-1")
                      .WithSorting(false).WithValueExpression(i => i.ChequeNumber);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a title='Edit' onclick=openModelpop('/Expense/_LoadExpense','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a><a title='Delete' onclick=DeleteConfirm('/Expense/DeleteExpenseById','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Expense/PrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");


               })
               .WithSorting(true, "Expense_Date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IExpense repo = new ExpenseService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   DateTime fromdate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("fromdate"));
                   DateTime todate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("todate"));
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.GetMyExpenses(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), fromdate, todate, globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Expense_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End Expense Grid **********************************************//


            // ******************* Start Supplier Invoice Grid   ***********************************************//

            MVCGridDefinitionTable.Add("SupplierInvoices", new MVCGridBuilder<Database.SSP_SupplierInvoices_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchSupplier_id", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("invoice_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(p => p.Invoice_date);
                   cols.Add("supplier_name").WithHeaderText("Supplier").WithCellCssClassExpression(p => "col-sm-3").WithSorting(true).WithValueExpression(i => i.supplier_name);
                   //cols.Add("company_name").WithHeaderText("Company").WithCellCssClassExpression(p => "col-sm-3").WithSorting(true).WithValueExpression(i => i.company_name);
                   cols.Add("ActionLink").WithSorting(false).WithHeaderText("Approve").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.Id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Invoice/LoadApproval','id',{Value}); class='btn-xs' title='Approve'><span class='glyphicon glyphicon-pencil'></span></a>");
                   //cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-5")
                   //     .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("SupplierInvoiceItems").WithHeaderText("Invoice/Items").WithCellCssClassExpression(p => "col-sm-5")
                        .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.SupplierInvoiceItems.Replace(";", "</br>").Replace("Invoice#", "<strong>Invoice#: </strong>").Replace("Amount", "<strong>Amount: </strong>").Replace("Site# ", "<strong>Site: </strong>").Replace("SalesPerson#", "<strong>Sales Person: </strong>"));
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2")
                    .WithValueExpression((p, c) => p.Id.ToString()).WithValueTemplate("<a onclick=ShowCustomerModel({Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a><a onclick=DeleteConfirm('/Invoice/Delete_SupplierInvoiceById','id',{Value});  class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>"); //<a title='Print' onclick=openModelpop('/Invoice/PrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>
               })
               .WithSorting(true, "invoice_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   SupplierInvoiceService repo = new SupplierInvoiceService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   DateTime SearchFrom = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom"));
                   DateTime SearchTo = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo"));
                   var items = repo.SearchSupplierInvoice(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchSupplier_id")), globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_SupplierInvoices_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            //********************End Supplier Invoice Grid **********************************************//


            // ******************* Start ExpenseCategory Grid   ***********************************************//
            MVCGridDefinitionTable.Add("ExpenseCategories", new MVCGridBuilder<PMS.Database.SSP_EXCategory_Result>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithPaging(true, 20)
                .AddColumns(cols =>
                {
                    cols.Add("ExpenseCategory").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.ExpenseCategory).WithCellCssClassExpression(p => "col-sm-4");
                    cols.Add().WithColumnName("Parentname").WithHeaderText("Parent Name").WithValueExpression(i => i.Parentname).WithCellCssClassExpression(p => "col-sm-2");
                    cols.Add().WithColumnName("IsActive").WithHeaderText("Is Active").WithValueExpression(i => i.IsActive.ToString()).WithCellCssClassExpression(p => "col-sm-1");
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Admin/CategoryLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/CategoryDeleteById','id',{Value}); class='btnDeletew btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a>");

                })
                .WithSorting(true, "ExpenseCategory")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    int totalRecords = 0;
                    IExpenseCategoryRepository repo = new ExpenseCategoryService();
                    string sortColumn = options.GetSortColumnData<string>();
                    var items = repo.SearchExpenseCategory(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                    if (items != null && items.Count > 0)
                    {
                        totalRecords = Convert.ToInt32(items[0].TotalRecords);
                    }
                    return new QueryResult<PMS.Database.SSP_EXCategory_Result>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };
                })
                );

            //********************End ExpenseCategory Grid **********************************************//


            // ******************* Start Designer Invoice Grid   ***********************************************//

            MVCGridDefinitionTable.Add("DesignerInvoices", new MVCGridBuilder<Database.SSP_DesignerInvoices_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("Invoice_number ").WithHeaderText("Invoice number ").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.Invoice_number);
                   cols.Add("invoice_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.Invoice_date);
                   cols.Add("customer_name").WithHeaderText("Designer").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.designer_name);
                   cols.Add("Total ").WithHeaderText("Amount").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => PMS.Common.CommonHelper.CalcualteInvoiceAmount(i.Total, i.tax, i.is_tax, i.tax_amount, true));
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.Id.ToString()).WithValueTemplate("<a onclick=ShowCustomerModel({Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a><a onclick=DeleteConfirm('/Invoice/Delete_DesignerInvoiceById','id',{Value});  class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Invoice/DesignerInvoicePrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");
               })
               .WithSorting(sorting: true, defaultSortColumn: "Invoice_number", defaultSortDirection: SortDirection.Asc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IDesignerInvoice repo = new DesignerInvoiceService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.SearchInvoice(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), false, globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_DesignerInvoices_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            //********************End Designer Invoice Grid **********************************************//

            // ******************* Start Designer Payment (Receipt) Grid   ***********************************************//
            MVCGridDefinitionTable.Add("DesignerReceipts", new MVCGridBuilder<Database.SSP_DesignerReceipts_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "searchText")
               .AddColumns(cols =>
               {
                   cols.Add("receipt_detail").WithHeaderText("Invoice Detail").WithCellCssClassExpression(p => "col-sm-2").WithHtmlEncoding(false).WithSorting(false).WithValueExpression(p => p.receipt_detail.Replace(";", " </br></br> ").Replace("Invoice# ", "</br><strong>Invoice#: </strong>").Replace("Amount ", "</br><strong>Amount: </strong>"));
                   cols.Add("receipt_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.receipt_date);
                   //cols.Add("designer_name").WithHeaderText("Designer").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.designer_name);
                   cols.Add("remarks").WithHeaderText("Notes").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.remarks);
                   cols.Add("cheque_details").WithHeaderText("Cheque Details").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.cheque_details);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2").WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Receipts/LoadAddEditDesignerReceipt','id',{Value}); class='btn-xs' title='Edit'><span class='glyphicon glyphicon-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Receipts/Delete_DesignerReceptById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='glyphicon glyphicon-trash'></span></a><a title='Print' onclick=openModelpop('/Receipts/DesignerReceiptPrintPreview','id',{Value}); class='btn-xs'><span class='glyphicon glyphicon-print'></span></a>");
               })
               .WithSorting(true, "receipt_date")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IDesignerReceiptsRepository repo = new DesignerReceiptsService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   string globalSearch = options.GetPageParameterString("searchText");
                   var items = repo.SearchReceipts(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), globalSearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_DesignerReceipts_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            // ******************* End Designer Payment (Receipt) Grid   ***********************************************//

        }
    }
}