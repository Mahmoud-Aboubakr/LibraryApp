﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class AppMessages
    {
        public const string INSERTED = "Inserted Successfully";
        public const string UPDATED = "Updated Successfully";
        public const string DELETED = "Deleted Successfully";
        public const string INVALID_ID = "This is invalid Id";
        public const string INVALID_PERMISSION = "This is invalid input (Permission)";
        public const string INVALID_MONTH = "This is invalid Month Value";
        public const string INVALID_PHONENUMBER = "This is invalid PhoneNumber";
        public const string FAILED_DELETE = "Can't delete because it used in other tables";
        public const string INVALID_QUANTITY = "This is invalid Quantity";
        public const string INVALID_PRICE = "This is invalid Price";
        public const string INVALID_CUSTOMER = "This is invalid Customer";
        public const string INVALID_BOOK = "This is invalid Book";
        public const string INVALID_ORDER = "This is invalid Order Id";
        public const string BANNED_CUSTOMER = "This customer is Banned";
        public const string MAX_BORROWING = "This customer reached the Maximum Number of Borrowed Books";
        public const string INVALID_EMPTYPE = "This is invalid Employee Type";
        public const string INVALID_AGE = "This is invalid Age";
        public const string FIRED = "This is employee is Fired successfully";
        public const string UNAVAILABLE_BOOK = "Book Is Unavailable";
        public const string MAX_NORMAL_VACATIONS = "You Can't take more normal vacations";
        public const string FAILED_RETURN = "You Can't Return This Order";
        public const string RETURNED = "Returned Successfully";
        public const string INTERNAL_SERVER = "Internal Server Error";
        public const string NOT_FOUND = "Not Found";
        public const string BAD_REQUEST = "Bad Request";
        public const string UNAUTHORIZED = "Unauthorized";
        public const string UNAUTHENTICATED = "Token i not authenticated";
        public const string NOTACCEPTABLE = "Not Accepted Input";
        public const string INAVIL_PAGING = "Inavild Page Size and Page Index";
        public const string EXISTING_EMAIL = "This Email is already exists";
        public const string INVALID_PAYLOAD = "Invalid Payload";
        public const string INVALID_CREDIENTIALS = "This is Invaild Credientials";
        public const string INVALID_LOGIN = "There is no existing email, sign up first";
        public const string REGISTERED_EMAIL = "Email is already registered!";
        public const string REGESTERED_USER = "Username is already registered!";
        public const string INCORRECT_CREDIENTIALS = "Email or Password is incorrect!";
        public const string INVALID_IDorRole = "Invalid user ID or Role";
        public const string ASSIGNED_ROLE = "User already assigned to this role";
        public const string WRONG = "Something went wrong";
        public const string INVALID_EMAIL = "This is invalid Email";
        public const string INVALID_TOKEN = "Invalid token";
        public const string INACTIVE_TOKEN = "Inactive token";
        public const string REQUIRED_TOKEN = "Token is required!";
        public const string NULL_DATA = "Can't find this data in the table!";
        public const string NOTFOUND_SEARCHDATA = "Can't find data that you search for!";
        public const string NOTFOUND_USER = "Can't find User!";
        public const string FAILDE_USER_DELETE = "Can't delete this User!";
    }
}
