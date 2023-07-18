using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Author, ReadAuthorDto>();
            CreateMap<ReadAuthorDto, Author>();

            CreateMap<Author, CreateAuthorDto>();
            CreateMap<CreateAuthorDto, Author>();

            CreateMap<Attendence, ReadAttendenceDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<ReadAttendenceDetailsDto, Attendence>();
            CreateMap<ReadAttendanceDto, Attendence>();
            CreateMap<Attendence, ReadAttendanceDto>();
            CreateMap<Attendence, CreateAttendenceDto>();
            CreateMap<CreateAttendenceDto, Attendence>();

            

            CreateMap<BannedCustomer, ReadBannedCustomerDto>()
               .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName))
               .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadBannedCustomerDto, BannedCustomer>();

            CreateMap<CreateBannedCustomerDto, BannedCustomer>().ReverseMap();


            CreateMap<Book, ReadBookDto>()
               .ForMember(dest => dest.AuthorName, option => option.MapFrom(src => src.Author.AuthorName))
               .ForMember(dest => dest.PublisherName, option => option.MapFrom(src => src.Publisher.PublisherName))
               .ReverseMap();


            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<UpdateBookDto, Book>().ReverseMap();
            CreateMap<ReadBookDto, Book>();

            CreateMap<Borrow, BorrowDto>().ReverseMap();

            CreateMap<Customer, ReadCustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>().ReverseMap();

            CreateMap<Employee, ReadEmployeeDto>();
            CreateMap<ReadEmployeeDto, Employee>();

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();

            CreateMap<Order, OrderDto>();

            CreateMap<OrderBooks, OrderBooksDto>()
              .ForMember(dest => dest.BookTitle, option => option.MapFrom(src => src.Books.BookTitle))
              .ForMember(dest => dest.Price, option => option.MapFrom(src => src.Books.Price));

            CreateMap<Payroll, ReadPayrollDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<ReadPayrollDetailsDto, Payroll>();
            CreateMap<ReadPayrollDto, Payroll>();
            CreateMap<Payroll, ReadPayrollDto>();
            CreateMap<Payroll, CreatePayrollDto>();
            CreateMap<CreatePayrollDto, Payroll>();
            CreateMap<UpdatePayrollDto, Payroll>();

            CreateMap<Publisher, PublisherDto>();

            
            CreateMap<Vacation, ReadVacationDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<ReadVacationDetailsDto, Vacation>();
            CreateMap<ReadVacationDto, Vacation>();
            CreateMap<Vacation, ReadVacationDto>();
            CreateMap<Vacation, CreateVacationDto>();
            CreateMap<CreateVacationDto, Vacation>();

        }
    }
}
