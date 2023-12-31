﻿using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IIdentityService;
using Application.Interfaces.IValidators;
using Application.Validators;
using Infrastructure.AppServices;
using Infrastructure.IdentityServices;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped(typeof(ISpecification<>), typeof(BaseSpecification<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBookServices, BookServices>();
            services.AddScoped<INumbersValidator, NumbersValidator>();
            services.AddScoped<IPhoneNumberValidator, PhoneNumberValidator>();
            services.AddTransient<IAuthorServices, AuthorServices>();
            services.AddTransient<ICustomerServices, CustomerServices>();
            services.AddTransient<IBannedCustomerServices, BannedCustomerservices>();
            services.AddTransient<IPublisherServices, PublisherServices>();
            services.AddTransient<IBorrowServices, BorrowServices>();
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<IAttendenceServices, AttendenceServices>();
            services.AddTransient<IPayrollServices, PayrollServices>();
            services.AddTransient<IVacationServices, VacationServices>();
            services.AddTransient<IReturnedOrderServices,  ReturnedOrderServices>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
