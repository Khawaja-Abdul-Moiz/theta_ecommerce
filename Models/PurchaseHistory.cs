﻿using System;
using System.Collections.Generic;

namespace theta_ecommerce.Models
{
    public partial class PurchaseHistory
    {
        public int Id { get; set; }
        public int? SellerId { get; set; }
        public int? VendorId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? Quantity { get; set; }
        public int? PurchasePrice { get; set; }
        // Navigation properties
        public Seller Seller { get; set; }
        public Vendor Vendor { get; set; }
        public Product Product { get; set; }
    }
}
