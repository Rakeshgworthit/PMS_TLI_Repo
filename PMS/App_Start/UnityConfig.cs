using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using PMS.Repository.Infrastructure;
using PMS.Repository.DataService;

namespace PMS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<PMS.Controllers.AccountController>(new InjectionConstructor());
            container.RegisterType<PMS.Controllers.ManageController>(new InjectionConstructor());

            container.RegisterType<IuserRepository, UserRepository>();
            container.RegisterType<ICustomerRepositor, CustomerService>();
            container.RegisterType<ISupplierRepositor, SupplierService>();
            container.RegisterType<IProject, ProjectService>();
            container.RegisterType<IProjectAdditionRepository, ProjectAdditionService>();
            container.RegisterType<IPaymentsRepositor, PaymentsService>();
            container.RegisterType<IReceiptsRepositor, ReceiptsService>();
            container.RegisterType<IPaymentDetailRepository, PaymentDetailService>();

            container.RegisterType<ICompanyRepositor, CompanyService>();
            container.RegisterType<IBanksRepositor, BanksService>();
            container.RegisterType<IBranchesRepositor, BranchesService>();
            container.RegisterType<ISalesmenRepositor, SalesmenService>();
            container.RegisterType<ILoanRepositor, LoanService>();

            container.RegisterType<IInvoice, InvoiceService>();
            container.RegisterType<IExpense, ExpenseService>();
            
            container.RegisterType<IReceiptDetailRepositor, ReceiptDetailService>();
            container.RegisterType<IExpenseCategoryRepository, ExpenseCategoryService>();

            container.RegisterType<ISupplierInvoiceRepository, SupplierInvoiceService>();
            container.RegisterType<ILoanDetailRepository, LoanDetailService>();
            container.RegisterType<IDesignerInvoice, DesignerInvoiceService>();
            container.RegisterType<IDesignerReceiptsRepository, DesignerReceiptsService>();
            container.RegisterType<IDesignerReceiptDetailRepository, DesignerReceiptDetailService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}