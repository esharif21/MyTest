using System;
using System.Collections.Generic;

namespace Oracle.Model.Entities
{
    public partial class Customer
    {
        public decimal Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public DateTime? Createddate { get; set; }
    }
}
