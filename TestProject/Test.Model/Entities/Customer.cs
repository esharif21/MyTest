using System;
using System.Collections.Generic;

namespace Test.Model.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public DateTime? CreatedDate { get; set; }
}
