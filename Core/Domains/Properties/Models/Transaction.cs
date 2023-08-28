using Core.Commons;

namespace Core.Domains.Properties.Models;


public sealed class Transaction : Enumeration<Transaction>
{
    public static readonly Transaction None = new Transaction(0, nameof(None));
    public static readonly Transaction Sale = new Transaction(1, nameof(Sale));
    public static readonly Transaction Rent = new Transaction(2, nameof(Rent));
    
    private Transaction(int id, string name) : base(id, name)
    {
    }
}
