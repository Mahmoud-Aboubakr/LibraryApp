using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Author
            CreateMap<Author, ReadAuthorDto>();
            CreateMap<ReadAuthorDto, Author>();
            CreateMap<Author, CreateAuthorDto>();
            CreateMap<CreateAuthorDto, Author>();
            #endregion


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

            CreateMap<Borrow, CreateBorrowDto>().ReverseMap();

            CreateMap<Borrow, ReadBorrowDto>()
               .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName))
               .ForMember(dest => dest.BookName, option => option.MapFrom(src => src.Book.BookTitle));
            CreateMap<ReadBorrowDto, Borrow>();


            CreateMap<Customer, ReadCustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>().ReverseMap();

            CreateMap<Employee, ReadEmployeeDto>();
            CreateMap<ReadEmployeeDto, Employee>();

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();

            CreateMap<Order, ReadOrderDto>()
                .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName));
            CreateMap<ReadOrderDto, Order>();

            CreateMap<BookOrderDetails, ReadBookOrderDetailsDto>()
              .ForMember(dest => dest.BookTitle, option => option.MapFrom(src => src.Book.BookTitle))
              .ForMember(dest => dest.Price, option => option.MapFrom(src => src.Book.Price))
              .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Order.Customer.CustomerName));
            
            CreateMap<ReadBookOrderDetailsDto, BookOrderDetails>();
            CreateMap<BookOrderDetails, CreateBookOrderDetailsDto>().ReverseMap();

            CreateMap<Payroll, ReadPayrollDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));

            CreateMap<Publisher, ReadPublisherDto>().ReverseMap();
            CreateMap<CreatePublisherDto, Publisher>();
            CreateMap<ReadPayrollDetailsDto, Payroll>();
            CreateMap<ReadPayrollDto, Payroll>();
            CreateMap<Payroll, ReadPayrollDto>();
            CreateMap<Payroll, CreatePayrollDto>();
            CreateMap<CreatePayrollDto, Payroll>();
            CreateMap<UpdatePayrollDto, Payroll>();
            
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
