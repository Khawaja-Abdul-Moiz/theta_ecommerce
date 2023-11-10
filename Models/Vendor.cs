using System;
using System.Collections.Generic;

namespace theta_ecommerce.Models
{
    public partial class Vendor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Cnic { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
