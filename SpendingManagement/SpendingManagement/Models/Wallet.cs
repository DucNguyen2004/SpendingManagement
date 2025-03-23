using System;
using System.Collections.Generic;

namespace SpendingManagement.Models;

public partial class Wallet
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Balance { get; set; }

    public virtual User User { get; set; } = null!;
}
