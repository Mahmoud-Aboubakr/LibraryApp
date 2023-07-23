using Application.DTOs;
using Application.DTOs.Attendance;
using Application.DTOs.Author;
using Application.DTOs.BannedCustomer;
using Application.DTOs.Book;
using Application.DTOs.BookOrderDetails;
using Application.DTOs.Borrow;
using Application.DTOs.Customer;
using Application.DTOs.Employee;
using Application.DTOs.Order;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Author
            CreateMap<Author, ReadAuthorDto>().ReverseMap();
            CreateMap<UpdateAuthorDto, Author>();
            CreateMap<CreateAuthorDto, Author>();
            #endregion

            #region Publisher
            CreateMap<Publisher, ReadPublisherDto>().ReverseMap();
            CreateMap<CreatePublisherDto, Publisher>();
            #endregion

            #region Book
            CreateMap<Book, ReadBookDto>()
               .ForMember(dest => dest.AuthorName, option => option.MapFrom(src => src.Author.AuthorName))
               .ForMember(dest => dest.PublisherName, option => option.MapFrom(src => src.Publisher.PublisherName))
               .ReverseMap();
            CreateMap<ReadBookDto, Book>();
            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<UpdateBookDto, Book>().ReverseMap();
            #endregion

            #region Order
            CreateMap<Order, ReadOrderDto>()
                .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName));
            CreateMap<ReadOrderDto, Order>();
            #endregion

            #region BookOrderDetails
            CreateMap<BookOrderDetails, ReadBookOrderDetailsDto>()
              .ForMember(dest => dest.BookTitle, option => option.MapFrom(src => src.Book.BookTitle))
              .ForMember(dest => dest.Price, option => option.MapFrom(src => src.Book.Price))
              .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Order.Customer.CustomerName));
            CreateMap<ReadBookOrderDetailsDto, BookOrderDetails>();
            CreateMap<BookOrderDetails, CreateBookOrderDetailsDto>().ReverseMap();
            #endregion

            #region Borrow
            CreateMap<Borrow, CreateBorrowDto>().ReverseMap();
            CreateMap<Borrow, ReadBorrowDto>()
               .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName))
               .ForMember(dest => dest.BookName, option => option.MapFrom(src => src.Book.BookTitle));
            CreateMap<ReadBorrowDto, Borrow>();
            #endregion

            #region Customer
            CreateMap<Customer, ReadCustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>().ReverseMap();
            #endregion

            #region BannedCustomer
            CreateMap<BannedCustomer, ReadBannedCustomerDto>()
               .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName))
               .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadBannedCustomerDto, BannedCustomer>();
            CreateMap<CreateBannedCustomerDto, BannedCustomer>().ReverseMap();
            #endregion

            #region Employee
            CreateMap<Employee, ReadEmployeeDto>();
            CreateMap<ReadEmployeeDto, Employee>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();
            #endregion

            #region Attendence
            CreateMap<Attendence, ReadAttendenceDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadAttendenceDetailsDto, Attendence>();
            CreateMap<ReadAttendanceDto, Attendence>();
            CreateMap<Attendence, ReadAttendanceDto>();
            CreateMap<Attendence, CreateAttendenceDto>();
            CreateMap<CreateAttendenceDto, Attendence>();
            #endregion

            #region Payroll
            CreateMap<Payroll, ReadPayrollDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadPayrollDetailsDto, Payroll>();
            CreateMap<ReadPayrollDto, Payroll>();
            CreateMap<Payroll, ReadPayrollDto>();
            CreateMap<Payroll, CreatePayrollDto>();
            CreateMap<CreatePayrollDto, Payroll>();
            CreateMap<UpdatePayrollDto, Payroll>();
            #endregion

            #region Vacation
            CreateMap<Vacation, ReadVacationDetailsDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadVacationDetailsDto, Vacation>();
            CreateMap<ReadVacationDto, Vacation>();
            CreateMap<Vacation, ReadVacationDto>();
            CreateMap<Vacation, CreateVacationDto>();
            CreateMap<CreateVacationDto, Vacation>();
            #endregion

            #region ReturnedOrder
            CreateMap<ReturnedOrder, ReadReturnedOrderWithDetailsDto>()
                .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName));
            CreateMap<ReadReturnedOrderWithDetailsDto, ReturnedOrder>();
            CreateMap<ReadReturnedOrderDto, ReturnedOrder>();
            CreateMap<ReturnedOrder, ReadReturnedOrderDto>();
            CreateMap<CreateReturnedOrderDto, ReturnedOrder>();
            CreateMap<ReturnedOrder, CreateReturnedOrderDto>();
            #endregion

            #region ReturnOrderDetails
            CreateMap<ReturnOrderDetails, ReadReturnOrderDetailsWithDetailsDto>()
              .ForMember(dest => dest.BookTitle, option => option.MapFrom(src => src.Book.BookTitle))
              .ForMember(dest => dest.Price, option => option.MapFrom(src => src.Book.Price))
              .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Order.Customer.CustomerName));
            CreateMap<ReadReturnOrderDetailsWithDetailsDto, ReturnOrderDetails>();
            CreateMap<ReadReturnOrderDetailsDto, ReturnOrderDetails>();
            CreateMap<ReturnOrderDetails, ReadReturnOrderDetailsDto>();
            CreateMap<ReturnOrderDetails, CreateReturnOrderDetailsDto>().ReverseMap();
            #endregion
        }
    }
}
