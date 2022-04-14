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

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("UsageExample", new MVCGridBuilder<PMS.Database.user_detail>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithPaging(true, 1)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("UniqueColumnName")
                        .WithHeaderText("Any Header")
                        .WithValueExpression(i => i.address1); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("UrlExample")
                        .WithHeaderText("Edit")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("detail", "demo", new { id = i.user_id }));
                })
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    int totalRecords;
                    // var repo = DependencyResolver.Current.GetService<IuserRepository>();
                    IuserRepository repo = new UserRepository();
        //IuserRepository _UserRepo;
        // var items = _UserRepo.SearchUser();
        string sortColumn = options.GetSortColumnData<string>();
                //var items = repo.SearchUser(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                //sortColumn, options.SortDirection == SortDirection.Dsc);
                var items = repo.SearchUser();

                    return new QueryResult<PMS.Database.user_detail>()
                    {
                        Items = items,
                        TotalRecords = items.Count() // if paging is enabled, return the total number of records of all pages
                    };

                })
            );

            /*
            MVCGridDefinitionTable.Add("UsageExample", new MVCGridBuilder<YourModelItem>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("UniqueColumnName")
                        .WithHeaderText("Any Header")
                        .WithValueExpression(i => i.YourProperty); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("UrlExample")
                        .WithHeaderText("Edit")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("detail", "demo", new { id = i.Id }));
                })
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>

                    return new QueryResult<YourModelItem>()
                    {
                        Items = new List<YourModelItem>(),
                        TotalRecords = 0 // if paging is enabled, return the total number of records of all pages
                    };

                })
            );
            */



            MVCGridDefinitionTable.Add("Customer", new MVCGridBuilder<PMS.Database.SSP_Customer_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 2)
               .AddColumns(cols =>
               {
                   // Add your columns here
                   cols.Add("customer_name").WithHeaderText("Name").WithCellCssClassExpression(p => "col-sm-4")
                   .WithSorting(true)
                   .WithValueExpression(p => p.customer_name);                  
                    cols.Add().WithColumnName("Email")
                       .WithCellCssClassExpression(p => "col-sm-4")
                       .WithHeaderText("Email")
                       .WithSorting(true)
                       .WithValueExpression(i => i.email);
                   cols.Add().WithColumnName("gst_no")
                       .WithCellCssClassExpression(p => "col-sm-3")
                       .WithHeaderText("Gst No")
                       .WithValueExpression(i => i.gst_no);

                   cols.Add("ViewLink").WithSorting(false)
                    .WithHeaderText("")
                    .WithHtmlEncoding(false)
                    .WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Customer/_LoadCustomer','id',{Value}); class='btn btn-primary'>Edit</a>");
               })
               .WithSorting(true, "customer_name")
               .WithRetrieveDataMethod((context) =>
               {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                   int totalRecords=0;
                    // var repo = DependencyResolver.Current.GetService<IuserRepository>();
                    ICustomerRepositor repo = new CustomerService();
                    //IuserRepository _UserRepo;
                    // var items = _UserRepo.SearchUser();
                    string sortColumn = options.GetSortColumnData<string>();
                   //var items = repo.SearchUser(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(),
                   //sortColumn, options.SortDirection == SortDirection.Dsc);
                   var items = repo.SearchCustomer(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if(items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<PMS.Database.SSP_Customer_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords // if paging is enabled, return the total number of records of all pages
                   };

               })
           );

            /* User Grid Start */

            MVCGridDefinitionTable.Add("Users", new MVCGridBuilder<PMS.Database.SSP_Users_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 2)
               .AddColumns(cols =>
               {                   
                   cols.Add("uname").WithHeaderText("Name").WithCellCssClassExpression(p => "col-sm-4")
                   .WithSorting(true)
                   .WithValueExpression(p => p.uname);

                   cols.Add("Email").WithColumnName("Email").WithCellCssClassExpression(p => "col-sm-4")                     
                      .WithSorting(true)
                      .WithValueExpression(i => i.email);

                   cols.Add("Country").WithColumnName("Country")
                       .WithCellCssClassExpression(p => "col-sm-3")
                       .WithValueExpression(i => i.Country);

                   cols.Add("ViewLink").WithSorting(false)
                    .WithHeaderText("")
                    .WithHtmlEncoding(false)
                    .WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a href='/Account/Edit?Id={Value}' class='btn btn-primary'>Edit</a>");
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
                       items = objDB.SSP_Users(HttpContext.Current.Request["uname"], HttpContext.Current.Request["email"], active,Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString()).ToList();
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
               .WithPaging(true, 2)
               .AddColumns(cols =>
               {
                   // Add your columns here
                   cols.Add("supplier_name").WithHeaderText("Name")
                  .WithSorting(true)
                       .WithValueExpression(p => p.supplier_name);
                   cols.Add().WithColumnName("Email")
                      .WithHeaderText("Email")
                      .WithSorting(true)
                      .WithValueExpression(i => i.email);
                   cols.Add().WithColumnName("Phone")
                    .WithHeaderText("Phone")
                    .WithSorting(true)
                    .WithValueExpression(i => i.phone);
                   cols.Add().WithColumnName("gst_no")
                       .WithHeaderText("Gst No")
                       .WithValueExpression(i => i.gst_no);
                   cols.Add("ViewLink").WithSorting(false)
                   .WithHeaderText("")
                   .WithHtmlEncoding(false)
                   .WithCellCssClassExpression(p => "col-sm-1")
                   .WithValueExpression((p, c) => p.id.ToString())
                   .WithValueTemplate("<a onclick=openModelpop('/Supplier/LoadAddEdit','id',{Value}); class='btn btn-primary'>Edit</a>");

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
                       TotalRecords = totalRecords // if paging is enabled, return the total number of records of all pages
                   };



               })
               );
            // ******************* End Supplier Grid *********************************************//

        }
    }
}