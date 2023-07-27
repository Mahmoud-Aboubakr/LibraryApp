using System;
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
    }
}
