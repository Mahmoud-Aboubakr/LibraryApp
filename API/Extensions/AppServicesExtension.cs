using Application.Interfaces;
using Application.Validators;
using Infrastructure.AppServices;
using Infrastructure.AppServicesContracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;

namespace API.Extensions
{
    public static class AppServicesExtension
    {
        public static IServiceCollection AppServices(this IServiceCollection services , IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<LibraryDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ISearchBookDataWithDetailService, SearchBookDataWithDetailService>();
            services.AddScoped<INumbersValidator, NumbersValidator>();
            services.AddScoped<IPhoneNumberValidator, PhoneNumberValidator>();
            services.AddTransient<ISearchAuthorDataService, SearchAuthorDataService>();
            services.AddTransient<ISearchCustomerService, SearchCustomerService>();
            services.AddTransient<ISearchBannedCustomerService, SearchBannedCustomerservice>();
            services.AddTransient<ISearchPublisherDataService, SearchPublisherDataService>();
            services.AddTransient<IBorrowServices, BorrowServices>();
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<IAttendenceServices, AttendenceServices>();
            services.AddTransient<ISearchPayrollDataWithDetailService, SearchPayrollDataWithDetailService>();
            services.AddTransient<IVacationServices, VacationServices>();
            services.AddTransient<IReturnedOrderServices,  ReturnedOrderServices>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
