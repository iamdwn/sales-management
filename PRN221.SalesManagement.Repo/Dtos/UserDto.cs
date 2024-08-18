using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.SalesManagement.Repo.Dtos
{
    public class UserDto
    {
        public bool isAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool isAdmin { get; set; }
    }
}
