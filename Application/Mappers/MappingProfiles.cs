using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Author, ReadAuthorDto>();

            CreateMap<Author, CreateAuthorDto>();

            CreateMap<Attendence, AttendanceDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<BannedCustomer, BannedCustomerDto>()
               .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName))
               .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<Book, ReadBookDto>()
               .ForMember(dest => dest.AuthorName, option => option.MapFrom(src => src.Author.AuthorName))
               .ForMember(dest => dest.PublisherName, option => option.MapFrom(src => src.Publisher.PublisherName))
               .ReverseMap();


            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<UpdateBookDto, Book>().ReverseMap();

            CreateMap<Borrow, BorrowDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>();

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Order, OrderDto>();

            CreateMap<OrderBooks, OrderBooksDto>()
              .ForMember(dest => dest.BookTitle, option => option.MapFrom(src => src.Books.BookTitle))
              .ForMember(dest => dest.Price, option => option.MapFrom(src => src.Books.Price));

            CreateMap<Payroll, PayrollDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<Publisher, PublisherDto>();

            CreateMap<Vacation, VacationDto>()
            .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
        }
    }
}
