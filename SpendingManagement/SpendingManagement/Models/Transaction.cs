namespace SpendingManagement.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int WalletId { get; set; }

    public int CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime Date { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
