using System;
using System.Collections.Generic;

namespace theta_ecommerce.Models
{
    public partial class SystemUser
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
