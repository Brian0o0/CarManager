using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BusinessModel
{
    public class UserResponModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string UserType { get; set; }

        public DateOnly? RegistrationDate { get; set; }
    }
    public class UserRequestModel
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateOnly? RegistrationDate { get; set; }
    }
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
