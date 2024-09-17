using API.Data.Repositories;
using API.Data.Repositories.Interface;
using API.Services;
using API.Services.Interface;

namespace API.Integrations
{
    public static class Injector
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPdfService, PdfService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
        }
    }
}
