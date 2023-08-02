using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityServices
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
    }
}
