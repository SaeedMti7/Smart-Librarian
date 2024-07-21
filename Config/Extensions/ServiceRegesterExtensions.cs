using Smart_Librarian.Interfaces;
using Smart_Librarian.Repositories;
using Smart_Librarian.Services;

namespace Smart_Librarian.Config.Extensions
{
    public static class ServiceConfigurationExtension
    {
        public static IServiceCollection AddRegesterService(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();    
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITestService, TestService>();
            //  services.AddScoped<IGenericRepository, GenericRepository>();

            services.AddScoped<IDapperContext, DapperContext>();


            return services;
        }
    }
}
