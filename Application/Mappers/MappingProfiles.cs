using Application.DTOs.Attendance;
using Application.DTOs.Author;
using Application.DTOs.BannedCustomer;
using Application.DTOs.Book;
using Application.DTOs.BookOrderDetails;
using Application.DTOs.Borrow;
using Application.DTOs.Customer;
using Application.DTOs.Employee;
using Application.DTOs.Identity;
using Application.DTOs.Order;
using Application.DTOs.Payroll;
using Application.DTOs.Publisher;
using Application.DTOs.ReturnedOrder;
using Application.DTOs.ReturnOrderDetails;
using Application.DTOs.Vacation;
using Application.IdentityModels;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;

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
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, CreateOrderDto>();
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
            CreateMap<Attendence, ReadAttendanceDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadAttendanceDto, Attendence>();
            CreateMap<Attendence, CreateAttendenceDto>();
            CreateMap<CreateAttendenceDto, Attendence>();
            #endregion

            #region Payroll
            CreateMap<Payroll, ReadPayrollDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadPayrollDto, Payroll>();
            CreateMap<Payroll, CreatePayrollDto>();
            CreateMap<CreatePayrollDto, Payroll>();
            CreateMap<UpdatePayrollDto, Payroll>();
            #endregion

            #region Vacation
            CreateMap<Vacation, ReadVacationDto>()
                .ForMember(dest => dest.EmpName, option => option.MapFrom(src => src.Employee.EmpName));
            CreateMap<ReadVacationDto, Vacation>();
            CreateMap<Vacation, CreateVacationDto>();
            CreateMap<CreateVacationDto, Vacation>();
            #endregion

            #region ReturnedOrder
            CreateMap<ReturnedOrder, ReadReturnedOrderDto>()
                .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Customer.CustomerName));
            CreateMap<ReadReturnedOrderDto, ReturnedOrder>();
            CreateMap<CreateReturnedOrderDto, ReturnedOrder>();
            CreateMap<ReturnedOrder, CreateReturnedOrderDto>();
            #endregion

            #region ReturnOrderDetails
            CreateMap<ReturnOrderDetails, ReadReturnOrderDetailsDto>()
              .ForMember(dest => dest.BookTitle, option => option.MapFrom(src => src.Book.BookTitle))
              .ForMember(dest => dest.Price, option => option.MapFrom(src => src.Book.Price))
              .ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Order.Customer.CustomerName));
            CreateMap<ReadReturnOrderDetailsDto, ReturnOrderDetails>();
            CreateMap<ReturnOrderDetails, CreateReturnOrderDetailsDto>().ReverseMap();
            #endregion

            #region Role
            CreateMap<RegisterModel, ApplicationUser>();
            #endregion

        }
    }
}
