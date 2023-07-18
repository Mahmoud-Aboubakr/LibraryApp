using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AttendencePermissionValidator : IAttendencePermissionValidator
    {
        public bool IsValidAttendencePermission(int permission)
        {
            return permission == 0 || permission == 1;
        }
    }
}
